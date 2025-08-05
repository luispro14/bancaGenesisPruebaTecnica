using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> ListarCuentas()
        {
            var cuentas = _context.Cuentas.ToList();
            return Ok(cuentas);
        }



    }
}
