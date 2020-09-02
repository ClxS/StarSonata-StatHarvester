namespace StatHarvester.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;
    using Dapper.Contrib.Extensions;
    using Models.Specs;
    using Serilog;

    internal class StatConnection
    {
        public static string DbFile => Environment.CurrentDirectory + "\\ssdb.sqlite";

        public static async Task<SQLiteConnection> Create()
        {
            if (!File.Exists(DbFile))
            {
                await CreateDatabaseAsync();
            }

            return new SQLiteConnection("Data Source=" + DbFile);
        }

        private static async Task CreateDatabaseAsync()
        {
            Log.Information("Creating database at {Db}", DbFile);
            SQLiteConnection.CreateFile(DbFile);
            await using var con = new SQLiteConnection("Data Source=" + DbFile);
            await con.OpenAsync();

            await con.ExecuteAsync(
                @"create table Items
                      (
                         NameHash                           unsigned big int identity primary key not null,
                         Name                               Text not null,
                         Tech                               integer not null,
                         Description                        Text not null,
                         Rarity                             Text not null,
                         Type                               Text not null,
                         StructuredDescription              Text not null,
                         Weight                             bigint not null,
                         Size                               bigint not null,
                         SpecId                             integer
                      )");

            var specs = typeof(ItemSpec).Assembly.GetTypes().Where(t => t.BaseType == typeof(ItemSpec));
            foreach (var spec in specs)
            {
                var fields = spec.GetProperties(BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.DeclaredOnly)
                    .Where(f => f.GetCustomAttributes(typeof(WriteAttribute), false)
                        .All(a => ((WriteAttribute) a).Write));
                var fieldSql = new List<string>
                {
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT",
                    "ItemId unsigned big int REFERENCES Items(NameHash) ON UPDATE CASCADE"
                };

                foreach (var field in fields)
                {
                    fieldSql.Add($"{field.Name} {GetSqlType(field.PropertyType)} not null");
                }

                var sb = new StringBuilder();
                sb.Append($"create table {spec.Name}s (");
                sb.Append(fieldSql.Aggregate((a, b) => $"{a},{b}"));
                sb.Append(")");

                await con.ExecuteAsync(sb.ToString());
            }
        }

        private static string GetSqlType(Type type)
        {
            if (type == typeof(ulong))
            {
                return "unsigned big int";
            }

            if (type == typeof(long))
            {
                return "big int";
            }

            if (type == typeof(int))
            {
                return "int";
            }

            if (type == typeof(string))
            {
                return "Text";
            }

            if (type == typeof(bool))
            {
                return "BOOLEAN";
            }

            if (type == typeof(double))
            {
                return "REAL";
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}