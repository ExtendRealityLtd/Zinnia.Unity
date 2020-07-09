using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformExtensionsTest
    {
        [Test]
        public void SetGlobalScaleValid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            child.SetParent(parent);
            Vector3 newScale = Vector3.one * 2f;

            Assert.AreEqual(Vector3.one, child.localScale);
            Assert.AreEqual(Vector3.one, child.lossyScale);

            child.SetGlobalScale(newScale);

            Assert.AreEqual(newScale, child.localScale);
            Assert.AreEqual(newScale, child.lossyScale);

            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void SignedEulerAngles()
        {
            Transform subject = new GameObject().transform;

            Assert.AreEqual(new Vector3(0f, 0f, 0f).ToString(), subject.SignedEulerAngles().ToString());

            subject.eulerAngles = new Vector3(0f, 90f, 90f);
            Assert.AreEqual(new Vector3(0f, 90f, 90f).ToString(), subject.SignedEulerAngles().ToString());

            subject.eulerAngles = new Vector3(0f, 179f, 90f);
            Assert.AreEqual(new Vector3(0f, 179f, 90f).ToString(), subject.SignedEulerAngles().ToString());

            subject.eulerAngles = new Vector3(0f, 180f, 90f);
            Assert.AreEqual(new Vector3(0f, 180f, 90f).ToString(), subject.SignedEulerAngles().ToString());

            subject.eulerAngles = new Vector3(0f, 181f, 90f);
            Assert.AreEqual(new Vector3(0f, -179f, 90f).ToString(), subject.SignedEulerAngles().ToString());

            subject.eulerAngles = new Vector3(0f, 270f, 90f);
            Assert.AreEqual(new Vector3(0f, -90f, 90f).ToString(), subject.SignedEulerAngles().ToString());

            subject.eulerAngles = new Vector3(0f, 360f, 90f);
            Assert.AreEqual(new Vector3(0f, 0f, 90f).ToString(), subject.SignedEulerAngles().ToString());

            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void SignedLocalEulerAngles()
        {
            Transform subject = new GameObject().transform;

            Assert.AreEqual(new Vector3(0f, 0f, 0f).ToString(), subject.SignedLocalEulerAngles().ToString());

            subject.localEulerAngles = new Vector3(0f, 90f, 90f);
            Assert.AreEqual(new Vector3(0f, 90f, 90f).ToString(), subject.SignedLocalEulerAngles().ToString());

            subject.localEulerAngles = new Vector3(0f, 179f, 90f);
            Assert.AreEqual(new Vector3(0f, 179f, 90f).ToString(), subject.SignedLocalEulerAngles().ToString());

            subject.localEulerAngles = new Vector3(0f, 180f, 90f);
            Assert.AreEqual(new Vector3(0f, 180f, 90f).ToString(), subject.SignedLocalEulerAngles().ToString());

            subject.localEulerAngles = new Vector3(0f, 181f, 90f);
            Assert.AreEqual(new Vector3(0f, -179f, 90f).ToString(), subject.SignedLocalEulerAngles().ToString());

            subject.localEulerAngles = new Vector3(0f, 270f, 90f);
            Assert.AreEqual(new Vector3(0f, -90f, 90f).ToString(), subject.SignedLocalEulerAngles().ToString());

            subject.localEulerAngles = new Vector3(0f, 360f, 90f);
            Assert.AreEqual(new Vector3(0f, 0f, 90f).ToString(), subject.SignedLocalEulerAngles().ToString());

            Object.DestroyImmediate(subject.gameObject);
        }
    }
}