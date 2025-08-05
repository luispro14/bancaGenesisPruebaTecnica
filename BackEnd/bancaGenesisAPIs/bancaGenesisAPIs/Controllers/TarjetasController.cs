using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bancaGenesisAPIs.Controllers
{
    [ApiController]
    [Route("api/tarjetas/[controller]")]
    public class TarjetasController : ControllerBase
    {

        private readonly AppDbContext _context;

        public TarjetasController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        [HttpPost]
        public async Task<IActionResult> CreateTarjeta([FromBody] Tarjeta tarjeta)
        {
            _context.Tarjetas.Add(tarjeta);
            await _context.SaveChangesAsync();
            return Ok(tarjeta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarTarjetas(int id)
        {
            var tarjetas = await _context.Tarjetas.Where(x => x.idCuenta == id).ToListAsync();
            return Ok(tarjetas);
        }

    }
}
