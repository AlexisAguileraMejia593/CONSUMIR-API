namespace ML
{
    public class Cine
    {
        public Cine()
        {
        }
        public Cine(List<object> cines)
        {
            Cines = cines;
        }
        public int IdCine { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public Zona Zona { get; set; }
        public decimal Ventas {  get; set; }
        public List<object> Cines { get; set; }
        public Estadistica estadistica { get; set; }
        public decimal Latitud {  get; set; }
        public decimal Longitud { get; set;}
    }
}