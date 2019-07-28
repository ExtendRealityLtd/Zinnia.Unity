using Zinnia.Tracking.Modification;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Tracking.Modification
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class ComponentEnabledStateModifierTest
    {
        private GameObject containingObject;
        private ComponentEnabledStateModifier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ComponentEnabledStateModifier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator SetEnabledStateOfBehaviour()
        {
            Behaviour behaviour = containingObject.AddComponent<Light>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(Light));
            subject.Target = containingObject;

            Assert.IsTrue(behaviour.enabled);
            subject.SetEnabledState(false);
            Assert.IsFalse(behaviour.enabled);
            subject.SetEnabledState(true);
            Assert.IsTrue(behaviour.enabled);
        }

        [UnityTest]
        public IEnumerator SetEnabledStateOfRenderer()
        {
            MeshRenderer renderer = containingObject.AddComponent<MeshRenderer>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(MeshRenderer));
            subject.Target = containingObject;

            Assert.IsTrue(renderer.enabled);
            subject.SetEnabledState(false);
            Assert.IsFalse(renderer.enabled);
            subject.SetEnabledState(true);
            Assert.IsTrue(renderer.enabled);
        }

        [UnityTest]
        public IEnumerator SetEnabledStateOfBehaviourAndRenderer()
        {
            MeshRenderer renderer = containingObject.AddComponent<MeshRenderer>();
            Behaviour behaviour = containingObject.AddComponent<Light>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(Light));
            subject.Types.Add(typeof(MeshRenderer));
            subject.Target = containingObject;

            Assert.IsTrue(behaviour.enabled);
            Assert.IsTrue(renderer.enabled);
            subject.SetEnabledState(false);
            Assert.IsFalse(behaviour.enabled);
            Assert.IsFalse(renderer.enabled);
            subject.SetEnabledState(true);
            Assert.IsTrue(behaviour.enabled);
            Assert.IsTrue(renderer.enabled);
        }

        [UnityTest]
        public IEnumerator SetEnabledStateOfInvalidType()
        {
            Behaviour behaviour = containingObject.AddComponent<Light>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(Renderer));
            subject.Target = containingObject;

            Assert.IsTrue(behaviour.enabled);
            subject.SetEnabledState(false);
            Assert.IsTrue(behaviour.enabled);
        }

        [UnityTest]
        public IEnumerator SetEnabledStateInvalidTarget()
        {
            Behaviour behaviour = containingObject.AddComponent<Light>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(Light));

            Assert.IsTrue(behaviour.enabled);
            subject.SetEnabledState(false);
            Assert.IsTrue(behaviour.enabled);
        }

        [UnityTest]
        public IEnumerator SetEnabledStateInactiveGameObject()
        {
            Behaviour behaviour = containingObject.AddComponent<Light>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(Light));
            subject.Target = containingObject;

            subject.gameObject.SetActive(false);

            Assert.IsTrue(behaviour.enabled);
            subject.SetEnabledState(false);
            Assert.IsTrue(behaviour.enabled);
        }


        [UnityTest]
        public IEnumerator SetEnabledStateInactiveComponent()
        {
            Behaviour behaviour = containingObject.AddComponent<Light>();
            subject.Types = containingObject.AddComponent<SerializableTypeComponentObservableList>();
            yield return null;

            subject.Types.Add(typeof(Light));
            subject.Target = containingObject;

            subject.enabled = false;

            Assert.IsTrue(behaviour.enabled);
            subject.SetEnabledState(false);
            Assert.IsTrue(behaviour.enabled);
        }
    }
}