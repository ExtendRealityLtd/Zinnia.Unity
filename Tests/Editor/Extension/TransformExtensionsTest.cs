using VRTK.Core.Extension;

namespace Test.VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformExtensionsTest
    {
        [Test]
        public void FindRigidbodyOnSameValid()
        {
            Transform valid = new GameObject().transform;
            Rigidbody rigidbody = valid.gameObject.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, valid.FindRigidbody());

            Object.DestroyImmediate(valid.gameObject);
        }

        [Test]
        public void FindRigidbodyInvalid()
        {
            Transform invalid = null;
            Assert.IsNull(invalid.FindRigidbody());
        }

        [Test]
        public void FindRigidbodyOnDescendantValid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            child.SetParent(parent);

            Rigidbody rigidbody = child.gameObject.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, parent.FindRigidbody(true));

            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void FindRigidbodyOnAncestorValid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            child.SetParent(parent);

            Rigidbody rigidbody = parent.gameObject.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.FindRigidbody(false, true));

            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void FindRigidbodyOnDescendantFirstValid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            Transform grandchild = new GameObject().transform;
            child.SetParent(parent);
            grandchild.SetParent(child);

            parent.gameObject.AddComponent<Rigidbody>();
            Rigidbody rigidbody = grandchild.gameObject.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.FindRigidbody(true, true));

            Object.DestroyImmediate(grandchild.gameObject);
            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void FindRigidbodyOnAncestorFirstValid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            Transform grandchild = new GameObject().transform;
            child.SetParent(parent);
            grandchild.SetParent(child);

            Rigidbody rigidbody = parent.gameObject.AddComponent<Rigidbody>();
            grandchild.gameObject.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.FindRigidbody(false, true));

            Object.DestroyImmediate(grandchild.gameObject);
            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void FindRigidbodyOnDescendantFirstInvalid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            Transform grandchild = new GameObject().transform;
            child.SetParent(parent);
            grandchild.SetParent(child);

            parent.gameObject.AddComponent<Rigidbody>();

            Assert.IsNull(child.FindRigidbody(true, false));

            Object.DestroyImmediate(grandchild.gameObject);
            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

        [Test]
        public void FindRigidbodyOnAncestorFirstInvalid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            Transform grandchild = new GameObject().transform;
            child.SetParent(parent);
            grandchild.SetParent(child);

            grandchild.gameObject.AddComponent<Rigidbody>();

            Assert.IsNull(child.FindRigidbody(false, true));

            Object.DestroyImmediate(grandchild.gameObject);
            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }

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
    }
}