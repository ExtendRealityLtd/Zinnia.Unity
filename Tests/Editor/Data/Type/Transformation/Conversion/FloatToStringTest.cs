using Zinnia.Data.Type.Transformation.Conversion;

namespace Test.Zinnia.Data.Type.Transformation.Conversion
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class FloatToStringTest
    {
        private GameObject containingObject;
        private FloatToString subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FloatToString>();
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

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            string result = subject.Transform(2.217354f);

            Assert.AreEqual("2.217354", result);
            Assert.AreEqual("2.217354", subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFormat()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = "0.00";

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            string result = subject.Transform(2.217354f);

            Assert.AreEqual("2.22", result);
            Assert.AreEqual("2.22", subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFormatNumber()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = "n3";

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            string result = subject.Transform(2.217354f);

            Assert.AreEqual("2.217", result);
            Assert.AreEqual("2.217", subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFormatPercent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = "P";

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            string result = subject.Transform(2.217354f);

            Assert.AreEqual("221.74%", result);
            Assert.AreEqual("221.74%", subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFormatWhitespace()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = "  ";

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            string result = subject.Transform(2.217354f);

            Assert.AreEqual("2.217354", result);
            Assert.AreEqual("2.217354", subject.Result);
            Assert.IsTrue(transformedListenerMock.Received);
        }

        [Test]
        public void TransformFormatUnknown()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.Format = "t";

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);

            NUnit.Framework.Assert.Throws<System.FormatException>(() => subject.Transform(2.217354f));

            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveGameObject()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.gameObject.SetActive(false);

            string result = subject.Transform(2.2f);

            Assert.IsNull(result);
            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }

        [Test]
        public void TransformInactiveComponent()
        {
            UnityEventListenerMock transformedListenerMock = new UnityEventListenerMock();
            subject.Transformed.AddListener(transformedListenerMock.Listen);
            subject.enabled = false;

            string result = subject.Transform(2.2f);

            Assert.IsNull(result);
            Assert.IsNull(subject.Result);
            Assert.IsFalse(transformedListenerMock.Received);
        }
    }
}