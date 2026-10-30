-- Ejecutar después de 01_CrearBase.sql, una sola vez en una base nueva.
USE ParqueoEstudiantilBasico;
GO

INSERT TextosInterfaz(Clave,Texto) VALUES (N'Operación',N'Operación'),(N'Catálogos',N'Catálogos'),(N'Empresa',N'Empresa'),(N'Seguridad',N'Seguridad'),(N'Reportes',N'Reportes'),(N'Bitácora',N'Bitácora'),(N'Registrar ingreso',N'Registrar ingreso'),(N'Salida y cobro',N'Salida y cobro'),(N'Actualizar',N'Actualizar'),(N'Disponibilidad por zona',N'Disponibilidad por zona'),(N'Vehículos dentro',N'Vehículos dentro'),(N'Nuevo',N'Nuevo'),(N'Editar',N'Editar'),(N'Eliminar',N'Eliminar'),(N'Guardar',N'Guardar'),(N'Consultar',N'Consultar'),(N'Ver comprobante',N'Ver comprobante'),(N'Ver ticket',N'Ver ticket'),(N'Cerrar sesión',N'Cerrar sesión'),(N'Placa',N'Placa'),(N'Tipo de vehículo',N'Tipo de vehículo'),(N'Tipo de espacio',N'Tipo de espacio'),(N'Convenio (opcional)',N'Convenio (opcional)'),(N'Evento (opcional)',N'Evento (opcional)'),(N'Asignar espacio y emitir ticket',N'Asignar espacio y emitir ticket'),(N'Desde',N'Desde'),(N'Hasta',N'Hasta'),(N'Tipo espacio',N'Tipo espacio'),(N'Usuario contiene',N'Usuario contiene'),(N'Nombre',N'Nombre'),(N'Direccion',N'Direccion'),(N'Moneda',N'Moneda'),(N'Impuesto',N'Impuesto'),(N'Logotipo',N'Logotipo'),(N'MinutosGracia',N'MinutosGracia'),(N'TipoEspacioId',N'TipoEspacioId'),(N'MinutosUnidad',N'MinutosUnidad'),(N'Precio',N'Precio'),(N'Mensual',N'Mensual'),(N'Regular',N'Regular'),(N'Activa',N'Activa'),(N'ZonaId',N'ZonaId'),(N'FueraServicio',N'FueraServicio'),(N'Contacto',N'Contacto'),(N'Inicio',N'Inicio'),(N'Fin',N'Fin'),(N'TarifaId',N'TarifaId'),(N'Activo',N'Activo'),(N'EspacioId',N'EspacioId'),(N'ConvenioId',N'ConvenioId'),(N'EventoId',N'EventoId'),(N'RolId',N'RolId'),(N'ModuloId',N'ModuloId'),(N'Modificar',N'Modificar'),(N'Clave',N'Clave'),(N'Texto',N'Texto');
INSERT Empresa VALUES(1,N'Parqueo estudiantil',N'Dirección del parqueo',N'Q',12,'',0);
INSERT Roles VALUES(N'Administrador'),(N'Operador');
INSERT Modulos VALUES(N'Operación'),(N'Catálogos'),(N'Empresa'),(N'Seguridad'),(N'Reportes'),(N'Bitácora');
INSERT Permisos(RolId,ModuloId,Consultar,Modificar) VALUES
(1,1,1,1),
(1,2,1,1),
(1,3,1,1),
(1,4,1,1),
(1,5,1,1),
(1,6,1,1);
INSERT Permisos(RolId,ModuloId,Consultar,Modificar) VALUES (2,1,1,1),(2,5,1,0);
INSERT Zonas VALUES(N'Nivel 1');
INSERT TiposEspacio VALUES(N'Normal'),(N'Discapacitados'),(N'Motocicleta'),(N'VIP');
INSERT TiposVehiculo VALUES(N'Automóvil'),(N'Motocicleta'),(N'Camioneta');
INSERT Tarifas VALUES(N'Normal por hora',1,60,10,0,1,1),(N'Discapacitados por hora',2,60,10,0,1,1),(N'Moto por fracción',3,30,3,0,1,1),(N'VIP por día',4,1440,75,0,1,1),(N'Mensualidad corporativa',1,43200,500,1,0,1);
INSERT Espacios VALUES(N'A01',1,1,0),(N'A02',1,1,0),(N'A03',1,1,0),(N'D01',1,2,0),(N'M01',1,3,0),(N'V01',1,4,0);
