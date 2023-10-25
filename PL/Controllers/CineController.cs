using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        /*
           [HttpGet]     
            public IActionResult GetAll()
            {
                ML.Cine cine = new ML.Cine();
                ML.Result result = BL.Cine.GetAll();
                cine.Cines = result.Objects;
                return View(cine);
        }
         */
        [HttpGet]
        public ActionResult Mapa()
        {
            ML.Result result = BL.Cine.GetAll();
            ML.Cine cine = new ML.Cine(result.Objects);
            ML.Estadistica estadistica = BL.Estadistica.CaculcarPorcentaje(cine.Cines);
            cine.estadistica = new ML.Estadistica(estadistica.Norte, estadistica.Sur, estadistica.Este, estadistica.Oeste);
            return View(cine);
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Cine resultcine = new ML.Cine();
            resultcine.Cines = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5265/api/");
                var responseTask = client.GetAsync("cine/GetAll");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultItem.ToString());
                        resultcine.Cines.Add(resultItemList);
                    }
                }
            }
            return View(resultcine);
        }
        /*       [HttpGet]
               public IActionResult Form(int? IdCine)
               {
                   ML.Cine cine = new ML.Cine();
                   if (IdCine != null)
                   {
                       ML.Result result = BL.Cine.GetById(IdCine.Value);
                       if (result.Correct)
                       {
                           //UNBOXING
                           cine = (ML.Cine)result.Object;
                       }
                   }
                   else
                   {

                   }
                   return View(cine);
               }

       */
        [HttpGet]
        public ActionResult Form(int? IdCine)
        {
            ML.Cine cine = new ML.Cine();
            if (IdCine != null)
            {
                using (var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri("http://localhost:5265/api/");
                    var responseTask = cliente.GetAsync("cine/GetById/" + IdCine);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultItem.ToString());
                            cine = resultItemList;
                        }
                    }

                }
            }
            return View(cine);
        }
        /*
       [HttpPost]
             public ActionResult Form(ML.Cine cine)
             {
                 if (ModelState.IsValid)
                 {
                     if (cine.IdCine == 0) //ADD
                     {
                             BL.Cine.Add(cine);
                             ViewBag.Mensaje = "Se ha completado el registro";
                     }
                     else //UPDATE
                     {
                         BL.Cine.Update(cine);
                         if (cine.IdCine != null)
                         {

                             ViewBag.Mensaje = "Se ha completado la actualizacion";

                         }
                         else
                         {
                             ViewBag.Mensaje = "Error";
                         }
                     }
                 }
                 else
                 {

                 }
                 return PartialView("Modal");
             }
      */
        [HttpPost]
        public ActionResult Form(ML.Cine cine)
        {
            if (cine.IdCine == 0) //ADD
            {
                using (var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri("http://localhost:5265/api/");

                    var postTask = cliente.PostAsJsonAsync("cine/Add", cine);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se Agrego correctamente el Cine";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se pudo agregar el Cine";
                    }
                }
            }
            else //UPDATE
            {
                using (var cliente = new HttpClient())
                {
                    int IdCine = cine.IdCine;

                    cliente.BaseAddress = new Uri("http://localhost:5265/api/");

                    var putTask = cliente.PutAsJsonAsync("cine/Update/" + IdCine, cine);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se actualizo correctamente el cine";
                    }
                    else
                    {
                        ViewBag.Mensaje = "No se logro actualizar el cine";
                    }
                }
            }
            return PartialView("Modal");
        }

        public ActionResult Delete(int? IdCine)
        {
            BL.Cine.Delete(IdCine.Value);
            if (IdCine > 0)
            {
                ViewBag.Mensaje = "Se ha completado el borrar";
            }
            else
            {
                ViewBag.Mensaje = "Error";
            }
            return PartialView("Modal");
        }

        /*      public ActionResult Delete(int IdCine)
              {
                  ML.Result resultListcine = new ML.Result();
                  using (var client = new HttpClient())
                  {
                      client.BaseAddress = new Uri("http://localhost:5265/api/");

                      //HTTP POST
                      var postTask = client.GetAsync("cine/Delete/" + IdCine);
                      postTask.Wait();

                      var result = postTask.Result;
                      if (result.IsSuccessStatusCode)
                      {
                          return RedirectToAction("GetAll");
                      }
                  }
                  return View("Error");
              }
        */
    }
}