using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Pdf;

namespace PL.Controllers
{
    public class Dulceria : Controller
    {
        public IActionResult Index()
        {
            ML.Dulceria dulceria = new ML.Dulceria();
            ML.Result result = BL.Dulceria.GetAll();
            dulceria.Dulcerias = result.Objects;
            return View(dulceria);
        }
        public IActionResult AddCarrito(int IdDulceria)
        {
            bool existe = false;
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            ML.Result result = BL.Dulceria.GetById(IdDulceria);
            if (HttpContext.Session.GetString("Carrito") == null)
            {

                if (result.Correct)
                {
                    ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                    dulceria.Cantidad = 1;
                    carrito.Carritos.Add(dulceria);
                    //serializar carrito
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }

            }
            else
            {

                ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                GetCarrito(carrito); //ya recupere el carrito
                foreach (ML.Dulceria dulceria1 in carrito.Carritos)
                {
                    if (dulceria.IdDulceria == dulceria1.IdDulceria)
                    {
                        dulceria.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe == true)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }
                else
                {
                    dulceria.Cantidad = 1;
                    carrito.Carritos.Add(dulceria);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }

            }

            return RedirectToAction("Index");
        }
        public ML.Carrito GetCarrito(ML.Carrito carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var obj in ventaSession)
            {
                ML.Dulceria objMateria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(obj.ToString());
                carrito.Carritos.Add(objMateria);
            }
            return carrito;
        }
        public IActionResult Carrito()
        {
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }

        }
        public ActionResult GenerarPDF()
        {
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            GetCarrito(carrito);

            // Crear un nuevo documento PDF en una ubicación temporal
            string rutaTempPDF = Path.GetTempFileName() + ".pdf";

            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(rutaTempPDF)))
            {
                using (Document document = new Document(pdfDocument))
                {
                    document.Add(new Paragraph("Resumen de Compra"));

                    // Crear la tabla para mostrar la lista de objetos
                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(5); // 5 columnas
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho de la tabla al 100% del documento

                    // Añadir las celdas de encabezado a la tabla
                    table.AddHeaderCell("ID Producto");
                    table.AddHeaderCell("Producto");
                    table.AddHeaderCell("Precio Unitario");
                    table.AddHeaderCell("Cantidad");
                    table.AddHeaderCell("Imagen");


                    foreach (ML.Dulceria dulceria in carrito.Carritos)
                    {
                        table.AddCell(dulceria.IdDulceria.ToString());
                        table.AddCell(dulceria.Nombre);
                        table.AddCell(dulceria.Precio.ToString());
                        table.AddCell(dulceria.Cantidad.ToString());
                        byte[] imageBytes = Convert.FromBase64String(dulceria.Imagen);
                        ImageData data = ImageDataFactory.Create(imageBytes);
                        Image image = new Image(data);
                        table.AddCell(image.SetWidth(50).SetHeight(50));

                    }

                    // Añadir la tabla al documento
                    document.Add(table);
                }
            }

            // Leer el archivo PDF como un arreglo de bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaTempPDF);

            // Eliminar el archivo temporal
            System.IO.File.Delete(rutaTempPDF);
            HttpContext.Session.Remove("Carrito");

            // Descargar el archivo PDF
            return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
            {
                FileDownloadName = "ReporteProductos.pdf"
            };
        }

    }
}