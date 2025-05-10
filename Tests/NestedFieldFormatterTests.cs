using Xunit;

namespace RedRover.Tests;

public class NestedFieldFormatterTests
{
    [Fact]
    public void FieldsAreNestedCorrectly()
    {
        const string input = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
        
        const string expected = """
            - id
            - name
            - email
            - type
              - id
              - name
              - customFields
                - c1
                - c2
                - c3
            - externalId
            """;
        
        var formatter = new NestedFieldFormatter();
        var result = formatter.FormatFields(input);
        
        Assert.Equal(expected, result);
    }


    [Fact]
    public void FieldsNestedAndSortedCorrectly()
    {
        const string input = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
        
        const string expected = """
            - email
            - externalId
            - id
            - name
            - type
              - customFields
                - c1
                - c2
                - c3
              - id
              - name
            """;
        
        var formatter = new NestedFieldFormatter();
        var result = formatter.FormatFields(input, true);
        
        Assert.Equal(expected, result);
    }
}