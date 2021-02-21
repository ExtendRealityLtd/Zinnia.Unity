using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using System.Globalization;
    using System.Threading;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    using SystemTimeSpan = System.TimeSpan;

    public class StringToTimeSpanTest
    {
        private GameObject containingObject;
        private StringToTimeSpan subject;
        private CultureInfo cachedCulture;

        [SetUp]
        public void SetUp()
        {
            cachedCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            containingObject = new GameObject();
            subject = containingObject.AddComponent<StringToTimeSpan>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
            Thread.CurrentThread.CurrentCulture = cachedCulture;
        }

        [Test]
        public void Transform()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan expected = new SystemTimeSpan(5, 4, 3, 2, 1);
            SystemTimeSpan result = subject.Transform("5:4:3:2.001");

            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan expected = new SystemTimeSpan(5, 4, 3, 2, 1);
            SystemTimeSpan result = subject.Transform("5:4:3:2.001");

            Assert.AreEqual(SystemTimeSpan.Zero, result);
            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.enabled = false;

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan expected = new SystemTimeSpan(5, 4, 3, 2, 1);
            SystemTimeSpan result = subject.Transform("5:4:3:2.001");

            Assert.AreEqual(SystemTimeSpan.Zero, result);
            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}