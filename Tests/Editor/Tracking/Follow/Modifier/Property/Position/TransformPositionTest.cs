using Zinnia.Data.Type;
using Zinnia.Extension;
using Zinnia.Tracking.Follow.Modifier.Property.Position;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Position
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class TransformPositionTest
    {
        private GameObject containingObject;
        private TransformPosition subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformPositionTest");
            subject = containingObject.AddComponent<TransformPosition>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionTest");
            GameObject target = new GameObject("TransformPositionTest");

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.Modify(source, target);

            Assert.That(source.transform.position, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator ModifySmoothed()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            UnityEventListenerMock transitionedMock = new UnityEventListenerMock();

            subject.Transitioned.AddListener(transitionedMock.Listen);
            subject.TransitionDuration = 0.1f;
            subject.EqualityTolerance = 0.01f;

            GameObject source = new GameObject("source");
            GameObject target = new GameObject("target");

            Vector3 sourcePosition = Vector3.one;
            Vector3 expectedPosition = Vector3.zero;

            source.transform.position = sourcePosition;
            target.transform.position = Vector3.zero;

            Assert.That(source.transform.position, Is.EqualTo(sourcePosition).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(expectedPosition).Using(comparer));
            Assert.IsFalse(transitionedMock.Received);

            do
            {
                subject.Modify(source, target);
                yield return null;
            }
            while (!target.transform.position.ApproxEquals(source.transform.position));

            expectedPosition = source.transform.position;

            Assert.That(source.transform.position, Is.EqualTo(sourcePosition).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(expectedPosition).Using(comparer));
            Assert.IsTrue(transitionedMock.Received);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionTest");
            GameObject target = new GameObject("TransformPositionTest");
            GameObject offset = new GameObject("TransformPositionTest");

            offset.transform.SetParent(target.transform);

            source.transform.position = Vector3.one * 2f;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 0.5f;

            subject.Modify(source, target, offset);

            Assert.That(source.transform.position, Is.EqualTo(Vector3.one * 2f).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(Vector3.one * 1.5f).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionTest");
            GameObject target = new GameObject("TransformPositionTest");
            GameObject offset = new GameObject("TransformPositionTest");

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;
            offset.transform.position = Vector3.one * 0.5f;

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.That(source.transform.position, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithAxisRestriction()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.ApplyModificationOnAxis = new Vector3State(true, false, true);
            GameObject source = new GameObject("TransformPositionTest");
            GameObject target = new GameObject("TransformPositionTest");

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.Modify(source, target);

            Assert.That(source.transform.position, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(new Vector3(1f, 0f, 1f)).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionTest");
            GameObject target = new GameObject("TransformPositionTest");

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.That(source.transform.position, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformPositionTest");
            GameObject target = new GameObject("TransformPositionTest");

            source.transform.position = Vector3.one;
            target.transform.position = Vector3.zero;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.That(source.transform.position, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.position, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}