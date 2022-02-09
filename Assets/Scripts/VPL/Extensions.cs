using System.Collections.Generic;

public static class Extensions
{
    // Allow retrieval of various value types from a dictionary
    public static T Get<T>(this Dictionary<string, object> dictionary, string value)
    {
        return (T)dictionary[value];
    }
}
