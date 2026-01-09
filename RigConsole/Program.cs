using RigCore.Interfaces;
using System;

namespace RigConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Oil Rig Management System ===");
            Console.WriteLine("Week 1 Milestone Complete!\n");

            Console.WriteLine("Interface methods ready:");
            Console.WriteLine("1. GetAllEquipment()");
            Console.WriteLine("2. GetEquipmentById(int id)");
            Console.WriteLine("3. GetEquipmentByStatus(string status)");
            Console.WriteLine("4. GetEquipmentByZone(string zone)");

            Console.WriteLine("\nReady for ADO.NET implementation in Week 2!");
            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}