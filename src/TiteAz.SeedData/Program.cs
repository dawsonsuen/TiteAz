using System;
using System.Collections.Concurrent;
using System.Reflection;
using NEvilES;
using NEvilES.Pipeline;
using TiteAz.Domain;
using Autofac;
using TiteAz.Common;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using NEvilES.DataStore;
using Npgsql;
using System.Data.SqlClient;

namespace TiteAz.SeedData
{
    public static class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            SetupConfig();

            var dbType = Configuration.GetSection("SeedData").GetSection("DbType").Value;
            var databaseType = dbType == "pgsql" ? DatabaseType.Postgres : DatabaseType.SqlServer;
            var connStrName = Configuration.GetSection("SeedData").GetSection("ConnectionString").Value;

            Console.WriteLine("Seed data.......");
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new CommandContext.User(Guid.NewGuid())).Named<CommandContext.IUser>("user");
            builder.RegisterType<ReadModel.SqlReadModel>().AsImplementedInterfaces();
            builder.RegisterInstance<IEventTypeLookupStrategy>(new EventTypeLookupStrategy());
            builder.RegisterModule(new EventStoreDatabaseModule(Configuration.GetConnectionString(connStrName), databaseType));
            builder.RegisterModule(new EventProcessorModule(typeof(User).GetTypeInfo().Assembly, typeof(ReadModel.User).GetTypeInfo().Assembly));

            var container = builder.Build();
            container.Resolve<IEventTypeLookupStrategy>().ScanAssemblyOfType(typeof(User));

            HandleDatabaseDropCreate(databaseType, dbType, connStrName);

            SeedData(container);

            using (var scope = container.BeginLifetimeScope())
            {
                ReplayEvents.Replay(container.Resolve<IFactory>(), scope.Resolve<IAccessDataStore>());
            }
            var reader = (ReadModel.SqlReadModel)container.Resolve<IReadFromReadModel>();

            // var x = reader.Get<ReadModel.User>(Guid.Empty);

           //Console.WriteLine("Read Model Document Count {0}", reader.Count());
            Console.WriteLine("Done - Hit any key!");
            Console.ReadKey();
        }

        private static void SeedData(IContainer container)
        {
            var id = CombGuid.NewGuid();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<PipelineTransaction>();
                var processor = scope.Resolve<ICommandProcessor>();

                var craig = new User.NewUser
                {
                    StreamId = CombGuid.NewGuid(),
                    Details = new User.Details("craig@test.com", "xxx", "Craig", "Gardiner")
                };
                processor.Process(craig);

                var elijah = new User.NewUser
                {
                    StreamId = CombGuid.NewGuid(),
                    Details = new User.Details("elijah@test.com", "xxx", "Elijah", "Bate")
                };
                processor.Process(elijah);

                var ourBill = new Bill.Created { StreamId = id, Description = "Sunday arvo fun ;)", Amount = 20.35m };
                processor.Process(ourBill);
                var youOweMe = new Debt.YouOweMe(CombGuid.NewGuid(), craig.StreamId, elijah.StreamId, ourBill.StreamId,
                    10.1725m);

                processor.Process(youOweMe);
                processor.Process(new Debt.Accept { StreamId = youOweMe.StreamId });
            }
        }

        public static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
               .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();
        }

        public static void HandleDatabaseDropCreate(DatabaseType databaseType, string dbType, string connStrName)
        {
            var connStr = new ConnectionString(Configuration.GetConnectionString(connStrName));
            var createSql = string.Empty;

            IDbConnection conn;
            var drop = Configuration.GetSection("DbTypes").GetSection(dbType).GetSection("drop").Value;


            if (databaseType == DatabaseType.Postgres)
            {
                conn = new NpgsqlConnection(string.Format("User ID={0};Password={1};Host={2};Port={3};Database=postgres;",
                    connStr.Keys["User ID"], connStr.Keys["Password"], connStr.Keys["Host"], connStr.Keys["Port"]));

                drop = string.Format(drop, connStr.Keys["Database"]);

                createSql = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "init.pgsql.sql"));
            }
            else if (databaseType == DatabaseType.SqlServer && dbType != "localdb")
            {
                conn = new SqlConnection(string.Format(@"Server={0};Database=Master;User Id={1};Password={2};",
                    connStr.Keys["Server"], connStr.Keys["User Id"], connStr.Keys["Password"]));

                drop = string.Format(drop, connStr.Keys["Database"]);
                createSql = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "init.mssql.sql"));
            }
            else
            {
                conn = new SqlConnection(string.Format(@"Server={0};Database=Master;Integrated Security=true;",
                   connStr.Keys["Server"]));

                drop = string.Format(drop, connStr.Keys["Database"]);
                createSql = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "init.mssql.sql"));
            }

            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = drop;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                cmd.CommandText = $"Create database {connStr.Keys["Database"]}";
                cmd.ExecuteNonQuery();
            }

            conn.ChangeDatabase(connStr.Keys["Database"]);

            using (var t = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = t;
                cmd.CommandText = createSql;

                cmd.ExecuteNonQuery();
                t.Commit();
            }
        }
    }
    public class InMemoryReadModel : IReadFromReadModel, IWriteReadModel
    {
        private readonly ConcurrentDictionary<Guid, object> data;

        public InMemoryReadModel()
        {
            data = new ConcurrentDictionary<Guid, object>();
        }

        public void Insert<T>(T item) where T : class, IHaveIdentity
        {
            data.TryAdd(item.Id, item);
        }

        public void Update<T>(T item) where T : class, IHaveIdentity
        {
            data[item.Id] = item;
        }

        public T Get<T>(Guid id) where T : IHaveIdentity
        {
            return (T)data[id];
        }

        public void Clear()
        {
            data.Clear();
        }

        public int Count()
        {
            return data.Count;
        }
    }

}