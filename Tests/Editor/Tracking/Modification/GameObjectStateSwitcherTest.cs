using Zinnia.Tracking.Modification;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Tracking.Modification
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectStateSwitcherTest
    {
        private GameObject containingObject;
        private GameObjectStateSwitcher subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectStateSwitcher>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator SwitchNext()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 0;

            subject.SwitchToCurrentIndex();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchPrevious()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 0;

            subject.SwitchToCurrentIndex();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchPrevious();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchPrevious();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchPrevious();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchTo()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 0;

            subject.SwitchToCurrentIndex();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchTo(2);

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchTo(0);

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchTo(1);

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchTo(5);

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchTo(-2);

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchFalseState()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = false;
            subject.Targets.CurrentIndex = 0;

            subject.SwitchToCurrentIndex();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchNextStartAt1()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 1;

            subject.SwitchToCurrentIndex();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchNextStartAt2()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 2;

            subject.SwitchToCurrentIndex();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchNotOnEnableStartAt0()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 0;

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchNotOnEnableStartAt1()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 1;

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsFalse(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchNotOnEnableStartAt2()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 2;

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsFalse(objectB.activeInHierarchy);
            Assert.IsFalse(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchInactiveGameObject()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 0;

            subject.gameObject.SetActive(false);

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }

        [UnityTest]
        public IEnumerator SwitchInactiveComponent()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;
            subject.Targets = targets;

            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.Targets.CurrentIndex = 0;

            subject.enabled = false;

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            subject.SwitchNext();

            Assert.IsTrue(objectA.activeInHierarchy);
            Assert.IsTrue(objectB.activeInHierarchy);
            Assert.IsTrue(objectC.activeInHierarchy);

            Object.DestroyImmediate(objectA);
            Object.DestroyImmediate(objectB);
            Object.DestroyImmediate(objectC);
        }
    }
}