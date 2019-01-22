using Zinnia.Tracking.Modification;
using Zinnia.Cast;

namespace Test.Zinnia.Tracking.Modification
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
            GameObject target = new GameObject();

            subject.target = target;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                targetHit = cast
            };

            subject.HandleData(data);

            Assert.AreEqual(Quaternion.Euler(90f, 0f, 0f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void NoHandleDataOnDisabledComponent()
        {
            GameObject target = new GameObject();

            subject.target = target;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

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

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target.gameObject);
        }
    }
}