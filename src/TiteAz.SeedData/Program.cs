using System;
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

            builder.RegisterModule(new EventStoreDatabaseModule("Server=(localdb)\\SQL2016;Database=Tite_Az;Integrated Security=true"));
            builder.RegisterModule(new EventProcessorModule(typeof(User).GetTypeInfo().Assembly,null));

            var container = builder.Build();

            EventStoreDatabaseModule.TestLocalDbExists(container.Resolve<IConnectionString>());

            container.Resolve<IEventTypeLookupStrategy>().ScanAssemblyOfType(typeof(User));

            var id = CombGuid.NewGuid();

            using (container.Resolve<PipelineTransaction>())
            {
                var processor = container.Resolve<ICommandProcessor>();

                var craig = new User.NewUser { StreamId = CombGuid.NewGuid(), Details = new User.Details("craig@test.com", "xxx", "Craig", "Gardiner") };
                processor.Process(craig);

                var elijah = new User.NewUser { StreamId = CombGuid.NewGuid(), Details = new User.Details("elijah@test.com", "xxx", "Elijah", "Bate") };
                processor.Process(elijah);

                var ourBill = new Bill.Created { StreamId = id, Description = "Sunday arvo fun ;)", Amount = 20.35m };
                processor.Process(ourBill);
                var youOweMe = new Debt.YouOweMe(CombGuid.NewGuid(), craig.StreamId, elijah.StreamId, ourBill.StreamId, 10.1725m);

                processor.Process(youOweMe);
                processor.Process(new Debt.Accept { StreamId = youOweMe.StreamId });

            }

            Console.WriteLine("Done - Hit any key!");
            Console.ReadKey();
        }
    }
}