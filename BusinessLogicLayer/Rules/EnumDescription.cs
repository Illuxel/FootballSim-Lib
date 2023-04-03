using System;
using System.ComponentModel;
using System.Linq;

namespace BusinessLogicLayer.Rules
{
    public static class EnumDescription
    {
        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>();

            foreach (var value in enumValues)
            {
                if (GetEnumDescription(value) == description)
                {
                    return value;
                }
            }

            throw new ArgumentException($"No enum value found with description: {description}", nameof(description));
        }

        private static string GetEnumDescription<T>(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }
    }
}
