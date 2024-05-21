using CRUD.Models;
using CRUD.NewFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext _dbContext;


        public BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ActionName(nameof(GetBrands))]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {

            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            return await _dbContext.Brands.ToListAsync();


        }


        [HttpGet("{id}")]
        [ActionName(nameof(GetBrand))]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }

        [HttpPost]
        [ActionName(nameof(Brand))]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)



        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();

            return brand;
        }


        [HttpPut]
        [ActionName(nameof(PutBrand))]

        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (BrandAvailable(id))
                { return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteBrand))]

        public async Task<IActionResult> DeleteBrand(int id)
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }



    }



}
