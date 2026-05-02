using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI.Data;
using ClienteAPI.Models;

namespace ClienteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposDocumentosController : ControllerBase
    {
        private readonly BdClientesContext _context;

        public TiposDocumentosController(BdClientesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiposDocumento>>> GetTiposDocumentos()
        {
            return await _context.TiposDocumentos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TiposDocumento>> GetTiposDocumento(byte id)
        {
            var tipoDocumento = await _context.TiposDocumentos.FindAsync(id);
            if (tipoDocumento == null) return NotFound();
            return tipoDocumento;
        }

        [HttpPost]
        public async Task<ActionResult<TiposDocumento>> PostTiposDocumento(TiposDocumento tiposDocumento)
        {
            _context.TiposDocumentos.Add(tiposDocumento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTiposDocumento), new { id = tiposDocumento.IdTipoDocumento }, tiposDocumento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiposDocumento(byte id, TiposDocumento tiposDocumento)
        {
            if (id != tiposDocumento.IdTipoDocumento) return BadRequest();
            _context.Entry(tiposDocumento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTiposDocumento(byte id)
        {
            var tipoDocumento = await _context.TiposDocumentos.FindAsync(id);
            if (tipoDocumento == null) return NotFound();
            _context.TiposDocumentos.Remove(tipoDocumento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}