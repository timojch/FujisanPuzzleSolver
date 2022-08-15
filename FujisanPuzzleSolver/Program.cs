using FujisanPuzzleSolver;

var topRow = new int[]
{
    //0, 1, 5, 0, 0, 2, 5, 3, 2, 3, 2, 5
    //5, 3, 0, 3, 2, 3, 2, 4, 4, 4, 1, 2
    4, 4, 0, 0, 2, 0, 0, 1, 5, 1, 4, 3
};
var bottomRow = new int[]
{
    //4, 5, 0, 2, 1, 3, 4, 0, 1, 4, 4, 2
    //3, 4, 5, 1, 1, 0, 3, 0, 5, 1, 5, 5
    2, 1, 3, 2, 3, 4, 1, 3, 0, 2, 5, 5
};

var board = new int[14, 2];
board[0, 0] = 0;
board[0, 1] = 0;
board[13, 0] = 0;
board[13, 1] = 0;

for (int x = 0; x < 12; ++x)
{
    board[x + 1, 0] = topRow[x];
    board[x + 1, 1] = bottomRow[x];
}

HashSet<PuzzleState> knownPuzzleStates = new HashSet<PuzzleState>();
Queue<PuzzleState> openList = new Queue<PuzzleState>();

var initialState = new PuzzleState(board);
openList.Enqueue(initialState);
knownPuzzleStates.Add(initialState);
var lastKnownSolutionDepth = 0;
var numberOfPriestsOnMountain = 0;

while (openList.TryDequeue(out var currentState))
{
    if (currentState.IsSolved())
    {
        Console.WriteLine($"Solved in {currentState.SolutionDepth} moves!");
        int moveNumber = 1;
        foreach (var move in currentState.GetMovesLeadingTo())
        {
            Console.WriteLine($">! {moveNumber++,3}: {move.ToShortString(board),-6} : {move}");
        }

        return;
    }
    else
    {
        foreach (var move in currentState.GetPossibleMoves())
        {
            var nextState = currentState.ApplyMove(move);
            if (knownPuzzleStates.Add(nextState))
            {
                openList.Enqueue(nextState);
            }
        }
    }

    if (currentState.SolutionDepth > lastKnownSolutionDepth)
    {
        lastKnownSolutionDepth = currentState.SolutionDepth;
        Console.WriteLine($"Expanding solution depth: {currentState.SolutionDepth} (width = {openList.Count})");
    }

    if (currentState.GetNumberOfPriestsOnMountain() > numberOfPriestsOnMountain)
    {
        numberOfPriestsOnMountain = currentState.GetNumberOfPriestsOnMountain();
        Console.WriteLine($"Now I can get {numberOfPriestsOnMountain} onto the mountain");
    }
}

Console.WriteLine("Open list is exhausted but no solution was found.");