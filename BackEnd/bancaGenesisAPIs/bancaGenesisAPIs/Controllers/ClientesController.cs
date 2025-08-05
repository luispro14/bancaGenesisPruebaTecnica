using bancaGenesisAPIs.Data;
using bancaGenesisAPIs.Models;
using Microsoft.AspNetCore.Mvc;

namespace bancaGenesisAPIs.Controllers
{
    [ApiController]
    [Route("api/cliente/[controller]")]
    public class ClientesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientes([FromBody] Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return Ok(cliente);
        }

        [HttpGet("id")]
        public async Task<IActionResult> ListarClientes(int id)
        {
            var clientes = _context.Clientes.FindAsync(id);
            if(clientes == null)
            {
                return NotFound();
            }
            return Ok(clientes);
        }
    } 
}
