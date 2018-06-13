namespace VRTK.Core.Tracking
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Cast;

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
            Transform source = new GameObject().transform;

            subject.source = source;

            Assert.AreEqual(Quaternion.identity, source.rotation);

            RaycastHit cast = new RaycastHit
            {
                normal = Vector3.forward * 90f
            };

            PointsCast.EventData data = new PointsCast.EventData
            {
                targetHit = cast
            };

            subject.HandleData(data);

            Assert.AreEqual(Quaternion.Euler(90f, 0f, 0f).ToString(), source.rotation.ToString());

            Object.DestroyImmediate(source.gameObject);
        }

        [Test]
        public void NoHandleDataOnDisabledComponent()
        {
            Transform source = new GameObject().transform;

            subject.source = source;

            Assert.AreEqual(Quaternion.identity, source.rotation);

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

            Assert.AreEqual(Quaternion.identity, source.rotation);

            Object.DestroyImmediate(source.gameObject);
        }
    }
}