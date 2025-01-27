﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atestados.Datos.Modelo
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AtestadosEntities : DbContext
    {
        public AtestadosEntities()
            : base("name=AtestadosEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Archivo> Archivo { get; set; }
        public virtual DbSet<Atestado> Atestado { get; set; }
        public virtual DbSet<AtestadoXPersona> AtestadoXPersona { get; set; }
        public virtual DbSet<DominioIdioma> DominioIdioma { get; set; }
        public virtual DbSet<Evaluacion> Evaluacion { get; set; }
        public virtual DbSet<EvaluaciónXAtestado> EvaluaciónXAtestado { get; set; }
        public virtual DbSet<Fecha> Fecha { get; set; }
        public virtual DbSet<Idioma> Idioma { get; set; }
        public virtual DbSet<InfoEditorial> InfoEditorial { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<PuntajeTiempo> PuntajeTiempo { get; set; }
        public virtual DbSet<Requisito> Requisito { get; set; }
        public virtual DbSet<Rubrica> Rubrica { get; set; }
        public virtual DbSet<Rubro> Rubro { get; set; }
        public virtual DbSet<SeleccionPuntaje> SeleccionPuntaje { get; set; }
        public virtual DbSet<TipoCategoria> TipoCategoria { get; set; }
        public virtual DbSet<TipoPuntaje> TipoPuntaje { get; set; }
        public virtual DbSet<TipoRubro> TipoRubro { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    }
}
