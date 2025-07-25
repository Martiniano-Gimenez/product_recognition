namespace Model.Utils
{
    public static class EnumUtils
    {
        public static T GetAttribute<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static bool IsValidShortEnumValue<T>(this short enumId) where T : Enum
        {
            return Enum.IsDefined(typeof(T), enumId);
        }
    }
}
