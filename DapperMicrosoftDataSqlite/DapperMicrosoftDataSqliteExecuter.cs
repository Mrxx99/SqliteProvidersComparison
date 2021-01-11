using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Shared;

namespace DapperMicrosoftDataSqlite
{
    public class DapperMicrosoftDataSqliteExecuter : ISqliteExecuter
    {
        private SqliteConnection _connection;

        public void Initilize()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
        }

        public void CreateTables()
        {
            _connection.Execute(
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
            ");
        }

        public void InsertEntries(IEnumerable<User> users)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                var insertAddressCommand = _connection.CreateCommand(
                @"
                    INSERT INTO address(city)
                    VALUES ($city)
                ");

                var cityParameter = insertAddressCommand.CreateAndAddParameter("$city");

                var idCommand = _connection.CreateCommand("SELECT last_insert_rowid()");

                foreach (var address in users.Select(u => u.Address).Distinct())
                {
                    cityParameter.Value = address.City;
                    insertAddressCommand.ExecuteNonQuery();

                    address.Id = (long)idCommand.ExecuteScalar();
                }

                var insertUserCommand = _connection.CreateCommand(
                @"
                    INSERT INTO user(name, addressId)
                    VALUES ($name, $addressId)
                ");

                var nameParameter = insertUserCommand.CreateAndAddParameter("$name");

                var addressIdParameter = insertUserCommand.CreateAndAddParameter("$addressId");

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
            return _connection.Query<User, Address, User>(
            @"
                SELECT name, '' as split, city, addressId as Id
                FROM user
                JOIN address
                ON user.addressId = address.id
            ", (user, address) =>
            {
                user.Address = address;
                return user;
            }, splitOn: "split");
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
