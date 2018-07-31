using VRTK.Core.Tracking.Modification;

namespace Test.VRTK.Core.Tracking.Modification
{
    using UnityEngine;
    using NUnit.Framework;

    public class PinchScalerTest
    {
        private GameObject containingObject;
        private PinchScaler subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PinchScaler>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            GameObject target = new GameObject();
            GameObject primaryPoint = new GameObject();
            GameObject secondaryPoint = new GameObject();

            subject.target = target;
            subject.primaryPoint = primaryPoint;
            subject.secondaryPoint = secondaryPoint;

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.AreEqual(Vector3.one * 3f, target.transform.localScale);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessWithMultiplier()
        {
            GameObject target = new GameObject();
            GameObject primaryPoint = new GameObject();
            GameObject secondaryPoint = new GameObject();

            subject.target = target;
            subject.primaryPoint = primaryPoint;
            subject.secondaryPoint = secondaryPoint;
            subject.multiplier = 2f;

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.AreEqual(Vector3.one * 5f, target.transform.localScale);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessNoPrimaryPoint()
        {
            GameObject target = new GameObject();
            GameObject secondaryPoint = new GameObject();

            subject.target = target;
            subject.secondaryPoint = secondaryPoint;

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            subject.Process();
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessNoSecondaryPoint()
        {
            GameObject target = new GameObject();
            GameObject primaryPoint = new GameObject();

            subject.target = target;
            subject.primaryPoint = primaryPoint;

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            subject.Process();

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            GameObject target = new GameObject();
            GameObject primaryPoint = new GameObject();
            GameObject secondaryPoint = new GameObject();

            subject.target = target;
            subject.primaryPoint = primaryPoint;
            subject.secondaryPoint = secondaryPoint;
            subject.gameObject.SetActive(false);

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            GameObject target = new GameObject();
            GameObject primaryPoint = new GameObject();
            GameObject secondaryPoint = new GameObject();

            subject.target = target;
            subject.primaryPoint = primaryPoint;
            subject.secondaryPoint = secondaryPoint;
            subject.enabled = false;

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void SaveAndRestoreScale()
        {
            GameObject target = new GameObject();
            subject.target = target;

            Assert.AreEqual(Vector3.one, target.transform.localScale);
            subject.SaveCurrentScale();
            Assert.AreEqual(Vector3.one, target.transform.localScale);
            target.transform.localScale = Vector3.one * 2f;
            subject.RestoreSavedScale();
            Assert.AreEqual(Vector3.one, target.transform.localScale);
        }
    }
}
