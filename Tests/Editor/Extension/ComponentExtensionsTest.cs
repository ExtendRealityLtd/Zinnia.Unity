using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class ComponentExtensionsTest
    {
        [Test]
        public void TryGetTransformValid()
        {
            Component valid = new GameObject().transform;
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
            Component valid = new GameObject().GetComponent<Component>();
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