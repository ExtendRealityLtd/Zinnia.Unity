using Zinnia.Data.Operation.Mutation;
using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Operation.Mutation
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformPositionMutatorTest
    {
        private GameObject containingObject;
        private TransformPositionMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformPositionMutator>();
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

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalLockYAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.localPosition.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((input * 2f).ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobal()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);

            Assert.AreEqual(input.ToString(), target.transform.position.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((input * 2f).ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10f, 0f, 30f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((expected * 2f).ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxis()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 20f, 0f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            subject.IncrementProperty(input);

            Assert.AreEqual((expected * 2f).ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetPropertyLocalWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);
            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyGlobalWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);
            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyLocalLockYAxisWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(18.5f, 0f, 25.6f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyGlobalLockXZAxisWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 17.1f, 0f);

            subject.SetProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyLocalWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            subject.IncrementProperty(input);

            expected = new Vector3(20.3f, 33.5f, 63.7f);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyGlobalWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = Vector3State.True;
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.IncrementProperty(input);
            Vector3 expected = new Vector3(10.2f, 16.8f, 31.9f);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            subject.IncrementProperty(input);

            expected = new Vector3(20.3f, 33.5f, 63.7f);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyLocalLockYAxisWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = new Vector3State(true, false, true);
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.localPosition);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(18.5f, 0f, 25.6f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            subject.IncrementProperty(input);

            expected = new Vector3(37.1f, 0f, 51.2f);

            Assert.AreEqual(expected.ToString(), target.transform.localPosition.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void IncrementPropertyGlobalLockXZAxisWithOffset()
        {
            GameObject target = new GameObject();
            GameObject offset = new GameObject();
            offset.transform.eulerAngles = new Vector3(10f, 20f, 30f);

            subject.Target = target;
            subject.UseLocalValues = false;
            subject.MutateOnAxis = new Vector3State(false, true, false);
            subject.FacingDirection = offset;

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            Vector3 expected = new Vector3(0f, 17.1f, 0f);

            subject.IncrementProperty(input);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            subject.IncrementProperty(input);

            expected = new Vector3(0f, 34.1f, 0f);

            Assert.AreEqual(expected.ToString(), target.transform.position.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetPropertyInactiveGameObject()
        {
            GameObject target = new GameObject();

            subject.Target = target;
            subject.UseLocalValues = true;
            subject.MutateOnAxis = Vector3State.True;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(Vector3.zero, target.transform.position);

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

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Vector3 input = new Vector3(10f, 20f, 30f);
            subject.SetProperty(input);

            Assert.AreEqual(Vector3.zero, target.transform.position);

            Object.DestroyImmediate(target);
        }
    }
}