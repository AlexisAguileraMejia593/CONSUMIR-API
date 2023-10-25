using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PL.Models;

namespace PL.Controllers
{
    public class PeliculasController : Controller
    {
        public ActionResult GetAll()
        {
            Movies movies = new Movies();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responseTask = client.GetAsync("movie/popular?api_key=19cb02bec6651b55d7a26dcfd15a0955");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result);    
                    readTask.Wait();
                    movies.Peliculas = new List<object>();
                    foreach (var resultItem in resultJSON.results)
                    {
                        Movies itemmovies = new Movies();
                        itemmovies.adult = resultItem.adult;
                        itemmovies.backdrop_path = resultItem.backdrop_path;

                        itemmovies.id = resultItem.id;
                        itemmovies.original_language = resultItem.original_language;
                        itemmovies.original_title = resultItem.original_title;
                        itemmovies.overview = resultItem.overview;
                        itemmovies.popularity = resultItem.popularity;
                        itemmovies.poster_path = "https://www.themoviedb.org/t/p/w220_and_h330_face/" + resultItem.poster_path;
                        itemmovies.release_date = resultItem.release_date;
                        itemmovies.title = resultItem.title;
                        itemmovies.video = resultItem.video;
                        itemmovies.vote_average = resultItem.vote_average;
                        itemmovies.vote_count = resultItem.vote_count;
                        movies.Peliculas.Add(itemmovies);
                    }
                }
            }
            return View(movies);
        }
        [HttpGet]
        public ActionResult AñadirFavoritos(int IdMovie)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var json = new
                {
                    media_type = "movie",
                    media_id = IdMovie,
                    favorite = true
                };

                var postTask = client.PostAsJsonAsync("account/20523574/favorite?api_key=19cb02bec6651b55d7a26dcfd15a0955&session_id=bb7a3229e46557420322b5fabe2ce73744170c88", json);
                postTask.Wait();

                var resultMovie = postTask.Result;
                if (resultMovie.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Pelicula añadida a tus favoritos!";
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrio un error al añadir la pelicula a tus favoritos";
                }
            }
            return RedirectToAction("GetAll", "Peliculas");
        }
        [HttpPost]
        public ActionResult Favorites(int? page)
        {
            Movies movies = new Movies();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
                page = page == null ? 1 : page;
                string url = "account/20522085/favorite/movies?api_key=19cb02bec6651b55d7a26dcfd15a0955&session_id=bb7a3229e46557420322b5fabe2ce73744170c88&language=es-ES&page=" + page;

                var responseTask = client.GetAsync(url);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movies.Peliculas = new List<object>();
                    foreach (var resultItem in resultJSON.results)
                    {
                        Movies itemmovies = new Movies();
                        itemmovies.adult = resultItem.adult;
                        itemmovies.backdrop_path = resultItem.backdrop_path;

                        itemmovies.id = resultItem.id;
                        itemmovies.original_language = resultItem.original_language;
                        itemmovies.original_title = resultItem.original_title;
                        itemmovies.overview = resultItem.overview;
                        itemmovies.popularity = resultItem.popularity;
                        itemmovies.poster_path = "https://www.themoviedb.org/t/p/w220_and_h330_face/" + resultItem.poster_path;
                        itemmovies.release_date = resultItem.release_date;
                        itemmovies.title = resultItem.title;
                        itemmovies.video = resultItem.video;
                        itemmovies.vote_average = resultItem.vote_average;
                        itemmovies.vote_count = resultItem.vote_count;
                        movies.Peliculas.Add(itemmovies);
                    }
                }
                ViewBag.page = page;
            }
            return View(movies);
        }
    }
}