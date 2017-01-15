using Modelo;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace AcessoDados.Contexto
{
    public class DocumentallCTDBEntities : DbContext
    {
        public DocumentallCTDBEntities()
          : base("name=conexao")
        {

            Database.CreateIfNotExists();

        }
        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<UsuarioPerfil> UsuarioPerfil {get;set;}
        public virtual DbSet<StatusSolicitacao> StatusSolicitacao { get; set; }
        public virtual DbSet<SolicitacaoDeRegistro> SolicitacaoDeRegistros { get; set; }
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<SolicitacaoDeRegistroCustas> SolicitacaoDeRegistrosCustas { get; set; }
        public virtual DbSet<SolicitacaoDeRegistroExigencias> SolicitacaoDeRegistrosExigencias { get; set; }
        public virtual DbSet<LogTransacao> LogTransacao { get; set; }
    }
}