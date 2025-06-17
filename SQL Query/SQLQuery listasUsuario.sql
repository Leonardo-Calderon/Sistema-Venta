insert into Rol(Nombre) values
('Administrador'),
('Ventas')

INSERT INTO Usuario (IdRol, NombreCompleto, Correo, NombreUsuario, Clave, ResetearClave)
VALUES (
    1,
    'Juan Pérez',
    'juan.perez@example.com',
    'jperez',
    'A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3',
    0                                       
);

Go

--Procedimientos
create procedure sp_listaRol
as
begin
	select IdRol, Nombre from Rol
end

Go

sp_helptext sp_listaCategoria

Go

create procedure sp_listaUsuario
(  
@Buscar varchar(50)=''  
)  
as  
begin  
 select 
 u.IdUsuario,
 r.IdRol,
 r.Nombre[NombreRol],
 u.NombreCompleto,
 u.Correo,
 u.NombreUsuario,
 u.Activo
 from Usuario u inner join Rol r on r.IdRol=u.IdRol
 where concat(r.Nombre,u.NombreCompleto, u.Correo, u.NombreUsuario, iif(u.Activo=1,'SI','NO')) 
 like '%'+ @Buscar +'%'  
end

Go

create procedure sp_crearUsuario(  
@IdRol int,
@NombreCompleto varchar(50),
@Correo varchar(50),
@NombreUsuario varchar(50),
@Clave varchar(100),
@MsjError varchar(100) output  
)  
as  
begin  
 set @MsjError=''  
  
 if(exists(select * from Usuario where Correo = @Correo))  
  begin  
   set @MsjError = 'El correo ya existe'  
   return  
  end  

 if(exists(select * from Usuario where NombreUsuario = @NombreUsuario))  
  begin  
   set @MsjError = 'El nombre de usuario ya existe'  
   return  
  end  

 insert into Usuario(IdRol, NombreCompleto, Correo, NombreUsuario, Clave)  
 values(@IdRol, @NombreCompleto, @Correo, @NombreUsuario, @Clave)  
end

Go

create procedure sp_editarUsuario(  
@IdUsuario int,
@IdRol int,
@NombreCompleto varchar(50),
@Correo varchar(50),
@NombreUsuario varchar(50),
@Activo int,
@MsjError varchar(100) output  
)  
as  
begin  
 set @MsjError=''  
  
 if(exists(select * from Usuario where Correo = @Correo
	and IdUsuario!=@IdUsuario
 ))  
  begin  
   set @MsjError = 'El correo ya existe'  
   return  
  end  

 if(exists(select * from Usuario where NombreUsuario = @NombreUsuario
	and IdUsuario!=@IdUsuario
 ))  
  begin  
   set @MsjError = 'El nombre de usuario ya existe'  
   return  
  end  

  update Usuario set
	IdRol = @IdRol,
	NombreCompleto = @NombreCompleto,
	Correo = @Correo,
	NombreUsuario = @NombreUsuario,
	Activo = @Activo
	where IdUsuario =@IdUsuario

end

Go

CREATE PROCEDURE sp_eliminarUsuario(
    @IdUsuario INT,
    @MsjError VARCHAR(100) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @MsjError = '';

    -- Verificar si el usuario existe
    IF NOT EXISTS (SELECT 1 FROM Usuario WHERE IdUsuario = @IdUsuario)
    BEGIN
        SET @MsjError = 'El usuario no existe';
        RETURN;
    END;

    BEGIN TRY
        DELETE FROM Usuario WHERE IdUsuario = @IdUsuario;
    END TRY
    BEGIN CATCH
        SET @MsjError = ERROR_MESSAGE();
    END CATCH;
END;

GO

select *  from Usuario

