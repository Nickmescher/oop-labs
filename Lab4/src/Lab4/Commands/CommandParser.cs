namespace Lab4.Commands;

public class CommandParser
{
    public ParsedCommand? Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        List<string> tokens = Tokenize(input);
        if (tokens.Count == 0) return null;

        string name = tokens[0];
        string? subCommand = null;
        List<CommandArgument> arguments = new();

        int start = 1;

        if (tokens.Count > 1 && !tokens[1].StartsWith('-'))
        {
            string next = tokens[1];
            if (IsSubCommand(name, next))
            {
                subCommand = next;
                start = 2;
            }
        }

        for (int i = start; i < tokens.Count; i++)
        {
            string token = tokens[i];

            if (token.StartsWith('-'))
            {
                string flag = token;
                string? value = null;

                if (i + 1 < tokens.Count && !tokens[i + 1].StartsWith('-'))
                {
                    value = tokens[i + 1];
                    i++;
                }

                arguments.Add(new FlagArgument(flag, value));
            }
            else
            {
                arguments.Add(new PositionalArgument(token));
            }
        }

        return new ParsedCommand(name, subCommand, arguments);
    }

    private static bool IsSubCommand(string command, string candidate) =>
        command is "tree" or "file";

    private static List<string> Tokenize(string input)
    {
        List<string> tokens = new();
        bool inQuotes = false;
        System.Text.StringBuilder current = new();

        foreach (char c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ' ' && !inQuotes)
            {
                if (current.Length > 0)
                {
                    tokens.Add(current.ToString());
                    current.Clear();
                }
            }
            else
            {
                current.Append(c);
            }
        }

        if (current.Length > 0)
            tokens.Add(current.ToString());

        return tokens;
    }
}
