using BeanBinaryConvertLib.Models;
using System.Text;

namespace BeanBinaryConvertLib.Utils;

public class BinaryUtil
{
    #region Get Long

    public static long GetLongFromBytes(IEnumerable<byte> bytes, BinaryStoreMode mode)
    {
        if (mode == BinaryStoreMode.LittleEnd)
        {
            return GetLongFromBytesLittleEnd(bytes);
        }
        else
        {
            return GetLongFromBytesBigEnd(bytes);
        }
    }

    public static long GetLongFromBytesLittleEnd(IEnumerable<byte> bytes)
    {
        return GetLongFromBytesBigEnd(bytes.Reverse());
    }

    public static long GetLongFromBytesBigEnd(IEnumerable<byte> bytes)
    {
        long result = 0;
        foreach (byte b in bytes)
        {
            result = result * 256 + b;
        }

        return result;
    }

    #endregion Get Long	

    #region Get Bytes

    public static byte[] GetBytesFromLong(long num, int len, BinaryStoreMode storeMode)
    {
        if (storeMode == BinaryStoreMode.LittleEnd)
        {
            return GetBytesFromLongLittleEnd(num, len);
        }
        else
        {
            return GetBytesFromLongBigEnd(num, len);
        }
    }

    public static byte[] GetBytesFromLongLittleEnd(long num, int len)
    {
        byte[] result = new byte[len];

        int index = 0;

        while (num > 0)
        {
            long val = num & 0xFF;
            num /= 256;
            if (index < len)
            {
                result[index++] = (byte)val;
            }
        }

        return result;
    }

    public static byte[] GetBytesFromLongBigEnd(long num, int len)
    {
        return GetBytesFromLongLittleEnd(num, len).Reverse().ToArray();
    }

    #endregion Get Bytes	

    #region Hex String

    public static string GetHexString(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in bytes)
        {
            sb.Append(item.ToString("X2") + " ");
        }
        return sb.ToString();
    }

    public static byte[] GetBytesFromHexStr(string hexStr)
    {
        List<byte> bytes = new List<byte>();


        if ((hexStr.Length & 1) == 1)
        {
            hexStr = "0" + hexStr;
        }

        for (int i = 0; i < hexStr.Length; i += 2)
        {
            bytes.Add(System.Convert.ToByte("" + hexStr[i] + hexStr[i + 1], 16));
        }

        return bytes.ToArray();
    }

    #endregion Hex String	
}
