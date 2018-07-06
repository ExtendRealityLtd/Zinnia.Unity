using VRTK.Core.Tracking.Follow.Modifier;

namespace Test.VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;

    public class ModifyTransformTest
    {
        private GameObject containingObject;
        private ModifyTransform subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ModifyTransform>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void UpdatePosition()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.one;

            subject.UpdatePosition(source.transform, target.transform);

            Assert.AreEqual(Vector3.one, source.transform.position);
            Assert.AreEqual(Vector3.one, target.transform.position);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void UpdateRotation()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion targetRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = Quaternion.identity;
            target.transform.rotation = targetRotation;

            subject.UpdateRotation(source.transform, target.transform);

            Assert.AreEqual(targetRotation, source.transform.rotation);
            Assert.AreEqual(targetRotation, target.transform.rotation);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void UpdateScale()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.localScale = Vector3.zero;
            target.transform.localScale = Vector3.one;

            subject.UpdateScale(source.transform, target.transform);

            Assert.AreEqual(Vector3.one, source.transform.localScale);
            Assert.AreEqual(Vector3.one, target.transform.localScale);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void UpdateWithOffset()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.one * 2f;
            offset.transform.position = Vector3.one * 0.5f;

            Quaternion targetRotation = new Quaternion(1f, 0f, 0f, 0f);
            source.transform.rotation = Quaternion.identity;
            target.transform.rotation = targetRotation;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            source.transform.localScale = Vector3.zero;
            target.transform.localScale = Vector3.one * 2f;
            offset.transform.localScale = Vector3.one * 0.5f;

            subject.UpdatePosition(source.transform, target.transform, offset.transform);
            subject.UpdateRotation(source.transform, target.transform, offset.transform);
            subject.UpdateScale(source.transform, target.transform, offset.transform);

            Assert.AreEqual(Vector3.one * 1.5f, source.transform.position);
            Assert.AreEqual(Vector3.one * 2f, target.transform.position);
            Assert.AreEqual(new Quaternion(0.9f, 0f, -0.4f, 0f).ToString(), source.transform.rotation.ToString());
            Assert.AreEqual(targetRotation, target.transform.rotation);
            Assert.AreEqual(Vector3.one * 1.5f, source.transform.localScale);
            Assert.AreEqual(Vector3.one * 2f, target.transform.localScale);

            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
            Assert.AreEqual(offset.transform, subject.CachedOffset);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }
    }
}