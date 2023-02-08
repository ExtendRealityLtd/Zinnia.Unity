using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class GameObjectExtensionsTest
    {
        [Test]
        public void TryGetComponentValid()
        {
            GameObject valid = new GameObject("GameObjectExtensionsTest");
            Assert.AreEqual(valid.GetComponent<Component>(), valid.TryGetComponent<Component>());
            Object.DestroyImmediate(valid);
        }

        [Test]
        public void TryGetComponentInvalid()
        {
            GameObject invalid = null;
            Assert.IsNull(invalid.TryGetComponent<Component>());
        }

        [Test]
        public void TrySetActive()
        {
            GameObject valid = new GameObject("GameObjectExtensionsTest");
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
            GameObject valid = new GameObject("GameObjectExtensionsTest");
            Rigidbody rigidbody = valid.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, valid.TryGetComponent<Rigidbody>(true));

            Object.DestroyImmediate(valid);
        }

        [Test]
        public void FindRigidbodyInvalid()
        {
            GameObject invalid = null;
            Assert.IsNull(invalid.TryGetComponent<Rigidbody>(true));
        }

        [Test]
        public void FindRigidbodyOnDescendantValid()
        {
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);

            Rigidbody rigidbody = child.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, parent.TryGetComponent<Rigidbody>(true));

            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnAncestorValid()
        {
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);

            Rigidbody rigidbody = parent.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.TryGetComponent<Rigidbody>(false, true));

            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnDescendantFirstValid()
        {
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            GameObject grandchild = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            parent.AddComponent<Rigidbody>();
            Rigidbody rigidbody = grandchild.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.TryGetComponent<Rigidbody>(true, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnAncestorFirstValid()
        {
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            GameObject grandchild = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            Rigidbody rigidbody = parent.AddComponent<Rigidbody>();
            grandchild.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, child.TryGetComponent<Rigidbody>(false, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnDescendantFirstInvalid()
        {
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            GameObject grandchild = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            parent.AddComponent<Rigidbody>();

            Assert.IsNull(child.TryGetComponent<Rigidbody>(true, false));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void FindRigidbodyOnAncestorFirstInvalid()
        {
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            GameObject grandchild = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);

            grandchild.AddComponent<Rigidbody>();

            Assert.IsNull(child.TryGetComponent<Rigidbody>(false, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetPosition()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 destinationPosition = Vector3.one * 2f;
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            parent.transform.position = destinationPosition;

            Assert.That(parent.TryGetPosition(), Is.EqualTo(destinationPosition).Using(comparer));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetPositionLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 destinationPosition = Vector3.one * 2f;
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            child.transform.position = destinationPosition;
            parent.transform.position = destinationPosition * 2f;

            Assert.That(child.TryGetPosition(true), Is.EqualTo(destinationPosition).Using(comparer));

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void TryGetRotation()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            Quaternion destinationRotation = Quaternion.Euler(Vector3.up * 90f);
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            parent.transform.rotation = destinationRotation;

            Assert.That(parent.TryGetRotation(), Is.EqualTo(destinationRotation).Using(comparer));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetRotationLocal()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            Quaternion destinationRotation = Quaternion.Euler(Vector3.up * 90f);
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            child.transform.localRotation = destinationRotation;
            parent.transform.localRotation = Quaternion.Euler(Vector3.up * 145f);

            Assert.That(child.TryGetRotation(true), Is.EqualTo(destinationRotation).Using(comparer));

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void TryGetEulerRotation()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 destinationEulerRotation = Vector3.up * 90f;
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            parent.transform.eulerAngles = destinationEulerRotation;

            Assert.That(parent.TryGetEulerRotation(), Is.EqualTo(destinationEulerRotation).Using(comparer));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetEulerRotationLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 destinationRotation = Vector3.up * 90f;
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            child.transform.localEulerAngles = destinationRotation;
            parent.transform.localEulerAngles = Vector3.up * 145f;

            Assert.That(child.TryGetEulerRotation(true), Is.EqualTo(destinationRotation).Using(comparer));

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void TryGetScale()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 destinationScale = Vector3.one * 2f;
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            parent.transform.SetGlobalScale(destinationScale);

            Assert.That(parent.TryGetScale(), Is.EqualTo(destinationScale).Using(comparer));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetScaleLocal()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Vector3 destinationScale = Vector3.one * 2f;
            GameObject parent = new GameObject("GameObjectExtensionsTest");
            GameObject child = new GameObject("GameObjectExtensionsTest");
            child.transform.SetParent(parent.transform);
            child.transform.localScale = destinationScale;
            parent.transform.SetGlobalScale(destinationScale * 2f);

            Assert.That(child.TryGetScale(true), Is.EqualTo(destinationScale).Using(comparer));

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }
    }
}