using Zinnia.Data.Type;
using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformDataExtensionsTest
    {
        [Test]
        public void TryGetGameObjectValid()
        {
            TransformData valid = new TransformData(new GameObject());
            Assert.IsNotNull(valid.TryGetGameObject());
            Object.DestroyImmediate(valid.Transform.gameObject);
        }

        [Test]
        public void TryGetGameObjectInvalid()
        {
            TransformData invalid = new TransformData();
            Assert.IsNull(invalid.TryGetGameObject());
        }

        [Test]
        public void TryGetGameObjectNull()
        {
            TransformData invalid = null;
            Assert.IsNull(invalid.TryGetGameObject());
        }
    }
}