﻿using Zinnia.Data.Operation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformEulerRotationMutatorTest
    {
        private GameObject containingObject;
        private TransformEulerRotationMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformEulerRotationMutator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SetPropertyLocal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.localEulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.localEulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.eulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalLockYAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.AreEqual(Vector3.zero, target.transform.localEulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localEulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.eulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.localEulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.localEulerAngles.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((input * 2f).ToString(), target.transform.localEulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.eulerAngles.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((input * 2f).ToString(), target.transform.eulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.AreEqual(Vector3.zero, target.transform.localEulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localEulerAngles.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((expected * 2f).ToString(), target.transform.localEulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.eulerAngles.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((expected * 2f).ToString(), target.transform.eulerAngles.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyInactiveGameObject()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyInactiveComponent()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.enabled = false;

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Object.DestroyImmediate(target);
        }
    }
}