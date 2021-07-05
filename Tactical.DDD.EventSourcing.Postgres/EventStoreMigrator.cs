using System.IO;
using System.Reflection;
using Dapper;
using Npgsql;

namespace Tactical.DDD.EventSourcing.Postgres
{
    public sealed class EventStoreMigrator
    {
        private readonly NpgsqlConnection _conn;

        public EventStoreMigrator(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public void EnsureEventStoreCreated() =>
            _conn.Execute(Script());

        private string Script()
        {
            var assembly = GetType().Assembly;
            var resourceName = $"{GetType().Namespace}.EventStore.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}