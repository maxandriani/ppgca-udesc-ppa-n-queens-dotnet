namespace NQueens.Resolutions;

public class Board : HashSet<int>
{
    public Board(int size) : base(size)
    {
    }

    public Board(HashSet<int> board) : base(board)
    {
    }

    public string Print()
    {
        return string.Join(", ", this);
    }
}