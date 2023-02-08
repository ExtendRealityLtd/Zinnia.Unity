using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class TransformExtensionsTest
    {
        [Test]
        public void SetGlobalScaleValid()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Transform parent = new GameObject("TransformExtensionsTest").transform;
            Transform child = new GameObject("TransformExtensionsTest").transform;
            child.SetParent(parent);
            Vector3 newScale = Vector3.one * 2f;

            Assert.That(child.localScale, Is.EqualTo(Vector3.one).Using(comparer));
            Assert.That(child.lossyScale, Is.EqualTo(Vector3.one).Using(comparer));

            child.SetGlobalScale(newScale);

            Assert.That(child.localScale, Is.EqualTo(newScale).Using(comparer));
            Assert.That(child.lossyScale, Is.EqualTo(newScale).Using(comparer));

            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void SignedEulerAngles()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Transform subject = new GameObject("TransformExtensionsTest").transform;

            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, 0f, 0f)).Using(comparer));

            subject.eulerAngles = new Vector3(0f, 90f, 90f);
            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, 90f, 90f)).Using(comparer));

            subject.eulerAngles = new Vector3(0f, 179f, 90f);
            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, 179f, 90f)).Using(comparer));

            subject.eulerAngles = new Vector3(0f, 180f, 90f);
            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, 180f, 90f)).Using(comparer));

            subject.eulerAngles = new Vector3(0f, 181f, 90f);
            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, -179f, 90f)).Using(comparer));

            subject.eulerAngles = new Vector3(0f, 270f, 90f);
            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, -90f, 90f)).Using(comparer));

            subject.eulerAngles = new Vector3(0f, 360f, 90f);
            Assert.That(subject.SignedEulerAngles(), Is.EqualTo(new Vector3(0f, 0f, 90f)).Using(comparer));

            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void SignedLocalEulerAngles()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Transform subject = new GameObject("TransformExtensionsTest").transform;

            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, 0f, 0f)).Using(comparer));

            subject.localEulerAngles = new Vector3(0f, 90f, 90f);
            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, 90f, 90f)).Using(comparer));

            subject.localEulerAngles = new Vector3(0f, 179f, 90f);
            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, 179f, 90f)).Using(comparer));

            subject.localEulerAngles = new Vector3(0f, 180f, 90f);
            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, 180f, 90f)).Using(comparer));

            subject.localEulerAngles = new Vector3(0f, 181f, 90f);
            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, -179f, 90f)).Using(comparer));

            subject.localEulerAngles = new Vector3(0f, 270f, 90f);
            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, -90f, 90f)).Using(comparer));

            subject.localEulerAngles = new Vector3(0f, 360f, 90f);
            Assert.That(subject.SignedLocalEulerAngles(), Is.EqualTo(new Vector3(0f, 0f, 90f)).Using(comparer));

            Object.DestroyImmediate(subject.gameObject);
        }
    }
}