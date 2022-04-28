using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

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
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            GameObject target = new GameObject();
            GameObject primaryPoint = new GameObject();
            GameObject secondaryPoint = new GameObject();

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;

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

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;
            subject.Multiplier = 2f;

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

            subject.Target = target;
            subject.SecondaryPoint = secondaryPoint;

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

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;

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

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;
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

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;
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
            subject.Target = target;

            Assert.AreEqual(Vector3.one, target.transform.localScale);
            subject.SaveCurrentScale();
            Assert.AreEqual(Vector3.one, target.transform.localScale);
            target.transform.localScale = Vector3.one * 2f;
            subject.RestoreSavedScale();
            Assert.AreEqual(Vector3.one, target.transform.localScale);

            Object.DestroyImmediate(target);
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

        [Test]
        public void ClearPrimaryPoint()
        {
            Assert.IsNull(subject.PrimaryPoint);
            subject.PrimaryPoint = containingObject;
            Assert.AreEqual(containingObject, subject.PrimaryPoint);
            subject.ClearPrimaryPoint();
            Assert.IsNull(subject.PrimaryPoint);
        }

        [Test]
        public void ClearPrimaryPointInactiveGameObject()
        {
            Assert.IsNull(subject.PrimaryPoint);
            subject.PrimaryPoint = containingObject;
            Assert.AreEqual(containingObject, subject.PrimaryPoint);
            subject.gameObject.SetActive(false);
            subject.ClearPrimaryPoint();
            Assert.AreEqual(containingObject, subject.PrimaryPoint);
        }

        [Test]
        public void ClearPrimaryPointInactiveComponent()
        {
            Assert.IsNull(subject.PrimaryPoint);
            subject.PrimaryPoint = containingObject;
            Assert.AreEqual(containingObject, subject.PrimaryPoint);
            subject.enabled = false;
            subject.ClearPrimaryPoint();
            Assert.AreEqual(containingObject, subject.PrimaryPoint);
        }

        [Test]
        public void ClearSecondaryPoint()
        {
            Assert.IsNull(subject.SecondaryPoint);
            subject.SecondaryPoint = containingObject;
            Assert.AreEqual(containingObject, subject.SecondaryPoint);
            subject.ClearSecondaryPoint();
            Assert.IsNull(subject.SecondaryPoint);
        }

        [Test]
        public void ClearSecondaryPointInactiveGameObject()
        {
            Assert.IsNull(subject.SecondaryPoint);
            subject.SecondaryPoint = containingObject;
            Assert.AreEqual(containingObject, subject.SecondaryPoint);
            subject.gameObject.SetActive(false);
            subject.ClearSecondaryPoint();
            Assert.AreEqual(containingObject, subject.SecondaryPoint);
        }

        [Test]
        public void ClearSecondaryPointInactiveComponent()
        {
            Assert.IsNull(subject.SecondaryPoint);
            subject.SecondaryPoint = containingObject;
            Assert.AreEqual(containingObject, subject.SecondaryPoint);
            subject.enabled = false;
            subject.ClearSecondaryPoint();
            Assert.AreEqual(containingObject, subject.SecondaryPoint);
        }
    }
}
