En cada directorio se encuentra un README.md explicando como funciona dicha estructura, las decisiones y los problemas que han surgido.
Por otro lado, en este Readme se comenta la justificación técnica del proyecto.

## Documento de Justificación Técnica del Proyecto

### 1. Arquitectura elegida: Arquitectura Hexagonal

Elegí una Arquitectura Hexagonal (Ports & Adapters) en lugar de una API tradicional por varias ventajas:

- Desacoplamiento total entre lógica de dominio y frameworks externos.

- Permite reemplazar la base de datos sin modificar la lógica interna.

- Mejora drásticamente la capacidad de testeo aunque en este caso no se aplica ningun test.

- Es extensible y escalable a futuro.

- Demuestra dominio de Clean Architecture y buenas prácticas profesionales.


### 2. Base de datos: SQLite

Utilicé SQLite porque:

- Permite un archivo único .db, ideal para pruebas y demostraciones.

- Asegura que el evaluador pueda ejecutar el proyecto sin errores.

- Funciona en cualquier sistema operativo.

- Fue la mejor opción para entregar un proyecto simple, portable y fácil de ejecutar.


### 3. Uso de DTOs

El uso de DTOs se debe a varios motivos:

- Evitar exponer entidades internas del dominio.

- Controlar qué campos son editables y cuáles no.

- Mantener un contrato estable con el frontend.

- Reducir acoplamiento entre API y modelo de datos.

- Facilitar la validación.


### 4. Reglas de negocio implementadas

No se añadieron campos adicionales, pero sí reglas esenciales:

- Título obligatorio.

- Título con longitud máxima de 200 caracteres.

- La fecha de creación se genera automáticamente.

- IsCompleted siempre debe ser booleano.

- La paginación requiere valores válidos.

Estas reglas garantizan datos consistentes y un flujo realista.

### 5. Paginación y filtrado

Implementé:

- Paginación (page, pageSize)

- Filtrado por estado (isCompleted)

Motivos:

- Controlar el tamaño de respuestas.

- Reducir carga en frontend.

- Mostrar dominio de LINQ.

- Ser fiel a casos reales de APIs REST.

### 6. Uso de Swagger

Swagger se utilizó porque:

- Permite probar la API sin herramientas externas.

- Facilita revisar el contrato de la API.

- Permite comprobar fácilmente paginación, filtros y reglas de validación.

### 7. Elección del frontend: Angular 21 (según las intrucciones) + Bootstrap

Elegí Bootstrap porque:

- Es liviano y rápido de implementar.

- No agrega estilos innecesarios.

- Proporciona una forma moderna de estilizar componentes.


### 8. Unit Tests: Por qué finalmente no se implementaron

Intenté implementar unit tests para el controlador, pero surgieron problemas:

- Desajuste entre las firmas de los métodos en ITaskService y el controlador.

- El servicio requería mockear paginación, filtrado y mapeo de DTOs.

- Los mocks requerían bastante tiempo de configuración.

- El problema del método GetAllAsync impidió avanzar de forma estable.

Decisión final

Elegí no incluir tests incompletos porque:

- Es preferible entregar un código limpio, estable y correctamente documentado antes que tests parciales que no aporten valor real.

Aun así, documenté los problemas.

### 9. Problemas Encontrados y Soluciones Aplicadas

Este listado detalla los principales desafíos técnicos que surgieron durante el desarrollo del proyecto y cómo fueron resueltos.

#### 1. Error de Incompatibilidad en `GetAllAsync`

* **Problema:** El compilador arrojaba el error `TaskService does not implement interface member ITaskService.GetAllAsync`, indicando una **incompatibilidad en la firma** del método.
* **Causa:** El método estaba retornando **entidades** (`Task`) directamente, mientras que la interfaz (`ITaskService`) esperaba el retorno de **DTOs** (`TaskDto`).
* **Solución:** Se modificó la implementación de `GetAllAsync` en `TaskService` para **alinear la firma** con la interfaz y se aplicó la lógica de **mapping** (conversión de Entidad a DTO) dentro del servicio antes de retornar la colección.

#### 2. Limpieza de Estructura en Angular (Frontend)

* **Problema:** La estructura inicial del proyecto generada por Angular **no coincidía con la arquitectura** de carpetas deseada (más modular y organizada).
* **Causa:** Uso de comandos de generación que crearon ficheros en ubicaciones no óptimas.
* **Solución:** Se realizó una **limpieza y reestructuración** manual, creando carpetas claras para la organización de los elementos, tales como: `components`, `services`, `models`, entre otras.

#### 3. Configuración de CORS (Cross-Origin Resource Sharing)

* **Problema:** El **frontend** (ejecutándose en un puerto) **no podía comunicarse** con la **API de backend** (ejecutándose en otro puerto). Esto resultaba en errores de política de origen cruzado en el navegador.
* **Causa:** La API de Spring Boot/ASP.NET Core no tenía habilitada una política que permitiera peticiones desde el dominio del frontend.
* **Solución:** Se configuró y **habilitó la política CORS** en el backend para permitir explícitamente las peticiones provenientes del origen de desarrollo del frontend, concretamente `http://localhost:4200`.

#### 4. Migraciones de EF Core que No se Aplicaban

* **Problema:** Al ejecutar el proyecto, las **migraciones de Entity Framework Core no creaban** o actualizaban la estructura de la base de datos esperada.
* **Causa:** La base de datos inicial (físicamente) no existía o no se había inicializado correctamente.
* **Solución:** Se forzó la creación y aplicación de la base de datos a través de la consola ejecutando los siguientes comandos:
    1.  `dotnet ef migrations add InitialCreate` (Para crear la primera migración)
    2.  `dotnet ef database update` (Para aplicar todas las migraciones pendientes a la base de datos)

### 10. Mejoras futuras
En un futuro una de los aspectos más importantes que se puede añadir es la seguridad a través de roles y tokens.
Una gestión hash para las contraseñas.
De este modo dependiendo del rol podrá crear o gestionar las tareas. 

### 11. Repositorio público: razón de la elección

El repositorio es público porque:
No se requieren permisos.

Permite examinar la evolución del código (commits).

Facilita el acceso desde cualquier máquina.

Muestra transparencia y profesionalidad.


### 12. Otros

Para una mayor comprensión del proyecto creado he indexado este repositorio con Deep Wiki, este que genera una explicación completa del repositorio.  
Link de Deep Wiki: https://deepwiki.com/rickyrm/simple-task-manager
