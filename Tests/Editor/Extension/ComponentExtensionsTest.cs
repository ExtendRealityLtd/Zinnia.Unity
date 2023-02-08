using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;

    public class ComponentExtensionsTest
    {
        [Test]
        public void TryGetTransformValid()
        {
            Component valid = new GameObject("ComponentExtensionsTest").transform;
            Assert.IsNotNull(valid.TryGetTransform());
            Object.DestroyImmediate(valid.gameObject);
        }

        [Test]
        public void TryGetTransformInvalid()
        {
            Component invalid = new Component();
            Assert.IsNull(invalid.TryGetTransform());
            Object.DestroyImmediate(invalid);
        }

        [Test]
        public void TryGetGameObjectValid()
        {
            Component valid = new GameObject("ComponentExtensionsTest").GetComponent<Component>();
            Assert.IsNotNull(valid.TryGetGameObject());
            Object.DestroyImmediate(valid.gameObject);
        }

        [Test]
        public void TryGetGameObjectInvalid()
        {
            Component invalid = new Component();
            Assert.IsNull(invalid.TryGetTransform());
            Object.DestroyImmediate(invalid);
        }
    }
}