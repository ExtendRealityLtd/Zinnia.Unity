using Zinnia.Data.Type.Observer;

namespace Test.Zinnia.Data.Type.Observer
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatObservablePropertyTest
    {
        private GameObject containingObject;
        private FloatObservableProperty subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatObservableProperty>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator SetData()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock defaultedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock definedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);
            subject.Defaulted.AddListener(defaultedListenerMock.Listen);
            subject.Defined.AddListener(definedListenerMock.Listen);

            yield return null;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.Data = 1f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsTrue(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.Data = 1f;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsTrue(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.Data = 2f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(2f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.Data = 0f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsTrue(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(0f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();
        }

        [UnityTest]
        public IEnumerator SetDataInDisabledEmitWhenReenabled()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock defaultedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock definedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);
            subject.Defaulted.AddListener(defaultedListenerMock.Listen);
            subject.Defined.AddListener(definedListenerMock.Listen);

            subject.ObserveChangesFromDisabledState = true;

            yield return null;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.Data = 1f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsTrue(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.enabled = false;
            yield return null;

            subject.Data = 2f;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(2f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.enabled = true;
            yield return null;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(2f, subject.Data);
        }

        [UnityTest]
        public IEnumerator SetDataInDisabledDontEmitWhenReenabled()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock defaultedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock definedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);
            subject.Defaulted.AddListener(defaultedListenerMock.Listen);
            subject.Defined.AddListener(definedListenerMock.Listen);

            subject.ObserveChangesFromDisabledState = false;

            yield return null;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(default, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.Data = 1f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsTrue(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.enabled = false;
            yield return null;

            subject.Data = 2f;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(2f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.enabled = true;
            yield return null;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(2f, subject.Data);
        }

        [Test]
        public void ResetToDefault()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock defaultedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock definedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);
            subject.Defaulted.AddListener(defaultedListenerMock.Listen);
            subject.Defined.AddListener(definedListenerMock.Listen);

            subject.Data = 1f;

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsTrue(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();

            subject.ResetToDefault();

            Assert.IsTrue(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsTrue(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(0f, subject.Data);

            modifiedListenerMock.Reset();
            unmodifiedListenerMock.Reset();
            defaultedListenerMock.Reset();
            definedListenerMock.Reset();
        }

        [Test]
        public void DontEmitEventsOnDisabledGameObject()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock defaultedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock definedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);
            subject.Defaulted.AddListener(defaultedListenerMock.Listen);
            subject.Defined.AddListener(definedListenerMock.Listen);

            subject.gameObject.SetActive(false);

            subject.Data = 1f;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);
        }

        [Test]
        public void DontEmitEventsOnDisabledComponent()
        {
            UnityEventListenerMock modifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock unmodifiedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock defaultedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock definedListenerMock = new UnityEventListenerMock();
            subject.Modified.AddListener(modifiedListenerMock.Listen);
            subject.Unmodified.AddListener(unmodifiedListenerMock.Listen);
            subject.Defaulted.AddListener(defaultedListenerMock.Listen);
            subject.Defined.AddListener(definedListenerMock.Listen);

            subject.enabled = false;

            subject.Data = 1f;

            Assert.IsFalse(modifiedListenerMock.Received);
            Assert.IsFalse(unmodifiedListenerMock.Received);
            Assert.IsFalse(defaultedListenerMock.Received);
            Assert.IsFalse(definedListenerMock.Received);
            Assert.AreEqual(1f, subject.Data);
        }
    }
}