USE [Atestados]
GO

CREATE TABLE [dbo].[Rubrica] (
  [RubricaID] INT NOT NULL IDENTITY(1, 1),
  [TipoPuntajeID] INT NOT NULL,
  [ValorPuntaje] FLOAT NULL,
  [DescripcionPuntaje] NVARCHAR(1000) NULL,
  [PuntajeTiempoID] INT NULL,
  [Fecha] DATETIME NOT NULL,
  [RubroID] INT NOT NULL CONSTRAINT [Rubrica_pkey] PRIMARY KEY ([RubricaID]),
);

CREATE TABLE [dbo].[TipoPuntaje] (
  [TipoPuntajeID] INT NOT NULL,
  [Nombre] NVARCHAR(1000) NOT NULL CONSTRAINT [TipoPuntaje_pkey] PRIMARY KEY ([TipoPuntajeID])
);

CREATE TABLE [dbo].[Requisito] (
  [RequisitoID] INT NOT NULL IDENTITY(1, 1),
  [Descripcion] NVARCHAR(1000) NOT NULL,
  [RubricaID] INT NOT NULL CONSTRAINT [Requisito_pkey] PRIMARY KEY ([RequisitoID])
);

CREATE TABLE [dbo].[SeleccionPuntaje] (
  [SeleccionPuntajeID] INT NOT NULL IDENTITY(1, 1),
  [Descripcion] NVARCHAR(1000) NOT NULL,
  [Puntaje] FLOAT NOT NULL,
  [RubricaID] INT NOT NULL CONSTRAINT [SeleccionPuntaje_pkey] PRIMARY KEY ([SeleccionPuntajeID])
);

CREATE TABLE [dbo].[PuntajeTiempo] (
  [PuntajeTiempoID] INT NOT NULL IDENTITY(1, 1),
  [Nombre] NVARCHAR(1000) NOT NULL,
  CONSTRAINT [PuntajeTiempo_pkey] PRIMARY KEY ([PuntajeTiempoID])
);

CREATE TABLE [dbo].[Evaluacion] (
  [EvaluacionID] INT NOT NULL IDENTITY(1, 1),
  [AtestadoID] INT NOT NULL,
  [RubricaID] INT NOT NULL,
  [Puntaje] INT NOT NULL,
  [Observaciones] NVARCHAR(1000) NULL CONSTRAINT [Evaluacion_pkey] PRIMARY KEY ([EvaluacionID]),
  CONSTRAINT [Evaluacion_AtestadoID_unique] UNIQUE ([AtestadoID])
);

ALTER TABLE
  [dbo].[Rubrica]
ADD
  CONSTRAINT [Rubrica_Rubro_fk] FOREIGN KEY ([RubroID]) REFERENCES [dbo].[Rubro]([RubroID]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE
  [dbo].[Rubrica]
ADD
  CONSTRAINT [Rubrica_TipoPuntaje_fk] FOREIGN KEY ([TipoPuntajeID]) REFERENCES [dbo].[TipoPuntaje]([TipoPuntajeID]) ON DELETE NO ACTION ON UPDATE CASCADE;

ALTER TABLE
  [dbo].[Requisito]
ADD
  CONSTRAINT [Requisito_Rubrica_fk] FOREIGN KEY ([RubricaID]) REFERENCES [dbo].[Rubrica]([RubricaID]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE
  [dbo].[SeleccionPuntaje]
ADD
  CONSTRAINT [SeleccionPuntaje_Rubrica_fk] FOREIGN KEY ([RubricaID]) REFERENCES [dbo].[Rubrica]([RubricaID]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE
  [dbo].[Evaluacion]
ADD
  CONSTRAINT [Evaluacion_Atestado_fk] FOREIGN KEY ([AtestadoID]) REFERENCES [dbo].[Atestado]([AtestadoID]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE
  [dbo].[Evaluacion]
ADD
  CONSTRAINT [Evaluacion_Rubrica_fk] FOREIGN KEY ([RubricaID]) REFERENCES [dbo].[Rubrica]([RubricaID]) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE
  [dbo].[Rubrica]
ADD
  CONSTRAINT [Rubrica_PuntajeTiempo_fk] FOREIGN KEY ([PuntajeTiempoID]) REFERENCES [dbo].[PuntajeTiempo]([PuntajeTiempoID]) ON DELETE CASCADE ON UPDATE CASCADE;

INSERT INTO
  [dbo].[TipoPuntaje] ([TipoPuntajeID], [Nombre])
VALUES
  (1, 'Valor Fijo');

INSERT INTO
  [dbo].[TipoPuntaje] ([TipoPuntajeID], [Nombre])
VALUES
  (2, 'Seleccion');

INSERT INTO
  [dbo].[TipoPuntaje] ([TipoPuntajeID], [Nombre])
VALUES
  (3, 'Tiempo');

INSERT INTO
  [dbo].[PuntajeTiempo] ([PuntajeTiempoID], [Nombre])
VALUES
  (1, 'Dia');

INSERT INTO
  [dbo].[PuntajeTiempo] ([PuntajeTiempoID], [Nombre])
VALUES
  (2, 'Mes');

INSERT INTO
  [dbo].[PuntajeTiempo] ([PuntajeTiempoID], [Nombre])
VALUES
  (3, 'Año');