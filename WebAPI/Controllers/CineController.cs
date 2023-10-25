using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Cine.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }

        }
        [Route("GetById/{idCine?}")]
        [HttpGet]
        public IActionResult GetById(int idCine)
        {
            var result = BL.Cine.GetById(idCine);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromBody] ML.Cine cine)
        {
            ML.Result result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
        [Route("Delete/{idCine?}")]
        [HttpDelete]
        public IActionResult Delete(int idCine)
        {
            ML.Result result = BL.Cine.Delete(idCine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }

        [Route("Update/{idCine?}")]
        [HttpPut]
        public IActionResult Update(int idCine, [FromBody] ML.Cine cine)
        {
            cine.IdCine = idCine;
            ML.Result result = BL.Cine.Update(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
    }
}