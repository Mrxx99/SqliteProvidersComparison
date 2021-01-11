using Microsoft.Data.Sqlite;

namespace DapperMicrosoftDataSqlite
{
    public static class Extensions
    {
        public static SqliteCommand CreateCommand(this SqliteConnection connection, string text)
        {
            var command = connection.CreateCommand();
            command.CommandText = text;
            return command;
        }

        public static SqliteParameter CreateAndAddParameter(this SqliteCommand command, string parameterName)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            command.Parameters.Add(parameter);
            return parameter;
        }
    }
}
