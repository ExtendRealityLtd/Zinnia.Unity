using Zinnia.Action;

namespace Test.Zinnia.Action
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class BooleanActionTest
    {
        private GameObject containingObject;
        private BooleanActionMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<BooleanActionMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivatedEmitted()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DeactivatedEmitted()
        {
            subject.SetIsActivated(true);
            subject.SetValue(true);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DeactivatedEmittedFromReceivingDefaultValue()
        {
            subject.SetIsActivated(true);
            subject.SetValue(true);

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.ReceiveDefaultValue();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void ChangedEmitted()
        {
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            changedListenerMock.Reset();
            unchangedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsTrue(unchangedListenerMock.Received);

            changedListenerMock.Reset();
            unchangedListenerMock.Reset();
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(false);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DefaultValueNotTypeDefault()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.DefaultValue = true;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void DefaultValueTypeDefault()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.DefaultValue = false;

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsTrue(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnInactiveGameObject()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.gameObject.SetActive(false);
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void EventsNotEmittedOnDisabledComponent()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.enabled = false;
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            subject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [Test]
        public void AddSource()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            Assert.AreEqual(0, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            sourceMock.SetIsActivated(false);
            sourceMock.SetValue(false);

            subject.AddSource(sourceMock);

            Assert.AreEqual(1, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void RemoveSource()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            subject.AddSource(sourceMock);

            Assert.AreEqual(1, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Assert.IsTrue(subject.RemoveSource(sourceMock));

            sourceMock.SetIsActivated(false);
            sourceMock.SetValue(false);
            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            Assert.AreEqual(0, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Assert.IsFalse(subject.RemoveSource(sourceMock));

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void ClearSources()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            subject.ValueChanged.AddListener(changedListenerMock.Listen);
            subject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            subject.AddSource(sourceMock);

            Assert.AreEqual(1, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            subject.ClearSources();

            sourceMock.SetIsActivated(false);
            sourceMock.SetValue(false);
            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            Assert.AreEqual(0, subject.ReadOnlySources.Count);

            sourceMock.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            Object.DestroyImmediate(sourceObject);
        }

        [Test]
        public void SourcesContains()
        {
            GameObject sourceObject = new GameObject();
            BooleanActionMock sourceMock = sourceObject.AddComponent<BooleanActionMock>();

            Assert.IsFalse(subject.SourcesContains(sourceMock));

            subject.AddSource(sourceMock);

            Assert.IsTrue(subject.SourcesContains(sourceMock));

            subject.RemoveSource(sourceMock);

            Assert.IsFalse(subject.SourcesContains(sourceMock));

            Object.DestroyImmediate(sourceObject);
        }

        [UnityTest]
        public IEnumerator DefaultValueFalseAndInitialValueTrue()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            BooleanActionLiveMock liveSubject = containingObject.AddComponent<BooleanActionLiveMock>();
            liveSubject.SetInitialValue(true);

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            yield return null;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator DefaultValueTrueAndInitialValueTrue()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            BooleanActionLiveMock liveSubject = containingObject.AddComponent<BooleanActionLiveMock>();

            liveSubject.DefaultValue = true;
            liveSubject.SetInitialValue(true);

            liveSubject.ForceAwake();

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            yield return null;

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(false);

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator DefaultValueTrueAndInitialValueFalse()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            BooleanActionLiveMock liveSubject = containingObject.AddComponent<BooleanActionLiveMock>();

            liveSubject.DefaultValue = true;
            liveSubject.SetInitialValue(false);

            liveSubject.ForceAwake();

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            yield return null;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(true);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }

        [UnityTest]
        public IEnumerator ResetToInitialValue()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock changedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unchangedListenerMock = new UnityEventListenerMock();

            BooleanActionLiveMock liveSubject = containingObject.AddComponent<BooleanActionLiveMock>();
            liveSubject.SetInitialValue(true);

            liveSubject.Activated.AddListener(activatedListenerMock.Listen);
            liveSubject.Deactivated.AddListener(deactivatedListenerMock.Listen);
            liveSubject.ValueChanged.AddListener(changedListenerMock.Listen);
            liveSubject.ValueUnchanged.AddListener(unchangedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsFalse(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            yield return null;

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.Receive(false);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            changedListenerMock.Reset();
            unchangedListenerMock.Reset();

            liveSubject.ReceiveInitialValue();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);
            Assert.IsTrue(changedListenerMock.Received);
            Assert.IsFalse(unchangedListenerMock.Received);
        }
    }

    public class BooleanActionMock : BooleanAction
    {
        public virtual void SetIsActivated(bool value)
        {
            IsActivated = value;
        }

        public virtual void SetValue(bool value)
        {
            Value = value;
        }
    }

    public class BooleanActionLiveMock : BooleanAction
    {
        public virtual void ForceAwake()
        {
            IsActivated = false;
            Awake();
        }

        public virtual void SetInitialValue(bool value)
        {
            InitialValue = value;
        }
    }
}