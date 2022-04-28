using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.TestTools;
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

        [Test]
        public void IsValidState()
        {
            GameObject parent = new GameObject();
            GameObject container = new GameObject();
            container.transform.SetParent(parent.transform);
            MockBehaviour mockBehaviour = container.AddComponent<MockBehaviour>();

            Assert.IsTrue(mockBehaviour.IsValidState());

            mockBehaviour.enabled = false;

            Assert.IsFalse(mockBehaviour.IsValidState());
            Assert.IsTrue(mockBehaviour.IsValidState(BehaviourExtensions.GameObjectActivity.InHierarchy, false));

            mockBehaviour.enabled = true;

            Assert.IsTrue(mockBehaviour.IsValidState());

            parent.SetActive(false);

            Assert.IsFalse(mockBehaviour.IsValidState(BehaviourExtensions.GameObjectActivity.InHierarchy));
            Assert.IsTrue(mockBehaviour.IsValidState(BehaviourExtensions.GameObjectActivity.Self));

            parent.SetActive(true);
            container.SetActive(false);

            Assert.IsFalse(mockBehaviour.IsValidState(BehaviourExtensions.GameObjectActivity.InHierarchy));
            Assert.IsFalse(mockBehaviour.IsValidState(BehaviourExtensions.GameObjectActivity.Self));

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void IsMemberChangeAllowed()
        {
            GameObject container = new GameObject();
            MockBehaviour mockBehaviour = container.AddComponent<MockBehaviour>();

            Assert.IsTrue(mockBehaviour.IsMemberChangeAllowed());

            mockBehaviour.enabled = false;

            Assert.IsFalse(mockBehaviour.IsMemberChangeAllowed());

            mockBehaviour.enabled = true;

            Assert.IsTrue(mockBehaviour.IsMemberChangeAllowed());

            mockBehaviour.gameObject.SetActive(false);

            Assert.IsFalse(mockBehaviour.IsMemberChangeAllowed());

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