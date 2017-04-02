using System;
using System.Data;
using System.Data.SqlClient;
using NEvilES;
using NEvilES.Pipeline;
using StructureMap;
using TiteAz.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;
using TiteAz.Common;
using System.Collections;
using NEvilES.DataStore;
using StructureMap.Pipeline;

namespace TiteAz.SeedData
{
    public static class Program
    {

        public static void Main(string[] args)
        {

            Console.WriteLine("GTD seed data.......");
            var container = new Container();


            var lookup = new EventTypeLookupStrategy();
            lookup.ScanAssemblyOfType(typeof(User.Created));
            lookup.ScanAssemblyOfType(typeof(Approval));

            var connString = "TiteAz";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            Console.WriteLine($"connection string: {configuration.GetConnectionString(connString)}");

            container = new Container(x =>
            {
                x.Scan(s =>
                {
                    lookup.ScanAssemblyOfType(typeof(User.Created));
                    s.AssemblyContainingType<User.Created>();
                    s.AssemblyContainingType<ICommandProcessor>();

                    s.ConnectImplementationsToTypesClosing(typeof(IProcessCommand<>));
                    s.ConnectImplementationsToTypesClosing(typeof(IHandleStatelessEvent<>));
                    s.ConnectImplementationsToTypesClosing(typeof(IHandleAggregateCommandMarker<>));
                    s.ConnectImplementationsToTypesClosing(typeof(INeedExternalValidation<>));
                    s.ConnectImplementationsToTypesClosing(typeof(IProject<>));
                    s.ConnectImplementationsToTypesClosing(typeof(IProjectWithResult<>));

                    s.WithDefaultConventions();
                    s.SingleImplementationsOfInterface();
                });

                x.For<IApprovalWorkflowEngine>().Use<ApprovalWorkflowEngine>();
                x.For<ICommandProcessor>().Use<PipelineProcessor>();
                x.For<IEventTypeLookupStrategy>().Add(lookup).Singleton();
                x.For<IRepository>().Use<DatabaseEventStore>();

                // x.For<IReadModel>().Use<TestReadModel>();

                //x.For<IConnectionString>().Use(s => new SqlConnectionString(configuration.GetConnectionString(connString)));
                x.For<CommandContext>().Use("CommandContext", s => new CommandContext(new CommandContext.User(Guid.NewGuid(), 666), new Transaction(Guid.NewGuid()), new CommandContext.User(Guid.NewGuid(), 007), ""));
                x.For<IDbConnection>().Use("Connection", s =>
                {
                    var conn = new SqlConnection(configuration.GetConnectionString(connString));
                    conn.Open();
                    return conn;
                });
                x.For<IDbTransaction>(Lifecycles.ThreadLocal).Use("Transaction", s => s.GetInstance<IDbConnection>().BeginTransaction());
                x.For<CommandContext.ITransaction>(Lifecycles.ThreadLocal).Use<PipelineTransaction>();
                x.For<IFactory>().Use<Factory>();
            });


            var id = CombGuid.NewGuid();

            using (container.GetInstance<PipelineTransaction>())
            {
                var processor = container.GetInstance<ICommandProcessor>();

                var craig = new User.NewUser { StreamId = CombGuid.NewGuid(), Details = new User.Details("craig@test.com", "xxx", "Craig", "Gardiner") };
                processor.Process(craig);

                var elijah = new User.NewUser { StreamId = CombGuid.NewGuid(), Details = new User.Details("elijah@test.com", "xxx", "Elijah", "Bate") };
                processor.Process(elijah);

                Bill.Created ourBill = new Bill.Created { StreamId = id, Description = "Sunday arvo fun ;)", Amount = 20.35m };
                processor.Process(ourBill);
                var youOweMe = new Debt.YouOweMe(CombGuid.NewGuid(), craig.StreamId, elijah.StreamId, ourBill.StreamId, 10.1725m);

                processor.Process(youOweMe);
                processor.Process(new Debt.Accept { StreamId = youOweMe.StreamId });

            }

            Console.WriteLine("Done - Hit any key!");
            Console.ReadKey();
        }
    }
    public class Transaction : CommandContext.ITransaction
    {
        public Guid Id { get; }
        public Transaction(Guid id)
        {
            Id = id;
        }
    }

    public class Factory : IFactory
    {
        private readonly IContainer _container;

        public Factory(IContainer container)
        {
            _container = container;
        }

        public object Get(Type type)
        {
            return _container.GetInstance(type);
        }

        public object TryGet(Type type)
        {
            return _container.TryGetInstance(type);
        }

        public IEnumerable GetAll(Type type)
        {
            return _container.GetAllInstances(type);
        }
    }

}