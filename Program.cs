namespace RedRover;

internal class Program
{
    public const string DefaultInput = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
    
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            throw new ArgumentException($"Invalid number of arguments - expected 1, received {args.Length}!");
        }

        var inputString = args.Length == 1 && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : DefaultInput;
        var formatter = new NestedFieldFormatter();

        var formattedString = formatter.FormatFields(inputString);
        Console.WriteLine(formattedString);
    } 
}
