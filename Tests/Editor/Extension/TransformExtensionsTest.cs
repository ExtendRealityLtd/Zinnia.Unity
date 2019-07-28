using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformExtensionsTest
    {
        [Test]
        public void SetGlobalScaleValid()
        {
            Transform parent = new GameObject().transform;
            Transform child = new GameObject().transform;
            child.SetParent(parent);
            Vector3 newScale = Vector3.one * 2f;

            Assert.AreEqual(Vector3.one, child.localScale);
            Assert.AreEqual(Vector3.one, child.lossyScale);

            child.SetGlobalScale(newScale);

            Assert.AreEqual(newScale, child.localScale);
            Assert.AreEqual(newScale, child.lossyScale);

            Object.DestroyImmediate(child.gameObject);
            Object.DestroyImmediate(parent.gameObject);
        }
    }
}