using Zinnia.Process;
using Zinnia.Process.Moment;

namespace Test.Zinnia.Process.Moment
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class MomentProcessTest
    {
        private GameObject containingObject;
        private MomentProcess subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);

            subject = containingObject.AddComponent<MomentProcess>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearSource()
        {
            Assert.IsNull(subject.Source);
            MockProcessable mockProcessable = new MockProcessable();
            ProcessContainer processContainer = new ProcessContainer
            {
                Interface = mockProcessable
            };
            subject.Source = processContainer;
            containingObject.SetActive(true);

            Assert.AreEqual(processContainer, subject.Source);
            subject.ClearSource();
            Assert.IsNull(subject.Source);
        }

        [Test]
        public void ClearSourceInactiveGameObject()
        {
            Assert.IsNull(subject.Source);
            MockProcessable mockProcessable = new MockProcessable();
            ProcessContainer processContainer = new ProcessContainer
            {
                Interface = mockProcessable
            };
            subject.Source = processContainer;
            containingObject.SetActive(true);

            Assert.AreEqual(processContainer, subject.Source);
            subject.gameObject.SetActive(false);
            subject.ClearSource();
            Assert.AreEqual(processContainer, subject.Source);
        }

        [Test]
        public void ClearSourceInactiveComponent()
        {
            Assert.IsNull(subject.Source);
            MockProcessable mockProcessable = new MockProcessable();
            ProcessContainer processContainer = new ProcessContainer
            {
                Interface = mockProcessable
            };
            subject.Source = processContainer;
            containingObject.SetActive(true);

            Assert.AreEqual(processContainer, subject.Source);
            subject.enabled = false;
            subject.ClearSource();
            Assert.AreEqual(processContainer, subject.Source);
        }

        [Test]
        public void Process()
        {
            MockProcessable mockProcessable = new MockProcessable();
            subject.Source = new ProcessContainer
            {
                Interface = mockProcessable
            };
            containingObject.SetActive(true);
            Assert.IsFalse(mockProcessable.WasProcessCalled);

            subject.Process();
            Assert.IsTrue(mockProcessable.WasProcessCalled);
            mockProcessable.Reset();

            subject.NextProcessTime = Time.time + 1f;
            Assert.IsFalse(mockProcessable.WasProcessCalled);
        }

        [Test]
        public void ProcessNow()
        {
            MockProcessable mockProcessable = new MockProcessable();
            subject.Source = new ProcessContainer
            {
                Interface = mockProcessable
            };
            containingObject.SetActive(true);
            Assert.IsFalse(mockProcessable.WasProcessCalled);
            mockProcessable.Reset();

            subject.ProcessNow();
            Assert.IsTrue(mockProcessable.WasProcessCalled);
            mockProcessable.Reset();

            subject.enabled = false;
            subject.ProcessNow();
            Assert.IsFalse(mockProcessable.WasProcessCalled);
            mockProcessable.Reset();

            subject.enabled = true;
            containingObject.SetActive(false);
            subject.ProcessNow();
            Assert.IsFalse(mockProcessable.WasProcessCalled);
            mockProcessable.Reset();

            containingObject.SetActive(true);
            subject.Source = null;
            subject.ProcessNow();
            Assert.IsFalse(mockProcessable.WasProcessCalled);
        }

        [Test]
        public void RandomizeNextProcessTime()
        {
            const float interval = 123.456f;
            subject.Interval = interval;
            containingObject.SetActive(true);

            subject.RandomizeNextProcessTime();

            Assert.IsTrue(subject.NextProcessTime >= Time.time);
            Assert.IsTrue(subject.NextProcessTime <= Time.time + interval);
        }

        [Test]
        public void Awake()
        {
            const float interval = 123.456f;
            subject.Interval = interval;

            Assert.AreEqual(0, subject.NextProcessTime);
            containingObject.SetActive(true);

            Assert.IsTrue(subject.NextProcessTime >= Time.time);
            Assert.IsTrue(subject.NextProcessTime <= Time.time + interval);
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