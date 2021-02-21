using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    using SystemTimeSpan = System.TimeSpan;

    public class TimeSpanToFloatTest
    {
        private GameObject containingObject;
        private TimeSpanToFloat subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TimeSpanToFloat>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformDays()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.Days;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(5f, result);
            Assert.AreEqual(5f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformHours()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.Hours;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(4f, result);
            Assert.AreEqual(4f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformMilliseconds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.Milliseconds;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(1f, result);
            Assert.AreEqual(1f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformMinutes()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.Minutes;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(3f, result);
            Assert.AreEqual(3f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformSeconds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.Seconds;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(2f, result);
            Assert.AreEqual(2f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformTicks()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.Ticks;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual("4.46582E+12", result.ToString());
            Assert.AreEqual("4.46582E+12", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformTotalDays()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.TotalDays;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(5.168773f, result);
            Assert.AreEqual(5.168773f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformTotalHours()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.TotalHours;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(124.0506f.ToString(), result.ToString());
            Assert.AreEqual(124.0506f.ToString(), subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformTotalMilliseconds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.TotalMilliseconds;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(4.46582E+08f, result);
            Assert.AreEqual(4.46582E+08f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformTotalMinutes()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.TotalMinutes;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(7443.033f, result);
            Assert.AreEqual(7443.033f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformTotalSeconds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = TimeSpanToFloat.TimeSpanProperty.TotalSeconds;

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(446582f, result);
            Assert.AreEqual(446582f, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.enabled = false;

            SystemTimeSpan timeSpan = new SystemTimeSpan(5, 4, 3, 2, 1);
            float result = subject.Transform(timeSpan);

            Assert.AreEqual(0f, result);
            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}