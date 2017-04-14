using System.Data;
using System.Data.SqlClient;
using Autofac;
using NEvilES.DataStore;
using Npgsql;

namespace TiteAz.Common
{
    public class EventStoreDatabaseModule : Module
    {
        private string ConnectionString { get; }
        private DatabaseType dbType { get; set; }

        public EventStoreDatabaseModule(string connectionString, DatabaseType dbType)
        {
            ConnectionString = connectionString;
            this.dbType = dbType;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionString(ConnectionString))
                .As<IConnectionString>().SingleInstance();


            if (dbType == DatabaseType.Postgres)
            {
                builder.Register(c =>
                {
                    var conn = new NpgsqlConnection(c.Resolve<IConnectionString>().Data);
                    conn.Open();
                    return conn;
                }).AsSelf().As<IDbConnection>().InstancePerLifetimeScope();
            }
            else if (dbType == DatabaseType.SqlServer)
            {
                builder.Register(c =>
                {
                    var conn = new SqlConnection(c.Resolve<IConnectionString>().Data);
                    conn.Open();
                    return conn;
                }).AsSelf().As<IDbConnection>().InstancePerLifetimeScope();
            }
            else
            {
                builder.Register(c =>
                {
                    var conn = new SqlConnection(c.Resolve<IConnectionString>().Data);
                    conn.Open();
                    return conn;
                }).AsSelf().As<IDbConnection>().InstancePerLifetimeScope();
            }

            builder.Register(c =>
          {
              var conn = c.Resolve<IDbConnection>();
              return conn.BeginTransaction();
          }).As<IDbTransaction>().InstancePerLifetimeScope();
        }

    }

    public enum DatabaseType
    {
        SqlServer,
        Postgres
    }
}
