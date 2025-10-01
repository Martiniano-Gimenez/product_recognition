using System.ComponentModel;

namespace Model.Domain
{
    public enum eRole : short
    {
        [Description("Administrador")]
        Administrator = 1,
        [Description("Ventas")]
        Purchasing = 2,
        [Description("Depósito")]
        StockManager = 3,  
    }
}
