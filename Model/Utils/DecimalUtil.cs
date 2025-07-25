namespace Model.Utils
{
    public static class DecimalUtil
    {
        public static string AsMoneyString(this decimal? value, bool withSign = true)
        {
            return value.HasValue ? value.Value.AsMoneyString(withSign) : string.Empty;
        }

        public static string AsMoneyString(this decimal value, bool withSign = true)
        {
            return withSign ? value.ToString("$#,##0.00") : value.ToString("#,##0.00");
        }
    }
}
