using System.Data;
using System.Data.SqlClient;
using Autofac;
using NEvilES.DataStore;


namespace TiteAz.SeedData
{
    public class SeedDataEventStoreDatabaseModule : Module
    {
        private string ConnectionString { get; }

        public SeedDataEventStoreDatabaseModule(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConnectionString(ConnectionString))
                .As<IConnectionString>().SingleInstance();

            builder.Register(c =>
            {
                var conn = new SqlConnection(c.Resolve<IConnectionString>().Data);
                conn.Open();
                return conn;
            }).AsSelf().As<IDbConnection>().InstancePerLifetimeScope();



            builder.Register(c =>
          {
              var conn = c.Resolve<IDbConnection>();
              return conn.BeginTransaction();
          }).As<IDbTransaction>().InstancePerLifetimeScope();
        }



    }
}