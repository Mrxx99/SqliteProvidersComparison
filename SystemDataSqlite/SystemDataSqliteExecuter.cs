﻿using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Shared;

namespace SystemDataSqlite
{
    public class SystemDataSqliteExecuter : ISqliteExecuter
    {
        private SQLiteConnection _connection;

        public void Initilize()
        {
            _connection = new SQLiteConnection("Data Source=:memory:");
            _connection.Open();
        }

        public void CreateTables()
        {
            var command = _connection.CreateCommand();

            command.CommandText =
            @"
                    CREATE TABLE address(
                        id INTEGER PRIMARY KEY,
                        city TEXT NOT NULL
                    );

                    CREATE TABLE user(
                        id INTEGER PRIMARY KEY,
                        name TEXT NOT NULL,
                        addressId INTEGER NOT NULL,
                        FOREIGN KEY(addressId)
                            REFERENCES address(id)
                    );
                ";

            command.ExecuteNonQuery();
        }

        public void InsertEntries(IEnumerable<User> users)
        {

            using (var transaction = _connection.BeginTransaction())
            {
                var insertAddressCommand = _connection.CreateCommand();
                insertAddressCommand.CommandText =
                @"
                    INSERT INTO address(city)
                    VALUES ($city)
                ";

                var cityParameter = insertAddressCommand.CreateParameter();
                cityParameter.ParameterName = "$city";
                insertAddressCommand.Parameters.Add(cityParameter);

                var idCommand = _connection.CreateCommand();
                idCommand.CommandText = "SELECT last_insert_rowid()";

                foreach (var address in users.Select(u => u.Address).Distinct())
                {
                    cityParameter.Value = address.City;
                    insertAddressCommand.ExecuteNonQuery();

                    address.Id = (long)idCommand.ExecuteScalar();
                }

                var insertUserCommand = _connection.CreateCommand();
                insertUserCommand.CommandText =
                @"
                    INSERT INTO user(name, addressId)
                    VALUES ($name, $addressId)
                ";

                var nameParameter = insertUserCommand.CreateParameter();
                nameParameter.ParameterName = "$name";
                insertUserCommand.Parameters.Add(nameParameter);

                var addressIdParameter = insertUserCommand.CreateParameter();
                addressIdParameter.ParameterName = "$addressId";
                insertUserCommand.Parameters.Add(addressIdParameter);

                foreach (var user in users)
                {
                    nameParameter.Value = user.Name;
                    addressIdParameter.Value = user.Address.Id;
                    insertUserCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        public IEnumerable<User> ReadEntries()
        {
            var result = new List<User>();

            var command = _connection.CreateCommand();

            command.CommandText =
            @"
                    SELECT name, city, addressId
                    FROM user
                    JOIN address
                    ON user.addressId = address.id
            ";

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new User
                    {
                        Name = reader.GetString(0),
                        Address = new Address
                        {
                            City = reader.GetString(1),
                            Id = reader.GetInt64(2)
                        }
                    });
                }
            }

            return result;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
