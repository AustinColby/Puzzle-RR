namespace RedRover;

public class Node(string? field = null, Node? parent = null)
{
    public string? Field { get; set; } = field;

    public List<Node> Children { get; set; } = [];

    public Node? Parent { get; set; } = parent;
}