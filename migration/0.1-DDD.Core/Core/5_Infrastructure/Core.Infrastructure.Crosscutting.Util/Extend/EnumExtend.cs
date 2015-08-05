using System;
using System.ComponentModel;
using System.Reflection;

namespace Core.Infrastructure.Crosscutting.Util.Extend
{
    /// <summary>
    /// 用于获取枚举扩展方法的类
    /// </summary>
    public static class EnumExtend
    {
        /// <summary>
        /// 为枚举加Describe属性，输出枚举元素的说明信息 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>[Description("...")]="..."</returns>
        public static string GetDescription(this Enum obj)
        {
            return GetDescription(obj, false);
        }

        /// <summary>
        /// 为枚举加Describe属性，输出枚举元素的说明信息 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isTop">如果为True，则返回【枚举】.ToString()</param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            try
            {
                Type enumType = obj.GetType();
                DescriptionAttribute da;
                if (isTop)
                {
                    da = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = enumType.GetField(Enum.GetName(enumType, obj));
                    da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                }

                if (da != null && string.IsNullOrEmpty(da.Description) == false)
                {
                    return da.Description;
                }
            }
            catch
            {
                throw;
            }

            return obj.ToString();
        }

        /// <summary>
        /// 将枚举转换成整数
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static int GetEnumInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// 将整形转换成枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TEnum GetEnum<TEnum>(int code)
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), code); 
        }

        /// <summary>
        /// 查询枚举Flag是否包含特定枚举
        /// </summary>
        /// <param name="enumValue">枚举Flag</param>
        /// <param name="valueToTest">特定枚举</param>
        /// <returns>包含，则返回True</returns> 
        public static bool IsSet(this Enum enumValue, Enum valueToTest)  
        {
            if (enumValue.GetType() != valueToTest.GetType())
            {
                throw new ArgumentException("valueToTest", "Value must share the same type.");
            }

            if (Convert.ToUInt64(valueToTest) == 0)
            {
                throw new ArgumentOutOfRangeException("valueToTest", "Value must not be 0");
            }

            return (Convert.ToUInt64(enumValue) & Convert.ToUInt64(valueToTest)) == Convert.ToUInt64(valueToTest);  
        }

        /// <summary>
        /// 查询枚举Flag是否包含特定枚举
        /// </summary>
        /// <param name="enumValue">枚举Flag</param>
        /// <param name="valueToTest">特定枚举</param>
        /// <returns>不包含，则返回True</returns>
        public static bool IsClear(this Enum enumValue, Enum valueToTest)  
        {
            if (enumValue.GetType() != valueToTest.GetType())
            {
                throw new ArgumentException("valueToTest", "Value must share the same type.");
            }

            if (Convert.ToUInt64(valueToTest) == 0)
            {
                throw new ArgumentOutOfRangeException("valueToTest", "Value must not be 0");
            }
  
            return !IsSet(enumValue, valueToTest);  
        }  
  
        /// <summary>   
        /// Test if one of these test flags is set to this value.   
        /// </summary>   
        public static bool AnyFlagsSet(this Enum enumValue, Enum testValues)  
        {
            if (enumValue.GetType() != testValues.GetType())
            {
                throw new ArgumentException("testValues", "Value must share the same type.");
            }
 
            return (Convert.ToUInt64(enumValue) & Convert.ToUInt64(testValues)) != 0;  
        }  
  
        /// <summary>   
        /// Return a new value that set with specific flags.   
        /// </summary>   
        public static TEnum Set<TEnum>(this Enum enumValue, TEnum setValues)  
        {
            if (enumValue.GetType() != setValues.GetType())
            {
                throw new ArgumentException("setValues", "Value must share the same type.");
            }
  
            return (TEnum)Enum.ToObject(enumValue.GetType(), Convert.ToUInt64(enumValue) | Convert.ToUInt64(setValues));  
        }  
 
        /// <summary>   
        /// Return a new value with specific flags removed from this value.   
        /// </summary>   
        public static TEnum Clear<TEnum>(this Enum enumValue, TEnum clearValues)  
        {
            if (enumValue.GetType() != clearValues.GetType())
            {
                throw new ArgumentException("clearValues", "Value must share the same type.");
            }
 
            return (TEnum)Enum.ToObject(enumValue.GetType(), Convert.ToUInt64(enumValue) & ~Convert.ToUInt64(clearValues));  
        }  
 
        /// <summary>   
        /// For each flag in the value, perform the specific action.   
        /// </summary>   
        public static void ForEach<TEnum>(this Enum enumValue, Action<TEnum> processValue)  
        {
            if (processValue == null)
            {
                throw new ArgumentNullException("processValue");
            }

            if (enumValue.GetType() != typeof(TEnum))
            {
                throw new ArgumentException("type:TEnum", "processValue's parameter must have the same type.");
            }
 
            for (ulong bit = 1; bit != 0; bit <<= 1)  
            {
                ulong temp = Convert.ToUInt64(enumValue) & bit;
                if (temp != 0)
                {
                    processValue((TEnum)Enum.ToObject(typeof(TEnum), temp));
                }
            }  
        }
    }
}
