using System.Collections.Generic;
using DI.Runtime;
using NUnit.Framework;

namespace DI.Tests.Editor
{
    public class TestObject { }
    
    [TestFixture]
    public class DiContainerTest
    {
        private DiContainer m_container;

        [Test]
        public void TestSingleton()
        {
            m_container = new DiContainer();
            m_container.RegisterSingleton(() => new List<int> { 1, 2, 3 }, "list");
            var resolution = m_container.Resolve<List<int>>("list");
            Assert.NotNull(resolution);
            Assert.AreEqual(1, resolution[0]);
            Assert.AreEqual(2, resolution[1]);
            Assert.AreEqual(3, resolution[2]);
            Assert.Catch(typeof(KeyNotFoundException), delegate { m_container.Resolve<List<int>>(); });
            Assert.Catch(typeof(KeyNotFoundException), delegate { m_container.Resolve<List<int>>("list1"); });
        }

        [Test]
        public void TestTransient()
        {
            m_container = new DiContainer();
            m_container.RegisterTransient(() => new TestObject());
            var resolution1 = m_container.Resolve<TestObject>();
            var resolution2 = m_container.Resolve<TestObject>();
            
            Assert.NotNull(resolution1);
            Assert.NotNull(resolution2);
            Assert.AreNotSame(resolution1, resolution2);
        }

        [Test]
        public void TestInstance()
        {
            m_container = new DiContainer();
            m_container.RegisterInstance(new TestObject());
            var resolution1 = m_container.Resolve<TestObject>();
            var resolution2 = m_container.Resolve<TestObject>();
            
            Assert.NotNull(resolution1);
            Assert.NotNull(resolution2);
            Assert.AreSame(resolution1, resolution2);
        }

        [TearDown]
        public void TearDown()
        {
            m_container = null;
        }
    }
}