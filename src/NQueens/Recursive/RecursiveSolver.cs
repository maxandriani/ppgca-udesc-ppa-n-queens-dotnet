using System.Diagnostics;
using NQueens.Resolutions;

namespace NQueens.Recursive;

public class RecursiveSolver : ISolver
{
    private readonly int BoardSize;
    private HashSet<int> BoardBuffer;
    private HashSet<int> SlashBuffer;
    private HashSet<int> BackSlashBuffer;
    private Stopwatch Stopwatch = new();
    private int Count = 0;

    public event EventHandler<Board>? Solved;

    public RecursiveSolver(int size)
    {
        BoardSize = size;
        BoardBuffer = new HashSet<int>(size);
        SlashBuffer = new HashSet<int>(size);
        BackSlashBuffer = new HashSet<int>(size);
    }

    public Result Solve()
    {
        if (Count > 0) throw new Exception("RecursiveSolver can only run once.");
        Stopwatch.Start();
        SolveUntil();
        Stopwatch.Stop();
        return new Result(Stopwatch.Elapsed, Count);
    }

    private int GetSlashIndex(int row, int col)
    {
        return row + col;
    }

    private int GetBackSlashIndex(int row, int col)
    {
        return row - col + (BoardSize - 1);
    }

    private bool IsSafe(int row, int col)
    {
        return SlashBuffer.Contains(GetSlashIndex(row, col)) is false && BackSlashBuffer.Contains(GetBackSlashIndex(row, col)) is false && BoardBuffer.Contains(col) is false;
    }

    private void SolveUntil()
    {
        if (BoardBuffer.Count() == BoardSize)
        {
            Count++;
            Solved?.Invoke(this, new Board(BoardBuffer));
            return;
        }

        var row = BoardBuffer.Count();

        foreach (int col in Enumerable.Range(0, BoardSize).Except(BoardBuffer))
        {
            if (IsSafe(row, col))
            {
                BoardBuffer.Add(col);
                SlashBuffer.Add(GetSlashIndex(row, col));
                BackSlashBuffer.Add(GetBackSlashIndex(row, col));

                SolveUntil();
                
                BoardBuffer.Remove(col);
                SlashBuffer.Remove(GetSlashIndex(row, col));
                BackSlashBuffer.Remove(GetBackSlashIndex(row, col));
            }
        }
    }
}