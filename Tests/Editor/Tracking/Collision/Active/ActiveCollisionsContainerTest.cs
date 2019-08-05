using Zinnia.Tracking.Collision.Active;
using Zinnia.Rule;
using Zinnia.Data.Collection.List;
using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision.Active
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;
    using Test.Zinnia.Utility.Helper;
    using Assert = UnityEngine.Assertions.Assert;

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

            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            subject.Add(twoData);

            Assert.AreEqual(2, subject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
        }

        [UnityTest]
        public IEnumerator AddInvalidCollisionDueToRule()
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
            oneContainer.AddComponent<RuleStub>();
            NegationRule negationRule = oneContainer.AddComponent<NegationRule>();
            AnyComponentTypeRule anyComponentTypeRule = oneContainer.AddComponent<AnyComponentTypeRule>();
            SerializableTypeComponentObservableList rules = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            anyComponentTypeRule.ComponentTypes = rules;
            rules.Add(typeof(RuleStub));

            negationRule.Rule = new RuleContainer
            {
                Interface = anyComponentTypeRule
            };
            subject.CollisionValidity = new RuleContainer
            {
                Interface = negationRule
            };

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
            ActiveCollisionsContainerNoDisableClearMock altSubject = containingObject.AddComponent<ActiveCollisionsContainerNoDisableClearMock>();

            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            altSubject.FirstStarted.AddListener(firstStartedMock.Listen);
            altSubject.CountChanged.AddListener(countChangedMock.Listen);
            altSubject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            altSubject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            altSubject.Add(oneData);
            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            altSubject.gameObject.SetActive(false);

            Assert.AreEqual(1, altSubject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            altSubject.Remove(oneData);

            Assert.AreEqual(1, altSubject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            Object.DestroyImmediate(oneContainer);
        }

        [Test]
        public void RemoveInactiveComponent()
        {
            ActiveCollisionsContainerNoDisableClearMock altSubject = containingObject.AddComponent<ActiveCollisionsContainerNoDisableClearMock>();

            UnityEventListenerMock firstStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock countChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock contentsChangedMock = new UnityEventListenerMock();
            UnityEventListenerMock allStoppedMock = new UnityEventListenerMock();

            altSubject.FirstStarted.AddListener(firstStartedMock.Listen);
            altSubject.CountChanged.AddListener(countChangedMock.Listen);
            altSubject.ContentsChanged.AddListener(contentsChangedMock.Listen);
            altSubject.AllStopped.AddListener(allStoppedMock.Listen);

            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);

            altSubject.Add(oneData);
            firstStartedMock.Reset();
            countChangedMock.Reset();
            contentsChangedMock.Reset();
            allStoppedMock.Reset();

            altSubject.enabled = false;

            Assert.AreEqual(1, altSubject.Elements.Count);

            Assert.IsFalse(firstStartedMock.Received);
            Assert.IsFalse(countChangedMock.Received);
            Assert.IsFalse(contentsChangedMock.Received);
            Assert.IsFalse(allStoppedMock.Received);

            altSubject.Remove(oneData);

            Assert.AreEqual(1, altSubject.Elements.Count);

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

        private class ActiveCollisionsContainerMock : ActiveCollisionsContainer
        {
            public virtual void ManualOnDisable()
            {
                base.OnDisable();
            }
        }

        private class ActiveCollisionsContainerNoDisableClearMock : ActiveCollisionsContainer
        {
            protected override void OnDisable() { }
        }
    }
}