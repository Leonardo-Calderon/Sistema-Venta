INSERT INTO Negocio (
    RazonSocial,
    RFC,
    Direccion,
    Celular,
    Correo,
    SimboloMoneda,
    NombreLogo,
    URL
) VALUES (
    'Cyber Solutions S.A. de C.V.',
    'CYS670401HQA',  -- 3 letras + 670401 (01/04/1967) + HQA
    'Blvd. Universidad 567, Monterrey',
    '8187654321',
    'soporte@cybersolutions.mx',
    'USD',
    'cyber_logo.jpg',
    'https://cybersolutions.mx/logo.jpg'
);
GO

CREATE PROCEDURE sp_obtenerNegocio
as
begin
	select RazonSocial, RFC,Direccion, Celular, Correo, SimboloMoneda,NombreLogo,URL from Negocio
	where IdNegocio=1
end
 exec sp_obtenerNegocio
 sp_help Negocio
GO

create procedure sp_editarNegocio
(
	@RazonSocial varchar(100),
	@RFC varchar(12),
	@Direccion varchar(100),
	@Celular varchar(10),
	@Correo varchar(30),
	@SimboloMoneda varchar(5),
	@NombreLogo varchar(100),
	@URL varchar(200)
)
as 
begin
	update Negocio set
	RazonSocial = @RazonSocial,
	RFC = @RFC,
	Direccion = @Direccion,
	Celular = @Celular,
	Correo = @Correo,
	SimboloMoneda = @SimboloMoneda,
	NombreLogo = @NombreLogo,
	URL = @URL
	WHERE IdNegocio =1
end

GO

