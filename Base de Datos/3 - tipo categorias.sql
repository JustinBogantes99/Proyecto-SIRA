/* 
	Archivo para ingresar las categorías del docente en la base de datos.
	Tipos de categorías 
	1 - usuario administrador
	2 - usuario revisor
	3 - usuario docente
*/
USE [Atestados]
GO

INSERT INTO [dbo].[TipoCategoria]
           ([Nombre]
           ,[Academicos]
           ,[Administrativos])
     VALUES
           ('Sin Categoría', '-', '-'),
           ('Primera', 'Instructor/a', 'Profesional 1'),
		   ('Segunda', 'Profesor/a Adjunto/a', 'Profesional 2'),
		   ('Tercera', 'Profesor/a Asociado/a', 'Profesional 3'),
		   ('Cuarta', 'Catedrático/a', 'Profesional 4')
GO

