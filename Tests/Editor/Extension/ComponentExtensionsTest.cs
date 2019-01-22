using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using NUnit.Framework;

    public class ComponentExtensionsTest
    {
        [Test]
        public void TryGetTransformValid()
        {
            Component valid = new GameObject().transform;
            Assert.NotNull(valid.TryGetTransform());
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
            Component valid = new GameObject().GetComponent<Component>();
            Assert.NotNull(valid.TryGetGameObject());
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