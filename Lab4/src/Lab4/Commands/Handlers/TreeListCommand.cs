using Lab4.Application;
using Lab4.FileSystem;
using Lab4.Output;

namespace Lab4.Commands.Handlers;

public class TreeListCommand : ICommand
{
    private readonly int _depth;
    private readonly ITreeRenderer _renderer;

    public TreeListCommand(int depth, ITreeRenderer renderer)
    {
        _depth = depth;
        _renderer = renderer;
    }

    public CommandResult Execute(FileSystemSession? session, IOutputWriter output)
    {
        if (session is null)
            return new CommandFailure("Not connected. Use 'connect' first.");

        IDirectoryNode root = session.FileSystem.GetRoot(session.CurrentAbsolutePath);
        output.WriteLine($"{_renderer.DirectorySymbol} {root.Name}");
        PrintTree(root, string.Empty, _depth, output);
        return new CommandSuccess();
    }

    private void PrintTree(IDirectoryNode directory, string indent, int remainingDepth, IOutputWriter output)
    {
        if (remainingDepth <= 0) return;

        List<IFileSystemNode> children = directory.GetChildren().ToList();

        for (int i = 0; i < children.Count; i++)
        {
            bool isLast = i == children.Count - 1;
            string connector = isLast ? _renderer.LastBranchConnector : _renderer.BranchConnector;
            string childIndent = indent + (isLast ? _renderer.EmptyIndent : _renderer.BranchIndent);

            IFileSystemNode child = children[i];

            if (child is IDirectoryNode dir)
            {
                output.WriteLine($"{indent}{connector}{_renderer.DirectorySymbol} {child.Name}");
                PrintTree(dir, childIndent, remainingDepth - 1, output);
            }
            else
            {
                output.WriteLine($"{indent}{connector}{_renderer.FileSymbol} {child.Name}");
            }
        }
    }
}
