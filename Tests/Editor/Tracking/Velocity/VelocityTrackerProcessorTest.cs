namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;

    public class VelocityTrackerProcessorTest
    {
        private GameObject containingObject;
        private VelocityTrackerProcessor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<VelocityTrackerProcessor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetVelocityFromFirstActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(true, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(1f, 1f, 1f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityFromSecondActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityFromThirdActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityFromNoneActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityWithoutTrackers()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetActiveVelocityTrackerAfterGetVelocity()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);
            subject.GetVelocity();

            VelocityTrackerMock expectedResult = trackerThree;
            VelocityTrackerMock unexpectedResult = trackerOne;
            VelocityTrackerMock actualResult = (VelocityTrackerMock)subject.GetActiveVelocityTracker();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetActiveVelocityTrackerAfterGetAngularVelocity()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);
            subject.GetAngularVelocity();

            VelocityTrackerMock expectedResult = trackerTwo;
            VelocityTrackerMock unexpectedResult = trackerOne;
            VelocityTrackerMock actualResult = (VelocityTrackerMock)subject.GetActiveVelocityTracker();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityFromFirstActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(true, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(1f, 1f, 1f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityFromSecondActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityFromThirdActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityFromNoneActive()
        {
            VelocityTrackerMock trackerOne = MockVelocityTracker(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = MockVelocityTracker(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = MockVelocityTracker(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            subject.velocityTrackers.Add(trackerOne);
            subject.velocityTrackers.Add(trackerTwo);
            subject.velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityWithoutTrackers()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        private VelocityTrackerMock MockVelocityTracker(bool mockActive, Vector3 mockVelocity, Vector3 mockAngularVelocity)
        {
            GameObject mockObject = new GameObject();
            VelocityTrackerMock mock = mockObject.AddComponent<VelocityTrackerMock>();
            mock.Construct(mockActive, mockVelocity, mockAngularVelocity);
            return mock;
        }
    }

    public class VelocityTrackerMock : VelocityTracker
    {
        private bool mockActive;
        private Vector3 mockVelocity;
        private Vector3 mockAngularVelocity;

        public virtual void Construct(bool active, Vector3 velocity, Vector3 angularVelocity)
        {
            mockActive = active;
            mockVelocity = velocity;
            mockAngularVelocity = angularVelocity;
        }

        public override bool IsActive()
        {
            return mockActive;
        }

        public override Vector3 GetVelocity()
        {
            return mockVelocity;
        }

        public override Vector3 GetAngularVelocity()
        {
            return mockAngularVelocity;
        }
    }
}