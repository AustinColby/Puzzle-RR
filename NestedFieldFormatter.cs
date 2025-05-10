using System.Text;

namespace RedRover;

public class NestedFieldFormatter
{
    private static char[] _specialCharacters = [',', '(', ')'];

    private const string NestedLevelPadding = "  ";
    private const string FieldPrefix = "- ";
    
    /// <summary>
    /// Takes a string of comma-separated values and returns a
    /// formatted string that spaces each field to its nested level
    /// </summary>
    /// <param name="input">The CSV input string</param>
    /// <param name="sort">Sort the fields at each depth</param>
    public string FormatFields(string input, bool sort = false)
    {
        // var tokens = input.Split(_specialCharacters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int i = 0;
        int currentDepth = -1;
        
        var sb = new StringBuilder();

        while (i < input.Length)
        {
            var c = input[i];

            switch (c)
            {
                case ',':
                    i++;

                    break;
                case '(':
                    currentDepth++;
                    i++;
                    break;
                case ')':
                    currentDepth--;
                    i++;
                    break;
                default:
                    var nextTokenIndex = input.IndexOfAny(_specialCharacters, i);
                    nextTokenIndex = nextTokenIndex == -1 ? input.Length : nextTokenIndex;
                    var fieldLength = nextTokenIndex - i;
                    var field = input.Substring(i, fieldLength).Trim();
                    string padding = GetPadding(currentDepth);

                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    
                    sb.Append($"{padding}{FieldPrefix}{field}");
                    i += fieldLength;
                    
                    break;
            }
        }

        if (currentDepth != -1)
        {
            throw new Exception("The number of open and closing parenthesis does not match!");
        }

        return sb.ToString();
    }


    private static string GetPadding(int depth)
    {
        if (depth < 0)
            throw new ArgumentException("Depth must >= 0", nameof(depth));

        var sb = new StringBuilder();

        for (int i = 0; i < depth; i++)
        {
            sb.Append(NestedLevelPadding);
        }

        return sb.ToString();
    }
}