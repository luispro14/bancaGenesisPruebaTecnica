using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("transferencia")]
        public async Task<IActionResult> Transferir([FromBody] Transferencia transferencia)
        {
            if (transferencia.Monto <= 0)
                return BadRequest("El monto debe ser mayor a cero.");

            var cuentaOrigen = await _context.Cuentas.FindAsync(transferencia.CuentaOrigenId);
            var cuentaDestino = await _context.Cuentas.FindAsync(transferencia.CuentaDestinoId);

            if (cuentaOrigen == null || cuentaDestino == null)
                return NotFound("Una o ambas cuentas no existen.");

            if (cuentaOrigen.disponible < transferencia.Monto)
                return BadRequest("Fondos insuficientes en la cuenta de origen.");

            // Realizar la transferencia
            cuentaOrigen.disponible -= transferencia.Monto;
            cuentaDestino.disponible += transferencia.Monto;

            // Registrar movimiento de salida (origen)
            var movimientoSalida = new Movimiento
            {
                idCuenta = cuentaOrigen.id,
                monto = transferencia.Monto,
                tipo = "salida",
                descripcion = $"Transferencia a cuenta {cuentaDestino.id}: {transferencia.Descripcion}"
            };

            // Registrar movimiento de entrada (destino)
            var movimientoEntrada = new Movimiento
            {
                idCuenta = cuentaDestino.id,
                monto = transferencia.Monto,
                tipo = "entrada",
                descripcion = $"Transferencia desde cuenta {cuentaOrigen.id}: {transferencia.Descripcion}"
            };

            _context.Movimientos.AddRange(movimientoSalida, movimientoEntrada);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Transferencia realizada con éxito",
                cuentaOrigen = cuentaOrigen.id,
                cuentaDestino = cuentaDestino.id,
                monto = transferencia.Monto
            });
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> ListarMovimientos(int id)
        {
            var movimientos = await _context.Movimientos.Where(c => c.idCuenta == id).ToListAsync();
            return Ok(movimientos);
        }

    }
}
