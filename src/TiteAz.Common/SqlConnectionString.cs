using System.Collections.Generic;
using System.Linq;

namespace TiteAz.Common
{
    public interface IConnectionString
    {
        string ConnectionString { get; }
        Dictionary<string, string> Keys { get; }
    }

    public class SqlConnectionString : IConnectionString
    {
        public Dictionary<string, string> Keys { get; }
        public string ConnectionString { get; }

        public SqlConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            Keys = ConnectionString.Split(';').ToDictionary(k => k.Split('=')[0], v => v.Split('=')[1]);
        }
    }
}