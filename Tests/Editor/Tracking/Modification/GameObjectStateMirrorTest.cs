using Zinnia.Tracking.Modification;
using Zinnia.Extension;

namespace Test.Zinnia.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;

    public class GameObjectStateMirrorTest
    {
        private GameObject containingObject;
        private GameObjectStateMirror subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectStateMirror>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivateTargets()
        {
            GameObject source = new GameObject();
            GameObject target1 = new GameObject();
            GameObject target2 = new GameObject();
            GameObject target3 = new GameObject();

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);

            source.gameObject.SetActive(true);

            target1.gameObject.SetActive(true);
            target2.gameObject.SetActive(false);
            target3.gameObject.SetActive(false);

            Assert.IsTrue(source.gameObject.activeInHierarchy);
            Assert.IsTrue(target1.gameObject.activeInHierarchy);
            Assert.IsFalse(target2.gameObject.activeInHierarchy);
            Assert.IsFalse(target3.gameObject.activeInHierarchy);

            subject.Process();

            Assert.IsTrue(source.gameObject.activeInHierarchy);
            Assert.IsTrue(target1.gameObject.activeInHierarchy);
            Assert.IsTrue(target2.gameObject.activeInHierarchy);
            Assert.IsTrue(target3.gameObject.activeInHierarchy);
        }

        [Test]
        public void DeactivateTargets()
        {
            GameObject source = new GameObject();
            GameObject target1 = new GameObject();
            GameObject target2 = new GameObject();
            GameObject target3 = new GameObject();

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);

            source.gameObject.SetActive(false);

            target1.gameObject.SetActive(true);
            target2.gameObject.SetActive(false);
            target3.gameObject.SetActive(true);

            Assert.IsFalse(source.gameObject.activeInHierarchy);
            Assert.IsTrue(target1.gameObject.activeInHierarchy);
            Assert.IsFalse(target2.gameObject.activeInHierarchy);
            Assert.IsTrue(target3.gameObject.activeInHierarchy);

            subject.Process();

            Assert.IsFalse(source.gameObject.activeInHierarchy);
            Assert.IsFalse(target1.gameObject.activeInHierarchy);
            Assert.IsFalse(target2.gameObject.activeInHierarchy);
            Assert.IsFalse(target3.gameObject.activeInHierarchy);
        }

        [Test]
        public void ActivateThenDeactivateTargets()
        {
            GameObject source = new GameObject();
            GameObject target1 = new GameObject();
            GameObject target2 = new GameObject();
            GameObject target3 = new GameObject();

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);

            source.gameObject.SetActive(true);

            target1.gameObject.SetActive(true);
            target2.gameObject.SetActive(false);
            target3.gameObject.SetActive(false);

            Assert.IsTrue(source.gameObject.activeInHierarchy);
            Assert.IsTrue(target1.gameObject.activeInHierarchy);
            Assert.IsFalse(target2.gameObject.activeInHierarchy);
            Assert.IsFalse(target3.gameObject.activeInHierarchy);

            subject.Process();

            Assert.IsTrue(source.gameObject.activeInHierarchy);
            Assert.IsTrue(target1.gameObject.activeInHierarchy);
            Assert.IsTrue(target2.gameObject.activeInHierarchy);
            Assert.IsTrue(target3.gameObject.activeInHierarchy);

            source.gameObject.SetActive(false);

            subject.Process();

            Assert.IsFalse(source.gameObject.activeInHierarchy);
            Assert.IsFalse(target1.gameObject.activeInHierarchy);
            Assert.IsFalse(target2.gameObject.activeInHierarchy);
            Assert.IsFalse(target3.gameObject.activeInHierarchy);
        }
    }
}