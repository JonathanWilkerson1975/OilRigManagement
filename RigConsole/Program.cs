using RigCore.Interfaces;
using RigCore.Models;
using RigInfrastructure.Repositories;
using System;

namespace RigConsole
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("OIL RIG MANAGEMENT SYSTEM - MILESTONE 3");
            Console.WriteLine("UPDATE, DELETE & TRANSACTION WORKFLOW\n");

            string connectionString = "Server=WILKTOPIA5678\\JCWMSSQLSERVER;Database=OilRigDB;Trusted_Connection=True;TrustServerCertificate=True;";

            try
            {
                IRigDataAccess repository = new EquipmentRepository(connectionString);
                Console.WriteLine("Repository created successfully!\n");

                // Test Update
                Console.WriteLine("=== TEST 1: UPDATE ===");
                TestUpdate(repository);

                // Test Delete
                Console.WriteLine("\n=== TEST 2: DELETE ===");
                TestDelete(repository);

                // Test Transaction
                Console.WriteLine("\n=== TEST 3: TRANSACTION ===");
                TestTransaction(repository);

                // Final verification
                Console.WriteLine("\n=== FINAL DATA ===");
                var all = repository.GetAll();
                Console.WriteLine($"Total equipment: {all.Count}");
                foreach (var item in all)
                {
                    Console.WriteLine($"  ID: {item.EquipmentId}, Name: {item.EquipmentName}, Zone: {item.LocationZone}");
                }

                Console.WriteLine("\n✅ MILESTONE 3 COMPLETE!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void TestUpdate(IRigDataAccess repo)
        {
            try
            {
                // Get first equipment
                var equipment = repo.GetById(1);
                if (equipment != null)
                {
                    string oldZone = equipment.LocationZone;
                    equipment.LocationZone = "Updated Zone " + DateTime.Now.Second;

                    repo.Update(equipment);
                    Console.WriteLine($"Updated equipment {equipment.EquipmentId} from {oldZone} to {equipment.LocationZone}");
                }
                else
                {
                    Console.WriteLine("No equipment found to update");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
            }
        }

        static void TestDelete(IRigDataAccess repo)
        {
            try
            {
                // Create test item
                var testItem = new Equipment
                {
                    EquipmentName = "Temp Item " + DateTime.Now.Ticks,
                    EquipmentType = "Test",
                    LocationZone = "Delete Zone"
                };

                repo.Add(testItem);
                Console.WriteLine("Added test item for deletion");

                // Find and delete it
                var all = repo.GetAll();
                var toDelete = all.Find(e => e.EquipmentName.Contains("Temp Item"));

                if (toDelete != null)
                {
                    repo.Delete(toDelete.EquipmentId);
                    Console.WriteLine($"Deleted test item ID: {toDelete.EquipmentId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
            }
        }

        static void TestTransaction(IRigDataAccess repo)
        {
            try
            {
                // Find equipment in Zone 1
                var all = repo.GetAll();
                var equipment = all.Find(e => e.LocationZone.Contains("Zone 1"));

                if (equipment != null)
                {
                    bool success = repo.TransferEquipmentWithTransaction(
                        equipment.EquipmentId,
                        equipment.LocationZone,
                        "Transaction Zone");

                    if (success)
                        Console.WriteLine($"Transaction successful: Moved equipment {equipment.EquipmentId}");
                    else
                        Console.WriteLine("Transaction rolled back (equipment not in expected zone)");
                }
                else
                {
                    Console.WriteLine("No equipment in Zone 1 for transaction test");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction error: {ex.Message}");
            }
        }
    }
}