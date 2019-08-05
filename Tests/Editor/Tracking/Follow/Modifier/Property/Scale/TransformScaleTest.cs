using Zinnia.Tracking.Follow.Modifier.Property.Scale;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Scale
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformScaleTest
    {
        private GameObject containingObject;
        private TransformScale subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformScale>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.Modify(source, target);

            Assert.AreEqual(Vector3.one, source.transform.localScale);
            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            source.transform.localScale = Vector3.one * 4f;
            target.transform.localScale = Vector3.one;
            offset.transform.localScale = Vector3.one * 2f;

            subject.Modify(source, target, offset);

            Assert.AreEqual(Vector3.one * 4f, source.transform.localScale);
            Assert.AreEqual(Vector3.one * 2f, target.transform.localScale);

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

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;
            offset.transform.localScale = Vector3.one * 0.5f;

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.AreEqual(Vector3.one, source.transform.localScale);
            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.one, source.transform.localScale);
            Assert.AreEqual(Vector3.zero, target.transform.localScale);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(Vector3.one, source.transform.localScale);
            Assert.AreEqual(Vector3.zero, target.transform.localScale);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}