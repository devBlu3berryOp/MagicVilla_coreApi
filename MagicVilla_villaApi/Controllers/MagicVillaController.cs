using MagicVilla_villaApi.Data;
using MagicVilla_villaApi.Logging;
using MagicVilla_villaApi.Model;
using MagicVilla_villaApi.Model.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_villaApi.Controllers
{
    [Route("api/MagicVilla")]
    [ApiController]                     //this helps in checking the data annotation that is applied to the model or else (ModelState.IsValid) is used for that
    public class MagicVillaController : ControllerBase
    {
        private readonly Ilogger _logger;
        public MagicVillaController(Ilogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.log("Getting all villas","");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}",Name = "GetVilla")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.log("Encountered error on villa with id= "+id,"error");
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return villa;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villadto)
        {
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villadto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom Error", "Name already exits..");                //custom validation error creation
                return BadRequest(ModelState);
            }
            if (villadto.Id == null)
            {
                return BadRequest(villadto);
            }
            if (villadto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villadto.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villadto);
            //return Ok(villadto);
            return CreatedAtRoute("GetVilla", new { id = villadto.Id }, villadto);            //inorder to return the url on creation of new villa
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}",Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villadto)
        {
            if (villadto.Id == 0 || id != villadto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == villadto.Id);
            if (villa == null)
            {
                return BadRequest();
            }
            villa.Name = villadto.Name;
            villa.occupancy = villadto.occupancy;
            villa.sqft = villadto.sqft;
            return NoContent();
        }
        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patch)
        {
            if (patch == null || id == 0)
            {
                return BadRequest();
            }
            var villa=VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return BadRequest();
            }
            patch.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
