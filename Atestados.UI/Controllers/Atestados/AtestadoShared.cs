using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Atestados.Datos.Modelo;
using Atestados.Negocios.Negocios;
using Atestados.Objetos.Dtos;
using Newtonsoft.Json;

/* 
 * Originalmente se quería hacer una interfaz que tuviera todos los métodos
 * comunes que pueden utilizar múltiples atestados. Sin embargo, ya que se está
 * usando una versión vieja de C#, no existe la implementación default de métodos
 * dentro de una interfaz. Por lo que se optó por hacer una clase que utiliza el
 * patrón singleton que contenga los métodos de uso común entre varios atestados.
 */

namespace Atestados.UI.Controllers.Atestados
{
    public class AtestadoShared
    {
        public static readonly AtestadoShared obj = new AtestadoShared();

        AtestadoShared() { }

        public void guardarArchivos(List<ArchivoDTO> archivos, InformacionAtestado infoAtestado,
            Atestado atestado_mapped)
        {
            foreach (ArchivoDTO archivo in archivos)
            {
                Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                ar.AtestadoID = atestado_mapped.AtestadoID;
                infoAtestado.GuardarArchivo(ar);
            }
        }

        public void guardarAutores(List<AutorDTO> autores, InformacionGeneral infoGeneral,
            InformacionAtestado infoAtestado, bool autoresEq, Atestado atestado_mapped)
        {
            int porcentajeEq = 100 / autores.Count;

            foreach (AutorDTO autor in autores)
            {
                // En caso de no ser un funcionario, agregarlo como Autor externo al Tec.
                if (!autor.esFuncionario)
                {
                    Persona persona = AutoMapper.Mapper.Map<AutorDTO, Persona>(autor);
                    persona.CategoriaActual = 1;
                    persona.TipoUsuario = 4;
                    infoGeneral.GuardarPersona(persona);
                    if (autoresEq)
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = persona.PersonaID,
                            Porcentaje = porcentajeEq
                        });
                    else
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = persona.PersonaID,
                            Porcentaje = autor.Porcentaje
                        });
                }
                // En caso de ser un funcionario, obtener sus datos de la BD y agregarlo.
                else
                {

                    UsuarioDTO usuario = infoGeneral.UsuarioPorEmail(autor.Email);
                    var id = usuario.UsuarioID;

                    if (autoresEq)
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = id,
                            Porcentaje = porcentajeEq
                        });
                    else
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = id,
                            Porcentaje = autor.Porcentaje
                        });
                }
            }

        }

        /*Las funciones de editar archivos y autores primero borran lo que se
        contiene en la base de datos y luego agregan los valores editados. Esto
        se hace de esta forma porque es lo más fácil, no necesariamente lo más
        eficiente. Estas funciones se pueden optimizar.*/
        public void editarArchivos(List<ArchivoDTO> archivosOld, List<ArchivoDTO> archivos, InformacionAtestado infoAtestado,
            Atestado atestado_mapped)
        {
            foreach (ArchivoDTO archivo in archivosOld)
            {
                infoAtestado.BorrarArchivo(archivo.ArchivoID);
            }
            foreach (ArchivoDTO archivo in archivos)
            {
                Archivo ar = AutoMapper.Mapper.Map<ArchivoDTO, Archivo>(archivo);
                ar.AtestadoID = atestado_mapped.AtestadoID;
                infoAtestado.GuardarArchivo(ar);
            }
        }

        
        public void editarAutores(List<AutorDTO> autores, InformacionGeneral infoGeneral,
            InformacionAtestado infoAtestado, bool autoresEq, Atestado atestado_mapped)
        {
            int porcentajeEq = 100 / autores.Count;

            infoAtestado.BorrarAtestadoXPersona(atestado_mapped.AtestadoID);

            foreach (AutorDTO autor in autores)
            {
                // En caso de no ser un funcionario, agregarlo como Autor externo al Tec.
                if (!autor.esFuncionario)
                {
                    Persona persona = AutoMapper.Mapper.Map<AutorDTO, Persona>(autor);
                    persona.CategoriaActual = 1;
                    persona.TipoUsuario = 4;
                    infoGeneral.GuardarPersona(persona);
                    if (autoresEq)
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = persona.PersonaID,
                            Porcentaje = porcentajeEq
                        });
                    else
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = persona.PersonaID,
                            Porcentaje = autor.Porcentaje
                        });
                }
                // En caso de ser un funcionario, obtener sus datos de la BD y agregarlo.
                else
                {

                    UsuarioDTO usuario = infoGeneral.UsuarioPorEmail(autor.Email);
                    var id = usuario.UsuarioID;

                    if (autoresEq)
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = id,
                            Porcentaje = porcentajeEq
                        });
                    else
                        infoAtestado.GuardarAtestadoXPersona(new AtestadoXPersona()
                        {
                            AtestadoID = atestado_mapped.AtestadoID,
                            PersonaID = id,
                            Porcentaje = autor.Porcentaje
                        });
                }
            }

        }

    }
}
