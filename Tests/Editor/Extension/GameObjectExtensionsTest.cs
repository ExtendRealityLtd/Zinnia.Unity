using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectExtensionsTest
    {
        [Test]
        public void TryGetComponentValid()
        {
            GameObject valid = new GameObject();
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
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);

            Rigidbody rigidbody = child.AddComponent<Rigidbody>();

            Assert.AreEqual(rigidbody, parent.TryGetComponent<Rigidbody>(true));

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

            Assert.AreEqual(rigidbody, child.TryGetComponent<Rigidbody>(false, true));

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

            Assert.AreEqual(rigidbody, child.TryGetComponent<Rigidbody>(true, true));

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

            Assert.AreEqual(rigidbody, child.TryGetComponent<Rigidbody>(false, true));

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

            Assert.IsNull(child.TryGetComponent<Rigidbody>(true, false));

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

            Assert.IsNull(child.TryGetComponent<Rigidbody>(false, true));

            Object.DestroyImmediate(grandchild);
            Object.DestroyImmediate(child);
            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetPosition()
        {
            Vector3 destinationPosition = Vector3.one * 2f;
            GameObject parent = new GameObject();
            parent.transform.position = destinationPosition;

            Assert.AreEqual(destinationPosition, parent.TryGetPosition());

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetPositionLocal()
        {
            Vector3 destinationPosition = Vector3.one * 2f;
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);
            child.transform.position = destinationPosition;
            parent.transform.position = destinationPosition * 2f;

            Assert.AreEqual(destinationPosition, child.TryGetPosition(true));

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void TryGetRotation()
        {
            Quaternion destinationRotation = Quaternion.Euler(Vector3.up * 90f);
            GameObject parent = new GameObject();
            parent.transform.rotation = destinationRotation;

            Assert.AreEqual(destinationRotation.ToString(), parent.TryGetRotation().ToString());

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetRotationLocal()
        {
            Quaternion destinationRotation = Quaternion.Euler(Vector3.up * 90f);
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);
            child.transform.localRotation = destinationRotation;
            parent.transform.localRotation = Quaternion.Euler(Vector3.up * 145f);

            Assert.AreEqual(destinationRotation.ToString(), child.TryGetRotation(true).ToString());

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void TryGetEulerRotation()
        {
            Vector3 destinationEulerRotation = Vector3.up * 90f;
            GameObject parent = new GameObject();
            parent.transform.eulerAngles = destinationEulerRotation;

            Assert.AreEqual(destinationEulerRotation, parent.TryGetEulerRotation());

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetEulerRotationLocal()
        {
            Vector3 destinationRotation = Vector3.up * 90f;
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);
            child.transform.localEulerAngles = destinationRotation;
            parent.transform.localEulerAngles = Vector3.up * 145f;

            Assert.AreEqual(destinationRotation.ToString(), child.TryGetEulerRotation(true).ToString());

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }

        [Test]
        public void TryGetScale()
        {
            Vector3 destinationScale = Vector3.one * 2f;
            GameObject parent = new GameObject();
            parent.transform.SetGlobalScale(destinationScale);

            Assert.AreEqual(destinationScale, parent.TryGetScale());

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void TryGetScaleLocal()
        {
            Vector3 destinationScale = Vector3.one * 2f;
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            child.transform.SetParent(parent.transform);
            child.transform.localScale = destinationScale;
            parent.transform.SetGlobalScale(destinationScale * 2f);

            Assert.AreEqual(destinationScale, child.TryGetScale(true));

            Object.DestroyImmediate(parent);
            Object.DestroyImmediate(child);
        }
    }
}