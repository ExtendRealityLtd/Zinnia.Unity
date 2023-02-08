using Zinnia.Data.Type;
using Zinnia.Extension;
using Zinnia.Tracking.Follow.Modifier.Property.Scale;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Scale
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class TransformScaleTest
    {
        private GameObject containingObject;
        private TransformScale subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("TransformScaleTest");
            subject = containingObject.AddComponent<TransformScale>();
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
            GameObject source = new GameObject("TransformScaleTest");
            GameObject target = new GameObject("TransformScaleTest");

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.Modify(source, target);

            Assert.That(source.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

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

            Vector3 sourceScale = Vector3.one;
            Vector3 expectedScale = Vector3.zero;

            source.transform.localScale = sourceScale;
            target.transform.localScale = Vector3.zero;

            Assert.That(source.transform.localScale, Is.EqualTo(sourceScale).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(expectedScale).Using(comparer));
            Assert.IsFalse(transitionedMock.Received);

            do
            {
                subject.Modify(source, target);
                yield return null;
            }
            while (!target.transform.localScale.ApproxEquals(source.transform.localScale));

            expectedScale = source.transform.localScale;

            Assert.That(source.transform.localScale, Is.EqualTo(sourceScale).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(expectedScale).Using(comparer));
            Assert.IsTrue(transitionedMock.Received);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyWithOffset()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

            GameObject source = new GameObject("TransformScaleTest");
            GameObject target = new GameObject("TransformScaleTest");
            GameObject offset = new GameObject("TransformScaleTest");

            offset.transform.SetParent(target.transform);

            source.transform.localScale = Vector3.one * 4f;
            target.transform.localScale = Vector3.one;
            offset.transform.localScale = Vector3.one * 2f;

            subject.Modify(source, target, offset);

            Assert.That(source.transform.localScale, Is.EqualTo(Vector3.one * 4f).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one * 2f).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithOffsetIgnored()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

            GameObject source = new GameObject("TransformScaleTest");
            GameObject target = new GameObject("TransformScaleTest");
            GameObject offset = new GameObject("TransformScaleTest");

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;
            offset.transform.localScale = Vector3.one * 0.5f;

            subject.ApplyOffset = false;
            subject.Modify(source, target, offset);

            Assert.That(source.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ModifyWithAxisRestriction()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.ApplyModificationOnAxis = new Vector3State(true, false, true);
            GameObject source = new GameObject("TransformScaleTest");
            GameObject target = new GameObject("TransformScaleTest");

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.Modify(source, target);

            Assert.That(source.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(new Vector3(1f, 0f, 1f)).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformScaleTest");
            GameObject target = new GameObject("TransformScaleTest");

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.gameObject.SetActive(false);
            subject.Modify(source, target);

            Assert.That(source.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            GameObject source = new GameObject("TransformScaleTest");
            GameObject target = new GameObject("TransformScaleTest");

            target.transform.localScale = Vector3.zero;
            source.transform.localScale = Vector3.one;

            subject.enabled = false;
            subject.Modify(source, target);

            Assert.That(source.transform.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(target.transform.localScale, Is.EqualTo(Vector3.zero).Using(comparer));

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}