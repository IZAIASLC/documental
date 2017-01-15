


 CREATE DATABASE [DocumentalCTDB]

 GO

 USE [DocumentalCTDB]

 GO

 CREATE TABLE [dbo].[StatusSolicitacao](
	[IdStatusSolicitacao] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.StatusSolicitacao] PRIMARY KEY CLUSTERED 
(
	[IdStatusSolicitacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]  

GO


CREATE TABLE [dbo].[SolicitacaoDeRegistro](
	[IdSolicitacaoDeRegistro] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](150) NULL,
	[CPF] [int] NOT NULL,
	[DataSolicitacao] [datetime] NOT NULL,
	[ValorDeclarado] [decimal](18, 2) NULL,
	[QuantidadePagina] [int] NULL,
	[IDCertisign] [nvarchar](150) NULL,
	[IdStatusSolicitacao] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SolicitacaoDeRegistro] PRIMARY KEY CLUSTERED 
(
	[IdSolicitacaoDeRegistro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[SolicitacaoDeRegistro]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SolicitacaoDeRegistro_dbo.StatusSolicitacao_IdStatusSolicitacao] FOREIGN KEY([IdStatusSolicitacao])
REFERENCES [dbo].[StatusSolicitacao] ([IdStatusSolicitacao])
ON DELETE CASCADE
GO


CREATE TABLE [dbo].[SolicitacaoDeRegistroCustas](
	[IdSolicitacaoDeRegistroCustas] [int] IDENTITY(1,1) NOT NULL,
	[IdSolicitacaoDeRegistro] [int] NOT NULL,
	[Descricao] [nvarchar](150) NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[DataSolicitacaoRegistroCustas] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SolicitacaoDeRegistroCustas] PRIMARY KEY CLUSTERED 
(
	[IdSolicitacaoDeRegistroCustas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[SolicitacaoDeRegistroCustas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SolicitacaoDeRegistroCustas_dbo.SolicitacaoDeRegistro_IdSolicitacaoDeRegistro] FOREIGN KEY([IdSolicitacaoDeRegistro])
REFERENCES [dbo].[SolicitacaoDeRegistro] ([IdSolicitacaoDeRegistro])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolicitacaoDeRegistroCustas] CHECK CONSTRAINT [FK_dbo.SolicitacaoDeRegistroCustas_dbo.SolicitacaoDeRegistro_IdSolicitacaoDeRegistro]
GO

CREATE TABLE [dbo].[SolicitacaoDeRegistroExigencias](
	[IdSolicitacaoDeRegistroExigencias] [int] IDENTITY(1,1) NOT NULL,
	[IdSolicitacaoDeRegistro] [int] NOT NULL,
	[Descricao] [nvarchar](150) NULL,
	[DataSolicitacaoRegistroExigencias] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.SolicitacaoDeRegistroExigencias] PRIMARY KEY CLUSTERED 
(
	[IdSolicitacaoDeRegistroExigencias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[SolicitacaoDeRegistroExigencias]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SolicitacaoDeRegistroExigencias_dbo.SolicitacaoDeRegistro_IdSolicitacaoDeRegistro] FOREIGN KEY([IdSolicitacaoDeRegistro])
REFERENCES [dbo].[SolicitacaoDeRegistro] ([IdSolicitacaoDeRegistro])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SolicitacaoDeRegistroExigencias] CHECK CONSTRAINT [FK_dbo.SolicitacaoDeRegistroExigencias_dbo.SolicitacaoDeRegistro_IdSolicitacaoDeRegistro]
GO




CREATE TABLE [dbo].[Documento](
	[IdDocumento][int] IDENTITY(1,1) NOT NULL,
	[IdSolicitacaoDeRegistro] [int] NOT NULL,
	[NomeDocumento] [nvarchar](150) NULL,
	[DocumentoBase64] [nvarchar](max) NULL,
	[ExtensaoDocumento] [nvarchar](5) NULL,
	[DataCadastro] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Documento] PRIMARY KEY CLUSTERED 
(
	[IdDocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[Documento]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Documento_dbo.SolicitacaoDeRegistro_IdSolicitacaoDeRegistro] FOREIGN KEY([IdSolicitacaoDeRegistro])
REFERENCES [dbo].[SolicitacaoDeRegistro] ([IdSolicitacaoDeRegistro])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Documento] CHECK CONSTRAINT [FK_dbo.Documento_dbo.SolicitacaoDeRegistro_IdSolicitacaoDeRegistro]
GO



ALTER TABLE [dbo].[SolicitacaoDeRegistro] CHECK CONSTRAINT [FK_dbo.SolicitacaoDeRegistro_dbo.StatusSolicitacao_IdStatusSolicitacao]
GO




CREATE TABLE [dbo].[Perfil](
	[IdPerfil] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.Perfil] PRIMARY KEY CLUSTERED 
(
	[IdPerfil] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](150) NULL,
	[Email] [nvarchar](150) NULL,
	[Senha] [nvarchar](150) NULL,
	[Ativo] [bit] NOT NULL,
	[DataCadastro] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO



CREATE TABLE [dbo].[UsuarioPerfil](
	[IdUsuarioPerfil] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPerfil] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UsuarioPerfil] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioPerfil] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UsuarioPerfil]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UsuarioPerfil_dbo.Perfil_IdPerfil] FOREIGN KEY([IdPerfil])
REFERENCES [dbo].[Perfil] ([IdPerfil])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UsuarioPerfil] CHECK CONSTRAINT [FK_dbo.UsuarioPerfil_dbo.Perfil_IdPerfil]
GO

ALTER TABLE [dbo].[UsuarioPerfil]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UsuarioPerfil_dbo.Usuario_IdUsuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UsuarioPerfil] CHECK CONSTRAINT [FK_dbo.UsuarioPerfil_dbo.Usuario_IdUsuario]
GO




CREATE TABLE [dbo].[LogTransacao](
	[IdLogTransacao] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuarioTransacao] [int] NOT NULL,
	[IdTransacaoEnum] [int] NOT NULL,
	[IdTransacao] [int] NOT NULL,
	[DataTransacao] [datetime] NULL,
 CONSTRAINT [PK_dbo.LogTransacao] PRIMARY KEY CLUSTERED 
(
	[IdLogTransacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1. SolicitacaoRegistro   2. AlterarStatusSolicitacaoRegistro   3. SolicitacaoDeRegistroDeCustas   4. SolicitacaoDeRegistroDeExigencias   ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogTransacao', @level2type=N'COLUMN',@level2name=N'IdTransacaoEnum'
GO



/*
  INSERE OS STATUS DA SOLICITACAO
*/

INSERT INTO [dbo].[StatusSolicitacao] VALUES('Enviado');
INSERT INTO [dbo].[StatusSolicitacao] VALUES('Recebido');
INSERT INTO [dbo].[StatusSolicitacao] VALUES('Registrado');


/*
INSERE OS PERFIS DO USUÁRIO
*/

INSERT INTO [dbo].[Perfil] VALUES ('Administrador');
INSERT INTO [dbo].[Perfil] VALUES ('Cadastro');