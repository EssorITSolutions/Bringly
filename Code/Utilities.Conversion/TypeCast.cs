using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public static class TypeCast
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val"></param>
    /// <param name="alt"></param>
    /// <returns></returns>
    public static T ToType<T>(this object val, T alt = default(T)) where T : struct, IConvertible
    {
        try
        {
            if (val == null) return alt;
            if (val is DBNull) return alt;
            return (T)Convert.ChangeType(val, typeof(T));
        }
        catch
        {
            return alt;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val"></param>
    /// <param name="alt"></param>
    /// <returns></returns>
    public static T? ToTypeOrNull<T>(this object val, T? alt = null) where T : struct, IConvertible
    {
        try
        {
            if (val != null && !(val is DBNull))
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            return null;
        }
        catch
        {
            return alt;
        }
    }

    public static string ToDisplayString<T>(this T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());

        var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

        if (descriptionAttributes[0].ResourceType != null)
            return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

        if (descriptionAttributes == null) return string.Empty;

        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }

    private static string lookupResource(Type resourceManagerProvider, string resourceKey)
    {
        foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
            {
                System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                return resourceManager.GetString(resourceKey);
            }
        }
        return resourceKey;
    }

}
