using Microsoft.EntityFrameworkCore;

namespace cMinesweeperApi.Models
{
    public class BoardContext : DbContext
    {
        public BoardContext(DbContextOptions<BoardContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
    }
}