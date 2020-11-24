using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class ConstantVelocityTrackerTest
    {
        private GameObject containingObject;
        private ConstantVelocityTracker subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
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
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.Velocity = expectedResult;

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocity()
        {
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.AngularVelocity = expectedResult;

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityUseLocal()
        {
            Vector3 v = new Vector3(-1.4f, 1.3f, 1.2f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.UseLocal = true;
            subject.Velocity = v;
            subject.transform.Rotate(0, 90, 0, Space.Self);

            Vector3 actualResult = subject.GetVelocity();
            Assert.IsTrue(expectedResult == actualResult);
            Assert.IsFalse(unexpectedResult == actualResult);
        }

        [Test]
        public void GetAngularVelocityUseLocal()
        {
            Vector3 v = new Vector3(-1.4f, 1.3f, 1.2f);
            Vector3 expectedResult = new Vector3(1.2f, 1.3f, 1.4f);
            Vector3 unexpectedResult = Vector3.zero;

            subject.UseLocal = true;
            subject.AngularVelocity = v;
            subject.transform.Rotate(0, 90, 0, Space.Self);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.IsTrue(expectedResult == actualResult);
            Assert.IsFalse(unexpectedResult == actualResult);
        }
    }
}