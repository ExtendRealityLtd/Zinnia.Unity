using Zinnia.Data.Type;
using Zinnia.Data.Type.Transformation;

namespace Test.Zinnia.Data.Type.Transformation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatRangeValueRemapperTest
    {
        private FloatRangeValueRemapper subject;

        [SetUp]
        public void SetUp()
        {
            subject = new GameObject().AddComponent<FloatRangeValueRemapper>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void RemapByLerp()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Mode = FloatRangeValueRemapper.OutputMode.Lerp;
            subject.From = new FloatRange(0f, 10f);
            subject.To = new FloatRange(0f, 1f);

            float input = 2f;
            float t = Mathf.InverseLerp(0, 10f, input);
            float expected = Mathf.Lerp(0f, 1f, t);
            float result = subject.Transform(input);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void RemapBySmoothStep()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);

            Assert.AreEqual(0f, subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            subject.Mode = FloatRangeValueRemapper.OutputMode.SmoothStep;
            subject.From = new FloatRange(0f, 10f);
            subject.To = new FloatRange(0f, 1f);

            float input = 2f;
            float t = Mathf.InverseLerp(0, 10f, input);
            float expected = Mathf.SmoothStep(0f, 1f, t);
            float result = subject.Transform(input);
            Assert.AreEqual(expected, result);
            Assert.AreEqual(expected, subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }
    }
}