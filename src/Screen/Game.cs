using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ActuallySudoku {
  public class SudokuGame {
    public const int GridSize = 9;
    private readonly Random _rand = new Random();

    public int[,] Solution { get; private set; }
    public int[,] Puzzle { get; private set; }

    public SudokuGame() {
      Solution = new int[GridSize, GridSize];
      Puzzle = new int[GridSize, GridSize];
    }

    public void GenerateNewPuzzle(int cellsToReveal = 25) {
      Solution = new int[GridSize, GridSize];
      FillGrid(0, 0);

      Puzzle = (int[,])Solution.Clone();
      int cellsToRemove = GridSize * GridSize - cellsToReveal;

      for (int i = 0; i < cellsToRemove; i++) {
        int row, col;
        do {
          row = _rand.Next(0, GridSize);
          col = _rand.Next(0, GridSize);
        } while (Puzzle[row, col] == 0);

        Puzzle[row, col] = 0;
      }
    }

    public List<Point> CheckSolution(int[,] userGrid) {
      var incorrectCells = new List<Point>();
      for (int row = 0; row < GridSize; row++) {
        for (int col = 0; col < GridSize; col++) {
          if (userGrid[row, col] != Solution[row, col]) {
            incorrectCells.Add(new Point(row, col));
          }
        }
      }
      return incorrectCells;
    }

    private bool FillGrid(int row, int col) {
      if (row == GridSize) return true;
      if (col == GridSize) return FillGrid(row + 1, 0);

      List<int> numbers = Enumerable.Range(1, GridSize).OrderBy(n => _rand.Next()).ToList();

      foreach (int num in numbers) {
        if (IsSafe(row, col, num)) {
          Solution[row, col] = num;
          if (FillGrid(row, col + 1)) return true;
          Solution[row, col] = 0;
        }
      }
      return false;
    }

    private bool IsSafe(int row, int col, int num) {
      for (int i = 0; i < GridSize; i++) {
        if (Solution[row, i] == num || Solution[i, col] == num)
          return false;
      }

      int startRow = row / 3 * 3;
      int startCol = col / 3 * 3;
      for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 3; j++) {
          if (Solution[startRow + i, startCol + j] == num)
            return false;
        }
      }

      return true;
    }
  }
}