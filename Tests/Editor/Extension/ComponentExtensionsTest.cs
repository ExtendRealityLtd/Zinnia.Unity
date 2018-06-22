using VRTK.Core.Extension;

namespace Test.VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

    public class ComponentExtensionsTest
    {
        [Test]
        public void TryGetTransformValid()
        {
            Transform validTransform = new GameObject().transform;
            Assert.NotNull(validTransform.TryGetTransform());
            Object.DestroyImmediate(validTransform.gameObject);
        }

        [Test]
        public void TryGetTransformInvalid()
        {
            Component invalidComponent = new Component();
            Assert.IsNull(invalidComponent.TryGetTransform());
            Object.DestroyImmediate(invalidComponent);
        }
    }
}