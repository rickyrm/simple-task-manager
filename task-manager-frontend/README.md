Frontend Angular para la API de tareas (Task Manager). Implementa:

- Lista de tareas (cards)
- Crear / Editar / Borrar tareas (modal)
- Filtrado por estado (todas / activas / completadas)
- Comunicación con la API REST (endpoints: GET/POST/PUT/DELETE `/api/tasks`)

Este repo usa Angular 21 y Bootstrap para estilos.

---

## Ejecutar en desarrollo

1. Instala dependencias (desde la raíz del frontend):

```bash
npm install
```

2. Ejecuta el servidor de desarrollo:

```bash
npm start
# o `ng serve`
```

Abre `http://localhost:4200`.

Nota: el frontend consume una API en `http://localhost:5121/api/tasks` por defecto. Si tu backend escucha en otro puerto, actualiza `src/app/services/task.ts` -> `baseUrl` o usa un proxy (recomendado para desarrollo, ver sección "Proxy de desarrollo").

---

## Estructura principal del proyecto

- `src/app/`
	- `app.ts` - Componente raíz que monta `app-task-list` dentro del layout.
	- `app.config.ts` - Proveedores globales (router, HttpClient, NgbModule).
	- `components/`
		- `task-list/` - Componente principal que muestra la lista, filtros y acciones. (archivos: `task-list.ts`, `task-list.html`, `task-list.scss`)
		- `task-modal/` - Modal standalone con formulario reactivo para crear/editar tareas. (archivos: `task-modal.ts`, `task-modal.html`, `task-modal.scss`)
	- `models/`
		- `task.model.ts` - Interfaz `TaskItem` usada en el frontend.
	- `services/`
		- `task.ts` - Servicio Angular que encapsula llamadas HTTP a la API (`getTasks`, `createTask`, `updateTask`, `deleteTask`).

- `src/styles.scss` - Estilos globales (Bootstrap cargado desde `angular.json`).
- `angular.json` - Contiene la referencia a `node_modules/bootstrap/dist/css/bootstrap.min.css` en la sección `styles`.

---

## Cómo funciona la comunicación con el backend

El servicio `Task` (en `src/app/services/task.ts`) expone métodos que devuelven `Observable<T>` usando `HttpClient`:

- `getTasks(): Observable<TaskItem[]>`
- `createTask(task: TaskItem): Observable<TaskItem>`
- `updateTask(task: TaskItem): Observable<TaskItem>`
- `deleteTask(id: number): Observable<void>`

El `TaskListComponent` realiza la carga inicial en `ngOnInit()` y mantiene dos arrays en memoria: `tasks` (todas) y `filtered` (según filtro). Al crear/editar/borrar actualiza `tasks` y `filtered` localmente para mostrar los cambios en tiempo real.

---

## Proxy de desarrollo (opcional, recomendado)

Para evitar usar URLs absolutas y problemas CORS en desarrollo, crea `proxy.conf.json` en la raíz del frontend con:

```json
{
	"/api": {
		"target": "http://localhost:5121",
		"secure": false,
		"changeOrigin": true,
		"logLevel": "info"
	}
}
```

Luego modifica `package.json` `start` script a: `ng serve --proxy-config proxy.conf.json` y en `task.ts` usa `private baseUrl = '/api/tasks'`.

---

## Errores encontrados y soluciones (resumen)

1) "Cannot find module '@ng-bootstrap/ng-bootstrap'"
	- Causa: la dependencia no estaba instalada.
	- Solución: instalé `@ng-bootstrap/ng-bootstrap`, `bootstrap` y `@popperjs/core`. Debido a incompatibilidades de peer deps con Angular 21 tuve que usar `--legacy-peer-deps` para la instalación local.

2) Error NG1010 / referencias a componentes standalone en `NgModule.declarations`
	- Causa: el proyecto inicial usaba bootstrap por `bootstrapApplication(...)` y componentes standalone; `AppModule` tenía declaraciones que causaban errores estáticos.
	- Solución: simplifiqué `src/app/app.modules.ts` dejando un módulo inofensivo (sin declarar los standalone components) y pasé a bootstrap standalone en `app.ts`.

3) `ERR_CONNECTION_REFUSED` al hacer peticiones a `http://localhost:5000` y error en consola `HttpErrorResponse status 0`.
	- Causa: backend corriendo en otro puerto (5121) o no arrancado.
	- Solución: comprobé con `curl` y actualicé `src/app/services/task.ts` para usar `http://localhost:5121/api/tasks`. También añadí instrucciones del proxy como alternativa.

4) Modal de edición/crud se quedaba "pegado" (no se cerraba / `ref.result` no se resolvía)
	- Causa: en `TaskModalComponent` estaba declarado `providers: [NgbActiveModal]`, eso creaba una instancia local distinta a la que `NgbModal` manejaba.
	- Solución: eliminar `providers: [NgbActiveModal]` del componente y usar la instancia que inyecta `NgbModal` al abrir el modal.

5) La lista aparecía vacía inicialmente en la pestaña "Todas"
	- Causa: render inicial ocurre antes de que la llamada HTTP asíncrona responda; la plantilla mostraba el estado vacío durante ese breve lapso.
	- Solución: asegurar un estado `loading` visible y forzar detección de cambios tras recibir la respuesta asíncrona (se inyectó `ChangeDetectorRef` y se llama `detectChanges()` tras actualizar `tasks`). Alternativa recomendada: usar Observables + `async` pipe.

6) Acceso a `id` de respuesta nula en actualizaciones
	- Causa: algunos endpoints devuelven `204 No Content` o `null` tras actualizar; el código asumía siempre un objeto en la respuesta y accedía a `res.id` causando TypeError.
	- Solución: aplicar fallback al payload enviado cuando la respuesta es `null` (`const server = updated ?? payload`) y actualizar la lista con ese `server`.

7) Mensaje en DevTools: "Unable to add filesystem: <illegal path>"
	- Causa: DevTools Workspaces o una extensión intentó mapear una ruta local inválida.
	- Solución: eliminar la entrada en DevTools > Sources > Filesystem o desactivar la extensión que la creó (no es un error del app).

---

## UX / mejoras aplicadas

- Resaltado y desplazamiento automático al crear/editar/marcar tareas (se desplaza al elemento y lo resalta brevemente).
- Interacción en tiempo real: las operaciones CRUD actualizan `tasks` localmente para que el usuario vea los cambios sin recargar.
- Estilos globales y layout básico con Bootstrap (navbar, contenedor central, cards para tareas).

---

## Siguientes pasos opcionales

- Convertir `TaskListComponent` a usar Observables con `async` pipe.
- Añadir toasts de confirmación (Bootstrap toasts) para operaciones exitosas y errores.
- Añadir pruebas unitarias para `TaskListComponent` y el servicio `Task`.

---

Si quieres que implemente alguna de las mejoras anteriores o que verifique algo en tu entorno, dime cuál y lo hago.
