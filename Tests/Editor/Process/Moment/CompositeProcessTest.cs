using Zinnia.Process;
using Zinnia.Process.Moment;
using Zinnia.Process.Moment.Collection;

namespace Test.Zinnia.Process.Moment
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class CompositeProcessTest
    {
        private GameObject containingObject;
        private CompositeProcess subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();

            subject = containingObject.AddComponent<CompositeProcess>();
            subject.Processes = containingObject.AddComponent<MomentProcessObservableList>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            MockProcessable mockProcessable1 = new MockProcessable();
            MomentProcess momentProcess1 = containingObject.AddComponent<MomentProcess>();
            momentProcess1.Source = new ProcessContainer
            {
                Interface = mockProcessable1
            };
            MockProcessable mockProcessable2 = new MockProcessable();
            MomentProcess momentProcess2 = containingObject.AddComponent<MomentProcess>();
            momentProcess2.Source = new ProcessContainer
            {
                Interface = mockProcessable2
            };
            subject.Processes.Add(momentProcess1);
            subject.Processes.Add(momentProcess2);

            Assert.IsFalse(mockProcessable1.WasProcessCalled);
            Assert.IsFalse(mockProcessable2.WasProcessCalled);
            subject.Process();
            Assert.IsTrue(mockProcessable1.WasProcessCalled);
            Assert.IsTrue(mockProcessable2.WasProcessCalled);
        }

        [Test]
        public void ProcessNestedList()
        {
            MockProcessable mockProcessable1 = new MockProcessable();
            MomentProcess momentProcess1 = containingObject.AddComponent<MomentProcess>();
            momentProcess1.Source = new ProcessContainer
            {
                Interface = mockProcessable1
            };
            MockProcessable mockProcessable2 = new MockProcessable();
            MomentProcess momentProcess2 = containingObject.AddComponent<MomentProcess>();
            momentProcess2.Source = new ProcessContainer
            {
                Interface = mockProcessable2
            };
            subject.Processes.Add(momentProcess1);
            subject.Processes.Add(momentProcess2);

            GameObject anotherObject = new GameObject();
            CompositeProcess anotherCompositeProcess = anotherObject.AddComponent<CompositeProcess>();
            anotherCompositeProcess.Processes = anotherObject.AddComponent<MomentProcessObservableList>();

            MockProcessable mockProcessable3 = new MockProcessable();
            MomentProcess momentProcess3 = containingObject.AddComponent<MomentProcess>();
            momentProcess3.Source = new ProcessContainer
            {
                Interface = mockProcessable3
            };
            MockProcessable mockProcessable4 = new MockProcessable();
            MomentProcess momentProcess4 = containingObject.AddComponent<MomentProcess>();
            momentProcess4.Source = new ProcessContainer
            {
                Interface = mockProcessable4
            };
            anotherCompositeProcess.Processes.Add(momentProcess3);
            anotherCompositeProcess.Processes.Add(momentProcess4);

            MomentProcess nestedProcess = containingObject.AddComponent<MomentProcess>();
            nestedProcess.Source = new ProcessContainer
            {
                Interface = anotherCompositeProcess
            };
            subject.Processes.Add(nestedProcess);

            Assert.IsFalse(mockProcessable1.WasProcessCalled);
            Assert.IsFalse(mockProcessable2.WasProcessCalled);
            Assert.IsFalse(mockProcessable3.WasProcessCalled);
            Assert.IsFalse(mockProcessable4.WasProcessCalled);
            subject.Process();
            Assert.IsTrue(mockProcessable1.WasProcessCalled);
            Assert.IsTrue(mockProcessable2.WasProcessCalled);
            Assert.IsTrue(mockProcessable3.WasProcessCalled);
            Assert.IsTrue(mockProcessable4.WasProcessCalled);
        }

        private sealed class MockProcessable : IProcessable
        {
            public bool WasProcessCalled;

            public void Process()
            {
                WasProcessCalled = true;
            }

            public void Reset()
            {
                WasProcessCalled = false;
            }
        }
    }
}