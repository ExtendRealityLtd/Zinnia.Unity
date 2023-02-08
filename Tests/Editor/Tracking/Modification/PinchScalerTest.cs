using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class PinchScalerTest
    {
        private GameObject containingObject;
        private PinchScaler subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("PinchScalerTest");
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            GameObject primaryPoint = new GameObject("PinchScalerTest");
            GameObject secondaryPoint = new GameObject("PinchScalerTest");

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one * 3f).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessWithMultiplier()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            GameObject primaryPoint = new GameObject("PinchScalerTest");
            GameObject secondaryPoint = new GameObject("PinchScalerTest");

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;
            subject.Multiplier = 2f;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one * 5f).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessNoPrimaryPoint()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            GameObject secondaryPoint = new GameObject("PinchScalerTest");

            subject.Target = target;
            subject.SecondaryPoint = secondaryPoint;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Process();
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessNoSecondaryPoint()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            GameObject primaryPoint = new GameObject("PinchScalerTest");

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            subject.Process();

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            GameObject primaryPoint = new GameObject("PinchScalerTest");
            GameObject secondaryPoint = new GameObject("PinchScalerTest");

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;
            subject.gameObject.SetActive(false);

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            GameObject primaryPoint = new GameObject("PinchScalerTest");
            GameObject secondaryPoint = new GameObject("PinchScalerTest");

            subject.Target = target;
            subject.PrimaryPoint = primaryPoint;
            subject.SecondaryPoint = secondaryPoint;
            subject.enabled = false;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            subject.Process();
            primaryPoint.transform.position = Vector3.forward * 1f;
            secondaryPoint.transform.position = Vector3.forward * -1f;
            subject.Process();

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(primaryPoint);
            Object.DestroyImmediate(secondaryPoint);
        }

        [Test]
        public void SaveAndRestoreScale()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject target = new GameObject("PinchScalerTest");
            subject.Target = target;

            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            subject.SaveCurrentScale();
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            target.transform.localScale = Vector3.one * 2f;
            subject.RestoreSavedScale();
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

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
