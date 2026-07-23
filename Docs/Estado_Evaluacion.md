# Reporte de Estado y Evaluación: RealEstateApp

Este documento resume el progreso actual del proyecto contra los criterios de evaluación especificados en las rúbricas (`RealEstateApp_Evaluaciones_Combinadas.md` y `Mini proyecto final - RealEstateApp (2).md`).

## ✅ Criterios Cumplidos (Implementados y Validados)

### Arquitectura y Estructura (Onion Architecture)
- **Separación correcta entre WebApp y API**: Ambos proyectos existen de manera independiente y consumen la capa `Application` directamente, sin peticiones HTTP redundantes entre ellos.
- **Inyección de Dependencias**: Registros correctos en `Program.cs` (`AddApplicationLayer`, `AddPersistenceInfrastructure`, `AddIdentityInfrastructure`, `AddSharedInfrastructure`).
- **Patrones de Diseño**: Implementación de repositorios genéricos y servicios de dominio aislados en sus respectivas capas.
- **Mapeos Funcionales**: `AutoMapper` configurado para manejar `ViewModels` en MVC y `DTOs` en el API.

### Web API (Back-End)
- **Seguridad y Control de Acceso**:
  - `AccountController` expone endpoints protegidos para autenticación JWT.
  - Atributos `[Authorize(Roles = "...")]` funcionales y respetando la separación entre Desarrolladores, Administradores, Clientes y Agentes.
  - Bloqueo para evitar que Clientes y Agentes usen la API.
- **Respuestas y Manejo de Errores**:
  - Los controladores (`Properties`, `Agents`, `Improvements`, `PropertyTypes`, `SaleTypes`) cumplen devolviendo los Status Codes correctos (200, 201, 204, 400, 401, 403, 404).
  - Implementación de `ExceptionHandlingMiddleware` para interceptar errores de 500 y devolverlos en JSON limpio y profesional.
- **Swagger**: Documentación dinámica adaptada a respuestas esperadas gracias al `SwaggerSuccessResponsesFilter` y `SwaggerErrorResponsesFilter`.

### WebApp (MVC)
- **Resolución de Dependencias**: Corregidos los errores 500 al iniciar el servidor (se añadieron las dependencias del negocio y DB al frontend).
- **Conexiones**: El `appsettings.json` está integrado correctamente y el ORM se comunica con SQL Server sin explotar.

---

## ⏳ Criterios Pendientes o por Completar (Lo que falta)

A continuación, los puntos de la rúbrica que **faltan por construir, o requieren validación manual/visual** (mayormente centrados en las funcionalidades gráficas de la WebApp y reglas de negocio específicas).

### 1. Datos Semilla (Seed Data)
- **Rol y Usuarios Semilla**: Se debe validar que al correr las migraciones o arrancar el sistema, existan clases de Seed que inserten automáticamente los roles ("Administrador", "Desarrollador", "Cliente", "Agente") y creen al menos un usuario Activo para los roles Administrador y Desarrollador.

### 2. Funcionalidades Visuales y de Vistas (WebApp MVC)
A nivel de vistas (interfaz gráfica), la rúbrica exige componentes que requieren ser probados o construidos manualmente:
- **Gestión de Imágenes**: Subir fotos de las propiedades a la carpeta `wwwroot/Images` de la WebApp al crear, y eliminarlas del servidor al borrar.
- **Filtros de Búsqueda**: Funcionalidad de filtrado por Tipo de Propiedad, Rango de Precio y Habitaciones/Baños en el Home.
- **Funcionalidades del Cliente (Chat y Favoritos)**:
  - Botón de "Favoritos" en las tarjetas de propiedades (Cliente).
  - Chat/Mensajería de consultas desde el Cliente al Agente.
- **Vistas del Agente (Mantenimiento de propiedades)**:
  - Vistas que le permitan a un agente ver el listado de SUS propiedades, crear nuevas propiedades y marcar las ofertas como aceptadas.
  - Sección de "Mi Perfil" donde el agente edita su foto y datos.
- **Vistas del Administrador**:
  - CRUD visual (pantallas y formularios) de Tipo de Propiedades, Desarrolladores, Administradores y Mejoras.

### 3. Reglas de Negocio Estrictas
- **Validación de Unicidad (Email, Cédula y Usuario)**: Asegurar que el `AuthService` valide e impida registrar dos cuentas con la misma cédula o correo (en ambos, WebApp y API).
- **Restricción de Eliminación (Borrado Restringido)**: Al borrar una mejora (Improvement), asegurarse de que EntityFramework no elimine en cascada las propiedades asociadas a ella.

---

**Nota Final**: El progreso a nivel back-end (estructura, arquitectura y la API completa) está prácticamente a un 100%. El trabajo restante consiste principalmente en "pintar" (crear el diseño visual) de los requerimientos y probar las interacciones en el navegador para la WebApp MVC.
