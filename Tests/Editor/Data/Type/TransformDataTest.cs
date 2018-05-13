namespace VRTK.Core.Data.Type
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformDataTest
    {
        [Test]
        public void DefaultConstructor()
        {
            TransformData transformData = new TransformData();
            Assert.IsNull(transformData.transform);
        }

        [Test]
        public void TransformConstructor()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.AreEqual(defaultTransform, transformData.transform);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverridePosition()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.AreEqual(Vector3.zero, transformData.Position);
            transformData.positionOverride = Vector3.one;
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
            transformData.rotationOverride = rotationOverride;
            Assert.AreEqual(rotationOverride, transformData.Rotation);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }

        [Test]
        public void OverrideScale()
        {
            Transform defaultTransform = new GameObject().transform;
            TransformData transformData = new TransformData(defaultTransform);
            Assert.AreEqual(Vector3.one, transformData.LocalScale);
            transformData.localScaleOverride = Vector3.zero;
            Assert.AreEqual(Vector3.zero, transformData.LocalScale);
            Object.DestroyImmediate(defaultTransform.gameObject);
        }
    }
}