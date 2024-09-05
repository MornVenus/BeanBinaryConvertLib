using BeanBinaryConvertLib.Factories;
using BeanBinaryConvertLib.Utils;
using System.Collections;
using System.Reflection;

namespace BeanBinaryConvertLib.Attributes;

public class BBConvertLoopAttribute : BBConvertBaseAttribute
{
    public override object? CreateObject(byte[] datas, int offsetIndex, out int len)
    {
        len = -1;
        var propertyType = AttachedProperty.PropertyType;

        if (!TypeUtil.IsList(propertyType)) throw new Exception("BBConvertLoopAttribute must attach on List<> property");

        var argType = propertyType.GenericTypeArguments[0];

        if (argType.GetCustomAttribute<BBConvertClassAttribute>() == null) throw new Exception($"class {argType.Name} must be attached with BinaryClassAttribute");

        var result = Activator.CreateInstance(propertyType);

        var addMethodInfo = propertyType.GetMethod("Add");

        int startIndex = offsetIndex;
        while (startIndex < datas.Length)
        {
            var item = BBConvertFactory.CreateBean(argType, datas, startIndex, out int itemLen);
            addMethodInfo.Invoke(result, new object[] { item });
            startIndex += itemLen;
        }

        len = startIndex - offsetIndex;

        return result;
    }

    public override byte[] CreateBinary()
    {
        var val = AttachedProperty.GetValue(AttachedObject);
        var propertyType = AttachedProperty.PropertyType;
        if (!TypeUtil.IsList(propertyType)) throw new Exception("BBConvertLoopAttribute must attach on List<> property");

        var argType = propertyType.GenericTypeArguments[0];
        var listVal = val as IList;
        List<byte> result = new List<byte>();

        if (TypeUtil.IsBasicType(argType))
        {
            foreach (var item in listVal)
            {
                result.Add((byte)Convert.ChangeType(item, typeof(byte)));
            }
            return result.ToArray();
        }

        if (argType.GetCustomAttribute<BBConvertClassAttribute>() == null) throw new Exception($"class {argType.Name} must be attached with BinaryClassAttribute");

        foreach (var item in listVal)
        {
            var bin = BBConvertFactory.CreateBinary(item);
            result.AddRange(bin);
        }
        return result.ToArray();
    }
}
