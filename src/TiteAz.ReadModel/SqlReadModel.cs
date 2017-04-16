using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NEvilES.Pipeline;
using Newtonsoft.Json;

namespace TiteAz.ReadModel
{
    public class SqlReadModel : IReadFromReadModel, IWriteReadModel
    {
        private readonly IDbConnection _db;
        private readonly IDbTransaction _trans;

        public SqlReadModel(IDbConnection db, IDbTransaction trans)
        {
            _db = db;
            _trans = trans;
        }

        public T Get<T>(Guid id) where T : IHaveIdentity
        {
            var readModel = new ReadModel();

            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "select id, stream_id, type, last_update, body from read_model where id=@id";
                CreateParam(cmd, "id", DbType.Guid, id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        readModel.Id = reader.GetInt32(0);
                        readModel.StreamId = reader.GetGuid(1);
                        readModel.Type = reader.GetString(2);
                        readModel.LastUpdate = reader.GetDateTime(3);
                        readModel.Body = reader.GetString(4);
                    }
                }
            }

            if (readModel.Id == 0)
            {
                return default(T);
            }

            var model = JsonConvert.DeserializeObject<T>(readModel.Body);

            return model;
        }

        public void Insert<T>(T item) where T : class, IHaveIdentity
        {
            var readModel = new ReadModel
            {
                StreamId = item.Id,
                Type = typeof(T).AssemblyQualifiedName,
                LastUpdate = DateTime.UtcNow,
                Body = JsonConvert.SerializeObject(item)
            };

            using (var cmd = _trans.Connection.CreateCommand())
            {
                cmd.CommandText = "insert into read_model (stream_id, type, last_update,body) values (@streamId, @type, @lastUpdate, @body)";
                cmd.Transaction = _trans;
                CreateParam(cmd, "@streamId", DbType.Guid, readModel.StreamId);
                CreateParam(cmd, "@type", DbType.String, readModel.Type);
                CreateParam(cmd, "@lastUpdate", DbType.DateTime, readModel.LastUpdate);
                CreateParam(cmd, "@body", DbType.String, readModel.Body);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update<T>(T item) where T : class, IHaveIdentity
        {
            var readModel = new ReadModel();

            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "select id, stream_id, type, last_update,body from read_model";
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    readModel.Id = reader.GetInt32(0);
                    readModel.StreamId = reader.GetGuid(1);
                    readModel.Type = reader.GetString(2);
                    readModel.LastUpdate = reader.GetDateTime(3);
                    readModel.Body = reader.GetString(4);
                }
            }


            readModel.Body = JsonConvert.SerializeObject(item);
            readModel.LastUpdate = DateTime.UtcNow;

            using (var cmd = _trans.Connection.CreateCommand())
            {
                cmd.CommandText = "update read_model set last_update=@lastUpdate, body=@body where stream_id=@streamId";
                cmd.Transaction = _trans;
                CreateParam(cmd, "@streamId", DbType.Guid, readModel.StreamId);
                CreateParam(cmd, "@lastUpdate", DbType.DateTime, readModel.LastUpdate);
                CreateParam(cmd, "@body", DbType.String, readModel.Body);
                cmd.ExecuteNonQuery();
            }

        }

        public IEnumerable<T> Query<T>(Func<T, bool> p)
        {
            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "select id, stream_id, type, last_update, body from read_model where type=@type";
                CreateParam(cmd, "type", DbType.String, typeof(T).AssemblyQualifiedName);
                cmd.Transaction = _trans;
                using (var reader = cmd.ExecuteReader())
                {
                    var results = new List<T>();
                    while (reader.Read())
                    {
                        results.Add(JsonConvert.DeserializeObject<T>(reader.GetString(4)));
                    }
                    return results.Where(p);
                }
            }
        }

        private static IDbDataParameter CreateParam(IDbCommand cmd, string name, DbType type, object value = null)
        {
            return CreateParam(cmd, name, type, null, value);
        }

        private static IDbDataParameter CreateParam(IDbCommand cmd, string name, DbType type, int? size,
            object value = null)
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.ParameterName = name;
            if (size.HasValue)
                param.Size = size.Value;
            if (value != null)
                param.Value = value;
            cmd.Parameters.Add(param);
            return param;
        }
    }
}