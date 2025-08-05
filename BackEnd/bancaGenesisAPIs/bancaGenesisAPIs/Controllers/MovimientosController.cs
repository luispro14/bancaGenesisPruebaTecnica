using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using Microsoft.AspNetCore.Mvc;

namespace bancaGenesisAPIs.Controllers
{
    [ApiController]
    [Route("api/movimientos/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimientosController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        [HttpPost("entrada")]
        public async Task<IActionResult> CreateMovimientoEntrada([FromBody] Movimiento movimiento)
        {
            var item = await _context.Cuentas.FindAsync(movimiento.idCuenta);
            if (item == null)
                return NotFound($"La cuenta con el id {movimiento.idCuenta} no se encontro.");

            item.disponible += movimiento.monto;
            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();
            return Ok(movimiento);
        }


        [HttpPost("salida")]
        public async Task<IActionResult> CreateMovimientoSalida([FromBody] Movimiento movimiento)
        {
            var item = await _context.Cuentas.FindAsync(movimiento.idCuenta);
            if (item == null)
                return NotFound($"La cuenta con el id {movimiento.idCuenta} no se encontro.");

            item.disponible -= movimiento.monto;
            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();
            return Ok(movimiento);
        }

        [HttpPost]
        public async Task<IActionResult> ListarMovimientos()
        {
            var movimientos = _context.Movimientos.ToList();
            return Ok(movimientos);
        }

    }
}
