namespace cMinesweeperApi.Models
{
    public class Cell
    {
        public long id { get; set; }
        public bool hasBomb { get; set; }
        public bool hasFlag { get; set; }
        public bool hasQuestion { get; set; }
        public bool isUncovered { get; set; }
        public int neightbors { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public long boardId { get; set; }
    }
}