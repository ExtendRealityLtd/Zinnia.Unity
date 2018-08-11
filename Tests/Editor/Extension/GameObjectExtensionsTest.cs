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
            GameObject valid = null;
            Assert.IsNull(valid.TryGetComponent());
        }
    }
}