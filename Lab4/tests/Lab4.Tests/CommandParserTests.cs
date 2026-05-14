using Lab4.Commands;
using Xunit;

namespace Lab4.Tests;

public class CommandParserTests
{
    private readonly CommandParser _parser = new();

    [Fact]
    public void Parse_ConnectCommand_WithModeFlag_ParsesCorrectly()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("connect /home/user -m local");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("connect", result.Name);
        Assert.Null(result.SubCommand);

        var positional = Assert.Single(result.Arguments.OfType<PositionalArgument>());
        Assert.Equal("/home/user", positional.Value);

        var flag = Assert.Single(result.Arguments.OfType<FlagArgument>());
        Assert.Equal("-m", flag.Flag);
        Assert.Equal("local", flag.Value);
    }

    [Fact]
    public void Parse_DisconnectCommand_ParsesCorrectly()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("disconnect");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("disconnect", result.Name);
        Assert.Null(result.SubCommand);
        Assert.Empty(result.Arguments);
    }

    [Fact]
    public void Parse_TreeGoto_AbsolutePath_ParsesCorrectly()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("tree goto /some/path");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("tree", result.Name);
        Assert.Equal("goto", result.SubCommand);

        var positional = Assert.Single(result.Arguments.OfType<PositionalArgument>());
        Assert.Equal("/some/path", positional.Value);
    }

    [Fact]
    public void Parse_TreeGoto_RelativePath_ParsesCorrectly()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("tree goto ../folder");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("tree", result.Name);
        Assert.Equal("goto", result.SubCommand);

        var positional = Assert.Single(result.Arguments.OfType<PositionalArgument>());
        Assert.Equal("../folder", positional.Value);
    }

    [Fact]
    public void Parse_TreeList_WithDepthFlag_ParsesCorrectly()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("tree list -d 3");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("tree", result.Name);
        Assert.Equal("list", result.SubCommand);

        var flag = Assert.Single(result.Arguments.OfType<FlagArgument>());
        Assert.Equal("-d", flag.Flag);
        Assert.Equal("3", flag.Value);
    }

    [Fact]
    public void Parse_FileShow_WithModeFlag_ParsesCorrectly()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("file show /path/file.txt -m console");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file", result.Name);
        Assert.Equal("show", result.SubCommand);

        var positional = Assert.Single(result.Arguments.OfType<PositionalArgument>());
        Assert.Equal("/path/file.txt", positional.Value);

        var flag = Assert.Single(result.Arguments.OfType<FlagArgument>());
        Assert.Equal("-m", flag.Flag);
        Assert.Equal("console", flag.Value);
    }

    [Fact]
    public void Parse_FileMove_ParsesSourceAndDestination()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("file move /src/file.txt /dst");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file", result.Name);
        Assert.Equal("move", result.SubCommand);

        var positionals = result.Arguments.OfType<PositionalArgument>().ToList();
        Assert.Equal(2, positionals.Count);
        Assert.Equal("/src/file.txt", positionals[0].Value);
        Assert.Equal("/dst", positionals[1].Value);
    }

    [Fact]
    public void Parse_FileCopy_ParsesSourceAndDestination()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("file copy /src/file.txt /dst");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file", result.Name);
        Assert.Equal("copy", result.SubCommand);

        var positionals = result.Arguments.OfType<PositionalArgument>().ToList();
        Assert.Equal(2, positionals.Count);
    }

    [Fact]
    public void Parse_FileDelete_ParsesPath()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("file delete /path/to/file.txt");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file", result.Name);
        Assert.Equal("delete", result.SubCommand);

        var positional = Assert.Single(result.Arguments.OfType<PositionalArgument>());
        Assert.Equal("/path/to/file.txt", positional.Value);
    }

    [Fact]
    public void Parse_FileRename_ParsesPathAndName()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("file rename /path/file.txt newname.txt");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file", result.Name);
        Assert.Equal("rename", result.SubCommand);

        var positionals = result.Arguments.OfType<PositionalArgument>().ToList();
        Assert.Equal(2, positionals.Count);
        Assert.Equal("/path/file.txt", positionals[0].Value);
        Assert.Equal("newname.txt", positionals[1].Value);
    }

    [Fact]
    public void Parse_EmptyInput_ReturnsNull()
    {
        // Arrange & Act
        ParsedCommand? result = _parser.Parse("   ");

        // Assert
        Assert.Null(result);
    }
}
