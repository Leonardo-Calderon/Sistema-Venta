--creamos la base de datos de la tienda con el siguiente comando

create database DBTienda

--para usarla tenemos el siguiente comando que nos pondrá directamente dentro de la base de datos
use DBTienda

--comenzamos creando las  tablas comenzando por medida

create table Medida(
IdMedida int primary key identity(1,1),
Nombre varchar(50) not null,
Abreviatura varchar(4) not null,
Equivalente varchar(4) not null,
valor int not null
)

create table Categoria(
IdCategoria int primary key identity(1,1), 
Nombre varchar(50) not null,
IdMedida int references	Medida(IdMedida),
Activo bit default 1
)

create table Producto(
IdProducto int primary key identity(1,1), 
IdCategoria int references Categoria(IdCategoria),
Codigo varchar(50) not null,
Descripcion varchar(50) not null,
PrecioCompra decimal(10,2) not null,
PrecioVenta decimal(10,2) not null,
Cantidad int not null,
Activo bit default 1
)

create table Rol(
IdRol int primary key identity(1,1),
Nombre varchar(50) not null,
)

create table Usuario(
IdUsuario int primary key identity(1,1),
IdRol int references Rol(IdRol),
NombreCompleto varchar(50) not null,
Correo varchar(50) not null,
NombreUsuario varchar(50) not null unique,
Clave varchar(100) not null,
ResetearClave bit default 1,
Activo bit default 1
)

create table correlativoVenta(
Serie varchar(3) not null,
Numero int not null,
Activo bit default 1,
primary key(Serie,Numero)
)

create table Venta(
IdVenta int primary key identity(1,1),
NumeroVenta varchar(10),
IdUsuarioRegistro int references Usuario(IdUsuario),
NombreCliente varchar(60),
PrecioTotal decimal(10,2) not null,
PagoCon decimal(10,2),
Cambio decimal(10,2),
FechaRegistro datetime default getdate()
)

create table DetalleVenta(
IdDetalleVenta int primary key identity(1,1),
IdVenta int references Venta(IdVenta),
IdProducto int references Producto(IdProducto),
Cantidad int,
PrecioVenta decimal(10,2),
PrecioTotal decimal(10,2)
)

create table Negocio(
IdNegocio int primary key identity(1,1),
RazonSocial varchar(100),
RFC varchar(13),
Direccion varchar(100),
Celular varchar(10),
Correo varchar(30),
SimboloMoneda varchar(5),
NombreLogo varchar(100),
URL varchar(200)
)

Create table Menu(
IdMenu int primary key identity(1,1),
NombreMenu varchar(50) not null,
IdMenuPadre int default 0 not null
)

Create table MenuRol(
IdMenuRol int primary key identity(1,1),
IdMenu int references Menu(IdMenu),
IdRol int references Rol(IdRol),
Activo bit default 1
)