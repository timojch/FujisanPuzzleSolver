using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FujisanPuzzleSolver;
internal class Move
{
    public int PriestNumber { get; init; }

    public char PriestLetter { get => (char)(this.PriestNumber + 'A'); }

    public Tuple<int, int> StartingLocation { get; init; }

    public Tuple<int, int> EndingLocation { get; init; }

    public override string ToString()
    {
        return $"Move Priest {this.PriestLetter} to the {(this.EndingLocation.Item2 == 0 ? "top" : "bottom")} of domino {this.EndingLocation.Item1}  ";
    }

    public string ToShortString(int[,] board)
    {
        int dx = this.EndingLocation.Item1 - this.StartingLocation.Item1;
        int dy = this.EndingLocation.Item2 - this.StartingLocation.Item2;
        if (dy != 0)
        {
            return $"{this.PriestLetter}";
        }
        else if (dx > 0)
        {
            return $"{this.PriestLetter}>{board[this.EndingLocation.Item1, this.EndingLocation.Item2]}";
        }
        else
        {
            return $"{this.PriestLetter}<{board[this.EndingLocation.Item1, this.EndingLocation.Item2]}";
        }
    }
}
