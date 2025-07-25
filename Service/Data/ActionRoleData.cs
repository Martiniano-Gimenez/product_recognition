using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Data
{
    public class ActionRoleData
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public List<eRole> Roles { get; set; }
    }
}
