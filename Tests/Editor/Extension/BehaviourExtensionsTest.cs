using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class BehaviourExtensionsTest
    {
        [UnityTest]
        public IEnumerator RunWhenActiveAndEnabled()
        {
            GameObject container = new GameObject();
            container.SetActive(false);
            MockBehaviour mockBehaviour = container.AddComponent<MockBehaviour>();
            yield return null;

            mockBehaviour.ExecuteOnlyWhenEnabled();

            Assert.IsFalse(mockBehaviour.hasEnabled);
            Assert.IsFalse(mockBehaviour.hasExecuted);

            container.SetActive(true);
            yield return null;

            Assert.IsTrue(mockBehaviour.hasEnabled);
            Assert.IsFalse(mockBehaviour.hasExecuted);

            container.SetActive(false);
            yield return null;

            Assert.IsFalse(mockBehaviour.hasEnabled);
            Assert.IsFalse(mockBehaviour.hasExecuted);

            mockBehaviour.RunWhenActiveAndEnabled(() => mockBehaviour.ExecuteOnlyWhenEnabled());

            container.SetActive(true);
            yield return null;

            Assert.IsTrue(mockBehaviour.hasEnabled);
            Assert.IsTrue(mockBehaviour.hasExecuted);

            Object.DestroyImmediate(container);
        }
    }

    public class MockBehaviour : MonoBehaviour
    {
        public bool hasEnabled;
        public bool hasExecuted;

        public void ExecuteOnlyWhenEnabled()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            hasExecuted = true;
        }

        private void OnEnable()
        {
            hasEnabled = true;
        }

        private void OnDisable()
        {
            hasEnabled = false;
        }
    }
}