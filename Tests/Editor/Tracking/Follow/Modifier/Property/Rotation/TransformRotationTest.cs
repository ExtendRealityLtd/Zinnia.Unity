using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformRotationTest
    {
        private GameObject containingObject;
        private TransformRotation subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformRotation>();
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
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(sourceRotation, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);
            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            subject.Modify(source, target, offset);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(new Quaternion(0.9f, 0f, -0.4f, 0f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(sourceRotation, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}