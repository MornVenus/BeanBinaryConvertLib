using BeanBinaryConvertLib.Factories;

namespace BeanBinaryConvertLib.Attributes;

public class BBConvertBeanAttribute : BBConvertBaseAttribute
{
    public override byte[] CreateBinary()
    {
        var val = AttachedProperty.GetValue(AttachedObject);
        return BBConvertFactory.CreateBinary(val);
    }
}
