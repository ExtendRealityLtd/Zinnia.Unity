using VRTK.Core.Extension;
using VRTK.Core.Data.Type;

namespace Test.VRTK.Core.Extension
{
    using UnityEngine;
    using NUnit.Framework;

    public class TransformDataExtensionsTest
    {
        [Test]
        public void TryGetGameObjectValid()
        {
            TransformData valid = new TransformData(new GameObject());
            Assert.NotNull(valid.TryGetGameObject());
            Object.DestroyImmediate(valid.transform.gameObject);
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