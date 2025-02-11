using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShopSem4;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductsController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductRepository _repo;

        public ProductsController(ProductRepository repository)
        {
            _repo = repository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {

            var x = _repo.GetAll();
            if (x != null && x.Count() != 0)
            {
                return Ok(x);
            }
            return NotFound(x);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            Product? x = _repo.GetById(id);
            if (x != null)
            {
                return Ok(x);
            }
            return NotFound(x);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product x)
        {
            try
            {
                Debug.WriteLine("************");
                _repo.Add(x);
                return Created("/" + x.Id, x);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public ActionResult<Product> Delete(int id)
        {
            Product? x = _repo.GetById(id);
            if (x != null)
            {
                _repo.Remove(x.Id);
                return Ok(x);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
