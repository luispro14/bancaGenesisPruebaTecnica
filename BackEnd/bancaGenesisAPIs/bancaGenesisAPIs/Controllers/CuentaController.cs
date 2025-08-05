using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bancaGenesisAPIs.Controllers
{
    [ApiController]
    [Route("api/Cuenta/[controller]")]
    public class CuentaController :  ControllerBase
    {

        private readonly AppDbContext _context;

        public CuentaController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCuenta([FromBody] Cuenta cuenta)
        {
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
            return Ok(cuenta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarCuentas(int id)
        {
            var cuentas = await _context.Cuentas.Where(x => x.idCliente == id).ToListAsync();
            return Ok(cuentas);
        }



    }
}
