using Zinnia.Data.Type;
using Zinnia.Extension;
using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class TransformRotationTest
    {
        private GameObject containingObject;
        private TransformRotation subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformRotationTest");
            subject = containingObject.AddComponent<TransformRotation>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformRotationTest");
            GameObject target = new GameObject("TransformRotationTest");

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.Modify(source, target);

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator ModifySmoothed()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            UnityEventListenerMock transitionedMock = new UnityEventListenerMock();

            subject.Transitioned.AddListener(transitionedMock.Listen);
            subject.TransitionDuration = 0.1f;
            subject.EqualityTolerance = 0.01f;

            GameObject source = new GameObject("source");
            GameObject target = new GameObject("target");

            Quaternion sourceRotation = new Quaternion(1f, 1f, 1f, 1f);
            Quaternion expectedRotation = Quaternion.identity;

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(expectedRotation).Using(comparer));
            Assert.IsFalse(transitionedMock.Received);

            do
            {
                subject.Modify(source, target);
                yield return null;
            }
            while (!target.transform.rotation.ApproxEquals(source.transform.rotation));

            expectedRotation = source.transform.rotation;

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(expectedRotation).Using(comparer));
            Assert.IsTrue(transitionedMock.Received);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformRotationTest");
            GameObject target = new GameObject("TransformRotationTest");
            GameObject offset = new GameObject("TransformRotationTest");

            offset.transform.SetParent(target.transform);

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);
            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            subject.Modify(source, target, offset);

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(new Quaternion(0.9f, 0f, -0.4f, 0f)).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);

            GameObject source = new GameObject("TransformRotationTest");
            GameObject target = new GameObject("TransformRotationTest");
            GameObject offset = new GameObject("TransformRotationTest");

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithAxisRestriction()
        {
            QuaternionEqualityComparer quaternionComparer = new QuaternionEqualityComparer(0.1f);
            Vector3EqualityComparer vectorComparer = new Vector3EqualityComparer(0.1f);

            subject.ApplyModificationOnAxis = new Vector3State(true, false, true);
            GameObject source = new GameObject("TransformRotationTest");
            GameObject target = new GameObject("TransformRotationTest");

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.Modify(source, target);

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(quaternionComparer));
            Assert.That(source.transform.eulerAngles, Is.EqualTo(new Vector3(0f, 180f, 180f)).Using(vectorComparer));
            Assert.That(target.transform.rotation, Is.EqualTo(new Quaternion(0f, 0f, 1f, 0f)).Using(quaternionComparer));
            Assert.That(target.transform.eulerAngles, Is.EqualTo(new Vector3(0f, 0f, 180f)).Using(vectorComparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformRotationTest");
            GameObject target = new GameObject("TransformRotationTest");

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject source = new GameObject("TransformRotationTest");
            GameObject target = new GameObject("TransformRotationTest");

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.That(source.transform.rotation, Is.EqualTo(sourceRotation).Using(comparer));
            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}