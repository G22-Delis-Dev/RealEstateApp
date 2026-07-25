# RealEstateApp

Sistema inmobiliario desarrollado en ASP.NET Core MVC y Web API (.NET 9) siguiendo Onion Architecture, principios SOLID y Clean Code.

## Tecnologías

* **Framework:** ASP.NET Core MVC y Web API (.NET 9)
* **ORM:** Entity Framework Core 9 — Code First
* **Base de datos:** SQL Server
* **Autenticación:** ASP.NET Core Identity & JWT (API) / Cookies (WebApp)
* **Arquitectura:** Onion Architecture (Clean Architecture)

## Arquitectura

El proyecto sigue Onion Architecture con las siguientes capas:

`Domain` → `Application` → `Infrastructure` → `Presentation` (Web & API)
↑ `Shared`

Las dependencias siempre apuntan hacia adentro. Domain no depende de ninguna capa externa.

## Estructura del proyecto

```text
RealEstateApp/
├── RealEstateApp.Domain         ← Entidades, Enums, Interfaces de repositorios
├── RealEstateApp.Application    ← Servicios, DTOs, Interfaces de servicios
├── RealEstateApp.Infrastructure ← EF Core, Repositorios, Identity (Autenticación)
├── RealEstateApp.Shared         ← Servicios transversales
└── Presentation
    ├── RealEstateApp.WebApp     ← Controllers, Views, MVC
    └── RealEstateApp.API        ← Controllers, Endpoints REST, Swagger
```

## Capas

### Domain
Núcleo del sistema. Sin dependencias externas. Contiene entidades del negocio, enumeraciones y las abstracciones (interfaces) necesarias.

### Application
Orquesta los casos de uso. Depende solo de Domain. Contiene servicios de negocio, DTOs (Objetos de transferencia de datos) y ViewModels.

### Infrastructure
Implementaciones técnicas. Depende de Domain.
* **Identity**: Gestión de usuarios, roles, login y tokens JWT.
* **Persistence**: Contexto de EF Core, migraciones y repositorios concretos.

### Shared
Servicios transversales compartidos por otras capas (por ejemplo, servicios de email, notificaciones).

### Presentation
Depende de Application, Infrastructure y Shared.
* **WebApp**: Capa MVC para usuarios que consumen las vistas directamente.
* **API**: Servicios RESTful protegidos por JWT.

## Roles del sistema

| Rol | Descripción |
|---|---|
| **Admin** | Administrador general, gestiona agentes, clientes, tipos de propiedades y mejoras. |
| **Agent** | Agente inmobiliario, gestiona y publica las propiedades a la venta. |
| **Client** | Cliente que busca propiedades para comprar o rentar. |
| **Developer** | Usuario con permisos especiales para el desarrollo y consumo de la API RESTful. |

## Módulos

* **Administrador**
  * Autenticación y control de acceso por rol.
  * Mantenimiento de Administradores.
  * Mantenimiento de Desarrolladores.
  * Mantenimiento de Agentes.
  * Mantenimiento de Tipos de Propiedades, Mejoras y Tipos de Ventas.
* **Agente (Agent)**
  * Autenticación y control de acceso por rol.
  * Mantenimiento y publicación de propiedades.
* **Cliente (Client)**
  * Búsqueda y visualización de propiedades.
* **Developer (API)**
  * Consumo de Endpoints RESTful de propiedades y catálogos.

## Principios aplicados

* **SOLID**
  * S — Cada clase tiene una sola responsabilidad.
  * O — Abierto para extensión, cerrado para modificación.
  * L — Las dependencias apuntan a interfaces genéricas y específicas.
  * I — Interfaces segregadas por entidad y por servicio.
  * D — Application depende de interfaces, no de implementaciones concretas.
* **Clean Code**
  * Nombres descriptivos consistentes con el dominio.
  * Métodos con una sola responsabilidad.
  * Separación de responsabilidades mediante la arquitectura.
* **Onion Architecture**
  * Domain sin dependencias externas.
  * Las dependencias apuntan siempre hacia adentro.
  * Infrastructure implementa interfaces de Domain.

## Seguridad

* Autenticación con **ASP.NET Core Identity**.
* Control de acceso por rol en cada controlador y endpoint.
* Autenticación basada en **Cookies** para la WebApp y basada en **JWT** para la API REST.

## Configuración

Ejemplo de `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RealEstateAppDB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
  },
  "JWTSettings": {
    "Key": "Esta_es_una_clave_secreta_super_segura_para_el_JWT_de_ejemplo",
    "Issuer": "RealEstateApp",
    "Audience": "RealEstateAppUsers",
    "DurationInMinutes": 60
  }
}
```

## Migraciones

```bash
# Aplicar migraciones y actualizar base de datos
dotnet ef database update --project src/Infrastructure/RealEstateApp.Infrastructure.Persistence --startup-project src/Presentation/RealEstateApp.WebApp
```

## Equipo

| Integrante | Matricula |
|---|---|
| Delis Manuel De La Cruz Castillo | 2025-1074 |
| Sky Luisahanie Andujar Victorino | 2025-1063 |
