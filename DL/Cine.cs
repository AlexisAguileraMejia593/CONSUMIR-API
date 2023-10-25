using System;
using System.Collections.Generic;

namespace DL;

public partial class Cine
{
    public int IdCine { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public int? IdZona { get; set; }

    public decimal? Ventas { get; set; }

    public virtual Zona? IdZonaNavigation { get; set; }
}
