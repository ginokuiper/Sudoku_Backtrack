using System.Runtime.CompilerServices;

int[] grid1 = new int[] {
    0, 0, 3, 0, 2, 0, 6, 0, 0,
    9, 0, 0, 3, 0, 5, 0, 0, 1,
    0, 0, 1, 8, 0, 6, 4, 0, 0,
    0, 0, 8, 1, 0, 2, 9, 0, 0,
    7, 0, 0, 0, 0, 0, 0, 0, 8,
    0, 0, 6, 7, 0, 8, 2, 0, 0,
    0, 0, 2, 6, 0, 9, 5, 0, 0,
    8, 0, 0, 2, 0, 3, 0, 0, 9,
    0, 0, 5, 0, 1, 0, 3, 0, 0
};
int[] grid2 = new int[]
{
    2, 0, 0, 0, 8, 0, 3, 0, 0,
    0, 6, 0, 0, 7, 0, 0, 8, 4,
    0, 3, 0, 5, 0, 0, 2, 0, 9,
    0, 0, 0, 1, 0, 5, 4, 0, 8,
    0, 0, 0, 0, 0, 0, 0, 0, 0,
    4, 0, 2, 7, 0, 6, 0, 0, 0,
    3, 0, 1, 0, 0, 7, 0, 4, 0,
    7, 2, 0, 0, 4, 0, 0, 6, 0,
    0, 0, 4, 0, 1, 0, 0, 0, 3
};
int[] grid3 = new int[]
{
    0, 0, 0, 0, 0, 0, 9, 0, 7,
    0, 0, 0, 4, 2, 0, 1, 8, 0,
    0, 0, 0, 7, 0, 5, 0, 2, 6,
    1, 0, 0, 9, 0, 4, 0, 0, 0,
    0, 5, 0, 0, 0, 0, 0, 4, 0,
    0, 0, 0, 5, 0, 7, 0, 0, 9,
    9, 2, 0, 1, 0, 8, 0, 0, 0,
    0, 3, 4, 0, 5, 9, 0, 0, 0,
    5, 0, 7, 0, 0, 0, 0, 0, 0
};
int[] grid4 = new int[]
{
    0, 3, 0, 0, 5, 0, 0, 4, 0,
    0, 0, 8, 0, 1, 0, 5, 0, 0,
    4, 6, 0, 0, 0, 0, 0, 1, 2,
    0, 7, 0, 5, 0, 2, 0, 8, 0,
    0, 0, 0, 6, 0, 3, 0, 0, 0,
    0, 4, 0, 1, 0, 9, 0, 3, 0,
    2, 5, 0, 0, 0, 0, 0, 9, 8,
    0, 0, 1, 0, 2, 0, 6, 0, 0,
    0, 8, 0, 0, 6, 0, 0, 2, 0
};
int[] grid5 = new int[]
{
    0, 2, 0, 8, 1, 0, 7, 4, 0,
    7, 0, 0, 0, 0, 3, 1, 0, 0,
    0, 9, 0, 0, 0, 2, 8, 0, 5,
    0, 0, 9, 0, 4, 0, 0, 8, 7,
    4, 0, 0, 2, 0, 8, 0, 0, 3,
    1, 6, 0, 0, 3, 0, 2, 0, 0,
    3, 0, 2, 7, 0, 0, 0, 6, 0,
    0, 0, 5, 6, 0, 0, 0, 0, 8,
    0, 7, 6, 0, 5, 1, 0, 9, 0
};

List<int[]> grids = new List<int[]> {grid1, grid2, grid3, grid4, grid5};
int[] sValues = new int[] {0, 1, 2, 3};
Sudoku sudoku;  

// Main method to solve sudoku and print outputs
foreach (int sValue in sValues)
{
    Console.WriteLine($"End evaluation of sudoku's with s = {sValue}");
    foreach (int[] g in grids)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        watch.Start();

        sudoku = new Sudoku(g, sValue);
        Console.WriteLine($"Grid {grids.IndexOf(g) + 1}: {sudoku.Evaluation}      in {sudoku.nrOfIterations} iterations");
        watch.Stop();

        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
    }
    Console.WriteLine();
}

public class Sudoku
{
    public Block[,] blocks = new Block[3, 3];
    public int Evaluation;
    private int s;
    public int nrOfIterations;
    private int[] rowEvals = new int[9];
    private int[] colEvals = new int[9];

    public Sudoku(int[] numbers, int sValue)
    {
        int row = 0;
        int col = 0;
        // Loop over blocks
        for (int i = 1; i <= 9; i++)
        {
            // Get values belonging to one 3x3 block from input grid and create new block
            blocks[row, col] = new Block(
                numbers[((col * 3) + (row * 27))..((col * 3) + (row * 27) + 3)].Concat(
                numbers[((col * 3) + (row * 27) + 9)..((col * 3) + (row * 27) + 12)].Concat(
                numbers[((col * 3) + (row * 27) + 18)..((col * 3) + (row * 27) + 21)])).ToList());

            // Keep track of row and column of blocks
            if (i % 3 == 0)
            {
                row++;
                col = 0;
            }
            else col++;
        }

        s = sValue;
        Evaluation = Evaluate();
        Search();
    }

    public int Evaluate()
    {     
        //Evaluate each column and row and put it in its respective list
        for (int n = 0; n < 9; n++)
        {
            int colNumber = EvalColumn(n);
            colEvals[n] = colNumber;
            int rowNumber = EvalRow(n);
            rowEvals[n] = rowNumber;
        }

        return (colEvals.Sum() + rowEvals.Sum());
    }
    private int EvalColumn(int column)
    {
        //Creating a list of all possible numbers in the column
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int col = (column / 3);
        int row = 0; 
        for (int j = 0; j < 9; j++)
        {
            row = (j / 3);
            int currentValue = blocks[row, col].values[j % 3, column % 3];

            // Check Whether there is a value in the column that also in the list.
            // When a value is found remove it from the list.
            if (numbers.Contains(currentValue))
                numbers.Remove(currentValue);
        }
        return numbers.Count();
    }

    private int EvalRow(int row)
    {
        //Creating a list of all possible numbers in the row
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int col = 0;
        int ro = (row / 3);
        for (int j = 0; j < 9; j++)
        {
            col = (j / 3);
            int currentValue = blocks[ro, col].values[row % 3, j % 3];

            // Check Whether there is a value in the column that also in the list.
            // When a value is found remove it from the list.
            if (numbers.Contains(currentValue))
                numbers.Remove(currentValue);
        }
        return numbers.Count();
    }


    // TODO: implement search algorithm
    private void Search()
    {
        // only try the random walk for n steps
        int n = 10000;
        nrOfIterations = n;
        int timeOnPlateau = 0;

        while(Evaluation > 0 && n > 0)
        {
            int dEvaluation = BlockSearch();
            // If on a plateau
            if (dEvaluation == 0)
            {
                timeOnPlateau++;
            } 
            // If an optimum is passed
            else if (dEvaluation < 0)
            {
                RandomWalk();
                timeOnPlateau = 0;
            }
            // If a better successor has been found
            else
            {
                timeOnPlateau = 0;
            }

            // If walking on a plateau for more than 62 steps, do a random walk
            if (timeOnPlateau >= 62) // 
            {
                RandomWalk();
                timeOnPlateau = 0;
            }

            n--;

            // Uncomment the line below to show evaluation per step
            //Console.WriteLine($"{Evaluation}");
        }
        nrOfIterations -= n;
    }

    // Method to try all swaps in 1 block
    private int BlockSearch()
    {
        // Pick a random block from the sudoku
        List<int> indices = new List<int> { 0, 1, 2 };
        Random r = new Random();
        int iRow = r.Next(indices.Count);
        int iCol = r.Next(indices.Count);
        Block block = blocks[iRow, iCol];

        // Save operator of best new evaluation
        int[][] bestSuccessor = new int[2][];
        int bestDEval = -1; // TODO: this is some random high number, might be good to give this a meaningfull value

        // Save best evaluation
        int[] Dres = new int[9];
        int[] bestDres = new int[9];

        // Try all (legal) swaps
        foreach (int[] v1 in block.SwappableValues)
        {
            List<int[]> potentialSwaps = new List<int[]>(block.SwappableValues);
            potentialSwaps.Remove(v1);
            foreach (int[] v2 in potentialSwaps)
            {
                // swap v1 & v2
                block.SwapValues(v1, v2);

                // Evaluate swap
                Dres = EvaluateSwap(iRow, iCol, v1, v2);
                int dEval = Dres[0];

                // Save v1, and v2 if better than or equal to previous evaluation ??the bigger the better??
                if (dEval >= bestDEval)
                {
                    bestDres = Dres;
                    bestDEval = dEval;
                    bestSuccessor[0] = v1;
                    bestSuccessor[1] = v2;
                }

                // revert swap for next iteration
                block.SwapValues(v1, v2);
            }
        }

        // Update block and total evaluation with best evaluation swap, but only when the successor is better than the original state
        if (bestDEval >= 0)
        {
            block.SwapValues(bestSuccessor[0], bestSuccessor[1]);
            UpdateEvaluation(bestDres);
        }
        
        return bestDEval;
    }

    // Random walk: pick random block and 2 random swappable fields and swap + update evaluation s times
    private void RandomWalk()
    {;
        List<int> indices = new List<int> { 0, 1, 2 };
        Random r = new Random();
        int[] v1, v2;
        Block block;
        int iRow, iCol;


        for (int i = 0; i < s; i++)
        {
            // Pick a random block from the sudoku
            iRow = r.Next(indices.Count);
            iCol = r.Next(indices.Count);
            block = blocks[iRow, iCol];

            // Swap two random fields from the block
            List<int[]> swappableValues = new List<int[]>(block.SwappableValues);
            v1 = swappableValues[r.Next(swappableValues.Count)];
            swappableValues.Remove(v1);
            v2 = swappableValues[r.Next(swappableValues.Count)];

            block.SwapValues(v1, v2);

            // Calculate and update new evaluation value
            int[] res = EvaluateSwap(iRow, iCol, v1, v2);
            UpdateEvaluation(res);
        }
    }

    //TODO check whole sudoku now, but should be just a delta value of course -> implemented
    private int[] EvaluateSwap(int iRow, int iCol, int[] v1, int[] v2)
    {
        int[] res = new int[9];
        // Calc row and column of swapped values
        int row1 = iRow * 3 + v1[0];
        int row2 = iRow * 3 + v2[0];
        int col1 = iCol * 3 + v1[1];
        int col2 = iCol * 3 + v2[1];

        int rowD2 = 0;
        int colD2 = 0;

        // Calculate delta row and column values
        int rowD1 = rowEvals[row1] - EvalRow(row1);
        if (row1 != row2)
        {
            rowD2 = rowEvals[row2] - EvalRow(row2);
        }
        int colD1 = colEvals[col1] - EvalColumn(col1);
        if (col1 != col2)
        {
            colD2 = colEvals[col2] - EvalColumn(col2);
        }
        res[0] = rowD1 + rowD2 + colD1 + colD2;
        res[1] = row1;
        res[2] = rowD1;
        res[3] = row2;
        res[4] = rowD2;
        res[5] = col1;
        res[6] = colD1;
        res[7] = col2;
        res[8] = colD2;
        return res;
    }

    private void UpdateEvaluation(int[] DValues)
    {
        Evaluation -= DValues[0];
        rowEvals[DValues[1]] -= DValues[2];
        rowEvals[DValues[3]] -= DValues[4];
        colEvals[DValues[5]] -= DValues[6];
        colEvals[DValues[7]] -= DValues[8];
    }
};

public class Block
{
    // private List<int[]> fixedValues = new List<int[]>();
    public List<int[]> SwappableValues = new List<int[]>();
    public int[,] values = new int[3, 3];

    public Block(List<int> numbers)
    {
        int row = 0;
        int col = 0;

        // Get list of all missing numbers in the block
        List<int> PotentialNumbers = new List<int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Except(numbers));
        Random r = new Random();

        foreach (int n in numbers)
        {
            // Set fixed value
            if (n != 0)
            {
                values[row, col] = n;
                // fixedValues.Add(new int[] { row, col });
            }
            // Pick random number from the missing numbers and set as swappable value
            else
            {
                int RandomNumber = PotentialNumbers[r.Next(PotentialNumbers.Count)];
                values[row, col] = RandomNumber;
                PotentialNumbers.Remove(RandomNumber);
                SwappableValues.Add(new int[] { row, col });
            }

            // Keep track of row and column of values
            if (col == 2)
            {
                row++;
                col = 0;
            }
            else col++;
        }
    }

    public void SwapValues(int[] v1, int[] v2)
    {
        int value1 = values[v1[0], v1[1]];
        values[v1[0], v1[1]] = values[v2[0], v2[1]];
        values[v2[0], v2[1]] = value1;
    }
}
