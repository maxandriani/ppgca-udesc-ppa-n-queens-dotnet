using System.CommandLine;
using NQueens.Linear;
using NQueens.Recursive;
using NQueens.Resolutions;

var outputOption = new Option<FileInfo>("--output", "Dump solutions into a file.");
var rowsArgument = new Argument<int>("rows", () => 0, "Number of rows and cols.");
var recursiveCmd = new Command("recursive", "Solve N-Queens by recursive approach.")
{
    rowsArgument,
    outputOption
};
recursiveCmd.SetHandler((int size, FileInfo? output) => {
    Console.WriteLine($"Solving {size}-Queens by recursive approach.");
    ISolver solver = new RecursiveSolver(size);
    StreamWriter? writer = null;

    if (output != null)
    {
        if (output.Exists)
        {
            output.Delete();
        }

        writer = new StreamWriter(output.Create());

        solver.Solved += (_, board) =>
        {
            writer.WriteLine(board.Print());
        };
    }

    try {
        var res = solver.Solve();
        if (res.Solutions == 0)
        {
            Console.WriteLine($"There are no solutions for {size}-Queens.");
        }
        else
        {
            Console.WriteLine($"{res.Solutions} solution found.");
        }

        Console.WriteLine($"Elapsed time: {res.Elapsed.TotalSeconds} seconds");
    }
    finally
    {
        writer?.Dispose();
    }
}, rowsArgument, outputOption);

var linearCmd = new Command("linear", "Solve N-Queens by linear approach")
{
    rowsArgument,
    outputOption
};
linearCmd.SetHandler(LinearHandler.Handle, rowsArgument);

var rootCmd = new RootCommand
{
    recursiveCmd,
    linearCmd
};

rootCmd.SetHandler((rows) =>
{
    Console.WriteLine($"Running {rows}-Queens!");


}, rowsArgument);

await rootCmd.InvokeAsync(args);
