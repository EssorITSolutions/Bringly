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
