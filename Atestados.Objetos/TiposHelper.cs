using Atestados.Datos.Modelo;
using Atestados.Objetos.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atestados.Objectos
{
    public static class TiposHelper
    {
        private static readonly AtestadosEntities db = new AtestadosEntities();

        #region Tipos

        public static List<TipoUsuario> ObtenerTiposUsuario()
        {
            List<TipoUsuario> tipoUsuario = db.TipoUsuario.ToList();
            return tipoUsuario;
        }

        public static int ObtenerTipoUsuarioID(string nombre)
        {
            TipoUsuario tipoUsuario = db.TipoUsuario.Where(x => x.Nombre == nombre).FirstOrDefault();

            if(tipoUsuario == null)
                throw new ArgumentException("Nombre invalido para el tipo de usuario ingresado.");

            return tipoUsuario.TipoUsuarioID;
        }

        public static string ObtenerTipoUsuarioNombre(int ID)
        {
            TipoUsuario tipoUsuario = db.TipoUsuario.Where(x => x.TipoUsuarioID == ID).FirstOrDefault();

            if(tipoUsuario == null)
                throw new ArgumentException("ID invalido para el tipo de usuario ingresado.");

            return tipoUsuario.Nombre;
        }

        public static List<TipoCategoria> ObtenerTiposCategoria()
        {
            List<TipoCategoria> tipoCategoria = db.TipoCategoria.ToList();
            return tipoCategoria;
        }

        public static int ObtenerTipoCategoriaID(string nombre)
        {
            TipoCategoria tipoCategoria = db.TipoCategoria.Where(x => x.Nombre == nombre).FirstOrDefault();

            if(tipoCategoria == null)
                throw new ArgumentException("Nombre invalido para el tipo de categoria ingresado.");

            return tipoCategoria.TipoCategoriaID;
        }
        public static string ObtenerTipoCategoriaNombre(int ID)
        {
            TipoCategoria tipoCategoria = db.TipoCategoria.Where(x => x.TipoCategoriaID == ID).FirstOrDefault();

            if(tipoCategoria == null)
                throw new ArgumentException("Nombre invalido para el tipo de categoria ingresado.");

            return tipoCategoria.Nombre;
        }

        #endregion
    }
}
