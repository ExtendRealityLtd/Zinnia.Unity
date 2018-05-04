namespace VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformSourceFollowActiveTargetTest
    {
        private GameObject containingObject;
        private TransformSourceFollowActiveTarget subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformSourceFollowActiveTarget>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ProcessFirstAndActiveOnly()
        {
            Assert.IsTrue(subject.ProcessFirstAndActiveOnly());
        }

        [Test]
        public void UpdatePosition()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.position = Vector3.zero;
            target.transform.position = Vector3.one;

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

            source.transform.rotation = Quaternion.identity;
            target.transform.rotation = targetRotation;

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

            source.transform.localScale = Vector3.zero;
            target.transform.localScale = Vector3.one;

            subject.UpdateScale(source.transform, target.transform);

            Assert.AreEqual(source.transform.localScale, Vector3.one);
            Assert.AreEqual(target.transform.localScale, Vector3.one);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);
        }
    }
}