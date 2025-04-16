using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LendSpace.Models;

namespace LendSpace.Services
{
    public class BookingService
    {
        private readonly DatabaseConnection _dbConnection;

        public BookingService(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CreateBookingAsync(Booking booking)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(
                    @"INSERT INTO bookings (facility_id, user_email, user_name, booking_date, 
                      start_time, end_time, attendee_count, purpose, total_cost, created_at, status) 
                      VALUES (@facilityId, @userEmail, @userName, @bookingDate, @startTime, 
                      @endTime, @attendeeCount, @purpose, @totalCost, @createdAt, @status);
                      SELECT LAST_INSERT_ID();", connection))
                {
                    command.Parameters.AddWithValue("@facilityId", booking.FacilityId);
                    command.Parameters.AddWithValue("@userEmail", booking.UserEmail);
                    command.Parameters.AddWithValue("@userName", booking.UserName);
                    command.Parameters.AddWithValue("@bookingDate", booking.BookingDate);
                    command.Parameters.AddWithValue("@startTime", booking.StartTime);
                    command.Parameters.AddWithValue("@endTime", booking.EndTime);
                    command.Parameters.AddWithValue("@attendeeCount", booking.AttendeeCount);
                    command.Parameters.AddWithValue("@purpose", booking.Purpose);
                    command.Parameters.AddWithValue("@totalCost", booking.TotalCost);
                    command.Parameters.AddWithValue("@createdAt", DateTime.Now);
                    command.Parameters.AddWithValue("@status", booking.Status.ToString());

                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<List<Booking>> GetUserBookingsAsync(string userEmail)
        {
            var bookings = new List<Booking>();

            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM bookings WHERE user_email = @userEmail", connection))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bookings.Add(new Booking
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                FacilityId = Convert.ToInt32(reader["facility_id"]),
                                UserEmail = reader["user_email"].ToString(),
                                UserName = reader["user_name"].ToString(),
                                BookingDate = Convert.ToDateTime(reader["booking_date"]),
                                StartTime = TimeSpan.Parse(reader["start_time"].ToString()),
                                EndTime = TimeSpan.Parse(reader["end_time"].ToString()),
                                AttendeeCount = Convert.ToInt32(reader["attendee_count"]),
                                Purpose = reader["purpose"].ToString(),
                                TotalCost = Convert.ToDecimal(reader["total_cost"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                Status = Enum.Parse<BookingStatus>(reader["status"].ToString())
                            });
                        }
                    }
                }
            }

            return bookings;
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM bookings WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Booking
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                FacilityId = Convert.ToInt32(reader["facility_id"]),
                                UserEmail = reader["user_email"].ToString(),
                                UserName = reader["user_name"].ToString(),
                                BookingDate = Convert.ToDateTime(reader["booking_date"]),
                                StartTime = TimeSpan.Parse(reader["start_time"].ToString()),
                                EndTime = TimeSpan.Parse(reader["end_time"].ToString()),
                                AttendeeCount = Convert.ToInt32(reader["attendee_count"]),
                                Purpose = reader["purpose"].ToString(),
                                TotalCost = Convert.ToDecimal(reader["total_cost"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                Status = Enum.Parse<BookingStatus>(reader["status"].ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> UpdateBookingStatusAsync(int id, BookingStatus status)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("UPDATE bookings SET status = @status WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@status", status.ToString());

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}