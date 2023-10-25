using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Cine
    {
        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
            {
                int rowAffected = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}', '{cine.Direccion}', '{cine.Zona.IdZona}', '{cine.Ventas}'");
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
        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
            {
                int rowAffected = context.Database.ExecuteSqlRaw($"CineUpdate '{cine.IdCine}','{cine.Nombre}', '{cine.Direccion}', '{cine.Zona.IdZona}', '{cine.Ventas}'");

                if (rowAffected > 0)
                {
                    result.Correct = true;
                    result.ErrorMessage = "Actualizado";
                    result.Ex = null;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error";
                    result.Ex = null;
                }
            }
            return result;
        }
        public static ML.Result Delete(int IdCine)
        {
            ML.Result result = new ML.Result();
            using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
            {
                var rowAffected = context.Database.ExecuteSqlRaw($"CineDelete {IdCine}");
                if (rowAffected > 0)
                {
                    Console.WriteLine("Eliminado");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
            return result;
        }
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
                {
                    var query = context.Cines.FromSqlRaw("CineGetAll").ToList();
                    result.Objects = new List<object>();
                    if (query.Count >= 1)
                    {

                        foreach (var registro in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.IdCine = registro.IdCine;
                            cine.Nombre = registro.Nombre;
                            cine.Direccion = registro.Direccion;
                            cine.Zona = new ML.Zona();
                            cine.Zona.IdZona = registro.IdZona.Value;
                            cine.Ventas = registro.Ventas.Value;
                            result.Objects.Add(cine);
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
        public static ML.Result GetById(int IdCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.AaguileraCineContext context = new DL.AaguileraCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"CineGetById '{IdCine}'").AsEnumerable().SingleOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.IdCine = query.IdCine;
                        cine.Nombre = query.Nombre;
                        cine.Direccion = query.Direccion;
                        cine.Zona = new ML.Zona();
                        cine.Zona.IdZona = query.IdZona.Value;
                        cine.Ventas = query.Ventas.Value;
                        result.Objects.Add(cine);

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