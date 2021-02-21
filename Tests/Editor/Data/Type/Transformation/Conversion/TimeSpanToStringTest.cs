using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    using SystemTimeSpan = System.TimeSpan;

    public class TimeSpanToStringTest
    {
        private GameObject containingObject;
        private TimeSpanToString subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TimeSpanToString>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Transform()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = @"d\:hh\:mm\:ss";

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            string result = subject.Transform(timeSpan);

            Assert.AreEqual("5:04:03:02", result);
            Assert.AreEqual("5:04:03:02", subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = @"d\:hh\:mm\:ss";
            subject.gameObject.SetActive(false);

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            string result = subject.Transform(timeSpan);

            Assert.IsNull(result);
            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = @"d\:hh\:mm\:ss";
            subject.enabled = false;

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            string result = subject.Transform(timeSpan);

            Assert.IsNull(result);
            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}