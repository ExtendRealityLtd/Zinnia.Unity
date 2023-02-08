using Zinnia.Cast;

namespace Test.Zinnia.Cast
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class FixedLineCastTest
    {
        private GameObject containingObject;
        private FixedLineCastMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("FixedLineCastTest");
            subject = containingObject.AddComponent<FixedLineCastMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CastPoints()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock castResultsChangedMock = new UnityEventListenerMock();
            subject.ResultsChanged.AddListener(castResultsChangedMock.Listen);
            subject.Origin = subject.gameObject;
            subject.CurrentLength = 10f;

            subject.ManualOnEnable();
            subject.Process();

            Vector3 expectedStart = Vector3.zero;
            Vector3 expectedEnd = new Vector3(0f, 0f, 10f);

            Assert.That(subject.Points[0], Is.EqualTo(expectedStart).Using(comparer));
            Assert.That(subject.Points[1], Is.EqualTo(expectedEnd).Using(comparer));

            Assert.IsTrue(castResultsChangedMock.Received);
        }
    }

    public class FixedLineCastMock : FixedLineCast
    {
        public void ManualOnEnable()
        {
            OnEnable();
        }

        public void ManualOnDisable()
        {
            OnDisable();
        }
    }
}