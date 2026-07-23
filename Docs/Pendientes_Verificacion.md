# Pendientes de Verificación — RealEstateApp

**Fecha:** 23 de julio de 2026  
**Estado general:** La solución compila sin errores ni warnings. Todos los fixes aplicados en la última sesión están integrados. Lo que sigue son puntos que **requieren prueba en tiempo de ejecución** para confirmar que están 100% operativos antes de la evaluación.

---

## ✅ Fixes ya aplicados (sesión actual)

| Fix | Archivo(s) |
|-----|-----------|
| Agregada propiedad `IsSold` a `PropertyViewModel` (resuelve build error CS1061) | `Application/ViewModels/Properties/PropertyViewModel.cs` |
| Vistas de Administrators (Index, Create) movidas a `Views/Admin/Administrators/` | Views folder |
| Vistas de Developers (Index, Create) movidas a `Views/Admin/Developers/` | Views folder |
| `throw new Exception(...)` de duplicados reemplazados por `ConflictException` en `AuthService` | `Identity/Services/AuthService.cs` |
| Eliminado `Class1.cs` (placeholder vacío) | `Infrastructure.Identity` |
| Eliminado `WeatherForecast.cs` (template por defecto) | `RealEstateApp.API` |

---

## 🔴 Prioridad Alta — Verificar en ejecución

### 1. Flujo completo de registro y activación por correo ✅ VERIFICADO (23/07/2026)

**Resultado:** `EmailService` funciona correctamente. Tests manuales pasaron y correos recibidos confirmados en `Jesusortiz221516@gmail.com`. El servicio SMTP (Gmail, puerto 465, SSL) está operativo con las credenciales configuradas en UserSecrets.

**Pendiente complementario:** Verificar en la WebApp que el link de activación dentro del correo redirige correctamente y activa la cuenta en la DB.

---

### 2. Vistas de Administrators y Developers (recién movidas)

**Qué verificar:**
- Ir a `/Admin/Administrators` → debe cargar el listado de admins.
- Ir a `/Admin/Administrators/Create` → debe mostrar el formulario de creación.
- Crear un administrador nuevo → confirmar que guarda, aparece en el listado y puede iniciar sesión.
- Repetir para `/Admin/Developers` → listado y creación.

**Por qué es crítico:** Las vistas fueron movidas de `Views/Administrators/` a `Views/Admin/Administrators/` en este fix. Es la primera vez que se ejecutan desde la nueva ubicación.

---

### 3. Validación de unicidad (Email, Usuario, Cédula)

**Qué verificar:**
- Intentar registrar dos usuarios con el mismo **correo** → debe mostrar error "El correo X ya está registrado." con HTTP 409 en la API, o mensaje en el formulario en la WebApp.
- Intentar registrar dos usuarios con el mismo **nombre de usuario** → idem.
- Intentar registrar dos usuarios con la misma **cédula** → idem.
- Verificar que el mensaje de error llega al usuario (no queda como pantalla en blanco o error 500).

**Estado del código:** `AccountService` valida con `BusinessRuleValidationException` (400). `AuthService` ahora lanza `ConflictException` (409) como segunda línea de defensa. El `ExceptionHandlingMiddleware` de la API maneja ambas. En la **WebApp**, el `catch (Exception ex)` en los controllers captura y agrega al `ModelState` — verificar que el mensaje llega al formulario.

---

### 4. Restricción de eliminación de Mejoras (cascade delete)

**Qué verificar:**
- Crear una mejora y asociarla a al menos una propiedad.
- Eliminar la mejora desde `/Admin/Improvements`.
- Confirmar que:
  - La mejora desaparece del catálogo.
  - Las propiedades asociadas **siguen existiendo** en la base de datos.
  - Las filas en `PropertyImprovements` (tabla join) se eliminan correctamente.
  - Las propiedades **no** desaparecen del Home ni del panel del agente.

**Estado del código:** La configuración de EF Core usa `DeleteBehavior.Cascade` solo sobre la tabla join `PropertyImprovements`, no sobre `Properties`. Esto es el comportamiento correcto, pero requiere validación visual en DB o a través de la UI.

---

## 🟡 Prioridad Media — Validar comportamiento

### 5. Bloqueo de administrador autenticado (no editarse/inactivarse a sí mismo)

**Qué verificar:**
- Iniciar sesión como `admin@realestate.com`.
- Ir a `/Admin/Administrators`.
- Confirmar que el botón "Inactivar" no aparece para el propio usuario autenticado, **o** que si se intenta, el sistema lo bloquea con mensaje de error.
- Confirmar que tampoco puede editarse datos críticos que rompan su propia sesión.

---

### 6. Restricción del último admin activo

**Qué verificar:**
- Con solo un administrador activo en el sistema, intentar inactivarlo.
- Debe mostrar mensaje: "No se puede inactivar el último administrador activo."
- La regla `LastActiveAdminCannotBeDeactivatedRule` está en el Domain y tiene tests unitarios que pasan. Lo pendiente es el test **de integración en la UI**.

---

### 7. Efectos de inactivar un agente

**Qué verificar:**
- Inactivar un agente desde `/Admin/AgentManagement`.
- Confirmar que:
  - El agente no puede iniciar sesión (mensaje claro).
  - El agente **no aparece** en el listado público de agentes (`/Agents`).
  - Las propiedades del agente inactivo **no aparecen** en el Home público.

---

### 8. Gestión de propiedades al eliminar un agente

**Qué verificar:**
- Eliminar un agente que tiene propiedades registradas.
- Confirmar que las propiedades se eliminan también (o quedan huérfanas según la regla de negocio definida).
- Confirmar que no hay excepción de FK en la base de datos.

---

### 9. Bloqueo de edición/eliminación de propiedades vendidas

**Qué verificar:**
- Aceptar una oferta sobre una propiedad (lo que la marca como Vendida).
- Intentar editar esa propiedad desde el panel del agente → debe estar bloqueado.
- Intentar eliminarla → debe estar bloqueado.
- En ambos casos debe aparecer un mensaje claro, no un error 500.

---

### 10. Bloqueo de nuevas ofertas sobre propiedad con oferta aceptada o vendida

**Qué verificar:**
- Con una propiedad que tiene una oferta aceptada (estado Vendida), intentar crear una nueva oferta desde otro cliente.
- Debe bloquear con mensaje claro.
- También verificar: un cliente con oferta **Pendiente** no puede hacer otra oferta sobre la misma propiedad.

---

## 🟢 Prioridad Baja — Smoke tests visuales

### 11. Indicadores del dashboard de administrador

**Qué verificar:** Los contadores del Home del admin muestran valores correctos:
- Propiedades disponibles y vendidas.
- Agentes activos e inactivos.
- Clientes activos e inactivos.
- Desarrolladores activos e inactivos.

### 12. Filtros en Home público y Home del cliente

**Qué verificar:**
- Filtro por tipo de propiedad, rango de precio, habitaciones, baños.
- Aplicación combinada de filtros.
- Botón "Limpiar filtros" restaura el listado completo.
- Búsqueda por código de propiedad.

### 13. Galería de imágenes en detalle de propiedad

**Qué verificar:**
- Al cargar una propiedad con múltiples imágenes, se muestra un slider/galería funcional.
- Desde el Home y desde el detalle del agente, las imágenes cargan correctamente.

### 14. Chat agente-cliente

**Qué verificar:**
- Cliente envía mensaje sobre una propiedad.
- Agente ve el mensaje en su panel de conversaciones.
- Agente responde.
- Cliente ve la respuesta.

### 15. Pruebas unitarias existentes ✅ VERIFICADO (23/07/2026)

```
✅ LastActiveAdminCannotBeDeactivatedRuleTests — 3/3 pasaron
✅ EmailServiceManualTest — 2/2 pasaron (correos recibidos confirmados)
```

---

## 📋 Checklist de verificación rápida

```
[✅] EmailService funciona — correos recibidos confirmados
[✅] dotnet test — 5/5 tests pasan (3 unit + 2 email)
[ ] Registro de cliente + correo de activación (link activa cuenta en DB)
[ ] Login bloqueado para cliente inactivo
[ ] Listado de Administrators carga desde /Admin/Administrators
[ ] Listado de Developers carga desde /Admin/Developers
[ ] Crear administrador nuevo funciona end-to-end
[ ] Crear desarrollador nuevo funciona end-to-end
[ ] Registro con email duplicado muestra error (no 500)
[ ] Registro con usuario duplicado muestra error (no 500)
[ ] Registro con cédula duplicada muestra error (no 500)
[ ] Eliminar mejora no elimina propiedades asociadas
[ ] Admin no puede inactivarse a sí mismo
[ ] No se puede inactivar el último admin activo
[ ] Agente inactivo no aparece en listado público
[ ] Propiedades de agente inactivo no aparecen en Home
[ ] Eliminar agente gestiona sus propiedades sin error FK
[ ] Propiedad vendida no se puede editar ni eliminar
[ ] No se puede crear oferta si ya hay una aceptada
[ ] Dashboard admin muestra contadores correctos
[ ] Filtros del Home funcionan combinados
[ ] Chat entre cliente y agente funciona
```

---

*Documento generado el 23/07/2026 — post-fix session.*
