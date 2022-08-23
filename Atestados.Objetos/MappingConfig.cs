using Atestados.Datos.Modelo;
using Atestados.Objectos;
using Atestados.Objetos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atestados.Objetos
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                #region Persona
                config.CreateMap<Persona, PersonaDTO>()
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PrimerApellido,
                    opt => opt.MapFrom(src => src.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido,
                    opt => opt.MapFrom(src => src.SegundoApellido))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Telefono,
                    opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.CategoriaActual,
                    opt => opt.MapFrom(src => src.CategoriaActual))
                .ForMember(dest => dest.TipoUsuario,
                    opt => opt.MapFrom(src => src.TipoUsuario))
                .ForMember(dest => dest.esActivo,
                    opt => opt.MapFrom(src => src.esActivo));

                config.CreateMap<Persona, UsuarioDTO>()
                .ForMember(dest => dest.UsuarioID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PrimerApellido,
                    opt => opt.MapFrom(src => src.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido,
                    opt => opt.MapFrom(src => src.SegundoApellido))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Telefono,
                    opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.CategoriaActual,
                    opt => opt.MapFrom(src => src.CategoriaActual))
                .ForMember(dest => dest.TipoUsuario,
                    opt => opt.MapFrom(src => src.TipoUsuario))
                .ForMember(dest => dest.TipoUsuarioNombre,
                    opt => opt.MapFrom(src => TiposHelper.ObtenerTipoUsuarioNombre(src.TipoUsuario)))
                .ForMember(dest => dest.CategoriaActualNombre,
                    opt => opt.MapFrom(src => TiposHelper.ObtenerTipoCategoriaNombre(src.CategoriaActual)))
                .ForMember(dest => dest.esActivo,
                    opt => opt.MapFrom(src => src.esActivo));

                config.CreateMap<UsuarioDTO, Persona>()
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.UsuarioID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PrimerApellido,
                    opt => opt.MapFrom(src => src.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido,
                    opt => opt.MapFrom(src => src.SegundoApellido))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Telefono,
                    opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.CategoriaActual,
                    opt => opt.MapFrom(src => src.CategoriaActual))
                .ForMember(dest => dest.TipoUsuario,
                    opt => opt.MapFrom(src => src.TipoUsuario))
                .ForMember(dest => dest.esActivo,
                    opt => opt.MapFrom(src => src.esActivo));

                config.CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.UsuarioID,
                    opt => opt.MapFrom(src => src.UsuarioID))
                .ForMember(dest => dest.Contrasena,
                    opt => opt.MapFrom(src => src.Contrasena))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.esActivo,
                    opt => opt.MapFrom(src => src.esActivo));

                config.CreateMap<UsuarioDTO, Usuario>()
                .ForMember(dest => dest.UsuarioID,
                    opt => opt.MapFrom(src => src.UsuarioID))
                .ForMember(dest => dest.Contrasena,
                    opt => opt.MapFrom(src => src.Contrasena))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.esActivo,
                    opt => opt.MapFrom(src => src.esActivo));

                config.CreateMap<UsuarioDTO, UsuarioDTO>()
                .ForMember(dest => dest.UsuarioID,
                    opt => opt.MapFrom(src => src.UsuarioID))
                .ForMember(dest => dest.Contrasena,
                    opt => opt.MapFrom(src => src.Contrasena))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PrimerApellido,
                    opt => opt.MapFrom(src => src.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido,
                    opt => opt.MapFrom(src => src.SegundoApellido))
                .ForMember(dest => dest.CategoriaActual,
                    opt => opt.MapFrom(src => src.CategoriaActual))
                .ForMember(dest => dest.TipoUsuario,
                    opt => opt.MapFrom(src => src.TipoUsuario))
                .ForMember(dest => dest.Telefono,
                    opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.esActivo,
                    opt => opt.MapFrom(src => src.esActivo));

                #endregion

                #region Atestado
                config.CreateMap<TipoRubro, TipoRubroDTO>()
                .ForMember(dest => dest.TipoRubroID,
                    opt => opt.MapFrom(src => src.TipoRubroID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre));

                config.CreateMap<Rubro, RubroDTO>()
                .ForMember(dest => dest.RubroID,
                    opt => opt.MapFrom(src => src.RubroID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.TipoRubro,
                    opt => opt.MapFrom(src => src.TipoRubro1.Nombre))
                .ForMember(dest => dest.Descripcion,
                    opt => opt.MapFrom(src => src.Descripcion));

                config.CreateMap<Pais, PaisDTO>()
                .ForMember(dest => dest.PaisID,
                    opt => opt.MapFrom(src => src.PaisID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre));

                config.CreateMap<Idioma, IdiomaDTO>()
                .ForMember(dest => dest.IdiomaID,
                    opt => opt.MapFrom(src => src.IdiomaID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre));

                config.CreateMap<Fecha, FechaDTO>()
                .ForMember(dest => dest.FechaID,
                    opt => opt.MapFrom(src => src.FechaID))
                .ForMember(dest => dest.FechaInicio,
                    opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFinal,
                    opt => opt.MapFrom(src => src.FechaFinal))
                .ForMember(dest => dest.Atestado,
                    opt => opt.MapFrom(src => src.Atestado.Nombre));

                config.CreateMap<InfoEditorial, InfoEditorialDTO>()
                .ForMember(dest => dest.InfoEditorialID,
                    opt => opt.MapFrom(src => src.InfoEditorialID))
                .ForMember(dest => dest.Editorial,
                    opt => opt.MapFrom(src => src.Editorial))
                .ForMember(dest => dest.Website,
                    opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.Atestado,
                    opt => opt.MapFrom(src => src.Atestado.Nombre));

                config.CreateMap<DominioIdioma, DominioIdiomaDTO>()
                .ForMember(dest => dest.DominioIdiomaID,
                    opt => opt.MapFrom(src => src.DominioIdiomaID))
                .ForMember(dest => dest.Idioma,
                    opt => opt.MapFrom(src => src.Idioma.Nombre))
                .ForMember(dest => dest.Auditiva,
                    opt => opt.MapFrom(src => src.Auditiva))
                .ForMember(dest => dest.Escrito,
                    opt => opt.MapFrom(src => src.Escrito))
                .ForMember(dest => dest.Oral,
                    opt => opt.MapFrom(src => src.Oral))
                .ForMember(dest => dest.Lectura,
                    opt => opt.MapFrom(src => src.Lectura))
                .ForMember(dest => dest.Atestado,
                    opt => opt.MapFrom(src => src.Atestado.Nombre));

                config.CreateMap<Archivo, ArchivoDTO>()
                .ForMember(dest => dest.ArchivoID,
                    opt => opt.MapFrom(src => src.ArchivoID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Obligatorio,
                    opt => opt.MapFrom(src => Convert.ToBoolean(src.Obligatorio)))
                .ForMember(dest => dest.Datos,
                    opt => opt.MapFrom(src => src.Datos))
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.Atestado.AtestadoID))
                .ForMember(dest => dest.TipoArchivo,
                    opt => opt.MapFrom(src => src.TipoArchivo));

                config.CreateMap<ArchivoDTO, Archivo>()
                .ForMember(dest => dest.ArchivoID,
                    opt => opt.MapFrom(src => src.ArchivoID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Obligatorio,
                    opt => opt.MapFrom(src => src.Obligatorio))
                .ForMember(dest => dest.Datos,
                    opt => opt.MapFrom(src => src.Datos))
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.TipoArchivo,
                    opt => opt.MapFrom(src => src.TipoArchivo));

                config.CreateMap<AtestadoXPersona, AtestadoXPersonaDTO>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Porcentaje,
                    opt => opt.MapFrom(src => src.Porcentaje))
                .ForMember(dest => dest.Departamento,
                    opt => opt.MapFrom(src => src.Departamento))
                .ForMember(dest => dest.Atestado,
                    opt => opt.MapFrom(src => src.Atestado.Nombre))
                .ForMember(dest => dest.Persona,
                    opt => opt.MapFrom(src => String.Format("{0} {1} {2}", src.Persona.Nombre, src.Persona.PrimerApellido, src.Persona.SegundoApellido)));


                config.CreateMap<Atestado, AtestadoDTO>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.CantidadHoras,
                    opt => opt.MapFrom(src => src.CantidadHoras))
                .ForMember(dest => dest.CatalogoTipo,
                    opt => opt.MapFrom(src => src.CatalogoTipo))
                .ForMember(dest => dest.Descargado,
                    opt => opt.MapFrom(src => src.Descargado))
                .ForMember(dest => dest.Enlace,
                    opt => opt.MapFrom(src => src.Enlace))
                .ForMember(dest => dest.Enviado,
                    opt => opt.MapFrom(src => src.Enviado))
                .ForMember(dest => dest.NumeroAutores,
                    opt => opt.MapFrom(src => src.NumeroAutores))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.HoraCreacion,
                    opt => opt.MapFrom(src => src.HoraCreacion))
                .ForMember(dest => dest.Lugar,
                    opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.Pais,
                    opt => opt.MapFrom(src => src.Pais))
                .ForMember(dest => dest.Rubro,
                    opt => opt.MapFrom(src => src.Rubro))
                .ForMember(dest => dest.Persona,
                    opt => opt.MapFrom(src => src.Persona))
                .ForMember(dest => dest.DominioIdioma,
                    opt => opt.MapFrom(src => src.DominioIdioma))
                .ForMember(dest => dest.Archivos,
                    opt => opt.MapFrom(src => src.Archivo))
                .ForMember(dest => dest.AtestadoXPersona,
                    opt => opt.MapFrom(src => src.AtestadoXPersona))
                .ForMember(dest => dest.Fecha,
                    opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.InfoEditorial,
                    opt => opt.MapFrom(src => src.InfoEditorial))
                .ForMember(dest => dest.PaisID,
                    opt => opt.MapFrom(src => src.PaisID))
                .ForMember(dest => dest.RubroID,
                    opt => opt.MapFrom(src => src.RubroID))
                .ForMember(dest => dest.Codigo,
                    opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID));

                config.CreateMap<AtestadoDTO, Atestado>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.CantidadHoras,
                    opt => opt.MapFrom(src => src.CantidadHoras))
                .ForMember(dest => dest.CatalogoTipo,
                    opt => opt.MapFrom(src => src.CatalogoTipo))
                .ForMember(dest => dest.Descargado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Enlace,
                    opt => opt.MapFrom(src => src.Enlace))
                .ForMember(dest => dest.Enviado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.NumeroAutores,
                    opt => opt.MapFrom(src => src.NumeroAutores))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.HoraCreacion,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Lugar,
                    opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.Pais,
                    opt => opt.MapFrom(src => src.Pais))
                .ForMember(dest => dest.Rubro,
                    opt => opt.MapFrom(src => src.Rubro))
                .ForMember(dest => dest.Persona,
                    opt => opt.MapFrom(src => src.Persona))
                .ForMember(dest => dest.Archivo,
                    opt => opt.MapFrom(src => src.Archivos))
                .ForMember(dest => dest.PaisID,
                    opt => opt.MapFrom(src => src.PaisID))
                .ForMember(dest => dest.RubroID,
                    opt => opt.MapFrom(src => src.RubroID))
                .ForMember(dest => dest.Codigo,
                    opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForAllOtherMembers(x => x.Ignore());

                config.CreateMap<Atestado, Atestado>()
                .ForMember(dest => dest.AtestadoID,
                   opt => opt.MapFrom(src => src.AtestadoID))
               .ForMember(dest => dest.Nombre,
                   opt => opt.MapFrom(src => src.Nombre))
               .ForMember(dest => dest.Nombre,
                   opt => opt.MapFrom(src => src.Nombre))
               .ForMember(dest => dest.NumeroAutores,
                   opt => opt.MapFrom(src => src.NumeroAutores))
               .ForMember(dest => dest.HoraCreacion,
                   opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.Enlace,
                   opt => opt.MapFrom(src => src.Enlace))
               .ForMember(dest => dest.PaisID,
                   opt => opt.MapFrom(src => src.PaisID))
               .ForMember(dest => dest.Observaciones,
                   opt => opt.MapFrom(src => src.Observaciones))
               .ForMember(dest => dest.Persona,
                   opt => opt.MapFrom(src => src.Persona));

                config.CreateMap<AtestadoDTO, Fecha>()
                .ForMember(dest => dest.FechaID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.FechaInicio,
                    opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFinal,
                    opt => opt.MapFrom(src => src.FechaFinal));

                config.CreateMap<EvaluacionXAtestadoDTO, EvaluaciónXAtestado>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.AutorID,
                    opt => opt.MapFrom(src => src.AutorID))
                .ForMember(dest => dest.PorcentajeObtenido,
                    opt => opt.MapFrom(src => src.PorcentajeObtenido))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones));

                config.CreateMap<EvaluaciónXAtestado, EvaluacionXAtestadoDTO>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.PorcentajeObtenido,
                    opt => opt.MapFrom(src => src.PorcentajeObtenido))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones));

                #endregion

                #region LibroArticulo
                config.CreateMap<LibroDTO, Atestado>()
               .ForMember(dest => dest.Nombre,
                   opt => opt.MapFrom(src => src.Nombre))
               .ForMember(dest => dest.Nombre,
                   opt => opt.MapFrom(src => src.Nombre))
               .ForMember(dest => dest.HoraCreacion,
                   opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.Enlace,
                   opt => opt.MapFrom(src => src.Enlace))
               .ForMember(dest => dest.PaisID,
                   opt => opt.MapFrom(src => src.PaisID))
               .ForMember(dest => dest.Enviado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Descargado,
                    opt => opt.MapFrom(src => 0))
               .ForMember(dest => dest.Pais,
                   opt => opt.MapFrom(src => src.Pais))
               .ForMember(dest => dest.Observaciones,
                   opt => opt.MapFrom(src => src.Observaciones))
               .ForMember(dest => dest.Persona,
                   opt => opt.MapFrom(src => src.Persona))
               .ForMember(dest => dest.PersonaID,
                   opt => opt.MapFrom(src => src.PersonaID))
               .ForMember(dest => dest.RubroID,
                   opt => opt.MapFrom(src => src.RubroID));

                config.CreateMap<LibroDTO, InfoEditorial>()
                .ForMember(dest => dest.InfoEditorialID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.Editorial,
                    opt => opt.MapFrom(src => src.Editorial))
                .ForMember(dest => dest.Website,
                    opt => opt.MapFrom(src => src.Website));

                config.CreateMap<LibroDTO, Fecha>()
                .ForMember(dest => dest.FechaID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.FechaFinal,
                    opt => opt.MapFrom(src => src.Annio));

                config.CreateMap<Persona, AutorDTO>()
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PrimerApellido,
                    opt => opt.MapFrom(src => src.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido,
                    opt => opt.MapFrom(src => src.SegundoApellido))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email));

                config.CreateMap<AtestadoXPersona, AutorDTO>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Porcentaje,
                    opt => opt.MapFrom(src => src.Porcentaje))
                .ForMember(dest => dest.Departamento,
                    opt => opt.MapFrom(src => src.Departamento));

                config.CreateMap<AutorDTO, Persona>()
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PrimerApellido,
                    opt => opt.MapFrom(src => src.PrimerApellido))
                .ForMember(dest => dest.SegundoApellido,
                    opt => opt.MapFrom(src => src.SegundoApellido))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email));

                #endregion

                #region Idioma
                config.CreateMap<IdiomaCertificadoDTO, Atestado>()
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.NumeroAutores,
                    opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.HoraCreacion,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Enlace,
                    opt => opt.MapFrom(src => src.Enlace))
                .ForMember(dest => dest.PaisID,
                    opt => opt.MapFrom(src => src.PaisID))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.Persona,
                    opt => opt.MapFrom(src => src.Persona))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID))
                .ForMember(dest => dest.Enviado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Descargado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Lugar,
                    opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.RubroID,
                    opt => opt.MapFrom(src => src.RubroID));

                config.CreateMap<IdiomaCertificadoDTO, DominioIdioma>()
                .ForMember(dest => dest.DominioIdiomaID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.IdiomaID,
                    opt => opt.MapFrom(src => src.IdiomaID))
                .ForMember(dest => dest.Auditiva,
                    opt => opt.MapFrom(src => src.Auditiva))
                .ForMember(dest => dest.Escrito,
                    opt => opt.MapFrom(src => src.Escrito))
                .ForMember(dest => dest.Oral,
                    opt => opt.MapFrom(src => src.Oral))
                .ForMember(dest => dest.Lectura,
                    opt => opt.MapFrom(src => src.Lectura));

                config.CreateMap<DominioIdiomaDTO, DominioIdioma>()
                .ForMember(dest => dest.DominioIdiomaID,
                    opt => opt.MapFrom(src => src.DominioIdiomaID))
                .ForMember(dest => dest.IdiomaID,
                    opt => opt.MapFrom(src => src.IdiomaID))
                .ForMember(dest => dest.Auditiva,
                    opt => opt.MapFrom(src => src.Auditiva))
                .ForMember(dest => dest.Escrito,
                    opt => opt.MapFrom(src => src.Escrito))
                .ForMember(dest => dest.Oral,
                    opt => opt.MapFrom(src => src.Oral))
                .ForMember(dest => dest.Lectura,
                    opt => opt.MapFrom(src => src.Lectura));

                config.CreateMap<IdiomaCertificadoDTO, Fecha>()
                .ForMember(dest => dest.FechaID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.FechaFinal,
                    opt => opt.MapFrom(src => src.Annio));
                #endregion

                #region ponencia
                config.CreateMap<PonenciaDTO, Atestado>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.HoraCreacion,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Lugar,
                    opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.Archivo,
                    opt => opt.MapFrom(src => src.Archivos))
                .ForMember(dest => dest.PaisID,
                    opt => opt.MapFrom(src => src.PaisID))
                .ForMember(dest => dest.RubroID,
                    opt => opt.MapFrom(src => src.RubroID))
                .ForMember(dest => dest.Enviado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Descargado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID));

                config.CreateMap<PonenciaDTO, Fecha>()
                .ForMember(dest => dest.FechaID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.FechaFinal,
                    opt => opt.MapFrom(src => src.FechaPonencia));
                #endregion

                #region proyecto
                config.CreateMap<ProyectoDTO, Atestado>()
                .ForMember(dest => dest.AtestadoID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Observaciones,
                    opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.HoraCreacion,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Lugar,
                    opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Archivo,
                    opt => opt.MapFrom(src => src.Archivos))
                .ForMember(dest => dest.PaisID,
                    opt => opt.MapFrom(src => src.PaisID))
                .ForMember(dest => dest.RubroID,
                    opt => opt.MapFrom(src => src.RubroID))
                .ForMember(dest => dest.Enviado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Descargado,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Codigo,
                    opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.PersonaID,
                    opt => opt.MapFrom(src => src.PersonaID));

                config.CreateMap<ProyectoDTO, Fecha>()
                .ForMember(dest => dest.FechaID,
                    opt => opt.MapFrom(src => src.AtestadoID))
                .ForMember(dest => dest.FechaFinal,
                    opt => opt.MapFrom(src => src.FechaFinal))
                .ForMember(dest => dest.FechaInicio,
                    opt => opt.MapFrom(src => src.FechaInicio));
                #endregion

            });
        }
    }
}
