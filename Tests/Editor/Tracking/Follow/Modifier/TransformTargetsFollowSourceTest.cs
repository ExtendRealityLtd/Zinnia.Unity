using VRTK.Core.Tracking.Follow.Modifier;

namespace Test.VRTK.Core.Tracking.Follow.Modifier
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformTargetsFollowSourceTest
    {
        private GameObject containingObject;
        private TargetsFollowSourceMock subject;
        private ModifyTransform modifier;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TargetsFollowSourceMock>();
            modifier = containingObject.AddComponent<ModifyTransform>();
            subject.appliedModifier = modifier;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(modifier);
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ProcessAll()
        {
            subject.ManualAwake();
            Assert.AreEqual(FollowModifier.ProcessTarget.All, subject.ProcessType);
        }

        [Test]
        public void UpdatePosition()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.ManualAwake();
            subject.UpdatePosition(source.transform, target.transform);

            Assert.AreEqual(source.transform.position, Vector3.one);
            Assert.AreEqual(target.transform.position, Vector3.one);
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

            source.transform.rotation = targetRotation;
            target.transform.rotation = Quaternion.identity;

            subject.ManualAwake();
            subject.UpdateRotation(source.transform, target.transform);

            Assert.AreEqual(source.transform.rotation, targetRotation);
            Assert.AreEqual(target.transform.rotation, targetRotation);
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

            source.transform.localScale = Vector3.one;
            target.transform.localScale = Vector3.zero;

            subject.ManualAwake();
            subject.UpdateScale(source.transform, target.transform);

            Assert.AreEqual(source.transform.localScale, Vector3.one);
            Assert.AreEqual(target.transform.localScale, Vector3.one);
            Assert.AreEqual(source.transform, subject.CachedSource);
            Assert.AreEqual(target.transform, subject.CachedTarget);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }

    public class TargetsFollowSourceMock : TargetsFollowSource
    {
        public void ManualAwake()
        {
            Awake();
        }
    }
}