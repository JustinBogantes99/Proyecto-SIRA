//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DominioIdioma
    {
        public int DominioIdiomaID { get; set; }
        public Nullable<int> IdiomaID { get; set; }
        public int Lectura { get; set; }
        public int Escrito { get; set; }
        public int Auditiva { get; set; }
        public int Oral { get; set; }
    
        public virtual Atestado Atestado { get; set; }
        public virtual Idioma Idioma { get; set; }
    }
}