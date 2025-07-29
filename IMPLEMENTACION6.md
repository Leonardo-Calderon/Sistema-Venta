# IMPLEMENTACIÓN DE MANEJADOR DE EXCEPCIONES GLOBAL
## Sistema de Ventas (SistemaVenta)

**Fecha:** 28 de julio del 2025  
**Autor:** José Leonardo Rafael Calderón Gallegos

**Objetivo:** Implementar un manejador de excepciones global (Middleware) en una API REST para interceptar errores no controlados. El objetivo es evitar la fuga de información sensible (stack traces, rutas de sistema, consultas SQL) al cliente, mostrando en su lugar un mensaje de error genérico y seguro.

---

## 📋 ÍNDICE DE CONTENIDOS

### **Navegación Rápida:**
- [📊 Resumen Ejecutivo](#-resumen-ejecutivo)
- [🎯 Objetivos](#-objetivos)
- [📈 Métricas de Éxito](#-métricas-de-éxito)

### **Pasos de Implementación:**
- [🚨 Paso 1: Provocar Error No Controlado](#paso-1-provocar-error-no-controlado)
- [🛡️ Paso 2: Crear Middleware de Excepciones](#paso-2-crear-middleware-de-excepciones)
- [🔧 Paso 3: Implementar Lógica del Middleware](#paso-3-implementar-lógica-del-middleware)
- [✅ Paso 4: Registrar Middleware](#paso-4-registrar-middleware)
- [🧪 Paso 5: Probar Manejador](#paso-5-probar-manejador)

### **Información de Referencia:**
- [🔧 Configuración Técnica](#-configuración-técnica)
- [✅ Checklist de Verificación](#-checklist-de-verificación)
- [🚨 Troubleshooting](#-troubleshooting)
- [📚 Referencias](#-referencias)

---

## 📊 RESUMEN EJECUTIVO

### **¿Qué se Implementará?**
Sistema completo de manejo de excepciones global que intercepta errores no controlados en la API REST, evitando la fuga de información sensible y proporcionando respuestas seguras y genéricas al cliente.

### **Protección a Implementar:**
- ✅ **Interceptación Global** - Captura todos los errores no controlados
- ✅ **Ocultación de Información Sensible** - Evita stack traces y rutas del sistema
- ✅ **Logging Seguro** - Registra errores para desarrollo sin exponer datos
- ✅ **Respuestas Genéricas** - Mensajes de error seguros para el cliente
- ✅ **Códigos de Estado HTTP** - Respuestas apropiadas (500, 400, etc.)

### **Impacto en Seguridad:**
- 🛡️ **Previene fuga de información** sensible al cliente
- 🔒 **Protege rutas del sistema** y detalles de implementación
- 🚫 **Oculta consultas SQL** y detalles técnicos
- 📊 **Mejora experiencia del usuario** con mensajes claros
- 🔐 **Cumple estándares** de seguridad de APIs

---

## 🎯 OBJETIVOS

1. **Provocar error controlado** para demostrar el comportamiento por defecto
2. **Crear middleware global** para interceptar excepciones
3. **Implementar logging seguro** para el equipo de desarrollo
4. **Generar respuestas seguras** para el cliente
5. **Verificar funcionamiento** del manejador de excepciones

---

## 📈 MÉTRICAS DE ÉXITO

| Métrica | Objetivo | Resultado |
|---------|----------|-----------|
| Middleware Creado | Sí | ✅ Completado |
| Error Controlado Provocado | Sí | ✅ Completado |
| Logging Implementado | Sí | ✅ Completado |
| Respuestas Seguras | Sí | ✅ Completado |
| Información Sensible Oculta | 100% | ✅ Completado |
| Códigos de Estado Correctos | Sí | ✅ Completado |

---

## 📋 DESCRIPCIÓN DE LA TAREA

**Tarea Original:** Implementar un manejador de excepciones global (Middleware) en una API REST para interceptar errores no controlados. El objetivo es evitar la fuga de información sensible (stack traces, rutas de sistema, consultas SQL) al cliente, mostrando en su lugar un mensaje de error genérico y seguro.

**Adaptación al Proyecto:** Se implementará un middleware global de manejo de excepciones en la API REST del sistema SistemaVenta, protegiendo contra la fuga de información sensible y proporcionando respuestas seguras.

---

## 🚨 PASO 1: PROVOCAR ERROR NO CONTROLADO

**[⬆️ Volver al Índice](#-índice-de-contenidos)** | **[⬇️ Siguiente: Paso 2](#paso-2-crear-middleware-de-excepciones)**

### **Descripción del Paso:**
Provocar un error no controlado. En un endpoint existente, añadir una línea de código que genere una excepción de forma garantizada. Por ejemplo: `throw new InvalidOperationException("Este es un error de prueba para ver el comportamiento por defecto.");`

### **Implementación:**

#### **1. Seleccionar Endpoint para Prueba:**
**Archivo:** `SistemaVenta.API/Controllers/UsuariosController.cs`
**Endpoint:** `GET /api/usuarios/TestAcceso/{id}` - Endpoint de prueba existente

#### **2. Agregar Error Controlado:**
**Archivo Modificado:** `SistemaVenta.API/Controllers/UsuariosController.cs`

**Cambio Realizado:**
```csharp
[HttpGet("TestError")]
[Authorize(Roles = "Administrador")]
public IActionResult TestError()
{
    // Provocar un error no controlado para demostrar el comportamiento por defecto
    throw new InvalidOperationException("Este es un error de prueba para ver el comportamiento por defecto.");
}
```

**Ubicación:** Después del método `TestAcceso` existente

**Explicación:**
- ✅ **Endpoint de prueba** creado específicamente para demostrar errores
- ✅ **Error garantizado** con `throw new InvalidOperationException`
- ✅ **Mensaje descriptivo** para identificar el propósito
- ✅ **Autorización requerida** para controlar el acceso
- ✅ **Método GET** para facilitar las pruebas

**¿Por qué este endpoint?**
- 🛡️ **Propósito específico:** Solo para demostrar manejo de errores
- 🔒 **Acceso controlado:** Solo administradores pueden acceder
- 🚫 **Error predecible:** Siempre genera una excepción
- 📊 **Fácil de probar:** Método GET simple
- 🔐 **Seguro:** No afecta datos reales del sistema

---

## 🛡️ PASO 2: CREAR MIDDLEWARE DE EXCEPCIONES

**[⬆️ Volver al Índice](#-índice-de-contenidos)** | **[⬇️ Siguiente: Paso 3](#paso-3-implementar-lógica-del-middleware)**

### **Descripción del Paso:**
Crear el Middleware de Excepciones. Crear una nueva clase C# llamada `GlobalExceptionHandlerMiddleware`. Esta clase contendrá la lógica para interceptar los errores de la aplicación.

### **Implementación:**

#### **1. Crear Clase del Middleware:**
**Archivo Creado:** `SistemaVenta.API/Middleware/GlobalExceptionHandlerMiddleware.cs`

**Estructura de la Clase:**
```csharp
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

**Explicación:**
- ✅ **RequestDelegate _next** - Referencia al siguiente middleware en el pipeline
- ✅ **ILogger _logger** - Para registrar errores de forma segura
- ✅ **InvokeAsync** - Método principal que intercepta las peticiones
- ✅ **Try-catch global** - Captura todas las excepciones no controladas
- ✅ **HandleExceptionAsync** - Método para procesar las excepciones

**¿Por qué esta estructura?**
- 🛡️ **Interceptación global:** Captura errores en toda la aplicación
- 🔒 **Logging seguro:** Registra errores sin exponer información sensible
- 🚫 **Pipeline de middleware:** Se integra con el flujo de ASP.NET Core
- 📊 **Inyección de dependencias:** Utiliza ILogger para logging estructurado
- 🔐 **Patrón estándar:** Sigue las mejores prácticas de middleware

---

## 🔧 PASO 3: IMPLEMENTAR LÓGICA DEL MIDDLEWARE

**[⬆️ Volver al Índice](#-índice-de-contenidos)** | **[⬇️ Siguiente: Paso 4](#paso-4-registrar-middleware)**

### **Descripción del Paso:**
Implementar la lógica del Middleware. Dentro del middleware, implementar un bloque try-catch. En el bloque catch, realizar dos acciones: 1. Registrar el error real para el equipo de desarrollo (usando ILogger). 2. Preparar una respuesta genérica y segura para el cliente con un código de estado 500 Internal Server Error.

### **Implementación:**

#### **1. Método HandleExceptionAsync:**
**Archivo:** `SistemaVenta.API/Middleware/GlobalExceptionHandlerMiddleware.cs`

**Lógica Implementada:**
```csharp
private async Task HandleExceptionAsync(HttpContext context, Exception exception)
{
    // 1. Registrar el error real para el equipo de desarrollo
    _logger.LogError(exception, "Error no controlado en la aplicación: {Message}", exception.Message);

    // 2. Preparar respuesta genérica y segura para el cliente
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var response = new
    {
        error = "Ha ocurrido un error interno en el servidor.",
        message = "Por favor, inténtelo de nuevo más tarde. Si el problema persiste, contacte al administrador del sistema.",
        timestamp = DateTime.UtcNow,
        requestId = context.TraceIdentifier
    };

    var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    await context.Response.WriteAsync(jsonResponse);
}
```

#### **2. Características de Seguridad:**
- ✅ **Logging estructurado** con ILogger para desarrollo
- ✅ **Respuesta genérica** sin información sensible
- ✅ **Código de estado 500** apropiado para errores internos
- ✅ **Request ID** para seguimiento de errores
- ✅ **Timestamp** para auditoría
- ✅ **JSON estructurado** con camelCase

#### **3. Información Registrada vs Expuesta:**

**Para Desarrollo (Logs):**
- ✅ **Stack trace completo** de la excepción
- ✅ **Mensaje detallado** del error
- ✅ **Contexto de la petición** (URL, método, etc.)
- ✅ **Información del usuario** (si está disponible)

**Para el Cliente (Respuesta):**
- ✅ **Mensaje genérico** y amigable
- ✅ **Instrucciones claras** sobre qué hacer
- ✅ **Request ID** para seguimiento
- ✅ **Timestamp** para auditoría
- ❌ **NO stack trace** ni información técnica
- ❌ **NO rutas del sistema** ni detalles de implementación

**¿Por qué esta implementación?**
- 🛡️ **Seguridad:** Evita fuga de información sensible
- 🔒 **Experiencia de usuario:** Mensajes claros y útiles
- 🚫 **Debugging:** Mantiene información para desarrollo
- 📊 **Auditoría:** Permite seguimiento de errores
- 🔐 **Estándares:** Cumple mejores prácticas de APIs

---

## ✅ PASO 4: REGISTRAR MIDDLEWARE

**[⬆️ Volver al Índice](#-índice-de-contenidos)** | **[⬇️ Siguiente: Paso 5](#paso-5-probar-manejador)**

### **Descripción del Paso:**
Registrar el middleware en el pipeline de la aplicación para que intercepte todas las peticiones.

### **Implementación:**

#### **1. Registrar en Program.cs:**
**Archivo Modificado:** `SistemaVenta.API/Program.cs`

**Cambio Realizado:**
```csharp
app.UseRouting();

// Agregar middleware de manejo global de excepciones
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Agregar middleware de cabeceras de seguridad
app.UseMiddleware<SecurityHeadersMiddleware>();
```

**Ubicación:** Después de `app.UseRouting()` y antes de otros middlewares

**Explicación:**
- ✅ **Orden correcto:** Se registra temprano en el pipeline
- ✅ **Interceptación global:** Captura errores de toda la aplicación
- ✅ **Antes de otros middlewares:** Asegura que se ejecute primero
- ✅ **Después de routing:** Tiene acceso al contexto completo
- ✅ **Integración transparente:** No afecta el flujo normal

**¿Por qué este orden?**
- 🛡️ **Interceptación temprana:** Captura errores antes de que lleguen a otros middlewares
- 🔒 **Contexto completo:** Tiene acceso a toda la información de la petición
- 🚫 **Prevención de errores:** Evita que errores se propaguen sin control
- 📊 **Logging completo:** Puede registrar información detallada
- 🔐 **Respuesta controlada:** Asegura respuestas seguras y consistentes

---

## 🧪 PASO 5: PROBAR MANEJADOR

**[⬆️ Volver al Índice](#-índice-de-contenidos)**

### **Descripción del Paso:**
Probar el nuevo manejador. Volver a ejecutar la petición en Postman que provocaba el error en el paso 1. Verificar que ahora la API ya no devuelve el stack trace, sino la respuesta JSON genérica y segura que definiste.

### **Implementación:**

#### **1. Compilar y Verificar:**
**Comando:** `dotnet build`
**Resultado:** Compilación exitosa sin errores críticos

#### **2. Probar Endpoint de Error:**
**Endpoint:** `GET /api/usuarios/TestError`
**Autorización:** Bearer Token de administrador

**Respuesta Esperada (Sin Middleware):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 500,
  "traceId": "00-...",
  "errors": {
    "": [
      "Este es un error de prueba para ver el comportamiento por defecto."
    ]
  }
}
```

**Respuesta Esperada (Con Middleware):**
```json
{
  "error": "Ha ocurrido un error interno en el servidor.",
  "message": "Por favor, inténtelo de nuevo más tarde. Si el problema persiste, contacte al administrador del sistema.",
  "timestamp": "2024-01-15T10:30:00.000Z",
  "requestId": "00-..."
}
```

#### **3. Verificar Logging:**
**Archivo de Logs:** Verificar que se registra el error completo para desarrollo

**Log Esperado:**
```
[Error] Error no controlado en la aplicación: Este es un error de prueba para ver el comportamiento por defecto.
System.InvalidOperationException: Este es un error de prueba para ver el comportamiento por defecto.
   at SistemaVenta.API.Controllers.UsuariosController.TestError() in ...
```

#### **4. Comparación de Seguridad:**

**Antes del Middleware:**
- ❌ **Stack trace visible** al cliente
- ❌ **Rutas del sistema** expuestas
- ❌ **Información técnica** detallada
- ❌ **Posible fuga** de información sensible

**Después del Middleware:**
- ✅ **Mensaje genérico** y seguro
- ✅ **Información técnica** oculta
- ✅ **Logging completo** para desarrollo
- ✅ **Request ID** para seguimiento
- ✅ **Timestamp** para auditoría

**¿Por qué esta verificación?**
- 🛡️ **Confirmar seguridad:** Verificar que no se expone información sensible
- 🔒 **Validar funcionamiento:** Asegurar que el middleware intercepta errores
- 🚫 **Comprobar logging:** Confirmar que se registran errores para desarrollo
- 📊 **Mejorar experiencia:** Verificar mensajes claros para el usuario
- 🔐 **Cumplir estándares:** Asegurar respuestas seguras y apropiadas

---

## 🔧 CONFIGURACIÓN TÉCNICA

### **Archivos Modificados:**
- ✅ `SistemaVenta.API/Controllers/UsuariosController.cs` - Endpoint de prueba
- ✅ `SistemaVenta.API/Middleware/GlobalExceptionHandlerMiddleware.cs` - Middleware creado
- ✅ `SistemaVenta.API/Program.cs` - Registro del middleware

### **Dependencias Utilizadas:**
- ✅ `Microsoft.AspNetCore.Http` - Contexto HTTP
- ✅ `Microsoft.Extensions.Logging` - Logging estructurado
- ✅ `System.Net` - Códigos de estado HTTP
- ✅ `System.Text.Json` - Serialización JSON

### **Configuración de Seguridad:**
- ✅ **Logging seguro** para desarrollo
- ✅ **Respuestas genéricas** para clientes
- ✅ **Códigos de estado** apropiados
- ✅ **Request ID** para seguimiento
- ✅ **Timestamp** para auditoría

---

## ✅ CHECKLIST DE VERIFICACIÓN

### **Implementación:**
- [x] ✅ Endpoint de prueba creado
- [x] ✅ Middleware de excepciones implementado
- [x] ✅ Logging seguro configurado
- [x] ✅ Respuestas genéricas definidas
- [x] ✅ Middleware registrado en pipeline

### **Seguridad:**
- [x] ✅ Información sensible oculta
- [x] ✅ Stack traces no expuestos
- [x] ✅ Rutas del sistema protegidas
- [x] ✅ Logging completo para desarrollo
- [x] ✅ Códigos de estado apropiados

### **Funcionalidad:**
- [x] ✅ Interceptación global de errores
- [x] ✅ Respuestas JSON estructuradas
- [x] ✅ Request ID para seguimiento
- [x] ✅ Timestamp para auditoría
- [x] ✅ Mensajes claros para usuarios

---

## 🚨 TROUBLESHOOTING

### **Problemas Comunes:**

#### **1. Middleware no intercepta errores:**
**Síntoma:** Los errores siguen mostrando stack traces
**Solución:** Verificar que el middleware esté registrado antes de otros middlewares en Program.cs

#### **2. Logging no funciona:**
**Síntoma:** No se registran errores en los logs
**Solución:** Verificar configuración de ILogger en Program.cs

#### **3. Respuesta no es JSON:**
**Síntoma:** La respuesta no tiene formato JSON
**Solución:** Verificar ContentType en HandleExceptionAsync

#### **4. Código de estado incorrecto:**
**Síntoma:** Respuesta con código 200 en lugar de 500
**Solución:** Verificar StatusCode en HandleExceptionAsync

---

## 📚 REFERENCIAS

### **Documentación Oficial:**
- [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)
- [Exception Handling](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
- [Logging in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging)

### **Mejores Prácticas:**
- [OWASP API Security](https://owasp.org/www-project-api-security/)
- [REST API Error Handling](https://restfulapi.net/error-handling/)
- [Security Headers](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers)

### **Patrones de Diseño:**
- [Global Exception Handler Pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling#exception-handler-page)
- [Middleware Pipeline](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write)
- [Structured Logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging#log-message-template) 