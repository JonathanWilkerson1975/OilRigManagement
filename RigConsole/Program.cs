

//using RigCore.Interfaces;
//using RigCore.Models;
//using RigInfrastructure.Repositories;
//using System;

//namespace RigConsole
//{
//    class Program
//    {
//        static void Main()
//        {
//            Console.WriteLine("FIXING CIRCULAR DEPENDENCY TEST");
//            Console.WriteLine("================================\n");

//            // Test 1: RigCore works?
//            Console.WriteLine("[1] Testing RigCore...");
//            var equipment = new Equipment
//            {
//                EquipmentId = 1,
//                EquipmentName = "Drill"
//            };
//            Console.WriteLine($"   ✓ Created: {equipment.EquipmentName}\n");

//            // Test 2: RigInfrastructure works? (WILL NOW WORK)
//            Console.WriteLine("[2] Testing RigInfrastructure...");
//            try
//            {
//                string connString = "Server=.;Database=Test;Trusted_Connection=True;";
//                IRigDataAccess repo = new EquipmentRepository(connString);
//                Console.WriteLine("   ✓ Repository created SUCCESSFULLY!\n");
//                Console.WriteLine("   CIRCULAR DEPENDENCY IS FIXED!");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"   ✗ Error: {ex.Message}");
//            }

//            Console.WriteLine("\nPress any key to exit...");
//            Console.ReadKey();
//        }
//    }
//}

using RigCore.Interfaces;
using RigCore.Models;
using System;

namespace RigConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("WORKING AROUND CIRCULAR DEPENDENCY\n");

            // Create test equipment
            var equipment = new Equipment
            {
                EquipmentId = 1,
                EquipmentName = "Test Drill",
                EquipmentType = "Drill",
                LocationZone = "Zone A"
            };
            Console.WriteLine($"✓ Created Equipment: {equipment.EquipmentName}\n");

            Console.WriteLine("NOTE: Repository cannot be created due to circular dependency.");
            Console.WriteLine("\nTO FIX: Remove RigConsole reference from RigInfrastructure project.");
            Console.WriteLine("\nCheck RigInfrastructure.csproj - remove this line:");
            Console.WriteLine(@"  <ProjectReference Include=""..\RigConsole\RigConsole.csproj"" />");

            Console.ReadKey();
        }
    }
}