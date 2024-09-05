namespace BeanBinaryConvertLib.Utils;

public class TypeUtil
{
    public static bool IsBasicType(Type type)
    {
        if (type.IsPrimitive) return true;
        if (type.IsEnum) return true;
        return false;
    }

    public static bool IsNumberType(Type type)
    {
        // if (type.IsEnum) return true;
        if (type.IsPrimitive && type != typeof(bool))
        {
            return true;
        }

        return false;
    }

    public static bool IsIntergerType(Type type)
    {
        return type == typeof(byte) ||
              type == typeof(sbyte) ||
              type == typeof(short) ||
              type == typeof(ushort) ||
              type == typeof(int) ||
              type == typeof(uint) ||
              type == typeof(long) ||
              type == typeof(ulong);
    }

    public static bool IsList(Type type)
    {
        // 检查类型是否是泛型类型，并且泛型定义是否是 List<>
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    }

    private static bool IsTuple(Type type)
    {
        return type.IsGenericType && type.ToString().StartsWith("System.Tuple");
    }
}
