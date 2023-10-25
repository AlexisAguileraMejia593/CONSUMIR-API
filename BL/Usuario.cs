using DL;
using Microsoft.EntityFrameworkCore;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetByEmail(string username)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{username}'").AsEnumerable().SingleOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.Email = query.Email;
                        usuario.Password = query.Password;
                        result.Object = usuario;
                        result.Objects.Add(usuario);
                    }
                    
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception and return an empty list
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            // Return the list of objects
            return result;
        }
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
            {
                int rowAffected = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.Email}', '{usuario.Password}'");
                if (rowAffected > 0)
                {
                    result.Correct = true;
                    result.ErrorMessage = "Cine insertado correctamente";
                    result.Ex = null;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error al insertar el cine";
                    result.Ex = null;
                }
            }
            return result;
        }
    }
}