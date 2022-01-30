using System.Collections.Generic;

public static class Extensions
{
    // Allow retrieval of multiple value types
    public static T Get<T>(this Dictionary<string, object> dictionary, string value)
    {
        return (T)dictionary[value];
    }
}
