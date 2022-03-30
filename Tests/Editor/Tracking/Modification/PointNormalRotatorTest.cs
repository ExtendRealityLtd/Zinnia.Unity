using Zinnia.Cast;
using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class PointNormalRotatorTest
    {
        private GameObject containingObject;
        private PointNormalRotator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PointNormalRotator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void HandleData()
        {
            GameObject target = new GameObject();

            subject.Target = target;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                HitData = cast
            };

            subject.HandleData(data);

            Assert.AreEqual(Quaternion.Euler(90f, 0f, 0f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void NoHandleDataOnDisabledComponent()
        {
            GameObject target = new GameObject();

            subject.Target = target;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

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

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

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