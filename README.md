# Sistema de Ventas (SistemaVenta)

**Fecha:** 28 de julio del 2025  
**Autor:** José Leonardo Rafael Calderón Gallegos  
**Versión:** 2.0.0  
**Estado:** Activo y en desarrollo

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Blazor](https://img.shields.io/badge/Blazor-5C2D91?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## 📋 Tabla de Contenidos

- [🎯 Descripción del Proyecto](#-descripción-del-proyecto)
- [🏗️ Arquitectura del Sistema](#️-arquitectura-del-sistema)
- [✨ Características Principales](#-características-principales)
- [🛡️ Seguridad Implementada](#️-seguridad-implementada)
- [🚀 Instalación y Configuración](#-instalación-y-configuración)
- [⚙️ Configuración del Entorno](#️-configuración-del-entorno)
- [🔧 Uso y Funcionalidades](#-uso-y-funcionalidades)
- [📊 API Endpoints](#-api-endpoints)
- [🧪 Pruebas y Verificación](#-pruebas-y-verificación)
- [📁 Estructura del Proyecto](#-estructura-del-proyecto)
- [🛠️ Tecnologías Utilizadas](#️-tecnologías-utilizadas)
- [📈 Estado del Proyecto](#-estado-del-proyecto)
- [🤝 Contribución](#-contribución)
- [📄 Licencia](#-licencia)
- [📞 Contacto](#-contacto)

---

## 🎯 Descripción del Proyecto

**SistemaVenta** es una solución integral de gestión de ventas e inventario que combina múltiples tecnologías para ofrecer una experiencia completa tanto para usuarios locales como remotos. El sistema está diseñado con una arquitectura moderna y escalable que permite el manejo eficiente de operaciones comerciales.

### 🎯 Objetivos del Proyecto

- **Gestión Integral:** Manejo completo de productos, categorías, usuarios, roles, ventas y reportes
- **Multiplataforma:** Aplicación de escritorio para uso local y aplicación web para acceso remoto
- **Seguridad Robusta:** Implementación de múltiples capas de seguridad y autenticación
- **Escalabilidad:** Arquitectura modular que permite crecimiento y nuevas funcionalidades
- **Experiencia de Usuario:** Interfaces intuitivas y responsivas para diferentes dispositivos

---

## 🏗️ Arquitectura del Sistema

El proyecto implementa una arquitectura de **N-Capas** con separación clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────────┐
│                    ARQUITECTURA DEL SISTEMA                 │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────────┐    ┌─────────────────┐                │
│  │   PRESENTACIÓN  │    │   PRESENTACIÓN  │                │
│  │   Windows Forms │    │   Blazor Web    │                │
│  └─────────────────┘    └─────────────────┘                │
│           │                       │                        │
│           └───────────────────────┼────────────────────────┘
│                                   │                        │
│  ┌─────────────────────────────────┼────────────────────────┐
│  │           API REST              │                        │
│  │      (ASP.NET Core 8)           │                        │
│  └─────────────────────────────────┘                        │
│                                   │                        │
│  ┌─────────────────────────────────┼────────────────────────┐
│  │           SERVICIOS             │                        │
│  │      (Lógica de Negocio)        │                        │
│  └─────────────────────────────────┘                        │
│                                   │                        │
│  ┌─────────────────────────────────┼────────────────────────┐
│  │          REPOSITORIO            │                        │
│  │      (Acceso a Datos)           │                        │
│  └─────────────────────────────────┘                        │
│                                   │                        │
│  ┌─────────────────────────────────┼────────────────────────┐
│  │         BASE DE DATOS           │                        │
│  │      (SQL Server LocalDB)       │                        │
│  └─────────────────────────────────┘                        │
└─────────────────────────────────────────────────────────────┘
```

### 📁 Estructura de Proyectos

```
SistemaVenta/
├── 📱 SVPresentation/                    # Aplicación Windows Forms (.NET 8)
│   ├── Forms/                           # Formularios de la aplicación
│   ├── ViewModels/                      # Modelos de vista
│   ├── Utilidades/                      # Utilidades y componentes personalizados
│   └── Resources/                       # Recursos de la aplicación
│
├── 🌐 SistemaVenta.Web/                 # Aplicación Blazor WebAssembly
│   ├── SistemaVenta.Web/                # Servidor Blazor
│   │   ├── Pages/                       # Páginas del servidor
│   │   └── Components/                  # Componentes compartidos
│   └── SistemaVenta.Web.Client/         # Cliente Blazor WebAssembly
│       ├── Pages/                       # Páginas de la aplicación web
│       ├── Services/                    # Servicios de comunicación con API
│       ├── Auth/                        # Autenticación y autorización
│       └── Layout/                      # Layouts y navegación
│
├── 🔌 SistemaVenta.API/                 # API REST (.NET 8)
│   ├── Controllers/                     # Controladores de la API
│   ├── Middleware/                      # Middlewares personalizados
│   ├── Utilidades/                      # Utilidades de la API
│   └── Properties/                      # Configuración de la aplicación
│
├── 🏗️ SVServices/                       # Capa de Servicios
│   ├── Interfaces/                      # Interfaces de servicios
│   ├── Implementation/                  # Implementaciones de servicios
│   └── DependencyInjection.cs           # Configuración de DI
│
├── 📊 SVRepository/                     # Capa de Acceso a Datos
│   ├── Interfaces/                      # Interfaces de repositorio
│   ├── Implementation/                  # Implementaciones de repositorio
│   ├── Entities/                        # Entidades del dominio
│   └── DB/                              # Configuración de base de datos
│
├── 📦 Shared/                           # DTOs y Modelos Compartidos
│   └── DTOs/                            # Objetos de transferencia de datos
│
├── 📄 SQL Query/                        # Scripts de base de datos
│   ├── tablas_base_de_datos.sql         # Estructura de tablas
│   └── SQLQuery *.sql                   # Procedimientos almacenados
│
└── 📚 Documentación/                    # Documentación del proyecto
    ├── IMPLEMENTACION*.md               # Documentos de implementación
    └── README.md                        # Este archivo
```

---

## ✨ Características Principales

### 🔐 Gestión de Usuarios y Autenticación
- **Autenticación JWT:** Tokens seguros con expiración configurable
- **Sistema de Roles:** Administrador y Vendedor con permisos específicos
- **Gestión de Usuarios:** CRUD completo con validaciones
- **Recuperación de Contraseña:** Sistema de recuperación vía email
- **Menú Dinámico:** Navegación basada en roles del usuario
- **Auditoría Completa:** Registro de todas las actividades del sistema

### 💰 Módulo de Ventas
- **Registro de Ventas:** Interfaz intuitiva para captura de ventas
- **Búsqueda de Productos:** Búsqueda en tiempo real con filtros
- **Cálculo Automático:** Totales, impuestos y cambio automático
- **Generación de Boletas:** PDF profesional con datos del negocio
- **Historial de Ventas:** Consulta con filtros por fecha y criterios
- **Detalle de Ventas:** Información completa de cada transacción
- **Reportes Excel:** Exportación de datos para análisis

### 📦 Gestión de Inventario
- **Administración de Productos:** CRUD completo con imágenes
- **Categorías:** Organización jerárquica de productos
- **Unidades de Medida:** Configuración flexible de unidades
- **Control de Stock:** Seguimiento de inventario en tiempo real
- **Búsqueda Avanzada:** Filtros múltiples y búsqueda por código
- **Gestión de Imágenes:** Almacenamiento en la nube con Cloudinary

### 📊 Reportes y Análisis
- **Reportes de Ventas:** Análisis detallado por períodos
- **Exportación a Excel:** Reportes personalizables
- **Filtros Avanzados:** Por fecha, usuario, producto, etc.
- **Estadísticas Visuales:** Gráficos y métricas de rendimiento
- **Reportes PDF:** Documentos profesionales para impresión

### ⚙️ Configuración del Negocio
- **Datos del Negocio:** Razón social, RFC, dirección, etc.
- **Gestión de Logo:** Carga y almacenamiento en la nube
- **Configuración de Moneda:** Personalización de formato monetario
- **Configuración SMTP:** Envío de correos electrónicos
- **Backup Automático:** Respaldo de configuración

---

## 🛡️ Seguridad Implementada

### 🔐 Autenticación y Autorización
- **JWT Tokens:** Autenticación segura con expiración
- **Autorización por Roles:** Control granular de acceso
- **Verificación de Propiedad:** Usuarios solo acceden a sus recursos
- **Middleware de Auditoría:** Registro automático de actividades

### 🛡️ Protección de Datos
- **Secret Manager:** Credenciales externalizadas del código
- **Validación de Entrada:** Prevención de SQL Injection
- **Sanitización de Datos:** Limpieza de entradas del usuario
- **CSRF Protection:** Tokens anti-falsificación
- **HTTP Security Headers:** Protección contra XSS y Clickjacking

### 🔒 Gestión de Errores
- **Global Exception Handler:** Manejo centralizado de errores
- **Logging Seguro:** Registro sin exposición de información sensible
- **Respuestas Genéricas:** Mensajes de error seguros para el cliente
- **Auditoría de Errores:** Seguimiento de problemas del sistema

---

## 🚀 Instalación y Configuración

### 📋 Prerrequisitos

#### Software Requerido
- **.NET 8 SDK** ([Descargar](https://dotnet.microsoft.com/download/dotnet/8.0))
- **SQL Server LocalDB** ([Descargar](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb))
- **Visual Studio 2022** o **VS Code** ([Descargar](https://visualstudio.microsoft.com/))

#### Servicios Opcionales
- **Cuenta de Cloudinary** para gestión de imágenes ([Registrarse](https://cloudinary.com/))
- **Servidor SMTP** para envío de correos (Gmail, Outlook, etc.)

### 🔧 Instalación Paso a Paso

#### 1. Clonar el Repositorio
```bash
git clone https://github.com/tu-usuario/SistemaVenta.git
cd SistemaVenta
```

#### 2. Configurar la Base de Datos
```bash
# Ejecutar el script de creación de base de datos
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "SQL Query/tablas_base_de_datos.sql"

# Ejecutar los procedimientos almacenados
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "SQL Query/SQLQuery login.sql"
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "SQL Query/SQLQuery listas.sql"
# ... (ejecutar todos los archivos .sql en la carpeta SQL Query)
```

#### 3. Configurar Secret Manager
```bash
# Navegar al proyecto API
cd SistemaVenta.API

# Inicializar Secret Manager
dotnet user-secrets init

# Configurar secretos (reemplazar con tus valores)
dotnet user-secrets set "ConnectionStrings:cadenaSQL" "Server=(localdb)\MSSQLLocalDB; Database=DBTienda; Trusted_Connection=True; TrustServerCertificate=True;"
dotnet user-secrets set "Jwt:Key" "TU_LLAVE_SECRETA_MUY_LARGA_Y_SEGURA_12345"
dotnet user-secrets set "Jwt:Issuer" "SistemaVenta"
dotnet user-secrets set "Jwt:Audience" "SistemaVenta"
dotnet user-secrets set "Cloudinary:CloudName" "tu-cloud-name"
dotnet user-secrets set "Cloudinary:ApiKey" "tu-api-key"
dotnet user-secrets set "Cloudinary:ApiSecret" "tu-api-secret"
dotnet user-secrets set "Smtp:Host" "smtp.gmail.com"
dotnet user-secrets set "Smtp:Port" "587"
dotnet user-secrets set "Smtp:User" "tu-email@gmail.com"
dotnet user-secrets set "Smtp:Pass" "tu-contraseña-de-aplicación"
```

#### 4. Restaurar Dependencias
```bash
# Desde la raíz del proyecto
dotnet restore
```

#### 5. Compilar el Proyecto
```bash
dotnet build
```

---

## ⚙️ Configuración del Entorno

### 🔧 Configuración de Desarrollo

#### Variables de Entorno
```bash
# Crear archivo .env en la raíz del proyecto (opcional)
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:7206;http://localhost:5288
```

#### Configuración de Base de Datos
```json
// appsettings.Development.json
{
  "ConnectionStrings": {
    "cadenaSQL": "Server=(localdb)\\MSSQLLocalDB; Database=DBTienda; Trusted_Connection=True; TrustServerCertificate=True;"
  }
}
```

### 🌐 Configuración de Producción

#### Variables de Entorno
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://tu-dominio.com
```

#### Configuración de Servicios
- **Base de Datos:** SQL Server en la nube (Azure, AWS, etc.)
- **Almacenamiento:** Cloudinary para imágenes
- **Email:** Servidor SMTP de producción
- **SSL:** Certificado SSL válido

---

## 🔧 Uso y Funcionalidades

### 🖥️ Aplicación de Escritorio (Windows Forms)

#### Inicio de Sesión
1. Ejecutar `SVPresentation.exe`
2. Ingresar credenciales de usuario
3. Seleccionar rol (Administrador/Vendedor)

#### Funcionalidades Principales
- **Gestión de Ventas:** Registro rápido de transacciones
- **Inventario:** Administración de productos y categorías
- **Usuarios:** Gestión de cuentas y permisos
- **Reportes:** Generación de reportes en PDF y Excel

### 🌐 Aplicación Web (Blazor WebAssembly)

#### Acceso
1. Abrir navegador en `https://localhost:7206`
2. Iniciar sesión con credenciales
3. Navegar por el menú dinámico

#### Funcionalidades Web
- **Dashboard:** Vista general del negocio
- **Ventas:** Registro y consulta de transacciones
- **Productos:** Gestión completa del inventario
- **Usuarios:** Administración de cuentas
- **Reportes:** Análisis y exportación de datos

### 🔌 API REST

#### Autenticación
```bash
# Obtener token JWT
curl -X POST "https://localhost:7206/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"correo":"admin@sistema.com","clave":"123456"}'
```

#### Uso de Endpoints
```bash
# Ejemplo: Obtener lista de productos
curl -X GET "https://localhost:7206/api/productos" \
  -H "Authorization: Bearer TU_TOKEN_JWT"
```

---

## 📊 API Endpoints

### 🔐 Autenticación
- `POST /api/auth/login` - Inicio de sesión
- `POST /api/auth/forgot-password` - Recuperar contraseña
- `POST /api/auth/reset-password` - Restablecer contraseña

### 👥 Usuarios
- `GET /api/usuarios` - Listar usuarios
- `POST /api/usuarios` - Crear usuario
- `PUT /api/usuarios/{id}` - Actualizar usuario
- `DELETE /api/usuarios/{id}` - Eliminar usuario
- `GET /api/usuarios/perfil` - Obtener perfil actual

### 📦 Productos
- `GET /api/productos` - Listar productos
- `POST /api/productos` - Crear producto
- `PUT /api/productos` - Actualizar producto
- `DELETE /api/productos/{id}` - Eliminar producto
- `GET /api/productos/search` - Búsqueda de productos

### 💰 Ventas
- `POST /api/ventas/registrar` - Registrar venta
- `GET /api/ventas/historial` - Historial de ventas
- `GET /api/ventas/{numeroVenta}` - Obtener venta específica
- `GET /api/ventas/detalle/{numeroVenta}` - Detalle de venta
- `POST /api/ventas/generarreporteexcel` - Generar reporte Excel

### 📊 Reportes
- `GET /api/ventas/reporte` - Reporte de ventas
- `GET /api/ventas/generarpdf/{numeroVenta}` - Generar PDF de venta

### ⚙️ Configuración
- `GET /api/negocio` - Obtener datos del negocio
- `PUT /api/negocio` - Actualizar datos del negocio
- `GET /api/categorias` - Listar categorías
- `GET /api/medidas` - Listar unidades de medida

### 📈 Auditoría
- `GET /api/auditoria/historial` - Historial de auditoría
- `GET /api/auditoria/estadisticas` - Estadísticas de auditoría
- `GET /api/auditoria/mi-actividad` - Actividad del usuario actual

---

## 🧪 Pruebas y Verificación

### 🔍 Verificación de Instalación

#### 1. Verificar Compilación
```bash
dotnet build
# Debe mostrar: "Compilación realizado correctamente"
```

#### 2. Verificar Base de Datos
```bash
# Conectar a SQL Server LocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB"
# Verificar que existe la base de datos DBTienda
```

#### 3. Verificar Secret Manager
```bash
cd SistemaVenta.API
dotnet user-secrets list
# Debe mostrar todos los secretos configurados
```

#### 4. Probar API
```bash
# Iniciar la API
cd SistemaVenta.API
dotnet run

# En otro terminal, probar endpoint de salud
curl -X GET "https://localhost:7206/api/usuarios"
```

### 🧪 Pruebas de Seguridad

#### Pruebas de Autenticación
```bash
# Probar login sin credenciales
curl -X POST "https://localhost:7206/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{}'
# Debe retornar 400 Bad Request

# Probar login con credenciales válidas
curl -X POST "https://localhost:7206/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"correo":"admin@sistema.com","clave":"123456"}'
# Debe retornar 200 OK con token JWT
```

#### Pruebas de Autorización
```bash
# Probar endpoint sin token
curl -X GET "https://localhost:7206/api/usuarios"
# Debe retornar 401 Unauthorized

# Probar endpoint con token válido
curl -X GET "https://localhost:7206/api/usuarios" \
  -H "Authorization: Bearer TU_TOKEN_JWT"
# Debe retornar 200 OK con lista de usuarios
```

### 🔧 Pruebas de Funcionalidad

#### Pruebas de Ventas
1. Iniciar aplicación de escritorio
2. Crear una venta de prueba
3. Verificar que se genera el PDF
4. Verificar que aparece en el historial

#### Pruebas Web
1. Abrir aplicación web en navegador
2. Iniciar sesión
3. Navegar por todas las secciones
4. Verificar funcionalidad de búsqueda
5. Probar generación de reportes

---

## 🛠️ Tecnologías Utilizadas

### 🎯 Backend y API
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **.NET 8** | 8.0 | Framework principal |
| **ASP.NET Core** | 8.0 | API REST |
| **C#** | 12.0 | Lenguaje de programación |
| **Entity Framework** | 8.0 | ORM (opcional) |
| **ADO.NET** | 8.0 | Acceso a datos |
| **JWT** | - | Autenticación |
| **AutoMapper** | 12.0 | Mapeo de objetos |

### 🌐 Frontend Web
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **Blazor WebAssembly** | 8.0 | Framework web |
| **Bootstrap** | 5.3 | Framework CSS |
| **JavaScript** | ES2020 | Interactividad |
| **Blazored.LocalStorage** | 4.4 | Almacenamiento local |

### 🖥️ Aplicación de Escritorio
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **Windows Forms** | 8.0 | UI de escritorio |
| **.NET 8** | 8.0 | Framework |
| **Custom Controls** | - | Componentes personalizados |

### 🗄️ Base de Datos
| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **SQL Server** | 2022 | Base de datos principal |
| **LocalDB** | 2022 | Base de datos local |
| **Stored Procedures** | - | Lógica de negocio en BD |

### 📦 Librerías Externas
| Librería | Versión | Propósito |
|----------|---------|-----------|
| **ClosedXML** | 0.102 | Generación de Excel |
| **QuestPDF** | 2023.12 | Generación de PDF |
| **CloudinaryDotNet** | 1.25 | Gestión de imágenes |
| **MailKit** | 4.3 | Envío de emails |
| **System.Drawing.Common** | 8.0 | Manipulación de imágenes |

### 🛡️ Seguridad
| Tecnología | Propósito |
|------------|-----------|
| **JWT Bearer Tokens** | Autenticación |
| **Role-based Authorization** | Autorización |
| **CSRF Protection** | Prevención de ataques |
| **SQL Injection Prevention** | Seguridad de datos |
| **HTTP Security Headers** | Protección del navegador |
| **Secret Manager** | Gestión de credenciales |

---

## 📈 Estado del Proyecto

### ✅ Funcionalidades Completadas
- [x] Sistema de autenticación y autorización
- [x] Gestión completa de usuarios y roles
- [x] Módulo de ventas con PDF y Excel
- [x] Gestión de inventario y productos
- [x] Sistema de reportes y análisis
- [x] Configuración del negocio
- [x] Aplicación web Blazor
- [x] API REST completa
- [x] Aplicación de escritorio Windows Forms
- [x] Sistema de auditoría
- [x] Protección de seguridad completa

### 🚧 Funcionalidades en Desarrollo
- [ ] Dashboard con gráficos en tiempo real
- [ ] Sistema de notificaciones push
- [ ] Integración con pasarelas de pago
- [ ] App móvil (React Native)
- [ ] Sistema de backup automático
- [ ] Integración con contabilidad

### 📋 Próximas Funcionalidades
- [ ] Multi-tenancy (múltiples negocios)
- [ ] Sistema de inventario avanzado
- [ ] Integración con proveedores
- [ ] Sistema de fidelización
- [ ] Análisis predictivo de ventas
- [ ] Integración con redes sociales

---

## 🤝 Contribución

### 📝 Cómo Contribuir

1. **Fork del Proyecto**
   ```bash
   git clone https://github.com/tu-usuario/SistemaVenta.git
   cd SistemaVenta
   ```

2. **Crear Rama de Desarrollo**
   ```bash
   git checkout -b feature/nueva-funcionalidad
   ```

3. **Realizar Cambios**
   - Seguir las convenciones de código
   - Agregar pruebas unitarias
   - Actualizar documentación

4. **Commit y Push**
   ```bash
   git add .
   git commit -m "feat: agregar nueva funcionalidad"
   git push origin feature/nueva-funcionalidad
   ```

5. **Crear Pull Request**
   - Describir los cambios realizados
   - Incluir capturas de pantalla si aplica
   - Referenciar issues relacionados

### 📋 Convenciones de Código

#### C# (.NET)
```csharp
// Nombres de clases: PascalCase
public class UsuarioService
{
    // Nombres de métodos: PascalCase
    public async Task<Usuario> ObtenerPorId(int id)
    {
        // Variables locales: camelCase
        var usuario = await _repository.ObtenerPorId(id);
        return usuario;
    }
}
```

#### JavaScript/TypeScript
```javascript
// Nombres de funciones: camelCase
function obtenerUsuario(id) {
    // Variables: camelCase
    const usuario = fetch(`/api/usuarios/${id}`);
    return usuario;
}
```

#### CSS/SCSS
```css
/* Clases: kebab-case */
.usuario-card {
    /* Propiedades: camelCase */
    background-color: #ffffff;
}
```

### 🧪 Pruebas

#### Pruebas Unitarias
```bash
# Ejecutar pruebas unitarias
dotnet test

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

#### Pruebas de Integración
```bash
# Ejecutar pruebas de API
dotnet test SistemaVenta.API.Tests
```

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

```
MIT License

Copyright (c) 2025 José Leonardo Rafael Calderón Gallegos

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 📞 Contacto

### 👨‍💻 Desarrollador
- **Nombre:** José Leonardo Rafael Calderón Gallegos
- **Email:** leo_cg2000@hotmail.com


</div>

