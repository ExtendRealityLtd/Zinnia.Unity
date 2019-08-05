using Zinnia.Tracking.Follow.Modifier.Property.Position;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformPositionTest
    {
        private GameObject containingObject;
        private TransformPosition subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformPosition>();
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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.Modify(source, target);

            Assert.AreEqual(Vector3.one, source.transform.position);
            Assert.AreEqual(Vector3.one, target.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            source.transform.position = Vector3.one * 2f;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 0.5f;

            subject.Modify(source, target, offset);

            Assert.AreEqual(Vector3.one * 2f, source.transform.position);
            Assert.AreEqual(Vector3.one * 1.5f, target.transform.position);

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

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 0.5f;

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.AreEqual(Vector3.one, source.transform.position);
            Assert.AreEqual(Vector3.one, target.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.one, source.transform.position);
            Assert.AreEqual(Vector3.zero, target.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.one, source.transform.position);
            Assert.AreEqual(Vector3.zero, target.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}