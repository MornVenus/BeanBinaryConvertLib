using BeanBinaryConvertLib.Models;
using BeanBinaryConvertLib.Utils;

namespace BeanBinaryConvertLib.Attributes;

/// <summary>
/// 适用于 基础类型: enum bool byte short int long float double decimal 等
/// </summary>
public class BBConvertPrimitiveAttribute : BBConvertBaseAttribute
{
    public double Rate { get; set; } = 1;

    public BinaryStoreMode StoreMode { get; set; }

    public override object? CreateObject(byte[] datas, int offsetIndex, out int len)
    {
        len = Len;

        if (len < 1) throw new Exception("Length of BBConvertPrimitiveAttribute cannot be less than 1");

        var propertyType = AttachedProperty.PropertyType;

        if (!TypeUtil.IsBasicType(propertyType))
        {
            throw new Exception($"property {AttachedProperty.Name} is not a basic type");
        }

        long dataVal = BinaryUtil.GetLongFromBytes(datas.Skip(offsetIndex).Take(Len), this.StoreMode);

        if (propertyType.IsEnum)
        {
            return Enum.ToObject(propertyType, dataVal);
        }

        if (Rate != 1)
        {
            return Convert.ChangeType(dataVal * Rate, propertyType);
        }

        return Convert.ChangeType(dataVal, propertyType);
    }

    public override byte[] CreateBinary()
    {
        long val;
        var propVal = AttachedProperty.GetValue(AttachedObject);
        var type = AttachedProperty.PropertyType;
        if (type.IsEnum)
        {
            val = Convert.ToInt32(propVal);
        }
        else
        {
            if (Rate != 1 && type == typeof(double))
            {
                propVal = (double)propVal / Rate;
            }
            val = (long)Convert.ChangeType(propVal, typeof(long));
        }

        return BinaryUtil.GetBytesFromLong(val, Len, StoreMode);
    }
}

