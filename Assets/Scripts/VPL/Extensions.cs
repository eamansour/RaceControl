using System.Collections.Generic;

public static class Extensions
{
    /// <summary>
    /// Helper method to get the value of a given key and cast it to a given type.
    /// </summary>
    public static T Get<T>(this Dictionary<string, object> dictionary, string key)
    {
        return (T)dictionary[key];
    }
}
