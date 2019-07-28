using Zinnia.Tracking.Follow.Modifier;
using Zinnia.Tracking.Follow.Modifier.Property;

namespace Test.Zinnia.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class FollowModifierTest
    {
        private GameObject containingObject;
        private FollowModifier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<FollowModifier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            UnityEventListenerMock premodifiedMock = new UnityEventListenerMock();
            UnityEventListenerMock modifiedMock = new UnityEventListenerMock();
            subject.Premodified.AddListener(premodifiedMock.Listen);
            subject.Modified.AddListener(modifiedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            PropertyModifierMock positionMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock rotationMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock scaleMock = source.AddComponent<PropertyModifierMock>();

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            subject.PositionModifier = positionMock;
            subject.RotationModifier = rotationMock;
            subject.ScaleModifier = scaleMock;

            subject.Modify(source, target, offset);

            Assert.AreEqual(source, subject.CachedSource);
            Assert.AreEqual(target, subject.CachedTarget);
            Assert.AreEqual(offset, subject.CachedOffset);
            Assert.IsTrue(premodifiedMock.Received);
            Assert.IsTrue(modifiedMock.Received);
            Assert.IsTrue(positionMock.modified);
            Assert.IsTrue(rotationMock.modified);
            Assert.IsTrue(scaleMock.modified);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyNoOffset()
        {
            UnityEventListenerMock premodifiedMock = new UnityEventListenerMock();
            UnityEventListenerMock modifiedMock = new UnityEventListenerMock();
            subject.Premodified.AddListener(premodifiedMock.Listen);
            subject.Modified.AddListener(modifiedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();

            PropertyModifierMock positionMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock rotationMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock scaleMock = source.AddComponent<PropertyModifierMock>();

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            subject.PositionModifier = positionMock;
            subject.RotationModifier = rotationMock;
            subject.ScaleModifier = scaleMock;

            subject.Modify(source, target);

            Assert.AreEqual(source, subject.CachedSource);
            Assert.AreEqual(target, subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsTrue(premodifiedMock.Received);
            Assert.IsTrue(modifiedMock.Received);
            Assert.IsTrue(positionMock.modified);
            Assert.IsTrue(rotationMock.modified);
            Assert.IsTrue(scaleMock.modified);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyNoSource()
        {
            UnityEventListenerMock premodifiedMock = new UnityEventListenerMock();
            UnityEventListenerMock modifiedMock = new UnityEventListenerMock();
            subject.Premodified.AddListener(premodifiedMock.Listen);
            subject.Modified.AddListener(modifiedMock.Listen);

            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            PropertyModifierMock positionMock = target.AddComponent<PropertyModifierMock>();
            PropertyModifierMock rotationMock = target.AddComponent<PropertyModifierMock>();
            PropertyModifierMock scaleMock = target.AddComponent<PropertyModifierMock>();

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            subject.PositionModifier = positionMock;
            subject.RotationModifier = rotationMock;
            subject.ScaleModifier = scaleMock;

            subject.Modify(null, target, offset);

            Assert.IsNull(subject.CachedSource);
            Assert.AreEqual(target, subject.CachedTarget);
            Assert.AreEqual(offset, subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyNoTarget()
        {
            UnityEventListenerMock premodifiedMock = new UnityEventListenerMock();
            UnityEventListenerMock modifiedMock = new UnityEventListenerMock();
            subject.Premodified.AddListener(premodifiedMock.Listen);
            subject.Modified.AddListener(modifiedMock.Listen);

            GameObject source = new GameObject();
            GameObject offset = new GameObject();

            PropertyModifierMock positionMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock rotationMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock scaleMock = source.AddComponent<PropertyModifierMock>();

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            subject.PositionModifier = positionMock;
            subject.RotationModifier = rotationMock;
            subject.ScaleModifier = scaleMock;

            subject.Modify(source, null, offset);

            Assert.AreEqual(source, subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.AreEqual(offset, subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            UnityEventListenerMock premodifiedMock = new UnityEventListenerMock();
            UnityEventListenerMock modifiedMock = new UnityEventListenerMock();
            subject.Premodified.AddListener(premodifiedMock.Listen);
            subject.Modified.AddListener(modifiedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            PropertyModifierMock positionMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock rotationMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock scaleMock = source.AddComponent<PropertyModifierMock>();

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            subject.PositionModifier = positionMock;
            subject.RotationModifier = rotationMock;
            subject.ScaleModifier = scaleMock;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target, offset);

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            UnityEventListenerMock premodifiedMock = new UnityEventListenerMock();
            UnityEventListenerMock modifiedMock = new UnityEventListenerMock();
            subject.Premodified.AddListener(premodifiedMock.Listen);
            subject.Modified.AddListener(modifiedMock.Listen);

            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            PropertyModifierMock positionMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock rotationMock = source.AddComponent<PropertyModifierMock>();
            PropertyModifierMock scaleMock = source.AddComponent<PropertyModifierMock>();

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            subject.PositionModifier = positionMock;
            subject.RotationModifier = rotationMock;
            subject.ScaleModifier = scaleMock;

            subject.enabled = false;
            subject.Modify(source, target, offset);

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }
    }

    public class PropertyModifierMock : PropertyModifier
    {
        public bool modified;

        protected override void DoModify(GameObject source, GameObject target, GameObject offset = null)
        {
            modified = true;
        }
    }
}