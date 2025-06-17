select * from Producto

GO

sp_helptext 'sp_editarCategoria'

GO
-- Primer procedimiento
CREATE PROCEDURE sp_listaProducto( 
    @Buscar varchar(50) = ''  
)  
AS  
BEGIN  
    SELECT 
        p.IdProducto,
        c.IdCategoria,
        c.Nombre AS [NombreCategoria],
        p.Codigo,
        p.Descripcion,
        p.PrecioCompra,
        p.PrecioVenta,
        p.Cantidad,
        p.Activo
    FROM Producto p
    INNER JOIN Categoria c ON c.IdCategoria = p.IdCategoria
    WHERE 
        CONCAT_WS(' ', p.Codigo, p.Descripcion, c.Nombre, IIF(p.Activo=1,'SI','NO')) 
        LIKE '%' + @Buscar + '%';  
END  
GO -- Separador de lotes

-- Segundo procedimiento (nombre corregido)
CREATE PROCEDURE sp_crearProducto(  
    @IdCategoria int,
    @Codigo varchar(50),
    @Descripcion varchar(150),
    @PrecioCompra decimal(10,2),
    @PrecioVenta decimal(10,2),
    @Cantidad int,
    @MsjError varchar(100) OUTPUT  
)  
AS  
BEGIN  
    SET NOCOUNT ON;
    SET @MsjError = '';  

    IF EXISTS(SELECT 1 FROM Producto WHERE Descripcion = @Descripcion)  
    BEGIN  
        SET @MsjError = 'La descripción ya existe';  
        RETURN;  
    END  

    INSERT INTO Producto (
        IdCategoria, Codigo, Descripcion, 
        PrecioCompra, PrecioVenta, Cantidad
    )  
    VALUES (
        @IdCategoria, @Codigo, @Descripcion, 
        @PrecioCompra, @PrecioVenta, @Cantidad
    );  
END  
GO

create procedure sp_editarProducto(  
    @IdProducto int,
	@IdCategoria int,
    @Codigo varchar(50),
    @Descripcion varchar(150),
    @PrecioCompra decimal(10,2),
    @PrecioVenta decimal(10,2),
    @Cantidad int,
	@Activo int,  
    @MsjError varchar(100) OUTPUT  
)  
as  
begin  
 set @MsjError=''  
  
 if(exists(select * from Producto where Descripcion = @Descripcion
 and IdProducto != @IdProducto
 ))  
  begin  
   set @MsjError = 'La descripcion del Producto ya existe'  
   return  
  end  
  
 update Producto set  
 IdCategoria = @IdCategoria,
 Codigo = @Codigo,
 Descripcion = @Descripcion,
 PrecioCompra = @PrecioCompra,
 PrecioVenta = @PrecioVenta,
 Cantidad = @Cantidad,
 Activo = @Activo  
 where IdProducto = @IdProducto  
  
end  

sp_help sp_listaProducto