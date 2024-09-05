using BeanBinaryConvertLib.Attributes;
using System.Reflection;

namespace BeanBinaryConvertLib.Factories;

public class BBConvertFactory
{
    #region Public Methods

    public static T CreateBean<T>(byte[] datas) where T : class, new()
    {
        var obj = CreateBean(typeof(T), datas, 0, out int _);
        return (T)obj;
    }

    #endregion Public Methods	

    #region Create Bean

    public static object? CreateBean(Type type, byte[] datas, int offsetIndex, out int usedLen)
    {
        usedLen = 0;
        ValidateBinaryClassAttr(type);

        var properties = type.GetProperties().Where(p => p.GetCustomAttribute(typeof(BBConvertBaseAttribute)) != null).ToList();

        properties.Sort((p1, p2) =>
        {
            var a1 = p1.GetCustomAttribute<BBConvertBaseAttribute>();
            var a2 = p2.GetCustomAttribute<BBConvertBaseAttribute>();
            return a1.CompareTo(a2);
        });

        var result = Activator.CreateInstance(type);

        int startIndex = offsetIndex;

        foreach (var property in properties)
        {
            property.SetValue(result, CreateObjectByProperty(result, property, datas, startIndex, out int len));
            startIndex += len;
        }

        usedLen = startIndex - offsetIndex;

        return result;
    }

    /// <summary>
    /// 通过属性创建实例对象
    /// </summary>
    /// <param name="property"></param>
    /// <param name="datas"></param>
    /// <param name="startIndex"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    private static object CreateObjectByProperty(object attachedObject, PropertyInfo property, byte[] datas, int offsetIndex, out int len)
    {
        var attribute = GetConvertAttribute(attachedObject, property);
        var result = attribute.CreateObject(datas, offsetIndex, out len);
        return result;
    }

    #endregion Create Bean	

    #region Create Binary

    public static byte[] CreateBinary<T>(T bean) where T : class, new()
    {
        if (bean.GetType().GetCustomAttribute<BBConvertClassAttribute>() == null) throw new InvalidDataException("object cannot convert to binary");
        var properties = bean.GetType().GetProperties().Where(p => p.GetCustomAttribute(typeof(BBConvertBaseAttribute)) != null).ToList();

        properties.Sort((p1, p2) =>
        {
            var a1 = p1.GetCustomAttribute<BBConvertBaseAttribute>();
            var a2 = p2.GetCustomAttribute<BBConvertBaseAttribute>();
            return a1.CompareTo(a2);
        });
        List<byte> result = new List<byte>();
        foreach (var property in properties)
        {
            var attr = property.GetCustomAttribute<BBConvertBaseAttribute>();
            attr.AttachedProperty = property;
            attr.AttachedObject = bean;

            byte[] bin = attr.CreateBinary();

            result.AddRange(bin);
        }

        var datas = result.ToArray();

        var processMethodInfo = bean.GetType().GetMethod("ProcessBinary");
        if (processMethodInfo != null)
        {
            processMethodInfo.Invoke(bean, new object[] { datas });
        }

        return datas;
    }

    #endregion Create Binary	

    #region Assist Methods

    private static void ValidateBinaryClassAttr(Type type)
    {
        if (type.GetCustomAttribute<BBConvertClassAttribute>() == null) throw new Exception($"class {type.Name} must be attached with BinaryClassAttribute");
    }

    private static BBConvertBaseAttribute GetConvertAttribute(object attachedObject, PropertyInfo attachedProperty)
    {
        var attribute = attachedProperty.GetCustomAttribute<BBConvertBaseAttribute>(true);
        if (attribute == null) throw new Exception($"BBConvertBaseAttribute is not attached on property {attachedProperty.Name}");

        attribute.AttachedProperty = attachedProperty;
        attribute.AttachedObject = attachedObject;
        return attribute;
    }

    #endregion Assist Methods	
}
