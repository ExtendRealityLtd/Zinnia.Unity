using Zinnia.Data.Operation.Mutation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

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
        public void SetPropertyWithOrigin()
        {
            GameObject target = new GameObject();
            GameObject origin = new GameObject();
            origin.transform.SetParent(target.transform);

            subject.Target = target;
            subject.Origin = origin;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            origin.transform.position = new Vector3(1f, 0f, 1f);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 inputRotation = new Vector3(10f, 20f, 30f);
            Vector3 expectedRotation = new Vector3(0f, 20f, 0f);
            Vector3 expectedPosition = new Vector3(-0.3f, 0f, 0.4f);

            subject.SetProperty(inputRotation);

            Assert.AreEqual(expectedRotation.ToString(), target.transform.eulerAngles.ToString());
            Assert.AreEqual(expectedPosition.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyWithOriginIgnoreZ()
        {
            GameObject target = new GameObject();
            GameObject origin = new GameObject();
            origin.transform.SetParent(target.transform);

            subject.Target = target;
            subject.Origin = origin;
            subject.MutateOnAxis = new Vector3State(false, true, false);
            subject.ApplyOriginOnAxis = new Vector3State(true, true, false);

            origin.transform.position = new Vector3(1f, 0f, 1f);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 inputRotation = new Vector3(10f, 20f, 30f);
            Vector3 expectedRotation = new Vector3(0f, 20f, 0f);
            Vector3 expectedPosition = new Vector3(0.1f, 0f, 0.3f);

            subject.SetProperty(inputRotation);

            Assert.AreEqual(expectedRotation.ToString(), target.transform.eulerAngles.ToString());
            Assert.AreEqual(expectedPosition.ToString(), target.transform.position.ToString());

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
        public void IncrementPropertyWithOrigin()
        {
            GameObject target = new GameObject();
            GameObject origin = new GameObject();
            origin.transform.SetParent(target.transform);

            subject.Target = target;
            subject.Origin = origin;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            origin.transform.position = new Vector3(1f, 0f, 1f);

            Assert.AreEqual(Vector3.zero, target.transform.eulerAngles);

            Vector3 inputRotation = new Vector3(10f, 20f, 30f);
            Vector3 expectedRotation = new Vector3(0f, 20f, 0f);
            Vector3 expectedPosition = new Vector3(-0.3f, 0f, 0.4f);

            subject.IncrementProperty(inputRotation);

            Assert.AreEqual(expectedRotation.ToString(), target.transform.eulerAngles.ToString());
            Assert.AreEqual(expectedPosition.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void OriginNotChildException()
        {
            GameObject target = new GameObject();
            GameObject origin = new GameObject();
            subject.Target = target;

            NUnit.Framework.Assert.Throws<System.ArgumentException>(() => subject.Origin = origin);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(origin);
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