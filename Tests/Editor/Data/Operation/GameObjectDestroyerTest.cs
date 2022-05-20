using Zinnia.Data.Operation;

namespace Test.Zinnia.Data.Operation
{
    using NUnit.Framework;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class GameObjectDestroyerTest
    {
        private GameObject containingObject;
        private GameObjectDestroyer subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectDestroyer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator DestroyGameObject()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.DoDestroy(test);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(test == null);
        }

        [UnityTest]
        public IEnumerator DestroyTarget()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.Target = test;
            subject.DoDestroy();
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(test == null);
        }

        [Test]
        public void DestroyGameObjectImmediately()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.DestroyAtEndOfFrame = false;
            subject.DoDestroy(test);
            Assert.IsTrue(test == null);
        }

        [Test]
        public void DestroyTargetImmediately()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.Target = test;
            subject.DestroyAtEndOfFrame = false;
            subject.DoDestroy();
            Assert.IsTrue(test == null);
        }

        [UnityTest]
        public IEnumerator DestroyInactiveGameObject()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.gameObject.SetActive(false);
            subject.DoDestroy(test);
            yield return new WaitForEndOfFrame();
            Assert.IsFalse(test == null);
        }

        [UnityTest]
        public IEnumerator DestroyInactiveComponent()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.enabled = false;
            subject.DoDestroy(test);
            yield return new WaitForEndOfFrame();
            Assert.IsFalse(test == null);
        }

        [Test]
        public void DestroyImmediatelyInactiveGameObject()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.DestroyAtEndOfFrame = false;
            subject.gameObject.SetActive(false);
            subject.DoDestroy(test);
            Assert.IsFalse(test == null);
        }

        [Test]
        public void DestroyImmediatelyInactiveComponent()
        {
            GameObject test = new GameObject();
            Assert.IsFalse(test == null);
            subject.DestroyAtEndOfFrame = false;
            subject.enabled = false;
            subject.DoDestroy(test);
            Assert.IsFalse(test == null);
        }
    }
}