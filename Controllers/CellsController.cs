using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cMinesweeperApi.Models;
using System.Net.Http;
using System.Text.Json;

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

        // PUT: api/Cells/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCell(long id, Cell cell)
        {
            if (id != cell.id)
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

            return CreatedAtAction("GetCell", new { id = cell.id }, cell);
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
            return _context.Cells.Any(e => e.id == id);
        }

        // PUT: api/Cells/flag?id=5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("flag")]
        public async Task<IActionResult> FlagCell(long id)
        {
            Cell cell = await _context.Cells.FindAsync(id);
            if (id != cell.id)
            {
                return BadRequest();
            }

            cell.hasFlag = !cell.hasFlag;

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

        // PUT: api/Cells/question?id=5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("question")]
        public async Task<IActionResult> QuestionCell(long id)
        {
            Cell cell = await _context.Cells.FindAsync(id);
            if (id != cell.id)
            {
                return BadRequest();
            }

            cell.hasQuestion = !cell.hasQuestion;

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

        // PUT: api/Cells/reveal?id=5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("revealcell")]
        public async Task<IActionResult> RevealCell(long id)
        {
            Cell cell = await _context.Cells.FindAsync(id);
            if (id != cell.id)
            {
                return BadRequest();
            }

            cell.hasFlag = false;
            cell.isUncovered = true;
            if (!cell.hasBomb)
            {
                RevealZeros(id);
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

        private void Reveal(long id)
        {
            
            Cell cell = _context.Cells.Find(id);
            cell.isUncovered = true;
            cell.hasFlag = false;

            _context.Entry(cell).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        private void RevealPanel(int x, int y, long boardId)
        {
            //Step 1: Find and reveal the clicked panel
            Cell selectedPanel = _context.Cells.FirstOrDefault(panel => panel.boardId == boardId
                                                        && panel.x == x
                                                        && panel.y == y);
            // selectedPanel.Reveal(selectedPanel.id);
            Reveal(selectedPanel.id);

            //Step 2: If the panel is a mine, show all mines. Game over!
            if (selectedPanel.hasFlag)
            {
                CompletionCheck(boardId, 4);
                RevealAllMines(boardId);
                return;
            }

            //Step 3: If the panel is a zero, cascade reveal neighbors.
            if (selectedPanel.neightbors == 0)
            {
                RevealZeros(selectedPanel.id);
            }

            //Step 4: If this move caused the game to be complete, mark it as such
            CompletionCheck(boardId, 4);
        }

        private List<Cell> GetNeighbors(long id)
        {
            Cell cell = _context.Cells.Find(id);
            int x = cell.x;
            int y = cell.y;
            long boardId = cell.boardId;

            var nearbyPanels = _context.Cells.Where(panel => panel.boardId == boardId
                                                    && panel.x >= (x - 1)
                                                    && panel.x <= (x + 1)
                                                    && panel.y >= (y - 1)
                                                    && panel.y <= (y + 1));

            var currentPanel = _context.Cells.Where(panel => panel.boardId == boardId
                                                    && panel.x == x
                                                    && panel.y == y);

            return nearbyPanels.Except(currentPanel).ToList();
        }

        private void RevealAllMines(long boardId)
        {
            _context.Cells.Where(x => x.hasBomb && x.boardId == boardId)
                  .ToList()
                  .ForEach(x => x.isUncovered = true);
        }

        private void RevealZeros(long i)
        {
            //Get all neighbor panels
            var neighborPanels = GetNeighbors(i)
                                   .Where(panel => !panel.isUncovered);

            foreach (var neighbor in neighborPanels)
            {
                //For each neighbor panel, reveal that panel.
                neighbor.isUncovered = true;

                //If the neighbor is also a 0, reveal all of its neighbors too.
                if (neighbor.neightbors == 0)
                {
                    RevealZeros(neighbor.id);
                }
            }
        }

        private async void CompletionCheck(long boardId, int status)
        {
            var hiddenPanels = _context.Cells.Where(x => !x.isUncovered && x.boardId == boardId)
                                     .Select(x => x.id);

            var minePanels = _context.Cells.Where(x => x.hasBomb && x.boardId == boardId)
                                   .Select(x => x.id);

            if (!hiddenPanels.Except(minePanels).Any())
            {
                HttpClient client = new HttpClient();
                string baseURL =  HttpContext.Request.Scheme.ToString() + "://" + HttpContext.Request.Host.ToString();
                await client.PutAsync(
                    baseURL + "/api/Boards/completioncheck/id=" + boardId + "&status=" + status,
                    null
                );
            }
        }
    }
}
