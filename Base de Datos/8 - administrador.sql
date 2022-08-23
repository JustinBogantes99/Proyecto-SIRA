/* 
	Para loguearse al sistema como administrador
	Usuario = admin@mail.com
	Contraseña = admin

	Despues de esto se puede crear un usuario de administracion con una contraseña apropiada y cambiar el tipo de usuario de esta cuenta
	Para crear otro usuario administrador solo debe registrar una cuenta nueva, luego tiene que ingresar con la cuenta admin@mail.com y
	en el panel de administración cambiar el tipo de usuario a administrador o 1

*/
Use Atestados;
INSERT INTO Persona VALUES ('Administrador', 'Admin', 'Admin', 'admin@mail.com', 1, 1, 00000000, 1); /* Crear Persona Primero */

/* Crear Usuario Segundo, el primer valor es el ID de Persona, sino es 1, cambiar por el correcto */
INSERT INTO Usuario VALUES (1, 'admin@mail.com', '$2a$11$NgjqSiLeSzWHFQWiLaMSbOARNgwS0dAakduZCckAcfPmBMPzjEKoC', 1)  

-- SELECT TOP (1000) [TipoCategoriaID] ,[Nombre] FROM [Atestados].[dbo].[TipoCategoria]
-- SELECT TOP (1000) [TipoUsuarioID] ,[Nombre] FROM [Atestados].[dbo].[TipoUsuario]