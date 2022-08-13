using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FujisanPuzzleSolver;
internal class PuzzleState
{
    public int[,] Board { get; }

    public Tuple<int, int>[] PriestLocations { get; }

    public Move? LastMove { get; private set; } = null;

    public PuzzleState? LastPuzzleState { get; private set; } = null;

    public int SolutionDepth { get; private set; } = 0;

    public PuzzleState(int[,] board)
    {
        this.Board = board;
        this.PriestLocations = new Tuple<int, int>[]
        {
            Tuple.Create(0, 0),
            Tuple.Create(0, 1),
            Tuple.Create(13, 0),
            Tuple.Create(13, 1),
        };
    }

    public PuzzleState(PuzzleState cloneFrom)
    {
        this.Board = cloneFrom.Board;
        this.PriestLocations = cloneFrom.PriestLocations.ToArray();
    }

    public PuzzleState ApplyMove(Move move)
    {
        var nextState = new PuzzleState(this);
        nextState.PriestLocations[move.PriestNumber] = move.EndingLocation;
        nextState.LastMove = move;
        nextState.LastPuzzleState = this;
        nextState.SolutionDepth = this.SolutionDepth + 1;
        return nextState;
    }

    public IEnumerable<Move> GetMovesLeadingTo()
    {
        if (this.LastPuzzleState == null || this.LastMove == null)
        {
            yield break;
        }
        else
        {
            foreach (var previousMove in this.LastPuzzleState.GetMovesLeadingTo())
            {
                yield return previousMove;
            }

            yield return this.LastMove;
        }
    }

    public IEnumerable<Move> GetPossibleMoves()
    {
        for (int priest = 0; priest < 4; ++priest)
        {
            var startLocation = this.PriestLocations[priest];
            var changeDominoPosition = Tuple.Create(startLocation.Item1, startLocation.Item2 == 0 ? 1 : 0);
            if (startLocation.Item1 != 0 && startLocation.Item1 != 13)
            {
                if (!this.PriestLocations.Contains(changeDominoPosition))
                {
                    yield return new Move
                    {
                        PriestNumber = priest,
                        StartingLocation = startLocation,
                        EndingLocation = changeDominoPosition
                    };
                }
            }

            if (this.IsOnMountain(startLocation))
            {
                var shiftOnMountain = Tuple.Create(startLocation.Item1 == 7 ? 6 : 7, startLocation.Item2);

                if (!this.PriestLocations.Contains(shiftOnMountain))
                {
                    yield return new Move
                    {
                        PriestNumber = priest,
                        StartingLocation = startLocation,
                        EndingLocation = shiftOnMountain
                    };
                }
            }
            else
            {
                for (int i = 0; i < 14; ++i)
                {
                    var endLocation = Tuple.Create(i, startLocation.Item2);
                    if (this.CanMove(startLocation, endLocation))
                    {
                        yield return new Move
                        {
                            PriestNumber = priest,
                            StartingLocation = startLocation,
                            EndingLocation = endLocation
                        };
                    }
                }
            }
        }
    }

    public bool IsOnMountain(Tuple<int, int> priest)
    {
        return (priest.Item1 == 6 || priest.Item1 == 7);
    }

    public int GetNumberOfPriestsOnMountain()
    {
        return this.PriestLocations.Where(priest => this.IsOnMountain(priest)).Count();
    }

    public bool IsSolved()
    {
        return this.GetNumberOfPriestsOnMountain() == 4;
    }

    public bool CanMove(Tuple<int, int> startLocation, Tuple<int, int> endLocation)
    {
        if (this.PriestLocations.Contains(endLocation))
        {
            // Cannot move into a space containing a priest
            return false;
        }

        // Priests can move back and forth on the same domino only
        if (endLocation.Item2 != startLocation.Item2)
        {
            return endLocation.Item1 == startLocation.Item1;
        }

        var xDistance = endLocation.Item1 - startLocation.Item1;
        var unaidedSteps = 0;
        var y = startLocation.Item2;
        if (xDistance > 0)
        {
            for (int x = startLocation.Item1 + 1; x <= endLocation.Item1; x++)
            {
                var stepLocation = Tuple.Create(x, y);
                if (!this.PriestLocations.Contains(stepLocation))
                {
                    unaidedSteps++;
                }
            }
        }
        else if (xDistance < 0)
        {
            for (int x = startLocation.Item1 - 1; x >= endLocation.Item1; x--)
            {
                var stepLocation = Tuple.Create(x, y);
                if (!this.PriestLocations.Contains(stepLocation))
                {
                    unaidedSteps++;
                }
            }
        }

        if (unaidedSteps == 0)
        {
            return false;
        }
        else
        {
            return unaidedSteps == this.Board[endLocation.Item1, y];
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Board, this.PriestLocations[0], this.PriestLocations[1], this.PriestLocations[2], this.PriestLocations[3]);
    }

    public override bool Equals(object? obj)
    {
        if (obj is PuzzleState other)
        {
            return other.Board == this.Board
                && other.PriestLocations[0].Item1 == this.PriestLocations[0].Item1
                && other.PriestLocations[0].Item2 == this.PriestLocations[0].Item2
                && other.PriestLocations[1].Item1 == this.PriestLocations[1].Item1
                && other.PriestLocations[1].Item2 == this.PriestLocations[1].Item2
                && other.PriestLocations[2].Item1 == this.PriestLocations[2].Item1
                && other.PriestLocations[2].Item2 == this.PriestLocations[2].Item2
                && other.PriestLocations[3].Item1 == this.PriestLocations[3].Item1
                && other.PriestLocations[3].Item2 == this.PriestLocations[3].Item2;
        }
        else
        {
            return false;
        }
    }
}
