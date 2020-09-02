namespace StatHarvester.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandHelper
    {
        public static IEnumerable<string> Split(string str,
            Func<char, bool> controller)
        {
            var nextPiece = 0;

            for (var c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            var inQuotes = false;

            return Split(commandLine, c =>
                {
                    if (c == '\"')
                    {
                        inQuotes = !inQuotes;
                    }

                    return !inQuotes && c == ' ';
                })
                .Select(arg => TrimMatchingQuotes(arg.Trim(), '\"'))
                .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static string TrimMatchingQuotes(string input, char quote)
        {
            if (input.Length >= 2 && input[0] == quote && input[^1] == quote)
            {
                return input.Substring(1, input.Length - 2);
            }

            return input;
        }
    }
}