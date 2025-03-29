using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ChildCareCalendar.Admin.Extensions
{
    public static class EnumExtensions  
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static string GetEnumDisplayName(this string enumValue, Type enumType)
        {
            if (Enum.TryParse(enumType, enumValue, true, out var result))
            {
                var fieldInfo = enumType.GetField(result.ToString());
                var attribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
                return attribute?.Name ?? result.ToString();
            }
            return "Không xác định";
        }
    }

}
