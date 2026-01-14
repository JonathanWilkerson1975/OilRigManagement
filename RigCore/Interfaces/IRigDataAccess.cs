#nullable enable
using RigCore.Models;
using System.Collections.Generic;

namespace RigCore.Interfaces
{
    public interface IRigDataAccess
    {
        List<Equipment> GetAll();
        Equipment? GetById(int id);
        void Add(Equipment equipment);
        void Update(Equipment equipment);
        void Delete(int id);
        bool TransferEquipmentWithTransaction(int equipmentId, string fromZone, string toZone);
    }
}