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
    }
}