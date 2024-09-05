using System.Text;

namespace BeanBinaryConvertLib.Attributes.Funcs;

/// <summary>
/// 固定字符串
/// 默认返回 List<byte>
/// </summary>
public class BBConvertFixedStringAttribute : BBConvertBaseAttribute
{
    public string FixedString { get; set; }

    public override object? CreateObject(byte[] datas, int offsetIndex, out int len)
    {
        if (string.IsNullOrWhiteSpace(FixedString))
        {
            throw new InvalidDataException($"{nameof(FixedString)} cannot be empty");
        }

        len = FixedString.Length;
        var result = Encoding.Default.GetBytes(FixedString);

        for (int i = 0; i < len; i++)
        {
            if (i + offsetIndex >= datas.Length) throw new IndexOutOfRangeException($"{nameof(FixedString)} is out of range of datas.");

            if (datas[i + offsetIndex] != result[i])
            {
                throw new InvalidDataException($"Fail to validate in {nameof(FixedString)} {FixedString}");
            }
        }

        return result.ToList();
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
