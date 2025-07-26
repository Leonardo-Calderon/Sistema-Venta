# Sistema de Ventas (SistemaVenta)

## Descripción

SistemaVenta es una solución integral de gestión de ventas e inventario que incluye múltiples aplicaciones:

- **Aplicación de Escritorio**: Desarrollada en .NET Windows Forms para uso local
- **Aplicación Web**: Desarrollada en Blazor WebAssembly para acceso desde cualquier navegador
- **API REST**: Backend en ASP.NET Core que sirve tanto a la aplicación web como a futuras integraciones

Este proyecto ofrece una solución completa para el manejo de productos, categorías, usuarios, roles, ventas y generación de reportes, facilitando la administración eficiente de un punto de venta tanto en entornos locales como en la nube.

## Arquitectura del Proyecto

El proyecto está estructurado en múltiples capas y aplicaciones:

### 📁 Estructura de Proyectos

```
SistemaVenta/
├── 📱 SVPresentation/           # Aplicación Windows Forms (.NET 8)
├── 🌐 SistemaVenta.Web/         # Aplicación Blazor WebAssembly
│   ├── SistemaVenta.Web/        # Servidor Blazor
│   └── SistemaVenta.Web.Client/ # Cliente Blazor WebAssembly
├── 🔌 SistemaVenta.API/         # API REST (.NET 8)
├── 🏗️ SVServices/              # Capa de Servicios
├── 📊 SVRepository/             # Capa de Acceso a Datos
└── 📦 Shared/                   # DTOs y Modelos Compartidos
```

### 🔧 Tecnologías Utilizadas

#### Backend y API
- **Lenguaje:** C# (.NET 8)
- **Framework:** ASP.NET Core 8
- **Autenticación:** JWT Bearer Tokens
- **Base de Datos:** SQL Server (MSSQLLocalDB)
- **Acceso a Datos:** ADO.NET con procedimientos almacenados

#### Aplicación Web (Blazor)
- **Framework:** Blazor WebAssembly
- **UI Components:** Bootstrap 5
- **Estado:** Blazored LocalStorage
- **Autenticación:** Custom Authentication State Provider

#### Aplicación de Escritorio
- **Framework:** .NET 8 Windows Forms
- **UI:** Windows Forms con componentes personalizados

#### Librerías Externas
- **ClosedXML:** Generación de reportes Excel
- **QuestPDF:** Generación de documentos PDF
- **CloudinaryDotNet:** Gestión de imágenes en la nube
- **MailKit:** Envío de correos electrónicos
- **Blazored.LocalStorage:** Almacenamiento local en el navegador

## Características Principales

### 🔐 Gestión de Usuarios y Autenticación
- Inicio de sesión seguro con JWT
- Administración de usuarios (CRUD completo)
- Sistema de roles y permisos
- Menú dinámico basado en roles
- Recuperación de contraseña vía email
- Actualización de contraseñas

### 💰 Módulo de Ventas
- Registro de nuevas ventas
- Selección de productos con búsqueda
- Cálculo automático de totales y cambio
- Generación de boletas en PDF
- Historial de ventas con filtros
- Detalle completo de ventas

### 📦 Gestión de Inventario
- Administración de productos
- Categorías y unidades de medida
- Control de stock
- Búsqueda avanzada de productos

### 📊 Reportes y Análisis
- Reportes de ventas detallados
- Exportación a Excel
- Filtros por fecha y criterios
- Visualización de estadísticas

### ⚙️ Configuración del Negocio
- Datos del negocio (Razón Social, RFC, etc.)
- Gestión del logo con Cloudinary
- Configuración de moneda
- Personalización de la aplicación

## Instalación y Configuración

### Prerrequisitos
- .NET 8 SDK
- SQL Server (LocalDB recomendado para desarrollo)
- Visual Studio 2022 o VS Code
- (Opcional) Cuenta de Cloudinary para imágenes
- (Opcional) Servidor SMTP para correos

### 1. Clonar el Repositorio
```bash
git clone https://github.com/Leonardo-Calderon/Sistema-Venta.git
cd SistemaVenta
```

### 2. Configuración de la Base de Datos
1. Asegúrate de tener SQL Server LocalDB instalado
2. Crea la base de datos `DBTienda`
3. Ejecuta el script `SQL Query/tablas_base_de_datos.sql`
4. Verifica la cadena de conexión en `appsettings.json`:
```json
"ConnectionStrings": {
  "cadenaSQL": "Server=(localdb)\\MSSQLLocalDB; Database=DBTienda; Trusted_Connection=True; TrustServerCertificate=True;"
}
```

### 3. Configuración de Servicios Externos (Opcional)
Actualiza las credenciales en `appsettings.json`:

```json
{
  "Cloudinary": {
    "CloudName": "TU_CLOUD_NAME",
    "ApiKey": "TU_API_KEY",
    "ApiSecret": "TU_API_SECRET"
  },
  "Smtp": {
    "Host": "TU_SMTP_HOST",
    "Port": "TU_SMTP_PORT",
    "User": "TU_SMTP_USER",
    "Pass": "TU_SMTP_PASS"
  },
  "Jwt": {
    "Key": "TU_JWT_SECRET_KEY_MUY_LARGA_Y_SEGURA",
    "Issuer": "SistemaVenta",
    "Audience": "SistemaVentaUsers"
  }
}
```

### 4. Ejecutar las Aplicaciones

#### Aplicación Web (Recomendada)
```bash
# Ejecutar la API
cd SistemaVenta.API
dotnet run

# En otra terminal, ejecutar la aplicación web
cd SistemaVenta.Web/SistemaVenta.Web
dotnet run
```
Accede a: `https://localhost:7289`

#### Aplicación de Escritorio
```bash
cd SVPresentation
dotnet run
```

## Uso de las Aplicaciones

### 🌐 Aplicación Web (Blazor)
- **Acceso:** Cualquier navegador moderno
- **Ventajas:** Acceso remoto, actualizaciones automáticas, multiplataforma
- **Ideal para:** Múltiples usuarios, acceso desde diferentes dispositivos

### 📱 Aplicación de Escritorio (Windows Forms)
- **Acceso:** Solo Windows
- **Ventajas:** Rendimiento nativo, integración con sistema operativo
- **Ideal para:** Uso local, entornos sin conexión estable

## API Endpoints

La API REST incluye endpoints para:

- **Autenticación:** `/api/auth/login`, `/api/auth/register`
- **Usuarios:** `/api/usuarios/*`
- **Productos:** `/api/productos/*`
- **Categorías:** `/api/categorias/*`
- **Ventas:** `/api/ventas/*`
- **Reportes:** `/api/reportes/*`

Documentación completa disponible en Swagger: `https://localhost:7001/swagger`

## Estado del Proyecto

- ✅ **Versión 1.0 Completada:** Funcional para producción
- ✅ **Aplicación Web:** Blazor WebAssembly implementada
- ✅ **API REST:** Backend completo con autenticación JWT
- ✅ **Aplicación de Escritorio:** Windows Forms funcional
- 🔄 **Mejoras Continuas:** Nuevas características en desarrollo

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## Contacto

- **Desarrollador:** Leonardo Calderón
- **GitHub:** [@Leonardo-Calderon](https://github.com/Leonardo-Calderon)
- **Proyecto:** [Sistema-Venta](https://github.com/Leonardo-Calderon/Sistema-Venta)

