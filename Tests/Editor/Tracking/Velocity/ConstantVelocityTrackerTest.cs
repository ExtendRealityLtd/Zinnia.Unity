using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class ConstantVelocityTrackerTest
    {
        private GameObject containingObject;
        private ConstantVelocityTracker subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("ConstantVelocityTrackerTest");
            subject = containingObject.AddComponent<ConstantVelocityTracker>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.Velocity = expectedResult;

            Vector3 actualResult = subject.GetVelocity();
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));
        }

        [Test]
        public void GetAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.AngularVelocity = expectedResult;

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));
        }

        [Test]
        public void GetVelocityUseLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject parent = new GameObject("ConstantVelocityTrackerTest_Parent");
            containingObject.transform.SetParent(parent.transform);

            Vector3 givenVelocity = new Vector3(-1.4f, 1.3f, 1.2f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.UseLocal = true;
            subject.Velocity = givenVelocity;
            parent.transform.Rotate(0, 90, 0, Space.Self);

            Vector3 actualResult = subject.GetVelocity();
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void GetAngularVelocityUseLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject parent = new GameObject("ConstantVelocityTrackerTest_Parent");
            containingObject.transform.SetParent(parent.transform);

            Vector3 givenVelocity = new Vector3(-1.4f, 1.3f, 1.2f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.UseLocal = true;
            subject.AngularVelocity = givenVelocity;
            parent.transform.Rotate(0, 90, 0, Space.Self);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void GetVelocityInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

            subject.Velocity = new Vector3(1.2f, 1.3f, 1.4f);
            subject.gameObject.SetActive(false);

            Vector3 actualResult = subject.GetVelocity();
            Assert.That(actualResult, Is.EqualTo(Vector3.zero).Using(comparer));
        }

        [Test]
        public void GetVelocityInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

            subject.Velocity = new Vector3(1.2f, 1.3f, 1.4f);
            subject.enabled = false;

            Vector3 actualResult = subject.GetVelocity();
            Assert.That(actualResult, Is.EqualTo(Vector3.zero).Using(comparer));
        }
    }
}