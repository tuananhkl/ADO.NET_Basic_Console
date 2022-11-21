using System;
using Npgsql;

namespace FirstAdoNetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // CheckPostgresqlVersion();

            // ConnectToPostgresql();
            
            //CreateTableAndInsertData();
            
            //InsertDataWithPreparedStatement();
            
            //ReadData();
            
            ReadDataWithHeaderColumn();
        }

        private static void ConnectToPostgresql()
        {
            var connectionString = "Server=127.0.0.1;Port=5432;Database=TestDatabase;" +
                                   "User Id=postgres;Password=Postgresql99;";
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            string sql = "SELECT * FROM actor";
            using var command = new NpgsqlCommand(sql, connection);

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var id = reader["actor_id"].ToString();
                var firstName = reader["first_name"].ToString();
                var lastName = reader["last_name"].ToString();
                var lastUpdate = reader["last_update"].ToString();

                Console.WriteLine($"Id: {id}");
                Console.WriteLine($"First Name: {firstName}");
                Console.WriteLine($"Last Name: {lastName}");
                Console.WriteLine($"Last Update: {lastUpdate}");
                Console.WriteLine("==========================");
            }
        }

        private static void CheckPostgresqlVersion()
        {
            var connectionString = "Server=127.0.0.1;Port=5432;Database=TestDatabase;" +
                                   "User Id=postgres;Password=Postgresql99;";
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var sql = "SELECT version()";
            using var command = new NpgsqlCommand(sql, connection);
            
            var version = command.ExecuteScalar()?.ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
        }

        private static void CreateTableAndInsertData()
        {
            var connectionString = "Server=127.0.0.1;Port=5432;Database=TestDatabase;" +
                                   "User Id=postgres;Password=Postgresql99;";        
            
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var command = new NpgsqlCommand();
            command.Connection = connection;
            
            command.CommandText = "DROP TABLE IF EXISTS cars";
            command.ExecuteNonQuery();

            //Create cars table
            command.CommandText = @"CREATE TABLE cars(id SERIAL PRIMARY KEY,
                                    name VARCHAR(255), price INT)";
            command.ExecuteNonQuery();

            //insert data
            command.CommandText = "INSERT INTO cars(name, price) VALUES('Audi',52642)";
            command.ExecuteNonQuery();
            
            command.CommandText = "INSERT INTO cars(name, price) VALUES('Mercedes',57127)";
            command.ExecuteNonQuery();
            
            command.CommandText = "INSERT INTO cars(name, price) VALUES('Skoda',9000)";
            command.ExecuteNonQuery();
            
            //read data here
            var sql = "SELECT * FROM cars";
            command.CommandText = sql;
            
            using NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var id = reader["id"].ToString();
                var name = reader["name"].ToString();
                var price = reader["price"].ToString();

                Console.WriteLine($"Id: {id}");
                Console.WriteLine($"Name: {name}");
                Console.WriteLine($"Price: {price}");
                Console.WriteLine("==========================");
            }
        }

        private static void InsertDataWithPreparedStatement()
        {
            var connectionString = "Server=127.0.0.1;Port=5432;Database=TestDatabase;" +
                                   "User Id=postgres;Password=Postgresql99;";        
            
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var sql = "INSERT INTO cars(name, price) VALUES(@name, @price)";
            using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("name", "BMW");
            command.Parameters.AddWithValue("price", 36600);
            command.Prepare();

            var result = command.ExecuteNonQuery();

            if (result != -1)
            {
                Console.WriteLine($"{result} rows inserted");
            }
            else
            {
                Console.WriteLine("Failed when executing queries");
            }
        }

        private static void ReadData()
        {
            var connectionString = "Server=127.0.0.1;Port=5432;Database=TestDatabase;" +
                                   "User Id=postgres;Password=Postgresql99;";
            
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var sql = "SELECT * FROM cars";
            using var command = new NpgsqlCommand(sql, connection);

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var id = reader["id"].ToString();
                var name = reader["name"].ToString();
                var price = reader["price"].ToString();
                
                Console.WriteLine($"Id: {id}\t Name: {name}\t Price: {price}");
            }

            // while (reader.Read())
            // {
            //     Console.WriteLine("Id: {0}\t Name: {1}\t Price: {2}", 
            //     reader.GetInt32(0), 
            //     reader.GetString(1), 
            //     reader.GetInt32(2));
            // }
        }

        private static void ReadDataWithHeaderColumn()
        {
            var connectionString = "Server=127.0.0.1;Port=5432;Database=TestDatabase;" +
                                   "User Id=postgres;Password=Postgresql99;";
            
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var sql = "SELECT * FROM cars";
            using var command = new NpgsqlCommand(sql, connection);
            
            using NpgsqlDataReader reader = command.ExecuteReader();
            //Get Column Name
            Console.WriteLine($"{reader.GetName(0),-4} {reader.GetName(1),-10} {reader.GetName(2),10}");

            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0),-4} {reader.GetString(1),-10} {reader.GetInt32(2),10}");
            }
        }

        #region Cach2

        // var id = reader["id"].ToString();
        // var name = reader["name"].ToString();
        // var price = reader["price"].ToString();
        //
        // Console.WriteLine($"Id: {id}\t Name: {name}\t Price: {price}");

        #endregion

        #region Cach 1

        // Console.WriteLine("Id: {0}\t Name: {1}\t Price: {2}", 
        // reader.GetInt32(0), 
        // reader.GetString(1), 
        // reader.GetInt32(2));

        #endregion
    }
}