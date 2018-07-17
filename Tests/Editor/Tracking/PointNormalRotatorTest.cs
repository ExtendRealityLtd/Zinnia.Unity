using VRTK.Core.Tracking;
using VRTK.Core.Cast;

namespace Test.VRTK.Core.Tracking
{
    using UnityEngine;
    using NUnit.Framework;

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
            Transform target = new GameObject().transform;

            subject.target = target;

            Assert.AreEqual(Quaternion.identity, target.rotation);

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                targetHit = cast
            };

            subject.HandleData(data);

            Assert.AreEqual(Quaternion.Euler(90f, 0f, 0f).ToString(), target.rotation.ToString());

            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void NoHandleDataOnDisabledComponent()
        {
            Transform target = new GameObject().transform;

            subject.target = target;

            Assert.AreEqual(Quaternion.identity, target.rotation);

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                targetHit = cast
            };

            subject.enabled = false;
            subject.HandleData(data);

            Assert.AreEqual(Quaternion.identity, target.rotation);

            Object.DestroyImmediate(target.gameObject);
        }
    }
}