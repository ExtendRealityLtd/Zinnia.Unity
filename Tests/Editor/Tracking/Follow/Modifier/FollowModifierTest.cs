using VRTK.Core.Tracking.Follow.Modifier;
using VRTK.Core.Tracking.Follow.Modifier.Property;

namespace Test.VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

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
            Object.DestroyImmediate(subject);
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

            subject.positionModifier = positionMock;
            subject.rotationModifier = rotationMock;
            subject.scaleModifier = scaleMock;

            subject.Modify(source.transform, target.transform, offset.transform);

            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
            Assert.AreEqual(offset.transform, subject.CachedOffset);
            Assert.IsTrue(premodifiedMock.Received);
            Assert.IsTrue(modifiedMock.Received);
            Assert.IsTrue(positionMock.modified);
            Assert.IsTrue(rotationMock.modified);
            Assert.IsTrue(scaleMock.modified);

            Object.DestroyImmediate(subject);
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

            subject.positionModifier = positionMock;
            subject.rotationModifier = rotationMock;
            subject.scaleModifier = scaleMock;

            subject.Modify(source.transform, target.transform);

            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsTrue(premodifiedMock.Received);
            Assert.IsTrue(modifiedMock.Received);
            Assert.IsTrue(positionMock.modified);
            Assert.IsTrue(rotationMock.modified);
            Assert.IsTrue(scaleMock.modified);

            Object.DestroyImmediate(subject);
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

            subject.positionModifier = positionMock;
            subject.rotationModifier = rotationMock;
            subject.scaleModifier = scaleMock;

            subject.Modify(null, target.transform, offset.transform);

            Assert.IsNull(subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
            Assert.AreEqual(offset.transform, subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(subject);
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

            subject.positionModifier = positionMock;
            subject.rotationModifier = rotationMock;
            subject.scaleModifier = scaleMock;

            subject.Modify(source.transform, null, offset.transform);

            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.AreEqual(offset.transform, subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(subject);
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

            subject.positionModifier = positionMock;
            subject.rotationModifier = rotationMock;
            subject.scaleModifier = scaleMock;

            subject.gameObject.SetActive(false);
            subject.Modify(source.transform, target.transform, offset.transform);

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(subject);
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

            subject.positionModifier = positionMock;
            subject.rotationModifier = rotationMock;
            subject.scaleModifier = scaleMock;

            subject.enabled = false;
            subject.Modify(source.transform, target.transform, offset.transform);

            Assert.IsNull(subject.CachedSource);
            Assert.IsNull(subject.CachedTarget);
            Assert.IsNull(subject.CachedOffset);
            Assert.IsFalse(premodifiedMock.Received);
            Assert.IsFalse(modifiedMock.Received);
            Assert.IsFalse(positionMock.modified);
            Assert.IsFalse(rotationMock.modified);
            Assert.IsFalse(scaleMock.modified);

            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }
    }

    public class PropertyModifierMock : PropertyModifier
    {
        public bool modified = false;

        protected override void DoModify(Transform source, Transform target, Transform offset = null)
        {
            modified = true;
        }
    }
}