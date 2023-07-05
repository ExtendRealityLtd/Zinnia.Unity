using Zinnia.Data.Operation.Mutation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformPositionMutatorTest
    {
        private GameObject containingObject;
        private TransformPositionMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformPositionMutatorTest");
            subject = containingObject.AddComponent<TransformPositionMutator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SetPropertyLocal()
        {
            UnityEventListenerMock preMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock postMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock mutationSkippedMock = new UnityEventListenerMock();
            subject.PreMutated.AddListener(preMutatedMock.Listen);
            subject.PostMutated.AddListener(postMutatedMock.Listen);
            subject.MutationSkipped.AddListener(mutationSkippedMock.Listen);

            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(input).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsTrue(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            preMutatedMock.Reset();
            postMutatedMock.Reset();
            mutationSkippedMock.Reset();

            subject.AllowMutate = false;

            Vector3 inputB = new Vector3(40f, 50f, 60f);
            subject.SetProperty(inputB);

            Assert.That(target.transform.localPosition, Is.EqualTo(input).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsTrue(mutationSkippedMock.Received);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalNoTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobal()
        {
            UnityEventListenerMock preMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock postMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock mutationSkippedMock = new UnityEventListenerMock();
            subject.PreMutated.AddListener(preMutatedMock.Listen);
            subject.PostMutated.AddListener(postMutatedMock.Listen);
            subject.MutationSkipped.AddListener(mutationSkippedMock.Listen);

            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(input).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsTrue(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            subject.AllowMutate = false;
            preMutatedMock.Reset();
            postMutatedMock.Reset();
            mutationSkippedMock.Reset();

            Vector3 inputB = new Vector3(40f, 50f, 60f);
            subject.SetProperty(inputB);

            Assert.That(target.transform.localPosition, Is.EqualTo(input).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsTrue(mutationSkippedMock.Received);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalNoTarget()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalLockYAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.SetProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocal()
        {
            UnityEventListenerMock preMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock postMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock mutationSkippedMock = new UnityEventListenerMock();
            subject.PreMutated.AddListener(preMutatedMock.Listen);
            subject.PostMutated.AddListener(postMutatedMock.Listen);
            subject.MutationSkipped.AddListener(mutationSkippedMock.Listen);

            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(input).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsTrue(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            preMutatedMock.Reset();
            postMutatedMock.Reset();
            mutationSkippedMock.Reset();
            subject.IncrementProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(input * 2f).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsTrue(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            subject.AllowMutate = false;
            preMutatedMock.Reset();
            postMutatedMock.Reset();
            mutationSkippedMock.Reset();

            Vector3 inputB = new Vector3(40f, 50f, 60f);
            subject.IncrementProperty(inputB);

            Assert.That(target.transform.localPosition, Is.EqualTo(input * 2f).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsTrue(mutationSkippedMock.Received);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobal()
        {
            UnityEventListenerMock preMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock postMutatedMock = new UnityEventListenerMock();
            UnityEventListenerMock mutationSkippedMock = new UnityEventListenerMock();
            subject.PreMutated.AddListener(preMutatedMock.Listen);
            subject.PostMutated.AddListener(postMutatedMock.Listen);
            subject.MutationSkipped.AddListener(mutationSkippedMock.Listen);

            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.IsFalse(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(input).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsTrue(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            subject.IncrementProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(input * 2f).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsTrue(postMutatedMock.Received);
            Assert.IsFalse(mutationSkippedMock.Received);

            subject.AllowMutate = false;
            preMutatedMock.Reset();
            postMutatedMock.Reset();
            mutationSkippedMock.Reset();

            Vector3 inputB = new Vector3(40f, 50f, 60f);
            subject.IncrementProperty(inputB);

            Assert.That(target.transform.localPosition, Is.EqualTo(input * 2f).Using(comparer));
            Assert.IsTrue(preMutatedMock.Received);
            Assert.IsFalse(postMutatedMock.Received);
            Assert.IsTrue(mutationSkippedMock.Received);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected * 2f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(expected * 2f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);
            subject.SetProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyGlobalWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);
            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyLocalLockYAxisWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);
            subject.FacingDirection = offset;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(18.5f, 0f, 25.6f);

            subject.SetProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxisWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);
            subject.FacingDirection = offset;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 17.1f, 0f);

            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyLocalWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            expected = new Vector3(20.3f, 33.5f, 63.7f);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyGlobalWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            expected = new Vector3(20.3f, 33.5f, 63.7f);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxisWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);
            subject.FacingDirection = offset;

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(18.5f, 0f, 25.6f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            expected = new Vector3(37.1f, 0f, 51.2f);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxisWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);
            subject.FacingDirection = offset;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 17.1f, 0f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            expected = new Vector3(0f, 34.1f, 0f);

            Assert.That(target.transform.position, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyLocalWithOffsetIgnoreY()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");
            GameObject offset = new GameObject("TransformPositionMutatorTest");
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;
            subject.ApplyFacingDirectionOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localPosition, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(-1.3f, 16.8f, 33.4f);
            subject.SetProperty(input);

            Assert.That(target.transform.localPosition, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.gameObject.SetActive(false);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformPositionMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.enabled = false;

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearMutateOnAxis()
        {
            Assert.AreEqual(Vector3State.True, subject.MutateOnAxis);
            subject.ClearMutateOnAxis();
            Assert.AreEqual(Vector3State.False, subject.MutateOnAxis);
        }

        [Test]
        public void ClearMutateOnAxisInactiveGameObject()
        {
            Assert.AreEqual(Vector3State.True, subject.MutateOnAxis);
            subject.gameObject.SetActive(false);
            subject.ClearMutateOnAxis();
            Assert.AreEqual(Vector3State.True, subject.MutateOnAxis);
        }

        [Test]
        public void ClearMutateOnAxisInactiveComponent()
        {
            Assert.AreEqual(Vector3State.True, subject.MutateOnAxis);
            subject.enabled = false;
            subject.ClearMutateOnAxis();
            Assert.AreEqual(Vector3State.True, subject.MutateOnAxis);
        }

        [Test]
        public void ClearFacingDirection()
        {
            Assert.IsNull(subject.FacingDirection);
            subject.FacingDirection = containingObject;
            Assert.AreEqual(containingObject, subject.FacingDirection);
            subject.ClearFacingDirection();
            Assert.IsNull(subject.FacingDirection);
        }

        [Test]
        public void ClearFacingDirectionInactiveGameObject()
        {
            Assert.IsNull(subject.FacingDirection);
            subject.FacingDirection = containingObject;
            Assert.AreEqual(containingObject, subject.FacingDirection);
            subject.gameObject.SetActive(false);
            subject.ClearFacingDirection();
            Assert.AreEqual(containingObject, subject.FacingDirection);
        }

        [Test]
        public void ClearFacingDirectionInactiveComponent()
        {
            Assert.IsNull(subject.FacingDirection);
            subject.FacingDirection = containingObject;
            Assert.AreEqual(containingObject, subject.FacingDirection);
            subject.enabled = false;
            subject.ClearFacingDirection();
            Assert.AreEqual(containingObject, subject.FacingDirection);
        }

        [Test]
        public void ClearApplyFacingDirectionOnAxis()
        {
            Assert.AreEqual(Vector3State.True, subject.ApplyFacingDirectionOnAxis);
            subject.ClearApplyFacingDirectionOnAxis();
            Assert.AreEqual(Vector3State.False, subject.ApplyFacingDirectionOnAxis);
        }

        [Test]
        public void ClearApplyFacingDirectionOnAxisInactiveGameObject()
        {
            Assert.AreEqual(Vector3State.True, subject.ApplyFacingDirectionOnAxis);
            subject.gameObject.SetActive(false);
            subject.ClearApplyFacingDirectionOnAxis();
            Assert.AreEqual(Vector3State.True, subject.ApplyFacingDirectionOnAxis);
        }

        [Test]
        public void ClearApplyFacingDirectionOnAxisInactiveComponent()
        {
            Assert.AreEqual(Vector3State.True, subject.ApplyFacingDirectionOnAxis);
            subject.enabled = false;
            subject.ClearApplyFacingDirectionOnAxis();
            Assert.AreEqual(Vector3State.True, subject.ApplyFacingDirectionOnAxis);
        }

        [Test]
        public void SetMutateOnAxisX()
        {
            subject.MutateOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.MutateOnAxis);
            subject.SetMutateOnAxisX(true);
            Assert.AreEqual(Vector3State.XOnly, subject.MutateOnAxis);
        }

        [Test]
        public void SetMutateOnAxisY()
        {
            subject.MutateOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.MutateOnAxis);
            subject.SetMutateOnAxisY(true);
            Assert.AreEqual(Vector3State.YOnly, subject.MutateOnAxis);
        }

        [Test]
        public void SetMutateOnAxisZ()
        {
            subject.MutateOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.MutateOnAxis);
            subject.SetMutateOnAxisZ(true);
            Assert.AreEqual(Vector3State.ZOnly, subject.MutateOnAxis);
        }

        [Test]
        public void SetApplyFacingDirectionOnAxisX()
        {
            subject.ApplyFacingDirectionOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyFacingDirectionOnAxis);
            subject.SetApplyFacingDirectionOnAxisX(true);
            Assert.AreEqual(Vector3State.XOnly, subject.ApplyFacingDirectionOnAxis);
        }

        [Test]
        public void SetApplyFacingDirectionOnAxisY()
        {
            subject.ApplyFacingDirectionOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyFacingDirectionOnAxis);
            subject.SetApplyFacingDirectionOnAxisY(true);
            Assert.AreEqual(Vector3State.YOnly, subject.ApplyFacingDirectionOnAxis);
        }

        [Test]
        public void SetApplyFacingDirectionOnAxisZ()
        {
            subject.ApplyFacingDirectionOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyFacingDirectionOnAxis);
            subject.SetApplyFacingDirectionOnAxisZ(true);
            Assert.AreEqual(Vector3State.ZOnly, subject.ApplyFacingDirectionOnAxis);
        }
    }
}