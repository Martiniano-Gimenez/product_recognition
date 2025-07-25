using System.ComponentModel;

namespace Model.Domain
{
    public enum eFileExtension : short
    {
        [Description("PDF")]
        Pdf = 1,
        [Description("XLSX")]
        Xlsx = 2
    }
}
