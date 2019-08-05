using Zinnia.Tracking.Velocity;

namespace Test.Zinnia.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class AverageVelocityEstimatorTest
    {
        private GameObject containingObject;
        private GameObject source;
        private AverageVelocityEstimatorMock subject;

        private static readonly Vector3[] exampleSourcePositions = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(3f, 3f, 3f),
            new Vector3(6f, 6f, 6f),
            new Vector3(8f, 8f, 8f)
        };

        private static readonly Quaternion[] exampleSourceRotations = new Quaternion[]
        {
            new Quaternion(1f, 0f, 0f, 0f),
            new Quaternion(0.707f, 0.707f, 0f, 0f),
            new Quaternion(0.707f, 0f, 0.707f, 0f),
            new Quaternion(0.707f, 0f, 45f, 0.707f),
            new Quaternion(0f, 0.707f, 0f, 0.707f)
        };

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            source = containingObject;
            subject = containingObject.AddComponent<AverageVelocityEstimatorMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void IsActiveSourceActive()
        {
            subject.Source = source;

            bool actualResult = subject.IsActive();
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsActiveSourceInActive()
        {
            source.gameObject.SetActive(false);
            subject.Source = source;

            bool actualResult = subject.IsActive();
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void IsActiveNoSource()
        {
            bool actualResult = subject.IsActive();
            Assert.IsFalse(actualResult);
        }

        [Test]
        public void GetVelocityAutoStartEstimating()
        {
            Vector3 expectedResult = new Vector3(1.6f, 1.6f, 1.6f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = true;
            subject.ManualOnEnable();

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityOver0Frames()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.VelocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityOver4Frames()
        {
            Vector3 expectedResult = new Vector3(2f, 2f, 2f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.VelocityAverageFrames = 4;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityOver5Frames()
        {
            Vector3 expectedResult = new Vector3(1.6f, 1.6f, 1.6f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.VelocityAverageFrames = 5;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityWhenNotEstimating()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.VelocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.IsEstimating = false;

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetVelocityWithoutSource()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityAutoStartEstimating()
        {
            Vector3 expectedResult = new Vector3(0.4172499f, -0.5427582f, -0.36673f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = true;
            subject.AngularVelocityAverageFrames = 5;
            subject.ManualOnEnable();

            ProcessRotations(exampleSourceRotations);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityOver0Frames()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.AngularVelocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessRotations(exampleSourceRotations);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityOver4Frames()
        {
            Vector3 expectedResult = new Vector3(-0.2638358f, -0.6784477f, -0.4584125f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.AngularVelocityAverageFrames = 4;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessRotations(exampleSourceRotations);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityOver5Frames()
        {
            Vector3 expectedResult = new Vector3(0.4172499f, -0.5427582f, -0.36673f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.AngularVelocityAverageFrames = 5;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessRotations(exampleSourceRotations);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult.ToString(), actualResult.ToString());
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityWhenNotEstimating()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.AngularVelocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.IsEstimating = false;

            ProcessRotations(exampleSourceRotations);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAngularVelocityWithoutSource()
        {
            Vector3 expectedResult = new Vector3(0f, 0f, 0f);
            Vector3 unexpectedResult = new Vector3(1.6f, 1.6f, 1.6f);

            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessRotations(exampleSourceRotations);

            Vector3 actualResult = subject.GetAngularVelocity();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        [Test]
        public void GetAccelerationOver5Frames()
        {
            Vector3 expectedResult = new Vector3(3f, 3f, 3f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.Source = source;
            subject.IsEstimating = false;
            subject.VelocityAverageFrames = 5;
            subject.ManualOnEnable();
            subject.IsEstimating = true;

            ProcessPositions(exampleSourcePositions);

            Vector3 actualResult = subject.GetAcceleration();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreNotEqual(unexpectedResult, actualResult);
        }

        private void ProcessPositions(Vector3[] positions)
        {
            //Loop through the set custom positions and update the source's position then manually process the subject emulating an update loop
            foreach (Vector3 updatedPosition in positions)
            {
                source.transform.localPosition = updatedPosition;
                subject.ManualLateUpdate();
            }
        }

        private void ProcessRotations(Quaternion[] rotations)
        {
            //Loop through the set custom rotations and update the source's rotation then manually process the subject emulating an update loop
            foreach (Quaternion updatedRotation in rotations)
            {
                source.transform.localRotation = updatedRotation;
                subject.ManualLateUpdate();
            }
        }
    }

    public class AverageVelocityEstimatorMock : AverageVelocityEstimator
    {
        public void ManualOnEnable()
        {
            OnEnable();
        }

        public void ManualLateUpdate()
        {
            ProcessEstimation();
        }

        protected override float GetFactor()
        {
            return 1f;
        }
    }
}