namespace BeanBinaryConvertLib.Attributes.Funcs;

public class BBConvertFixedByteListAttribute : BBConvertBaseAttribute
{
    public override object? CreateObject(byte[] datas, int offsetIndex, out int len)
    {
        if (Len < 1) throw new Exception($"The Len of {nameof(BBConvertFixedStringAttribute)} cannot be less than 1");
        len = Len;

        var result = new List<byte>(len);
        for (int i = 0; i < len; i++)
        {
            result.Add(datas[offsetIndex + i]);
        }
        return result;
    }

    public override byte[] CreateBinary()
    {
        var val = AttachedProperty.GetValue(AttachedObject);
        if (val is List<byte> byteList)
        {
            return byteList.ToArray();
        }
        throw new Exception($"The type of {AttachedProperty.Name} is not List<byte>");
    }
}
