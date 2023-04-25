using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Tactical.DDD.EventSourcing.Postgres.Aperture
{
    public class PostgresOffsetTracker
    {
        // private readonly IDbConnection _conn;
        private readonly NpgsqlConnection _conn;

        public PostgresOffsetTracker(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public async Task SaveOffsetAsync(Type projection, int currentOffset)
        {
            var sql = @"UPDATE @TableName SET last_offset = @Offset;";

            sql = sql.Replace("@TableName", TableNameFor(projection));
            
            await _conn.ExecuteAsync(
                sql,
                new
                {
                    Offset = currentOffset
                });
        }

        public async Task<int> GetOffsetAsync(Type projection)
        {
            CreateTrackingTableFor(projection);

            var sql = @"SELECT last_offset LastOffset FROM public.@TableName LIMIT 1;";
            
            sql = sql.Replace("@TableName", TableNameFor(projection));

            var result = await _conn.QueryFirstAsync<OffsetEntry>(sql);

            return (int) result.LastOffset;
        }

        private void CreateTrackingTableFor(Type projection)
        {
            var sql = @"
            BEGIN;
            CREATE TABLE IF NOT EXISTS @TableName 
            (
                last_offset bigint NOT NULL,
                updated_on timestamptz NOT NULL DEFAULT (now() at time zone 'utc')
            );
            INSERT INTO @TableName (last_offset) VALUES (0);
            COMMIT;";

            sql = sql.Replace("@TableName", TableNameFor(projection));

            _conn.Execute(sql);
        }

        private static string TableNameFor(Type projection) =>
            $"aperture_offset_{projection.Name}";
    }
}