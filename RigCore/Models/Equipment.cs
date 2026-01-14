namespace RigCore.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string LocationZone { get; set; }

        // CONSTRUCTOR to fix CS8618
        public Equipment()
        {
            EquipmentName = string.Empty;
            EquipmentType = string.Empty;
            LocationZone = string.Empty;
        }
    }
}