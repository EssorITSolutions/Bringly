using System;
using System.Collections.Generic;
using System.Web.Mvc;
public static class StringHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToFirstLetterCapitalize(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        if (string.IsNullOrWhiteSpace(value)) return value;
        var textInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(value);
    }
}

public static class CustomeExtensions
{
    /// <summary>
    /// Custom func for distinct method.
    /// Get distinct base on func expression
    /// </summary>
    /// <typeparam name="TSource">Source type on which distinct will be applied.</typeparam>
    /// <typeparam name="TKey">Source key type to distinct.</typeparam>
    /// <param name="source">IEnumerable Source</param>
    /// <param name="keySelector">Func key</param>
    /// <returns></returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}


