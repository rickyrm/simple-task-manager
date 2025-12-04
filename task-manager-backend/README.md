# Task Manager - Backend

El proyecto sigue principios de **Hexagonal Architecture** y buenas prácticas de desarrollo.

## **⚙️ Funcionalidades implementadas**

- CRUD completo de tareas (Create, Read, Update, Delete)  
- Marcar tareas como completadas/incompletas  
- Paginación y filtrado por estado (`isCompleted`)  
- Validación de datos:
  - Título obligatorio
  - Máximo 200 caracteres  
- DTOs para separar dominio de la API y evitar acoplamiento directo  
- Swagger habilitado para testeo fácil  

---

## **🚧 Problemas encontrados y decisiones técnicas**

### 1. Unit Tests
- Intenté implementar tests para `TasksController` usando **xUnit y Moq**.
- Problema: los métodos del servicio (`TaskService`) no eran virtuales, por lo que Moq no podía crear mocks:
    Unsupported expression: s => s.DeleteAsync(1)
    Non-overridable members may not be used
- Decisión: **no implementar los tests** para cumplir con la entrega a tiempo.
- Justificación: aunque los tests son importantes, prioricé un backend funcional y limpio, y documenté el problema como pendiente de mejora.

### 2. Elección de DTOs
- Para evitar exponer directamente las entidades de dominio, creé:
- `TaskCreateDto`, `TaskUpdateDto`, `TaskReadDto`
- Beneficio: desacopla API de la base de datos y permite validar datos antes de persistirlos.

### 3. Paginación y filtrado
- Implementados en `TaskService.GetAllAsync`.
- Permiten que el frontend reciba solo la página requerida y aplicar filtros sin sobrecargar la API.

### 4. Arquitectura Hexagonal
- Separación clara entre capas:
- **Domain:** entidad `TaskItem` y reglas de negocio
- **Application:** servicios que implementan la lógica de negocio
- **Infrastructure:** acceso a la base de datos
- **API:** controladores REST
- Justificación: reduce acoplamiento y facilita mantenimiento/futuras pruebas unitarias.

### 5. Elección de SQLite
- Ligera, fácil de configurar y ejecutar en cualquier entorno.
- Justificación: el evaluador puede ejecutar la aplicación inmediatamente sin instalaciones complejas.

### 6. Swagger
- Implementado para probar la API sin configurar Postman u otras herramientas.
- Permite al evaluador inspeccionar la API y probar endpoints fácilmente.

### 7. Decisiones no implementadas
- **JWT / Autenticación:** no requerida por la prueba y hubiera incrementado la complejidad innecesariamente.  
- **Unit tests:** problemas con Moq y métodos no virtuales; documentado como mejora pendiente.  
- **Logs avanzados:** no se añadieron para simplificar la entrega; el manejo de errores es básico pero funcional.

---


