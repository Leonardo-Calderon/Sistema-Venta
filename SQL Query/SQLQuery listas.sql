use DBTienda

select * from Medida

insert into Medida(Nombre,Abreviatura,Equivalente,valor)
values
('Unidad','ud','ud',1),
('Kilogramo','Kkg','g',1000)

--

create procedure sp_listaMedida
as
begin
	select * from Medida
end

create procedure sp_listaCategoria
(
@Buscar varchar(50)=''
)
as
begin
	select c.IdCategoria, c.Nombre, m.IdMedida, m.Nombre[NombreMedida], c.Activo from Categoria c inner join Medida m on m.IdMedida=c.IdMedida
	where concat(c.Nombre,m.Nombre,iif(c.Activo=1,'SI','NO')) like '%'+ @Buscar +'%'
end

sp_help Medida
sp_help Categoria

create procedure sp_crearCategoria(
@Nombre varchar(50),
@IdMedida int,
@MsjError varchar(100) output
)
as
begin
	set @MsjError=''

	if(exists(select * from Categoria where Nombre = @Nombre))
		begin
			set @MsjError = 'El nombre de categoria ya existe'
			return
		end
	insert into Categoria(Nombre,IdMedida)
	values(@Nombre,@IdMedida)
end

create procedure sp_editarCategoria(
@IdCategoria int,
@Nombre varchar(50),
@IdMedida int,
@MsjError varchar(100) output,
@Activo int
)
as
begin
	set @MsjError=''

	if(exists(select * from Categoria where Nombre = @Nombre
	and IdCategoria != @IdCategoria
	))
		begin
			set @MsjError = 'El nombre de categoria ya existe'
			return
		end

	update Categoria set
	Nombre = @Nombre,
	IdMedida = @IdMedida,
	Activo = @Activo
	where IdCategoria = @IdCategoria

end


insert into Categoria(Nombre,IdMedida)
values('Accesorios',1)

select * from Medida
select * from Categoria