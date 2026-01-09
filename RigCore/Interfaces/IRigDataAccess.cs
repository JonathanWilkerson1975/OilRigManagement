//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RigCore.Interfaces
//{
//    internal class IRigDataAccess
//    {
//    }
//}
using System;
using System.Collections.Generic;

namespace RigCore.Interfaces
{
    public interface IRigDataAccess
    {
        // Basic CRUD operations
        List<DrillingEquipment> GetAllEquipment();
        DrillingEquipment GetEquipmentById(int id);

        // Domain-specific methods
        List<DrillingEquipment> GetEquipmentByStatus(string status);
        List<DrillingEquipment> GetEquipmentByZone(string zone);
    }

    public class DrillingEquipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string LocationZone { get; set; }
        public string Status { get; set; }
        public DateTime LastInspection { get; set; }
    }
}