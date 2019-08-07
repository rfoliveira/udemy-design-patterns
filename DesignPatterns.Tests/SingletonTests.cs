using Autofac;
using Execution.Singleton;
using NUnit.Framework;

namespace DesignPatterns.Tests
{
    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            var tp = rf.GetTotalCalculation(names);

            Assert.That(tp, Is.EqualTo(17500000 + 17400000));
        }

        [Test]
        public void ConfigurableRecordFinderTest()
        {
            var rf = new ConfigurablerecordFinder(new DummyDatabase());
            var names = new[] { "Alpha", "Gamma" };
            var tp = rf.GetTotalCalculation(names);

            Assert.That(tp, Is.EqualTo(4));
        }

        [Test]
        public void DIPopulationTest()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<OrdinaryDatabase>()
                .As<IDatabase>()
                .SingleInstance();
            cb.RegisterType<ConfigurablerecordFinder>();

            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurablerecordFinder>();
            }
        }
    }
}
