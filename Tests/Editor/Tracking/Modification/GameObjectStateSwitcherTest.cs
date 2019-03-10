﻿using Zinnia.Tracking.Modification;
using Zinnia.Data.Collection;

namespace Test.Zinnia.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;

    public class GameObjectStateSwitcherTest
    {
        private GameObject containingObject;
        private GameObjectStateSwitcherMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectStateSwitcherMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SwitchNext()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 0;

            subject.ManualOnEnable();
            subject.SwitchToStartIndex();

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

        [Test]
        public void SwitchPrevious()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 0;

            subject.ManualOnEnable();
            subject.SwitchToStartIndex();

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

        [Test]
        public void SwitchTo()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 0;

            subject.ManualOnEnable();
            subject.SwitchToStartIndex();

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

        [Test]
        public void SwitchFalseState()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = false;
            subject.StartIndex = 0;

            subject.ManualOnEnable();
            subject.SwitchToStartIndex();

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

        [Test]
        public void SwitchNextStartAt1()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 1;

            subject.ManualOnEnable();
            subject.SwitchToStartIndex();

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

        [Test]
        public void SwitchNextStartAt2()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 2;

            subject.ManualOnEnable();
            subject.SwitchToStartIndex();

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

        [Test]
        public void SwitchNotOnEnableStartAt0()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 0;

            subject.ManualOnEnable();

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

        [Test]
        public void SwitchNotOnEnableStartAt1()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 1;

            subject.ManualOnEnable();

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

        [Test]
        public void SwitchNotOnEnableStartAt2()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 2;

            subject.ManualOnEnable();

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

        [Test]
        public void SwitchInactiveGameObject()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 0;

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

        [Test]
        public void SwitchInactiveComponent()
        {
            GameObject objectA = new GameObject();
            GameObject objectB = new GameObject();
            GameObject objectC = new GameObject();

            GameObjectObservableList targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = targets;
            targets.Add(objectA);
            targets.Add(objectB);
            targets.Add(objectC);

            subject.TargetState = true;
            subject.StartIndex = 0;

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

    public class GameObjectStateSwitcherMock : GameObjectStateSwitcher
    {
        public virtual void ManualOnEnable()
        {
            OnEnable();
        }
    }
}