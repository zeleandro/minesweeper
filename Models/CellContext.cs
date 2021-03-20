using Microsoft.EntityFrameworkCore;

namespace cMinesweeperApi.Models
{
    public class CellContext : DbContext
    {
        public CellContext(DbContextOptions<CellContext> options)
            : base(options)
        {
        }

        public DbSet<Cell> Cells { get; set; }
    }
}