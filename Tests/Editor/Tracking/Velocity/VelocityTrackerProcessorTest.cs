using Zinnia.Tracking.Velocity;
using Zinnia.Tracking.Velocity.Collection;

namespace Test.Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;

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
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(true, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(1f, 1f, 1f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetVelocityFromSecondActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetVelocityFromThirdActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetVelocityFromNoneActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
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
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            subject.GetVelocity();

            VelocityTrackerMock expectedResult = trackerThree;
            VelocityTrackerMock unexpectedResult = trackerOne;
            VelocityTrackerMock actualResult = (VelocityTrackerMock)subject.ActiveVelocityTracker;

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetActiveVelocityTrackerAfterGetAngularVelocity()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            subject.GetAngularVelocity();

            VelocityTrackerMock expectedResult = trackerTwo;
            VelocityTrackerMock unexpectedResult = trackerOne;
            VelocityTrackerMock actualResult = (VelocityTrackerMock)subject.ActiveVelocityTracker;

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetAngularVelocityFromFirstActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(true, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(1f, 1f, 1f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetAngularVelocityFromSecondActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetAngularVelocityFromThirdActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetAngularVelocityFromNoneActive()
        {
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
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
    }
}