using Execution.Flyweight;
using JetBrains.dotMemoryUnit;
using JetBrains.dotMemoryUnit.Kernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Tests
{
    [TestFixture]
    public class FlyweightTests
    {
        [Test]
        public void TestUser()
        {
            var firstnames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastnames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User>();

            foreach (var firstname in firstnames)
            {
                foreach (var lastname in lastnames)
                {
                    users.Add(new User($"{firstname} {lastname}"));
                }
            }

            ForceGC();
            //DotMemoryUnitAttribute.Default.FailIfRunWithoutSupport = false;
            //dotMemory.Check(memory =>
            //{
            //    Assert.That(memory.SizeInBytes, Is.GreaterThan(1000));
            //    Assert.Pass($"SizeInBytes = {memory.SizeInBytes}");
            //});

            if (dotMemoryApi.IsEnabled)
            {
                var snapshot = dotMemoryApi.GetSnapshot();
                Assert.That(snapshot.SizeInBytes, Is.GreaterThan(10));
                Assert.Pass($"SizeInBytes = {snapshot.SizeInBytes}");
            }
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private string RandomString()
        {
            Random rand = new Random();

            return new string(
                Enumerable.Range(0, 10)
                .Select(i => (char)('a' + rand.Next(26)))
                .ToArray()
            );
        }
    }
}