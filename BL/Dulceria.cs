using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Dulceria
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw("DulceriaGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query.Count >= 1)
                    {

                        foreach (var registro in query)
                        {
                            ML.Dulceria dulceria = new ML.Dulceria();
                            dulceria.IdDulceria = registro.IdDulceria;
                            dulceria.Nombre = registro.Nombre;
                            dulceria.Precio = registro.Precio.Value;
                            dulceria.Imagen = registro.Imagen;
                            result.Objects.Add(dulceria);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron los registros";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetById(int IdDulceria)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw($"DulceriaGetById '{IdDulceria}'").AsEnumerable().SingleOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        ML.Dulceria dulceria = new ML.Dulceria();
                        dulceria.IdDulceria = query.IdDulceria;
                        dulceria.Nombre = query.Nombre;
                        dulceria.Precio = query.Precio.Value;
                        dulceria.Imagen = query.Imagen;
                        result.Object = dulceria;
                        result.Objects.Add(dulceria);

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
    }
}