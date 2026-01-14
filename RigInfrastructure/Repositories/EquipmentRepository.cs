using RigCore.Interfaces;
using RigCore.Models;
using Microsoft.Data.SqlClient;  // CHANGED FROM System.Data.SqlClient
using System.Collections.Generic;

namespace RigInfrastructure.Repositories
{
    public class EquipmentRepository : IRigDataAccess
    {
        private readonly string _connectionString;

        public EquipmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Equipment> GetAll()
        {
            var equipmentList = new List<Equipment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Equipment", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        equipmentList.Add(new Equipment
                        {
                            EquipmentId = (int)reader["EquipmentId"],
                            EquipmentName = reader["EquipmentName"].ToString() ?? "",
                            EquipmentType = reader["EquipmentType"].ToString() ?? "",
                            LocationZone = reader["LocationZone"].ToString() ?? ""
                        });
                    }
                }
            }

            return equipmentList;
        }

        public Equipment? GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT * FROM Equipment WHERE EquipmentId = @Id",
                    connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Equipment
                        {
                            EquipmentId = (int)reader["EquipmentId"],
                            EquipmentName = reader["EquipmentName"].ToString() ?? "",
                            EquipmentType = reader["EquipmentType"].ToString() ?? "",
                            LocationZone = reader["LocationZone"].ToString() ?? ""
                        };
                    }
                }
            }

            return null;
        }

        public void Add(Equipment equipment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Equipment (EquipmentName, EquipmentType, LocationZone) " +
                    "VALUES (@Name, @Type, @Zone)",
                    connection);

                command.Parameters.AddWithValue("@Name", equipment.EquipmentName);
                command.Parameters.AddWithValue("@Type", equipment.EquipmentType);
                command.Parameters.AddWithValue("@Zone", equipment.LocationZone);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Equipment equipment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Equipment SET EquipmentName = @Name, EquipmentType = @Type, " +
                    "LocationZone = @Zone WHERE EquipmentId = @Id",
                    connection);

                command.Parameters.AddWithValue("@Name", equipment.EquipmentName);
                command.Parameters.AddWithValue("@Type", equipment.EquipmentType);
                command.Parameters.AddWithValue("@Zone", equipment.LocationZone);
                command.Parameters.AddWithValue("@Id", equipment.EquipmentId);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "DELETE FROM Equipment WHERE EquipmentId = @Id",
                    connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}