namespace Lab4.Output;

public class DefaultTreeRenderer : ITreeRenderer
{
    public string FileSymbol => "[F]";
    public string DirectorySymbol => "[D]";
    public string BranchConnector => "├── ";
    public string LastBranchConnector => "└── ";
    public string BranchIndent => "│   ";
    public string EmptyIndent => "    ";
}
