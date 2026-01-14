#nullable enable
using RigCore.Interfaces;
using RigCore.Models;
using Microsoft.Data.SqlClient;
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

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new System.Exception($"No equipment found with ID {equipment.EquipmentId} to update");
                }
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

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new System.Exception($"No equipment found with ID {id} to delete");
                }
            }
        }

        public bool TransferEquipmentWithTransaction(int equipmentId, string fromZone, string toZone)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Check equipment exists in source zone
                        var checkCommand = new SqlCommand(
                            "SELECT COUNT(*) FROM Equipment WHERE EquipmentId = @Id AND LocationZone = @Zone",
                            connection, transaction);
                        checkCommand.Parameters.AddWithValue("@Id", equipmentId);
                        checkCommand.Parameters.AddWithValue("@Zone", fromZone);

                        int count = (int)checkCommand.ExecuteScalar();
                        if (count == 0)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        // Update equipment to new zone
                        var updateCommand = new SqlCommand(
                            "UPDATE Equipment SET LocationZone = @NewZone WHERE EquipmentId = @Id",
                            connection, transaction);
                        updateCommand.Parameters.AddWithValue("@Id", equipmentId);
                        updateCommand.Parameters.AddWithValue("@NewZone", toZone);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        if (rowsAffected != 1)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        // Log the transfer (create table in SQL first)
                        try
                        {
                            var logCommand = new SqlCommand(
                                "INSERT INTO EquipmentTransferLog (EquipmentId, FromZone, ToZone) " +
                                "VALUES (@Id, @FromZone, @ToZone)",
                                connection, transaction);
                            logCommand.Parameters.AddWithValue("@Id", equipmentId);
                            logCommand.Parameters.AddWithValue("@FromZone", fromZone);
                            logCommand.Parameters.AddWithValue("@ToZone", toZone);
                            logCommand.ExecuteNonQuery();
                        }
                        catch (SqlException)
                        {
                            // If log table doesn't exist, continue without logging
                            // This shows transaction still works with partial operations
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (System.Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}