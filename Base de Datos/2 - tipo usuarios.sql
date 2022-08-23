/* 
	Tipos de usuario
	1 - usuario administrador
	2 - usuario revisor
	3 - usuario docente
*/
USE [Atestados]
GO

INSERT INTO [dbo].[TipoUsuario]
           ([Nombre])
     VALUES
           ('Admin'),
		   ('Revisor'),
		   ('Docente'),
		   ('No registrado')
GO