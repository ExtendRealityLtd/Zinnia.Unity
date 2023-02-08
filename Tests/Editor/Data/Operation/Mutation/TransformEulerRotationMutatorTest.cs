using Zinnia.Data.Operation.Mutation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformEulerRotationMutatorTest
    {
        private GameObject containingObject;
        private TransformEulerRotationMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformEulerRotationMutatorTest");
            subject = containingObject.AddComponent<TransformEulerRotationMutator>();
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
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(input).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(input).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalLockYAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.SetProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.SetProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(expected).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyWithOrigin()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");
            GameObject origin = new GameObject("TransformEulerRotationMutatorTest");
            origin.transform.SetParent(target.transform);

            subject.Target = target;
            subject.Origin = origin;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            origin.transform.position = new Vector3(1f, 0f, 1f);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 inputRotation = new Vector3(10f, 20f, 30f);
            Vector3 expectedRotation = new Vector3(0f, 20f, 0f);
            Vector3 expectedPosition = new Vector3(-0.3f, 0f, 0.4f);

            subject.SetProperty(inputRotation);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(expectedRotation).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(expectedPosition).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyWithOriginIgnoreZ()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");
            GameObject origin = new GameObject("TransformEulerRotationMutatorTest");
            origin.transform.SetParent(target.transform);

            subject.Target = target;
            subject.Origin = origin;
            subject.MutateOnAxis = new Vector3State(false, true, false);
            subject.ApplyOriginOnAxis = new Vector3State(true, true, false);

            origin.transform.position = new Vector3(1f, 0f, 1f);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 inputRotation = new Vector3(10f, 20f, 30f);
            Vector3 expectedRotation = new Vector3(0f, 20f, 0f);
            Vector3 expectedPosition = new Vector3(0.06f, 0f, 0.34f);

            subject.SetProperty(inputRotation);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(expectedRotation).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(expectedPosition).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(input).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(input * 2f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(input).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(input * 2f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.localEulerAngles, Is.EqualTo(expected * 2f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxis()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.IncrementProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(expected).Using(comparer));

            subject.IncrementProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(expected * 2f).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyWithOrigin()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");
            GameObject origin = new GameObject("TransformEulerRotationMutatorTest");
            origin.transform.SetParent(target.transform);

            subject.Target = target;
            subject.Origin = origin;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            origin.transform.position = new Vector3(1f, 0f, 1f);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 inputRotation = new Vector3(10f, 20f, 30f);
            Vector3 expectedRotation = new Vector3(0f, 20f, 0f);
            Vector3 expectedPosition = new Vector3(-0.3f, 0f, 0.4f);

            subject.IncrementProperty(inputRotation);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(expectedRotation).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(expectedPosition).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void OriginNotChildException()
        {
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");
            GameObject origin = new GameObject("TransformEulerRotationMutatorTest");
            subject.Target = target;

            Assert.Throws<System.ArgumentException>(() => subject.Origin = origin);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(origin);
        }

        [Test]
        public void SetPropertyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.gameObject.SetActive(false);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("TransformEulerRotationMutatorTest");

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.enabled = false;

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.That(target.transform.eulerAngles, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ClearOrigin()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = containingObject;
            Assert.AreEqual(containingObject, subject.Origin);
            subject.ClearOrigin();
            Assert.IsNull(subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveGameObject()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = containingObject;
            Assert.AreEqual(containingObject, subject.Origin);
            subject.gameObject.SetActive(false);
            subject.ClearOrigin();
            Assert.AreEqual(containingObject, subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveComponent()
        {
            Assert.IsNull(subject.Origin);
            subject.Origin = containingObject;
            Assert.AreEqual(containingObject, subject.Origin);
            subject.enabled = false;
            subject.ClearOrigin();
            Assert.AreEqual(containingObject, subject.Origin);
        }

        [Test]
        public void ClearApplyOriginOnAxis()
        {
            Assert.AreEqual(Vector3State.True, subject.ApplyOriginOnAxis);
            subject.ClearApplyOriginOnAxis();
            Assert.AreEqual(Vector3State.False, subject.ApplyOriginOnAxis);
        }

        [Test]
        public void ClearApplyOriginOnAxisInactiveGameObject()
        {
            Assert.AreEqual(Vector3State.True, subject.ApplyOriginOnAxis);
            subject.gameObject.SetActive(false);
            subject.ClearApplyOriginOnAxis();
            Assert.AreEqual(Vector3State.True, subject.ApplyOriginOnAxis);
        }

        [Test]
        public void ClearApplyOriginOnAxisInactiveComponent()
        {
            Assert.AreEqual(Vector3State.True, subject.ApplyOriginOnAxis);
            subject.enabled = false;
            subject.ClearApplyOriginOnAxis();
            Assert.AreEqual(Vector3State.True, subject.ApplyOriginOnAxis);
        }

        [Test]
        public void SetApplyOriginOnAxisX()
        {
            subject.ApplyOriginOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyOriginOnAxis);
            subject.SetApplyOriginOnAxisX(true);
            Assert.AreEqual(Vector3State.XOnly, subject.ApplyOriginOnAxis);
        }

        [Test]
        public void SetApplyOriginOnAxisY()
        {
            subject.ApplyOriginOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyOriginOnAxis);
            subject.SetApplyOriginOnAxisY(true);
            Assert.AreEqual(Vector3State.YOnly, subject.ApplyOriginOnAxis);
        }

        [Test]
        public void SetApplyOriginOnAxisZ()
        {
            subject.ApplyOriginOnAxis = Vector3State.False;
            Assert.AreEqual(Vector3State.False, subject.ApplyOriginOnAxis);
            subject.SetApplyOriginOnAxisZ(true);
            Assert.AreEqual(Vector3State.ZOnly, subject.ApplyOriginOnAxis);
        }
    }
}