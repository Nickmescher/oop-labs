using Lab4.Application;
using Lab4.Output;

namespace Lab4.Commands;

public interface ICommand
{
    CommandResult Execute(FileSystemSession? session, IOutputWriter output);
}
