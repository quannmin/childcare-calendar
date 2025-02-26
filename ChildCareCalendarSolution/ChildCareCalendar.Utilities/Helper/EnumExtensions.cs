﻿using System.ComponentModel.DataAnnotations;
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
    }

}
