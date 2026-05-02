using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesDocumentosController : ControllerBase
    {
        private readonly BdClientesContext _context;

        public ClientesDocumentosController(BdClientesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientesDocumento>>> GetClientesDocumentos()
        {
            return await _context.ClientesDocumentos
                .Include(cd => cd.IdClienteNavigation)
                .Include(cd => cd.IdTipoDocumentoNavigation)
                .ToListAsync();
        }

        [HttpGet("{idCliente}/{idTipoDocumento}")]
        public async Task<ActionResult<ClientesDocumento>> GetClientesDocumento(int idCliente, byte idTipoDocumento)
        {
            var clienteDocumento = await _context.ClientesDocumentos
                .Include(cd => cd.IdClienteNavigation)
                .Include(cd => cd.IdTipoDocumentoNavigation)
                .FirstOrDefaultAsync(cd => cd.IdCliente == idCliente && cd.IdTipoDocumento == idTipoDocumento);
            if (clienteDocumento == null) return NotFound();
            return clienteDocumento;
        }

        [HttpPost]
        public async Task<ActionResult<ClientesDocumento>> PostClientesDocumento(ClientesDocumento clientesDocumento)
        {
            _context.ClientesDocumentos.Add(clientesDocumento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientesDocumentoExists(clientesDocumento.IdCliente, clientesDocumento.IdTipoDocumento))
                    return Conflict();
                throw;
            }
            return CreatedAtAction(nameof(GetClientesDocumento),
                new { idCliente = clientesDocumento.IdCliente, idTipoDocumento = clientesDocumento.IdTipoDocumento },
                clientesDocumento);
        }

        [HttpPut("{idCliente}/{idTipoDocumento}")]
        public async Task<IActionResult> PutClientesDocumento(int idCliente, byte idTipoDocumento, ClientesDocumento clientesDocumento)
        {
            if (idCliente != clientesDocumento.IdCliente || idTipoDocumento != clientesDocumento.IdTipoDocumento)
                return BadRequest();
            _context.Entry(clientesDocumento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{idCliente}/{idTipoDocumento}")]
        public async Task<IActionResult> DeleteClientesDocumento(int idCliente, byte idTipoDocumento)
        {
            var clienteDocumento = await _context.ClientesDocumentos
                .FirstOrDefaultAsync(cd => cd.IdCliente == idCliente && cd.IdTipoDocumento == idTipoDocumento);
            if (clienteDocumento == null) return NotFound();
            _context.ClientesDocumentos.Remove(clienteDocumento);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ClientesDocumentoExists(int idCliente, byte idTipoDocumento)
        {
            return _context.ClientesDocumentos.Any(e => e.IdCliente == idCliente && e.IdTipoDocumento == idTipoDocumento);
        }
    }
}