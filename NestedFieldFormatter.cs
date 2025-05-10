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
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        input = input.Trim();

        if (!input.StartsWith('(') || !input.EndsWith(')'))
        {
            throw new ArgumentException("The input should start and end with open and closing parenthesis!", nameof(input));
        }
        
        var nodes = ParseNodes(input);

        if (sort)
        {
            SortNodes(nodes);
        }
        
        var sb = new StringBuilder();
        FormatNodes(sb, nodes);
        
        return sb.ToString();
    }


    /// <summary>
    /// Parses an input string into a tree of Nodes
    /// </summary>
    private List<Node> ParseNodes(string input)
    {
        var rootNode = new Node();
        
        int i = 0;
        var currentNode = rootNode;

        while (i < input.Length)
        {
            var c = input[i];

            switch (c)
            {
                case ',':
                    i++;
                    break;
                case '(':
                    if (!currentNode.Children.Any())
                    {
                        currentNode.Children.Add(new Node(null, currentNode));
                    }

                    currentNode = currentNode.Children.Last();
                    
                    i++;
                    break;
                case ')':
                    currentNode = currentNode.Parent!;
                    
                    i++;
                    break;
                default:
                    var nextTokenIndex = input.IndexOfAny(_specialCharacters, i);
                    nextTokenIndex = nextTokenIndex == -1 ? input.Length : nextTokenIndex;
                    var fieldLength = nextTokenIndex - i;
                    var field = input.Substring(i, fieldLength).Trim();

                    var node = new Node(field, currentNode);
                    currentNode.Children.Add(node);
                    
                    i += fieldLength;
                    
                    break;
            }
        }

        return rootNode.Children;
    }

    /// <summary>
    /// Sorts each level of nodes alphabetically by Field name
    /// </summary>
    /// <param name="nodes">List of fields to sort</param>
    private static void SortNodes(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            node.Children.Sort((node1, node2) => node1.Field.CompareTo(node2.Field));
            SortNodes(node.Children);
        }
    }


    /// <summary>
    /// Appends fields to the StringBuilder padded to their current depth
    /// </summary>
    private static void FormatNodes(StringBuilder sb, List<Node> nodes, int depth = -1)
    {
        foreach (var node in nodes)
        {
            if (depth >= 0)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }

                var padding = GetPadding(depth);
                sb.Append($"{padding}{FieldPrefix}{node.Field}");
            }

            FormatNodes(sb, node.Children, depth + 1);
        }
    }


    /// <summary>
    /// Returns a padding string representing the current depth of a field
    /// </summary>
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