using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cMinesweeperApi.Models;

namespace cMinesweeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellsController : ControllerBase
    {
        private readonly CellContext _context;

        public CellsController(CellContext context)
        {
            _context = context;
        }

        // GET: api/Cells
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cell>>> GetCells()
        {
            return await _context.Cells.ToListAsync();
        }

        // GET: api/Cells/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cell>> GetCell(long id)
        {
            var cell = await _context.Cells.FindAsync(id);

            if (cell == null)
            {
                return NotFound();
            }

            return cell;
        }

        // GET: api/BoardCells/5
        [HttpGet("{id}")]
        public List<Cell> GetBoardCells(long id)
        {
            var cell = _context.Cells.Where(c => c.boardId == id).ToList();

            return cell;
        }

        // PUT: api/Cells/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCell(long id, Cell cell)
        {
            if (id != cell.Id)
            {
                return BadRequest();
            }

            _context.Entry(cell).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CellExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cells
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cell>> PostCell(Cell cell)
        {
            _context.Cells.Add(cell);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCell", new { id = cell.Id }, cell);
        }

        // DELETE: api/Cells/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCell(long id)
        {
            var cell = await _context.Cells.FindAsync(id);
            if (cell == null)
            {
                return NotFound();
            }

            _context.Cells.Remove(cell);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CellExists(long id)
        {
            return _context.Cells.Any(e => e.Id == id);
        }
    }
}
