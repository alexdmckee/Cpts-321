// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;
using System;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            // Block to generate a list with 10 random numbers.
            Random rand = new Random();
            List<int> list = new List<int>();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    list.Add(rand.Next());
                }
            }


            var thread = new Thread(
                            () =>
                            {
                                list.Sort(); // Publish the return value
                            });
            thread.Start();

            // Run until it sorts the list
            bool running = true;
            while (running)
            {
                if (!thread.IsAlive)
                {
                    running = false;
                }
            }
            Assert.That(list, Is.Ordered);
        }
    }
}
