namespace Actually.Core;

class Sudoku {
  private uint[,] def;
  private uint[,] solv;

  const int N = 9;

  private readonly string H = "───────────────────────────\n";

  public Sudoku(uint[,] _def) {
    def = new uint[9,9];
    solv = new uint[9,9];

    Array.Copy(_def, def, _def.Length);
    Array.Copy(_def, solv, _def.Length);

  }

  private bool IsSafe(int x,int y, uint n) {
    for (int i = 0; i < 9; i++) {
      if (solv[x, i] == n) return false;
    }

    for (int i = 0; i < 9; i++) {
      if (solv[i, y] == n) return false;
    }

    int startx = x - x % 3;
    int starty = y - y % 3;

    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        if (solv[i + startx,j + starty] == n)
          return false;
      }
    }

    return true;
  }

  public bool Solve(int x = 0, int y = 0) {
    if (x == 8 && y == 9) return true;

    if (y == N) {
      x++;
      y = 0;
    }

    if (solv[x,y] != 0) {
      return Solve(x, y + 1);
    }

    for (uint n = 1; n <= 9; n++) {
      if (IsSafe(x, y, n)) {
        solv[x,y] = n;
        if (Solve(x, y + 1)) {
          return true;
        }
      }
      solv[x,y] = 0;
    }

    return false;
  }

  private void Log(string c, bool colored = true, bool red = false) {
    if (colored) { Console.ForegroundColor = red ? ConsoleColor.DarkGreen : ConsoleColor.Cyan; }
    
    Console.Write(c);

    Console.ForegroundColor  = ConsoleColor.White;
  }

  public void Print() {
    

    Log(H);
    for (int i = 0; i < 9; i++) {
      Log("||");
      for (int j = 0; j < 9; j++) {
        
        Log($" {solv[i,j] }",  solv[i,j] == def[i,j], true);

        if (j == 2 || j == 5) {
          Log(" |");
        }
      }
      Log(" || \n");
      if (i == 2 || i == 5) {
        Log(H);
      }
    }
    Log(H);
  }
}