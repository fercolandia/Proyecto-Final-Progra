-- Primero cree una base vacía llamada ParqueoEstudiantilBasico en SQL Server.
USE ParqueoEstudiantilBasico;
GO

CREATE TABLE TextosInterfaz (
    Id int IDENTITY PRIMARY KEY,
    Clave nvarchar(300) NOT NULL,
    Texto nvarchar(500) NOT NULL
);

CREATE TABLE Empresa (
    Id int PRIMARY KEY,
    Nombre nvarchar(120) NOT NULL,
    Direccion nvarchar(250) NOT NULL,
    Moneda nvarchar(10) NOT NULL,
    Impuesto decimal(5,2) NOT NULL,
    Logotipo nvarchar(500) NOT NULL,
    MinutosGracia int NOT NULL
);

CREATE TABLE Roles (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(60) NOT NULL
);

CREATE TABLE Modulos (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(60) NOT NULL
);

CREATE TABLE Permisos (
    Id int IDENTITY PRIMARY KEY,
    RolId int NOT NULL,
    ModuloId int NOT NULL,
    Consultar bit NOT NULL,
    Modificar bit NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(Id),
    FOREIGN KEY (ModuloId) REFERENCES Modulos(Id)
);

CREATE TABLE Usuarios (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(60) NOT NULL,
    Hash nvarchar(300) NOT NULL,
    RolId int NOT NULL,
    Activo bit NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(Id)
);

CREATE TABLE Zonas (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(60) NOT NULL
);

CREATE TABLE TiposEspacio (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(60) NOT NULL
);

CREATE TABLE TiposVehiculo (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(60) NOT NULL
);

CREATE TABLE Tarifas (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(80) NOT NULL,
    TipoEspacioId int NOT NULL,
    MinutosUnidad int NOT NULL,
    Precio decimal(12,2) NOT NULL,
    Mensual bit NOT NULL,
    Regular bit NOT NULL,
    Activa bit NOT NULL,
    FOREIGN KEY (TipoEspacioId) REFERENCES TiposEspacio(Id)
);

CREATE TABLE Convenios (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(100) NOT NULL,
    Contacto nvarchar(150) NOT NULL,
    Inicio datetime2 NOT NULL,
    Fin datetime2 NOT NULL,
    TarifaId int NOT NULL,
    Activo bit NOT NULL,
    FOREIGN KEY (TarifaId) REFERENCES Tarifas(Id)
);

CREATE TABLE Eventos (
    Id int IDENTITY PRIMARY KEY,
    Nombre nvarchar(100) NOT NULL,
    Inicio datetime2 NOT NULL,
    Fin datetime2 NOT NULL,
    TarifaId int NOT NULL,
    Activo bit NOT NULL,
    FOREIGN KEY (TarifaId) REFERENCES Tarifas(Id)
);

CREATE TABLE Espacios (
    Id int IDENTITY PRIMARY KEY,
    Codigo nvarchar(20) NOT NULL,
    ZonaId int NOT NULL,
    TipoEspacioId int NOT NULL,
    FueraServicio bit NOT NULL,
    FOREIGN KEY (ZonaId) REFERENCES Zonas(Id),
    FOREIGN KEY (TipoEspacioId) REFERENCES TiposEspacio(Id)
);

CREATE TABLE Reservas (
    Id int IDENTITY PRIMARY KEY,
    EspacioId int NOT NULL,
    ConvenioId int NULL,
    EventoId int NULL,
    FOREIGN KEY (EspacioId) REFERENCES Espacios(Id),
    FOREIGN KEY (ConvenioId) REFERENCES Convenios(Id),
    FOREIGN KEY (EventoId) REFERENCES Eventos(Id)
);

CREATE TABLE Tickets (
    Id int IDENTITY PRIMARY KEY,
    Placa nvarchar(15) NOT NULL,
    TipoVehiculoId int NOT NULL,
    EspacioId int NOT NULL,
    Entrada datetime2 NOT NULL,
    Salida datetime2 NULL,
    TarifaId int NOT NULL,
    Precio decimal(12,2) NOT NULL,
    MinutosUnidad int NOT NULL,
    Mensual bit NOT NULL,
    Impuesto decimal(5,2) NOT NULL,
    Gracia int NOT NULL,
    ConvenioId int NULL,
    EventoId int NULL,
    Subtotal decimal(12,2) NULL,
    Total decimal(12,2) NULL,
    UsuarioIngresoId int NOT NULL,
    UsuarioSalidaId int NULL,
    FOREIGN KEY (TipoVehiculoId) REFERENCES TiposVehiculo(Id),
    FOREIGN KEY (EspacioId) REFERENCES Espacios(Id),
    FOREIGN KEY (TarifaId) REFERENCES Tarifas(Id),
    FOREIGN KEY (ConvenioId) REFERENCES Convenios(Id),
    FOREIGN KEY (EventoId) REFERENCES Eventos(Id),
    FOREIGN KEY (UsuarioIngresoId) REFERENCES Usuarios(Id),
    FOREIGN KEY (UsuarioSalidaId) REFERENCES Usuarios(Id)
);

CREATE TABLE Mensualidades (
    Id int IDENTITY PRIMARY KEY,
    ConvenioId int NOT NULL,
    Periodo char(7) NOT NULL,
    TicketId int NOT NULL,
    Monto decimal(12,2) NOT NULL,
    FOREIGN KEY (ConvenioId) REFERENCES Convenios(Id),
    FOREIGN KEY (TicketId) REFERENCES Tickets(Id)
);

CREATE TABLE Bitacora (
    Id bigint IDENTITY PRIMARY KEY,
    Usuario nvarchar(60) NOT NULL,
    Accion varchar(10) NOT NULL,
    Tabla nvarchar(60) NOT NULL,
    Fecha datetime2 NOT NULL,
    Valores nvarchar(max) NOT NULL
);
