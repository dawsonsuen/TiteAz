using System;
using System.Data.SqlClient;
using NEvilES.Pipeline;
using Newtonsoft.Json;
using TiteAz.Common;

namespace TiteAz.ReadModel
{
    public class ReadData : IReadData
    {
        private readonly SqlConnection _sqlConn;

        public ReadData(SqlConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public T Get<T>(Guid id) where T : IHaveIdentity
        {
            var cmd = _sqlConn.CreateCommand();
            cmd.CommandText = "select id, body, model_type from read_model where id = :id and model_type = :type";
            cmd.Parameters.AddWithValue(":id", id);
            cmd.Parameters.AddWithValue(":type", typeof(T).AssemblyQualifiedName);

            T model;
            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();

                model = JsonConvert.DeserializeObject<T>(reader.GetString(1));
            }

            return model;

        }
    }
}