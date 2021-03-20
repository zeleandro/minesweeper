using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cMinesweeperApi.Models;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Net;

namespace cMinesweeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly BoardContext _context;



        public BoardsController(BoardContext context)
        {
            _context = context;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
        {
            return await _context.Boards.ToListAsync();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(long id)
        {
            var board = await _context.Boards.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }
            
            HttpClient client = new HttpClient();
            try
            {
                string responseBody = await client.GetStringAsync("https://localhost:5001/api/Cells");
                board.cells = JsonSerializer.Deserialize<List<Cell>>(responseBody);
                board.cells = board.cells.Where(c => c.boardId == id).ToList();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return board;
        }

        // PUT: api/Boards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(long id, Board board)
        {
            if (id != board.Id)
            {
                return BadRequest();
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        // POST: api/Boards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Board>> PostBoard(Board board)
        {
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            var response = CreatedAtAction(nameof(GetBoard), new { id = board.Id }, board);

            board.cells = new List<Cell>();

            for (int i = 0; i <= board.rows - 1; i++)
            {
                for (int j = 0; j <= board.columns - 1; j++)
                {
                    Cell aCell = new Cell();
                    aCell.boardId = board.Id;
                    aCell.x = i;
                    aCell.y = j;
                    aCell.hasBomb = false;
                    aCell.hasFlag = false;
                    aCell.hasQuestion = false;
                    aCell.isUncovered = false;
                    board.cells.Add(aCell);

                    var client = new HttpClient();
                    await client.PostAsync(
                        "https://localhost:5001/api/Cells",
                        new StringContent(JsonSerializer.Serialize(aCell), Encoding.UTF8, "application/json")
                    );

                }
            }

            return response;
        }

        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(long id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoardExists(long id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }
    }
}
