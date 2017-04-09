using System;
using System.Data;
using System.Data.SqlClient;
using NEvilES.DataStore;
using NEvilES.Pipeline;
using Newtonsoft.Json;
using TiteAz.Common;
using static NEvilES.Pipeline.CommandContext;

namespace TiteAz.ReadModel
{
    public class WriteReadModel : IWriteReadModel
    {
        private readonly IDbTransaction _trans;

        public WriteReadModel(IDbTransaction trans)
        {
            _trans = trans;
        }

        public void Insert<T>(T item) where T : class, IHaveIdentity
        {

            using (var cmd = _trans.Connection.CreateCommand())
            {
                cmd.Transaction = _trans;
                cmd.CommandText = "insert into read_model (id, model_type, body, last_updated) values (@id, @model_type, @body, @last_updated)";
                CreateParam(cmd, "@id", DbType.Guid, item.Id);
                CreateParam(cmd, "@model_type", DbType.String, typeof(T).AssemblyQualifiedName);
                CreateParam(cmd, "@body", DbType.String, JsonConvert.SerializeObject(item));
                CreateParam(cmd, "@last_updated", DbType.Date, DateTime.UtcNow);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update<T>(T item) where T : class, IHaveIdentity
        {
            throw new NotImplementedException();
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