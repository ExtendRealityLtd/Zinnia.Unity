using Zinnia.Data.Operation.Mutation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformScaleMutatorTest
    {
        private GameObject containingObject;
        private TransformScaleMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformScaleMutator>();
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

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.localScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.lossyScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalLockYAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expected = new Vector3(2f, 1f, 2f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expected = new Vector3(1f, 2f, 1f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.lossyScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.IncrementProperty(input);

            Assert.AreEqual((Vector3.one * 3f).ToString(), target.transform.localScale.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((Vector3.one * 5f).ToString(), target.transform.localScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.IncrementProperty(input);

            Assert.AreEqual((Vector3.one * 3f).ToString(), target.transform.lossyScale.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((Vector3.one * 5f).ToString(), target.transform.lossyScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expectedA = new Vector3(3f, 1f, 3f);
            Vector3 expectedB = new Vector3(5f, 1f, 5f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expectedA.ToString(), target.transform.localScale.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual(expectedB.ToString(), target.transform.localScale.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expectedA = new Vector3(1f, 3f, 1f);
            Vector3 expectedB = new Vector3(1f, 5f, 1f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expectedA.ToString(), target.transform.lossyScale.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual(expectedB.ToString(), target.transform.lossyScale.ToString());

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

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

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

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.AreEqual(Vector3.one, target.transform.lossyScale);

            Object.DestroyImmediate(target);
        }
    }
}