
using System;
using System.Collections.Generic;

namespace cMinesweeperApi.Models
{
    public class Board
    {
        public long Id { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        public int numberOfBombs { get; set; }
        public int status {get; set;} //1: not initialized 2: in progress 3: lost 4: won
        public bool initialized = false;
        public IList<Cell> cells { get; set; }
    }
}