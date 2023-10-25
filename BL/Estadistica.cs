using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Estadistica
    {
        //metodo para calcular los promedios
        public static ML.Estadistica CaculcarPorcentaje(List<object> cines)
        {
            ML.Estadistica estadistica = new ML.Estadistica();
            decimal total = 0;
            try
            {
                foreach (ML.Cine cine in cines)
                {
                    if (cine.Zona.IdZona == 1)
                    {
                        estadistica.Norte += cine.Ventas;
                    }
                    if(cine.Zona.IdZona == 2)
                    {
                        estadistica.Sur += cine.Ventas;
                    }
                    if(cine.Zona.IdZona == 3)
                    {
                        estadistica.Este += cine.Ventas;
                    }
                    if (cine.Zona.IdZona == 4)
                    {
                        estadistica.Oeste += cine.Ventas;
                    }
                    //repetir proceso
                    //calcular total
                    total += cine.Ventas;
                }
                estadistica.Norte = (estadistica.Norte / total) * 100;
                estadistica.Sur = (estadistica.Sur / total) * 100;
                estadistica.Este = (estadistica.Este / total) * 100;
                estadistica.Oeste = (estadistica.Oeste / total) * 100;
                //repetir calculo


            }
            catch (Exception ex)
            {

            }
            return estadistica;
        }
    }
}