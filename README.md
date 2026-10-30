# Sistema de parqueo

Proyecto estudiantil en C# Windows Forms, SQL Server y Entity Framework Core 8.0.11.

## Ejecución

1. Crear la base de datos `ParqueoEstudiantilBasico` en SQL Server.
2. Ejecutar `BaseDatos/01_CrearBase.sql` y luego `BaseDatos/02_InsertarDatos.sql` sobre la base nueva.
3. Ajustar el servidor en `conexion.txt` si se ejecuta en otra computadora. La conexión utiliza autenticación de Windows.
4. Abrir `Parqueo.slnx` en Visual Studio con soporte para .NET 10 y Windows Forms, restaurar los paquetes y ejecutar con F5.
5. Si no hay usuarios, el primer acceso permite crear el administrador.

## Funciones

Registro de ingresos, asignación de espacios, búsqueda por placa o ticket, salida y cobro, historial por placa, resumen del día, catálogos, convenios, eventos, reservas, usuarios, permisos y bitácora.

Los formularios y el control de Operación se pueden editar con el diseñador de Visual Studio.

Más información en `LEEME.txt` y `docs/Documentacion_Parqueo.docx`.
