using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FujisanPuzzleSolver;
internal class Move
{
    public int PriestNumber { get; init; }

    public Tuple<int, int> StartingLocation { get; init; }

    public Tuple<int, int> EndingLocation { get; init; }

    public override string ToString()
    {
        return $">! Move Priest {(char)(this.PriestNumber + 'A')} to the {(this.EndingLocation.Item2 == 0 ? "top" : "bottom")} of domino {this.EndingLocation.Item1}  ";
    }
}
