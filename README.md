# Sistema de Ventas (SistemaVenta)

## Descripción

SistemaVenta es una aplicación de escritorio robusta desarrollada en .NET Windows Forms, diseñada para gestionar las operaciones de venta e inventario de un negocio. Este proyecto busca ofrecer una solución integral para el manejo de productos, categorías, usuarios, roles, y la generación de reportes de ventas, facilitando la administración eficiente de un punto de venta.

## Capturas de Pantalla / GIFs

<img width="622" alt="image" src="https://github.com/user-attachments/assets/fb3a91cc-c3e7-4436-a99e-5988c95a3039" />

<img width="715" alt="image" src="https://github.com/user-attachments/assets/5cb9af5b-c2e5-4c7a-b395-16a8b7a677a0" />

<img width="721" alt="image" src="https://github.com/user-attachments/assets/e2d3ace5-4e0a-47cb-8a92-da7588a3169e" />

<img width="720" alt="image" src="https://github.com/user-attachments/assets/2ed77bab-c5e1-4b47-bd01-8521a6454821" />

<img width="721" alt="image" src="https://github.com/user-attachments/assets/59043b71-c54b-40fa-955b-4556410723ec" />


**Ejemplos de lo que podrías mostrar:**
* Ventana de Inicio de Sesión
* Panel Principal con Menú de Navegación
* Formulario de Registro de Nueva Venta
* Módulo de Gestión de Productos
* Módulo de Gestión de Usuarios
* Ejemplo de un Reporte de Ventas en PDF o Excel

## Tecnologías Utilizadas

Este proyecto fue construido utilizando las siguientes tecnologías y librerías:

* **Lenguaje de Programación:** C#
* **Framework:** .NET 8 (para Windows Desktop)
* **Interfaz de Usuario:** Windows Forms
* **Arquitectura:** Aplicación de N-Capas (Presentation, Services, Repository)
* **Base de Datos:**
    * SQL Server (configurado para MSSQLLocalDB con una base de datos llamada `DBTienda`)
    * Acceso a datos mediante ADO.NET con procedimientos almacenados (inferido de `Microsoft.Data.SqlClient` y la estructura de los repositorios).
* **Gestión de Dependencias y Configuración:**
    * `Microsoft.Extensions.Hosting` para Inyección de Dependencias y Configuración.
    * `appsettings.json` para la configuración de la aplicación.
* **Librerías Externas:**
    * **ClosedXML:** Para la generación de reportes en formato Excel.
    * **QuestPDF:** Para la generación de documentos PDF (probablemente para boletas de venta o reportes).
    * **CloudinaryDotNet:** Para la gestión y almacenamiento de imágenes en la nube (ej. logo del negocio, imágenes de productos).
    * **MailKit:** Para el envío de correos electrónicos (ej. recuperación de contraseñas).

## Características Principales

* **Gestión de Usuarios y Roles:**
    * Inicio de sesión seguro para usuarios.
    * Administración de usuarios (crear, editar, habilitar/deshabilitar).
    * Menú dinámico basado en el rol del usuario.
    * Funcionalidad para recuperar contraseña olvidada vía correo electrónico.
    * Opción para que el usuario actualice su contraseña si se ha reseteado.
* **Módulo de Ventas:**
    * Registro de nuevas ventas, incluyendo selección de productos y cálculo de totales.
    * Cálculo de pago y cambio.
    * Generación de boleta de venta en formato PDF.
    * Historial de ventas con filtros por fecha y término de búsqueda.
    * Visualización del detalle de ventas pasadas.
* **Gestión de Inventario:**
    * Administración de productos: creación, edición, búsqueda.
    * Manejo de categorías de productos, asociadas a unidades de medida.
    * Control de stock de productos.
* **Reportes:**
    * Generación de reportes de ventas detallados.
    * Exportación de reportes a formato Excel.
* **Configuración del Negocio:**
    * Permite configurar los datos del negocio (Razón Social, RFC, Dirección, Celular, Correo).
    * Gestión del logo del negocio, con carga y almacenamiento en Cloudinary.
    * Configuración del símbolo de moneda.

## Instalación y Uso

Para ejecutar este proyecto localmente, sigue estos pasos:

1.  **Prerrequisitos:**
    * .NET 8 Desktop Runtime o SDK.
    * SQL Server (se puede usar LocalDB, que es la configuración por defecto).
    * (Opcional) Una cuenta en Cloudinary si deseas probar la funcionalidad de carga de logos.
    * (Opcional) Configurar un servidor SMTP (como el de Ethereal Email que está en `appsettings.json` para pruebas) si deseas probar el envío de correos.

2.  **Clonar el Repositorio:**
    ```bash
    git clone [https://github.com/Leonardo-Calderon/Sistema-Venta.git](https://github.com/Leonardo-Calderon/Sistema-Venta.git)
    cd SistemaVenta
    ```

3.  **Configuración de la Base de Datos:**
    * Asegúrate de tener una instancia de SQL Server accesible.
    * La cadena de conexión por defecto en `SVPresentation/appsettings.json` apunta a `(localdb)\\MSSQLLocalDB` y una base de datos llamada `DBTienda`.
        ```json
        "ConnectionStrings": {
          "cadenaSQL": "Server=(localdb)\\MSSQLLocalDB; Database=DBTienda; Trusted_Connection=True; TrustServerCertificate=True;"
        }
        ```
    * Crea la base de datos `DBTienda` en tu instancia de SQL Server.
    * *(Opcional: Si tienes un script SQL para la creación de tablas y datos iniciales, menciónalo aquí y añádelo al repositorio).*

4.  **Configuración de Servicios Externos (Opcional):**
    * Si deseas utilizar las funcionalidades completas de carga de imágenes y envío de correos, actualiza las credenciales en el archivo `SVPresentation/appsettings.json`:
        * **Cloudinary:**
            ```json
            "Cloudinary": {
              "CloudName": "TU_CLOUD_NAME",
              "ApiKey": "TU_API_KEY",
              "ApiSecret": "TU_API_SECRET"
            }
            ```
        * **SMTP (para correos):**
            ```json
            "Smtp": {
              "Host": "TU_SMTP_HOST",
              "Port": "TU_SMTP_PORT",
              "User": "TU_SMTP_USER",
              "Pass": "TU_SMTP_PASS"
            }
            ```

5.  **Ejecutar la Aplicación:**
    * Abre la solución en Visual Studio (o tu IDE preferido para .NET).
    * Establece `SVPresentation` como proyecto de inicio.
    * Compila y ejecuta el proyecto. El ejecutable se encontrará en `SistemaVenta/SVPresentation/bin/Debug/net8.0-windows/SVPresentation.exe`.

## Estado del Proyecto

* **Versión 1.0 Completada:** Funcional para demostración.

