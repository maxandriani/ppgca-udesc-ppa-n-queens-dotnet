namespace NQueens.Resolutions;

public interface ISolver
{
    event EventHandler<Board> Solved;
    public Result Solve();
}