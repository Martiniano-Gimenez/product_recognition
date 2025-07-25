using Model.Attributes;
using Model.Domain;
using Model.Utils;
using System.ComponentModel;

namespace Service.Data
{
    public class OrderGridData : BaseGridData
    {
        public long Id { get; set; }
        public string Date { get; set; }    
        public string Total { get; set; }
        public string Client { get; set; }
        public string Seller { get; set; }
        public eOrderState OrderState {  get; set; }

        public string StateDescription => OrderState.GetAttribute<DescriptionAttribute>().Description;
        public string StateClass => OrderState.GetAttribute<ClassesAtributte>().Classes;
    }
}
