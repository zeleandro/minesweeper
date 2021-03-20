
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
        private Cell[,] board;
        public bool initialized = false;
        public IList<Cell> cells { get; set; }

        // public Board(int rows, int columns, int numberOfBombs)
        // {
        //     board = new Cell[rows, columns];
        //     int generatedBombs = 0;
        //     for (int i = 0; i <= rows - 1; i++)
        //     {
        //         for (int j = 0; j <= columns - 1; j++)
        //         {
        //             board[i, j] = new Cell();
        //             board[i, j].x = i;
        //             board[i, j].y = j;
        //             board[i, j].hasBomb = false;
        //             board[i, j].hasFlag = false;
        //             board[i, j].hasQuestion = false;
        //         }
        //     }

        //     var randomRow = new Random();
        //     var randomColumn = new Random();
        //     int n, m;
        //     while (generatedBombs < numberOfBombs)
        //     {
        //         n = randomRow.Next(0, rows);
        //         m = randomColumn.Next(0, columns);
        //         if (!board[n, m].hasBomb)
        //         {
        //             board[n, m].hasBomb = true;
        //             generatedBombs++;
        //         }
        //     }
        // }

        // public void printBoard()
        // {
        //     for (int i = 0; i <= rows - 1; i++)
        //     {
        //         for (int j = 0; j <= columns - 1; j++)
        //         {
        //             Console.WriteLine(this.board[i, j].hasBomb.ToString());
        //         }
        //     }
        // }

        // public String printCell(int i, int j)
        // {
        //     return board[i, j].hasBomb.ToString();
        // }

        // public void flagCell(int i, int j)
        // {
        //     board[i, j].hasFlag = true;
        // }

        // public void questionCell(int i, int j)
        // {
        //     board[i, j].hasQuestion = true;
        // }
    }
}