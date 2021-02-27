using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    using SystemTimeSpan = System.TimeSpan;

    public class FloatToTimeSpanTest
    {
        private GameObject containingObject;
        private FloatToTimeSpan subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatToTimeSpan>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void TransformFromSecondsA()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan result = subject.Transform(120f);

            Assert.AreEqual("00:02:00", result.ToString());
            Assert.AreEqual("00:02:00", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFromSecondsB()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = FloatToTimeSpan.TimeSpanProperty.Seconds;

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan result = subject.Transform(150f);

            Assert.AreEqual("00:02:30", result.ToString());
            Assert.AreEqual("00:02:30", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFromDays()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = FloatToTimeSpan.TimeSpanProperty.Days;

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan result = subject.Transform(2f);

            Assert.AreEqual("2.00:00:00", result.ToString());
            Assert.AreEqual("2.00:00:00", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFromHours()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = FloatToTimeSpan.TimeSpanProperty.Hours;

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan result = subject.Transform(2.5f);

            Assert.AreEqual("02:30:00", result.ToString());
            Assert.AreEqual("02:30:00", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFromMilliseconds()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = FloatToTimeSpan.TimeSpanProperty.Milliseconds;

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan result = subject.Transform(2.5f);

            Assert.AreEqual("00:00:00.0030000", result.ToString());
            Assert.AreEqual("00:00:00.0030000", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFromMinutes()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.TimeSpanValue = FloatToTimeSpan.TimeSpanProperty.Minutes;

            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            SystemTimeSpan result = subject.Transform(2.5f);

            Assert.AreEqual("00:02:30", result.ToString());
            Assert.AreEqual("00:02:30", subject.Result.ToString());
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            SystemTimeSpan result = subject.Transform(1.3f);

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

            SystemTimeSpan result = subject.Transform(1.3f);

            Assert.AreEqual(SystemTimeSpan.Zero, result);
            Assert.AreEqual(SystemTimeSpan.Zero, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}