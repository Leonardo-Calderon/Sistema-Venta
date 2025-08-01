-- Script de verificación de base de datos para SistemaVenta
-- Ejecutar en SQL Server Management Studio o Azure Data Studio

USE DBTienda;
GO

-- 1. Verificar que las tablas existan
SELECT 'Tabla Usuario' as Tabla, COUNT(*) as Registros FROM Usuario
UNION ALL
SELECT 'Tabla Rol' as Tabla, COUNT(*) as Registros FROM Rol
UNION ALL
SELECT 'Tabla Categoria' as Tabla, COUNT(*) as Registros FROM Categoria
UNION ALL
SELECT 'Tabla Producto' as Tabla, COUNT(*) as Registros FROM Producto;
GO

-- 2. Verificar que el procedimiento sp_login exista
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_login')
    PRINT '✓ Procedimiento sp_login existe'
ELSE
    PRINT '✗ Procedimiento sp_login NO existe'
GO

-- 3. Verificar usuarios existentes
SELECT 
    IdUsuario,
    NombreUsuario,
    NombreCompleto,
    Correo,
    Activo,
    r.Nombre as Rol
FROM Usuario u
INNER JOIN Rol r ON u.IdRol = r.IdRol
ORDER BY IdUsuario;
GO

-- 4. Verificar roles existentes
SELECT * FROM Rol ORDER BY IdRol;
GO

-- 5. Crear un usuario de prueba si no existe
IF NOT EXISTS (SELECT * FROM Usuario WHERE NombreUsuario = 'admin')
BEGIN
    INSERT INTO Usuario (IdRol, NombreCompleto, Correo, NombreUsuario, Clave, Activo, ResetearClave)
    VALUES (1, 'Administrador del Sistema', 'admin@sistemaventa.com', 'admin', 
            '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1, 0);
    PRINT '✓ Usuario admin creado (contraseña: admin)'
END
ELSE
    PRINT '✓ Usuario admin ya existe'
GO

-- 6. Verificar la estructura del procedimiento sp_login
EXEC sp_helptext 'sp_login';
GO 