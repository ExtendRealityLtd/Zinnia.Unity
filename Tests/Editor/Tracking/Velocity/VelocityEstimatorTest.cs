namespace VRTK.Core.Tracking.Velocity
{
    using UnityEngine;
    using NUnit.Framework;

    public class VelocityEstimatorTest
    {
        private GameObject containingObject;
        private Transform source;
        private VelocityEstimatorMock subject;

        private Vector3[] exampleSourcePositions = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(3f, 3f, 3f),
            new Vector3(6f, 6f, 6f),
            new Vector3(8f, 8f, 8f)
        };

        private Quaternion[] exampleSourceRotations = new Quaternion[]
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
            source = containingObject.transform;
            subject = containingObject.AddComponent<VelocityEstimatorMock>();
        }

        [TearDown]
        public void TearDown()
        {
            subject = null;
            source = null;
            containingObject = null;
        }

        [Test]
        public void IsActiveSourceActive()
        {
            subject.source = source;

            bool actualResult = subject.IsActive();
            Assert.IsTrue(actualResult);
        }

        [Test]
        public void IsActiveSourceInActive()
        {
            source.gameObject.SetActive(false);
            subject.source = source;

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
        public void StartEstimation()
        {
            subject.autoStartSampling = false;
            subject.ManualOnEnable();
            subject.StartEstimation();

            Assert.IsTrue(subject.IsEstimating());
        }

        [Test]
        public void EndEstimation()
        {
            subject.autoStartSampling = false;
            subject.ManualOnEnable();
            subject.StartEstimation();
            subject.EndEstimation();

            Assert.IsFalse(subject.IsEstimating());
        }

        [Test]
        public void AutoStartEstimation()
        {
            subject.autoStartSampling = true;
            subject.ManualOnEnable();

            Assert.IsTrue(subject.IsEstimating());
        }

        [Test]
        public void GetVelocityAutoStartEstimating()
        {
            Vector3 expectedResult = new Vector3(1.6f, 1.6f, 1.6f);
            Vector3 unexpectedResult = Vector3.zero;

            // Ensure the subject has a valid source to check
            subject.source = source;
            subject.autoStartSampling = true;
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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.velocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.velocityAverageFrames = 4;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.velocityAverageFrames = 5;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.velocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.EndEstimation();

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
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = true;
            subject.angularVelocityAverageFrames = 5;
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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.angularVelocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.angularVelocityAverageFrames = 4;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.angularVelocityAverageFrames = 5;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.angularVelocityAverageFrames = 0;
            subject.ManualOnEnable();
            subject.EndEstimation();

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
            subject.StartEstimation();

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
            subject.source = source;
            subject.autoStartSampling = false;
            subject.velocityAverageFrames = 5;
            subject.ManualOnEnable();
            subject.StartEstimation();

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
                source.localPosition = updatedPosition;
                subject.ManualLateUpdate();
            }
        }

        private void ProcessRotations(Quaternion[] rotations)
        {
            //Loop through the set custom rotations and update the source's rotation then manually process the subject emulating an update loop
            foreach (Quaternion updatedRotation in rotations)
            {
                source.localRotation = updatedRotation;
                subject.ManualLateUpdate();
            }
        }
    }

    public class VelocityEstimatorMock : VelocityEstimator
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