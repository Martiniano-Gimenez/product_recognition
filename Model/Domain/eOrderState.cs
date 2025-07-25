using Model.Attributes;
using System.ComponentModel;

namespace Model.Domain
{
    public enum eOrderState : short
    {
        [Description("En análisis")]
        [ClassesAtributte("badge rounded-pill bg-info")]
        Analysis = 1,
        [Description("En preparación")]
        [ClassesAtributte("badge rounded-pill bg-warning")]
        Preparation = 2,
        [Description("Rechazado")]
        [ClassesAtributte("badge rounded-pill bg-danger")]
        Rejected = 3,
        [Description("Completado")]
        [ClassesAtributte("badge rounded-pill bg-success")]
        Completed = 4
    }
}
