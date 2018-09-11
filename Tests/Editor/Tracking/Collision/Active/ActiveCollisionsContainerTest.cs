using VRTK.Core.Tracking.Collision;
using VRTK.Core.Tracking.Collision.Active;

namespace Test.VRTK.Core.Tracking.Collision.Active
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;
    using Test.VRTK.Core.Utility.Helper;

    public class ActiveCollisionsContainerTest
    {
        private GameObject containingObject;
        private ActiveCollisionsContainerMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActiveCollisionsContainerMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Add()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Add(oneData);

            Assert.AreEqual(1, subject.Elements.Count);

            Assert.IsTrue(firstStartedMock.Received);
            Assert.IsTrue(countChangedMock.Received);
            Assert.IsTrue(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            subject.Add(twoData);

            Assert.AreEqual(2, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsTrue(countChangedMock.Received);
            Assert.IsTrue(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void Remove()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Add(oneData);
            subject.Add(twoData);

            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            Assert.AreEqual(2, subject.Elements.Count);

            subject.Remove(oneData);

            Assert.AreEqual(1, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsTrue(countChangedMock.Received);
            Assert.IsTrue(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            subject.Remove(twoData);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsTrue(countChangedMock.Received);
            Assert.IsTrue(contentsChangedMock.Received);
            Assert.IsTrue(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void InvalidRemove()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            Assert.AreEqual(0, subject.Elements.Count);

            subject.Remove(oneData);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
        }

        [Test]
        public void ProcessContentsChanged()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.ProcessContentsChanged();

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsTrue(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);
        }

        [Test]
        public void ClearsOnDisabled()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Add(oneData);
            subject.Add(twoData);

            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            Assert.AreEqual(2, subject.Elements.Count);

            subject.ManualOnDisable();

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsTrue(countChangedMock.Received);
            Assert.IsTrue(contentsChangedMock.Received);
            Assert.IsTrue(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [Test]
        public void AddInactiveGameObject()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            subject.gameObject.SetActive(false);
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Add(oneData);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
        }

        [Test]
        public void AddInactiveComponent()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            subject.enabled = false;
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Add(oneData);

            Assert.AreEqual(0, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
        }

        [Test]
        public void RemoveInactiveGameObject()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            subject.Add(oneData);
            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            subject.gameObject.SetActive(false);

            Assert.AreEqual(1, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Remove(oneData);

            Assert.AreEqual(1, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
        }

        [Test]
        public void RemoveInactiveComponent()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            subject.Add(oneData);
            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            subject.enabled = false;

            Assert.AreEqual(1, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.Remove(oneData);

            Assert.AreEqual(1, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
        }

        [Test]
        public void ProcessContentsChangedInactiveGameObject()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.gameObject.SetActive(false);

            subject.ProcessContentsChanged();

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);
        }

        [Test]
        public void ProcessContentsChangedInactiveComponent()
        {
            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            subject.FirstStarted.AddListener(firstStartedMock.Listen);
            subject.CountChanged.AddListener(countChangedMock.Listen);
            subject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            subject.AllStopped.AddListener(allStoppedMock.Listen);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            subject.enabled = false;

            subject.ProcessContentsChanged();

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);
        }
    }

    public class ActiveCollisionsContainerMock : ActiveCollisionsContainer
    {
        public virtual void ManualOnDisable()
        {
            base.OnDisable();
        }
    }
}