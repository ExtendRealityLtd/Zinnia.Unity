using Zinnia.Tracking.Velocity;
using Zinnia.Tracking.Velocity.Collection;

namespace Test.Zinnia.Tracking.Velocity
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class VelocityTrackerProcessorTest
    {
        private GameObject containingObject;
        private VelocityTrackerProcessor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("VelocityTrackerProcessorTest");
            subject = containingObject.AddComponent<VelocityTrackerProcessor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator GetVelocityFromFirstActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(true, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(1f, 1f, 1f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetVelocityFromSecondActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetVelocityFromThirdActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetVelocityFromNoneActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetVelocityWithoutTrackers()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));
        }

        [UnityTest]
        public IEnumerator GetActiveVelocityTrackerAfterGetVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            subject.GetVelocity();

            VelocityTrackerMock expectedResult = trackerThree;
            VelocityTrackerMock unexpectedResult = trackerOne;
            VelocityTrackerMock actualResult = (VelocityTrackerMock)subject.ActiveVelocityTracker;

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetActiveVelocityTrackerAfterGetAngularVelocity()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            subject.GetAngularVelocity();

            VelocityTrackerMock expectedResult = trackerTwo;
            VelocityTrackerMock unexpectedResult = trackerOne;
            VelocityTrackerMock actualResult = (VelocityTrackerMock)subject.ActiveVelocityTracker;

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAngularVelocityFromFirstActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(true, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(1f, 1f, 1f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAngularVelocityFromSecondActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(true, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAngularVelocityFromThirdActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(true, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = new Vector3(0f, 0f, 0f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [UnityTest]
        public IEnumerator GetAngularVelocityFromNoneActive()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            VelocityTrackerMock trackerOne = VelocityTrackerMock.Generate(false, new Vector3(1f, 1f, 1f), new Vector3(1f, 1f, 1f));
            VelocityTrackerMock trackerTwo = VelocityTrackerMock.Generate(false, new Vector3(2f, 2f, 2f), new Vector3(2f, 2f, 2f));
            VelocityTrackerMock trackerThree = VelocityTrackerMock.Generate(false, new Vector3(3f, 3f, 3f), new Vector3(3f, 3f, 3f));

            VelocityTrackerObservableList velocityTrackers = containingObject.AddComponent<VelocityTrackerObservableList>();
            yield return null;
            subject.VelocityTrackers = velocityTrackers;

            velocityTrackers.Add(trackerOne);
            velocityTrackers.Add(trackerTwo);
            velocityTrackers.Add(trackerThree);

            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));

            Object.DestroyImmediate(trackerOne.gameObject);
            Object.DestroyImmediate(trackerTwo.gameObject);
            Object.DestroyImmediate(trackerThree.gameObject);
        }

        [Test]
        public void GetAngularVelocityWithoutTrackers()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1f, 1f, 1f);
            Vector3 actualResult = subject.GetAngularVelocity();

            Assert.That(actualResult, Is.EqualTo(expectedResult).Using(comparer));
            Assert.That(actualResult, Is.Not.EqualTo(unexpectedResult).Using(comparer));
        }
    }
}