using System;

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

}
