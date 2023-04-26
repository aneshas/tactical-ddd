using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Aperture.Core;
using Dapper;
using Npgsql;

namespace Tactical.DDD.EventSourcing.Postgres.Aperture
{
    public class PostgresOffsetTracker : ITrackOffset
    {
        private readonly NpgsqlDataSource _conn;

        public PostgresOffsetTracker(NpgsqlDataSource conn)
        {
            _conn = conn;
        }

        public async Task SaveOffsetAsync(Type projection, int currentOffset)
        {
            await using var conn = _conn.CreateConnection();
            
            var sql = @"INSERT INTO @TableName (id, last_offset) VALUES (1, 0) 
                        ON CONFLICT(id) DO UPDATE SET last_offset = @Offset";

            sql = sql.Replace("@TableName", TableNameFor(projection));
            
            await conn.ExecuteAsync(
                sql,
                new
                {
                    Offset = currentOffset
                });
        }

        public async Task<int> GetOffsetAsync(Type projection)
        {
            await using var conn = _conn.CreateConnection();
            
            CreateTrackingTableFor(projection, conn);

            var sql = @"SELECT last_offset LastOffset FROM public.@TableName LIMIT 1;";
            
            sql = sql.Replace("@TableName", TableNameFor(projection));

            var results = await conn.QueryAsync<OffsetEntry>(sql);

            var offsetEntries = results as OffsetEntry[] ?? results.ToArray();
            
            if (!offsetEntries.Any())
            {
                return 0;
            } 

            return (int) offsetEntries.First().LastOffset;
        }

        private void CreateTrackingTableFor(Type projection, NpgsqlConnection conn)
        {
            var sql = @"
            BEGIN;
            CREATE TABLE IF NOT EXISTS @TableName 
            (
                id bigint NOT NULL PRIMARY KEY,
                last_offset bigint NOT NULL,
                updated_on timestamptz NOT NULL DEFAULT (now() at time zone 'utc')
            );
            COMMIT;";

            sql = sql.Replace("@TableName", TableNameFor(projection));

            conn.Execute(sql);
        }

        private static string TableNameFor(Type projection) =>
            $"aperture_offset_{projection.Name}";
    }
}