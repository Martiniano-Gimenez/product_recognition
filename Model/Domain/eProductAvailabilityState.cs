using Model.Attributes;
using System.ComponentModel;

namespace Model.Domain
{
    public enum eProductAvailabilityState : short
    {
        [Description("Disponible")]
        [ClassesAtributte("badge rounded-pill bg-success text-white")]
        Available = 1,
        [Description("A consultar")]
        [ClassesAtributte("badge rounded-pill bg-warning text-white")]
        ToConsult = 2,
        [Description("No disponible")]
        [ClassesAtributte("badge rounded-pill bg-danger text-white")]
        NotAvailable = 3
    }
}
