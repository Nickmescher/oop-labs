namespace Lab4.Output;

public interface ITreeRenderer
{
    string FileSymbol { get; }
    string DirectorySymbol { get; }
    string BranchConnector { get; }
    string LastBranchConnector { get; }
    string BranchIndent { get; }
    string EmptyIndent { get; }
}
