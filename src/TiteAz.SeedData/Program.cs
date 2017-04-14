using System;
using System.Collections.Concurrent;
using System.Reflection;
using NEvilES;
using NEvilES.Pipeline;
using TiteAz.Domain;
using Autofac;
using TiteAz.Common;

namespace TiteAz.SeedData
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("GTD seed data.......");
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new CommandContext.User(Guid.NewGuid())).Named<CommandContext.IUser>("user");
            //builder.RegisterType<ReadModel.SqlReadModel>().AsImplementedInterfaces();

            builder.RegisterType<InMemoryReadModel>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance<IEventTypeLookupStrategy>(new EventTypeLookupStrategy());            

            builder.RegisterModule(new EventStoreDatabaseModule("Server=(localdb)\\SQL2016;Database=Tite_Az;Integrated Security=true"));
            builder.RegisterModule(new EventProcessorModule(typeof(User).GetTypeInfo().Assembly, typeof(ReadModel.User).GetTypeInfo().Assembly));

            var container = builder.Build();
            container.Resolve<IEventTypeLookupStrategy>().ScanAssemblyOfType(typeof(User));

            //EventStoreDatabaseModule.TestLocalDbExists(container.Resolve<IConnectionString>());

            //SeedData(container);

            using (var scope = container.BeginLifetimeScope())
            {
                ReplayEvents.Replay(container.Resolve<IFactory>(), scope.Resolve<IAccessDataStore>());
            }
            var reader = (InMemoryReadModel)container.Resolve<IReadFromReadModel>();

            // var x = reader.Get<ReadModel.User>(Guid.Empty);

            Console.WriteLine("Read Model Document Count {0}", reader.Count());
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

                var ourBill = new Bill.Created {StreamId = id, Description = "Sunday arvo fun ;)", Amount = 20.35m};
                processor.Process(ourBill);
                var youOweMe = new Debt.YouOweMe(CombGuid.NewGuid(), craig.StreamId, elijah.StreamId, ourBill.StreamId,
                    10.1725m);

                processor.Process(youOweMe);
                processor.Process(new Debt.Accept {StreamId = youOweMe.StreamId});
            }
        }
    }

    //TODO: REMOVE
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