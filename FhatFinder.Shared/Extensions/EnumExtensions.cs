using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FhatFinder.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                 .GetMember(enumValue.ToString())?
                 .First()?
                 .GetCustomAttribute<DisplayNameAttribute>()?
                 .DisplayName;
        }

        public static string GetDisplay(this Enum enumValue)
        {
            return enumValue.GetType()?
                 .GetMember(enumValue.ToString())?
                 .First()?
                 .GetCustomAttribute<DisplayAttribute>()?
                 .Name;
        }
    }
}
