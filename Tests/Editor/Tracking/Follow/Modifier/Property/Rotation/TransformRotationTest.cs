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
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformRotation>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(sourceRotation, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator ModifySmoothed()
        {
            UnityEventListenerMock transitionedMock = new UnityEventListenerMock();

            subject.Transitioned.AddListener(transitionedMock.Listen);
            subject.TransitionDuration = 0.1f;
            subject.EqualityTolerance = 0.01f;

            GameObject source = new GameObject("source");
            GameObject target = new GameObject("target");
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(10e-6f);

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
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            offset.transform.SetParent(target.transform);

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);
            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            subject.Modify(source, target, offset);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(new Quaternion(0.9f, 0f, -0.4f, 0f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            GameObject offset = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;
            offset.transform.rotation = Quaternion.Euler(0f, 45f, 0f);

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(sourceRotation, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithAxisRestriction()
        {
            subject.ApplyModificationOnAxis = new Vector3State(true, false, true);
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(new Vector3(0f, 180f, 180f), source.transform.eulerAngles);
            Assert.AreEqual(new Quaternion(0f, 0f, 1f, 0f).ToString(), target.transform.rotation.ToString());
            Assert.AreEqual(new Vector3(0f, 0f, 180f), target.transform.eulerAngles);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            Quaternion sourceRotation = new Quaternion(1f, 0f, 0f, 0f);

            source.transform.rotation = sourceRotation;
            target.transform.rotation = Quaternion.identity;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.AreEqual(sourceRotation, source.transform.rotation);
            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}