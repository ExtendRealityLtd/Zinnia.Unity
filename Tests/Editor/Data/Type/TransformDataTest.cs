using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Transform defaultTransform = new GameObject("TransformDataTest").transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.That(transformData.Position, Is.EqualTo(Vector3.zero).Using(comparer));
            transformData.PositionOverride = Vector3.one;
            Assert.That(transformData.Position, Is.EqualTo(Vector3.one).Using(comparer));
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverrideRotation()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            Transform defaultTransform = new GameObject("TransformDataTest").transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.That(transformData.Rotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            transformData.RotationOverride = new Quaternion(1f, 1f, 1f, 0f);
            Assert.That(transformData.Rotation, Is.EqualTo(new Quaternion(1f, 1f, 1f, 0f)).Using(comparer));
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverrideScale()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Transform defaultTransform = new GameObject("TransformDataTest").transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.That(transformData.Scale, Is.EqualTo(Vector3.one).Using(comparer));
            transformData.ScaleOverride = Vector3.zero;
            Assert.That(transformData.Scale, Is.EqualTo(Vector3.zero).Using(comparer));
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void UseLocalValues()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Transform parent = new GameObject("TransformDataTest").transform;
            Transform child = new GameObject("TransformDataTest").transform;

            child.SetParent(parent);
            parent.localPosition = Vector3.one;
            child.localPosition = Vector3.one * 2f;

            TransformData transformData = new TransformData(child);

            Assert.That(transformData.Position, Is.EqualTo(Vector3.one * 3f).Using(comparer));

            transformData.UseLocalValues = true;

            Assert.That(transformData.Position, Is.EqualTo(Vector3.one * 2f).Using(comparer));

            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void Comparison()
        {
            Transform subject = new GameObject("TransformDataTest").transform;

            TransformData subjectA = new TransformData(subject);
            TransformData subjectB = new TransformData(subject);

            Assert.IsFalse(subjectA == subjectB);
            Assert.IsTrue(subjectA.Equals(subjectB));
            Assert.AreEqual(subjectA, subjectB);

            Object.DestroyImmediate(subject.gameObject);
        }
    }
}