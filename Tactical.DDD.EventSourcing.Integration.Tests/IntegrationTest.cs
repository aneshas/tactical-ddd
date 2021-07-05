using System;
using Dapper;
using Npgsql;
using Tactical.DDD.EventSourcing.Postgres;
using Tactical.DDD.EventSourcing.Postgres.Aperture;

namespace Tactical.DDD.EventSourcing.Integration.Tests
{
    public abstract class IntegrationTest
    {
        protected readonly EventStore EventStore;

        protected readonly PostgresOffsetTracker OffsetTracker;

        protected IntegrationTest()
        {
            var conn = new NpgsqlConnection(CreateDb());

            new EventStoreMigrator(conn).EnsureEventStoreCreated();

            EventStore = new EventStore(conn);
            OffsetTracker = new PostgresOffsetTracker(conn);
        }

        private static string CreateDb()
        {
            var conn = new NpgsqlConnection(
                "Server=127.0.0.1;Port=5432;User Id=postgres;");

            var dbName = $"db_{Guid.NewGuid().ToString().Replace("-", "_")}";

            conn.Execute($"CREATE DATABASE {dbName};");

            conn.Close();

            return
                $"Server=127.0.0.1;Port=5432;Database={dbName};User Id=postgres;";
        }
    }
}