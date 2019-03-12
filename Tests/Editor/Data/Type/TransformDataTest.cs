﻿using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using UnityEngine;
    using NUnit.Framework;

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
    }
}