using Lab4.Application;
using Lab4.Commands;
using Lab4.Output;

var parser = new CommandParser();
var output = new ConsoleOutputWriter();
var renderer = new DefaultTreeRenderer();
var app = new FileSystemApplication(parser, output, renderer);

app.Run();
