using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformDataTest
    {
        [Test]
        public void DefaultConstructor()
        {
            TransformData transformData = new TransformData();
            Assert.IsNull(transformData.Transform);
        }

        [Test]
        public void TransformConstructor()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.AreEqual(defaultTransform, transformData.Transform);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void Clear()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);

            transformData.Clear();

            Assert.IsNull(transformData.Transform);

            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverridePosition()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.AreEqual(Vector3.zero, transformData.Position);
            transformData.PositionOverride = Vector3.one;
            Assert.AreEqual(Vector3.one, transformData.Position);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverrideRotation()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Quaternion rotationOverride = new Quaternion(1f, 1f, 1f, 0f);
            Assert.AreEqual(Quaternion.identity, transformData.Rotation);
            transformData.RotationOverride = rotationOverride;
            Assert.AreEqual(rotationOverride, transformData.Rotation);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverrideScale()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.AreEqual(Vector3.one, transformData.Scale);
            transformData.ScaleOverride = Vector3.zero;
            Assert.AreEqual(Vector3.zero, transformData.Scale);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void UseLocalValues()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;

            child.SetParent(parent);
            parent.localPosition = Vector3.one;
            child.localPosition = Vector3.one * 2f;

            TransformData transformData = new TransformData(child);

            Assert.AreEqual(Vector3.one * 3f, transformData.Position);

            transformData.UseLocalValues = true;

            Assert.AreEqual(Vector3.one * 2f, transformData.Position);

            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void Comparison()
        {
            Transform subject = new GameObject().transform;

            TransformData subjectA = new TransformData(subject);
            TransformData subjectB = new TransformData(subject);

            Assert.IsFalse(subjectA == subjectB);
            Assert.IsTrue(subjectA.Equals(subjectB));
            Assert.AreEqual(subjectA, subjectB);

            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void ConvertToString()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);

            transformData.PositionOverride = Vector3.one * 2;
            transformData.RotationOverride = new Quaternion(3f, 2f, 1f, 0f);
            transformData.ScaleOverride = Vector3.one * 2;

            Assert.AreEqual("{ Transform = New Game Object (UnityEngine.Transform) | UseLocalValues = False | PositionOverride = (2.0, 2.0, 2.0) | RotationOverride = (3.0, 2.0, 1.0, 0.0) | ScaleOverride = (2.0, 2.0, 2.0) }", transformData.ToString());

            Object.DestroyImmediate(defaultTransform.gameObject);
        }
    }
}