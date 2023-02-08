using Zinnia.Cast;
using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class PointNormalRotatorTest
    {
        private GameObject containingObject;
        private PointNormalRotator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("PointNormalRotatorTest");
            subject = containingObject.AddComponent<PointNormalRotator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void HandleData()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("PointNormalRotatorTest");

            subject.Target = target;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                HitData = cast
            };

            subject.HandleData(data);

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.Euler(90f, 0f, 0f)).Using(comparer));

            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void NoHandleDataOnDisabledComponent()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("PointNormalRotatorTest");

            subject.Target = target;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                HitData = cast
            };

            subject.enabled = false;
            subject.HandleData(data);

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(target.gameObject);
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
    }
}