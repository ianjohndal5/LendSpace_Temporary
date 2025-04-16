using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LendSpace.Models;
using System.Data;

namespace LendSpace.Services
{
    public class FacilityService
    {
        private readonly DatabaseConnection _dbConnection;

        public FacilityService(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Facility>> GetAllFacilitiesAsync()
        {
            var facilities = new List<Facility>();

            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM facilities", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            facilities.Add(new Facility
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Description = reader.GetString("description"),
                                Location = reader.GetString("location"),
                                Capacity = reader.GetInt32("capacity"),
                                HourlyRate = reader.GetDecimal("hourly_rate"),
                                ImageUrl = reader.GetString("image_url"),
                                IsAvailable = reader.GetBoolean("is_available")
                            });
                        }
                    }
                }
            }

            return facilities;
        }

        public async Task<Facility> GetFacilityByIdAsync(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM facilities WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Facility
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Description = reader.GetString("description"),
                                Location = reader.GetString("location"),
                                Capacity = reader.GetInt32("capacity"),
                                HourlyRate = reader.GetDecimal("hourly_rate"),
                                ImageUrl = reader.GetString("image_url"),
                                IsAvailable = reader.GetBoolean("is_available")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> CheckFacilityAvailabilityAsync(int facilityId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(
                    @"SELECT COUNT(*) FROM bookings 
                      WHERE facility_id = @facilityId 
                      AND booking_date = @date 
                      AND status != 'Cancelled'
                      AND ((start_time <= @endTime AND end_time > @startTime) 
                           OR (start_time < @endTime AND end_time >= @startTime)
                           OR (start_time >= @startTime AND end_time <= @endTime))", connection))
                {
                    command.Parameters.AddWithValue("@facilityId", facilityId);
                    command.Parameters.AddWithValue("@date", date.Date);
                    command.Parameters.AddWithValue("@startTime", startTime);
                    command.Parameters.AddWithValue("@endTime", endTime);

                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count == 0;
                }
            }
        }
    }
}
