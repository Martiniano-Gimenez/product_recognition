using Model.Attributes;
using Model.Domain;
using Model.Utils;
using System.ComponentModel;

namespace Service.Data
{
    public class OrderHistoryViewData
    {
        public string Date {  get; set; }
        public eOrderState OrderState { get; set; }

        public string StateDescription => OrderState.GetAttribute<DescriptionAttribute>().Description;
        public string StateClass => OrderState.GetAttribute<ClassesAtributte>().Classes;
    }
}
