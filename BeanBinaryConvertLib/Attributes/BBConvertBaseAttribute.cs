using System.Reflection;

namespace BeanBinaryConvertLib.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public abstract class BBConvertBaseAttribute : Attribute, IComparable<BBConvertBaseAttribute>
{
    public int Order { get; set; }

    public int Len { get; set; } = -1;

    public string Name { get; set; }

    public string Description { get; set; }

    public PropertyInfo AttachedProperty { get; set; }

    public object? AttachedObject { get; set; }

    public int CompareTo(BBConvertBaseAttribute? other)
    {
        if (other == null) return 0;

        return Order.CompareTo(other.Order);
    }

    public virtual object? CreateObject(byte[] datas, int offsetIndex, out int len)
    {
        throw new NotImplementedException();
    }

    public virtual byte[] CreateBinary()
    {
        throw new NotImplementedException();
    }
}
