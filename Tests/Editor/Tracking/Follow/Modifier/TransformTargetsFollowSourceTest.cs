namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformTargetsFollowSourceTest
    {
        private GameObject containingObject;
        private TransformTargetsFollowSource subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformTargetsFollowSource>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ProcessAll()
        {
            Assert.IsFalse(subject.ProcessFirstAndActiveOnly());
        }

        [Test]
        public void UpdatePosition()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.UpdatePosition(source.transform, target.transform);

            Assert.AreEqual(source.transform.position, Vector3.one);
            Assert.AreEqual(target.transform.position, Vector3.one);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
        }

        [Test]
        public void UpdateRotation()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion targetRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = targetRotation;
            target.transform.rotation = Quaternion.identity;

            subject.UpdateRotation(source.transform, target.transform);

            Assert.AreEqual(source.transform.rotation, targetRotation);
            Assert.AreEqual(target.transform.rotation, targetRotation);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
        }

        [Test]
        public void UpdateScale()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.localScale = Vector3.one;
            target.transform.localScale = Vector3.zero;

            subject.UpdateScale(source.transform, target.transform);

            Assert.AreEqual(source.transform.localScale, Vector3.one);
            Assert.AreEqual(target.transform.localScale, Vector3.one);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
        }
    }
}