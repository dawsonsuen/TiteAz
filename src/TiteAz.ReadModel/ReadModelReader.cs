using System;
using System.Data;
using NEvilES.Pipeline;
using Newtonsoft.Json;

namespace TiteAz.ReadModel
{
    public class ReadModelReader : IReadFromReadModel
    {
        private readonly IDbConnection _db;
        private readonly IDbTransaction _trans;

        public ReadModelReader(IDbConnection db, IDbTransaction trans)
        {
            _db = db;
            _trans = trans;
        }

        public T Get<T>(Guid id) where T : IHaveIdentity
        {
            var readModel = new ReadModel();

            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "select id, stream_id, type, last_update, body from read_model";
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
    }
}