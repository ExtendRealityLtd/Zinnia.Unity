using VRTK.Core.Extension;

namespace Test.VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

    public class GameObjectExtensionsTest
    {
        [Test]
        public void TryGetComponentValid()
        {
            GameObject valid = new GameObject();
            Assert.AreEqual(valid.GetComponent<Component>(), valid.TryGetComponent());
            Object.DestroyImmediate(valid);
        }

        [Test]
        public void TryGetComponentInvalid()
        {
            GameObject invalid = null;
            Assert.IsNull(invalid.TryGetComponent());
        }

        [Test]
        public void TrySetActive()
        {
            GameObject valid = new GameObject();
            Assert.IsTrue(valid.activeInHierarchy);
            valid.TrySetActive(false);
            Assert.IsFalse(valid.activeInHierarchy);
            valid.TrySetActive(true);
            Assert.IsTrue(valid.activeInHierarchy);
            Object.DestroyImmediate(valid);
        }

        [Test]
        public void FindRigidbodyOnSameValid()
        {
            GameObject valid = new GameObject();
            Rigidbody rigidbody = valid.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, valid.FindRigidbody());

            Object.DestroyImmediate(valid);
        }

        [Test]
        public void FindRigidbodyInvalid()
        {
            GameObject invalid = null;
            Assert.IsNull(invalid.FindRigidbody());
        }

        [Test]
        public void FindRigidbodyOnDescendantValid()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);

            Rigidbody rigidbody = child.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, parent.FindRigidbody(true));

            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnAncestorValid()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);

            Rigidbody rigidbody = parent.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.FindRigidbody(false, true));

            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnDescendantFirstValid()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            parent.AddComponent<Rigidbody>();
            Rigidbody rigidbody = grandchild.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.FindRigidbody(true, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnAncestorFirstValid()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            Rigidbody rigidbody = parent.AddComponent<Rigidbody>();
            grandchild.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.FindRigidbody(false, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnDescendantFirstInvalid()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            parent.AddComponent<Rigidbody>();

            Assert.IsNull(child.FindRigidbody(true, false));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnAncestorFirstInvalid()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            grandchild.AddComponent<Rigidbody>();

            Assert.IsNull(child.FindRigidbody(false, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }
    }
}