using MichalRymaszewskiGeolocation.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MichalRymaszewskiGeolocation.Services
{
    public class MySqlDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public MySqlDatabaseService(string server, string database, string user, string password)
        {
            _connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS GeoLocations (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        IpOrUrl VARCHAR(100) UNIQUE,
                        Country VARCHAR(100),
                        City VARCHAR(100),
                        Latitude DOUBLE,
                        Longitude DOUBLE,
                        RetrievedAt DATETIME
                    );";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database initialization error: {ex.Message}");
            }
        }

        public bool Insert(GeoLocation geo)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO GeoLocations (IpOrUrl, Country, City, Latitude, Longitude, RetrievedAt)
                    VALUES (@IpOrUrl, @Country, @City, @Latitude, @Longitude, @RetrievedAt)
                    ON DUPLICATE KEY UPDATE
                        Country=@Country, City=@City, Latitude=@Latitude, Longitude=@Longitude, RetrievedAt=@RetrievedAt;";
                cmd.Parameters.AddWithValue("@IpOrUrl", geo.IpOrUrl);
                cmd.Parameters.AddWithValue("@Country", geo.Country);
                cmd.Parameters.AddWithValue("@City", geo.City);
                cmd.Parameters.AddWithValue("@Latitude", geo.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", geo.Longitude);
                cmd.Parameters.AddWithValue("@RetrievedAt", geo.RetrievedAt);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database insert error: {ex.Message}");
                return false;
            }
        }

        public bool Delete(string ipOrUrl)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM GeoLocations WHERE IpOrUrl=@IpOrUrl";
                cmd.Parameters.AddWithValue("@IpOrUrl", ipOrUrl);
                //Debug.WriteLine($"Database delete: {cmd.ExecuteNonQuery() > 0}");
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database delete error: {ex.Message}");
                return false;
            }
        }

        public GeoLocation? Find(string ipOrUrl)
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT IpOrUrl, Country, City, Latitude, Longitude, RetrievedAt
                    FROM GeoLocations
                    WHERE IpOrUrl = @IpOrUrl
                    LIMIT 1";
                cmd.Parameters.AddWithValue("@IpOrUrl", ipOrUrl);

                Debug.WriteLine($"Database OK");

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new GeoLocation
                    {
                        IpOrUrl = reader.GetString("IpOrUrl"),
                        Country = reader.GetString("Country"),
                        City = reader.GetString("City"),
                        Latitude = reader.GetDouble("Latitude"),
                        Longitude = reader.GetDouble("Longitude"),
                        RetrievedAt = reader.GetDateTime("RetrievedAt")
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
            }

            return null;
        }

        public bool CheckDbStatus()
        {
            using var conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                Debug.WriteLine($"Database OK");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }
    }
}
