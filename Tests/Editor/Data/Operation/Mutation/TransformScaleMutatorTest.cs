using Zinnia.Data.Operation.Mutation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformScaleMutatorTest
    {
        private GameObject containingObject;
        private TransformScaleMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformScaleMutatorTest");
            subject = containingObject.AddComponent<TransformScaleMutator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SetPropertyLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.That(target.transform.localScale, Is.EqualTo(input).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(input).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalLockYAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expected = new Vector3(2f, 1f, 2f);

            subject.SetProperty(input);

            Assert.That(target.transform.localScale, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expected = new Vector3(1f, 2f, 1f);

            subject.SetProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.IncrementProperty(input);

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one * 3f).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one * 5f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.IncrementProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one * 3f).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one * 5f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expectedA = new Vector3(3f, 1f, 3f);
            Vector3 expectedB = new Vector3(5f, 1f, 5f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.localScale, Is.EqualTo(expectedA).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.localScale, Is.EqualTo(expectedB).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            Vector3 expectedA = new Vector3(1f, 3f, 1f);
            Vector3 expectedB = new Vector3(1f, 5f, 1f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(expectedA).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(expectedB).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.gameObject.SetActive(false);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformScaleMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.enabled = false;

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Vector3 input = new Vector3(2f, 2f, 2f);
            subject.SetProperty(input);

            Assert.That(target.transform.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(target);
        }
    }
}