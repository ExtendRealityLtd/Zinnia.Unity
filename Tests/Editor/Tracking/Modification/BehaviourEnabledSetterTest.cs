using VRTK.Core.Tracking.Modification;

namespace Test.VRTK.Core.Tracking.Modification
{
    using NUnit.Framework;
    using UnityEngine;

    public class BehaviourEnabledSetterTest
    {
        private GameObject containingObject;
        private BehaviourEnabledSetterMock subject;
        private Behaviour behaviour;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<BehaviourEnabledSetterMock>();

            subject.behaviourTypes.Add(typeof(Light));
            subject.target = containingObject;
            behaviour = containingObject.AddComponent<Light>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void OnlyWorksWhenEnabled()
        {
            subject.enabled = false;

            Assert.IsFalse(subject.wasSetBehavioursEnabledCalled);
            subject.SetBehavioursEnabled(true);
            Assert.IsTrue(subject.wasSetBehavioursEnabledCalled);
        }

        [Test]
        public void EnablesAlreadyEnabled()
        {
            Assert.IsTrue(behaviour.enabled);

            subject.SetBehavioursEnabled(true);
            Assert.IsTrue(behaviour.enabled);
        }

        [Test]
        public void EnablesDisabled()
        {
            behaviour.enabled = false;
            Assert.IsFalse(behaviour.enabled);

            subject.SetBehavioursEnabled(true);
            Assert.IsTrue(behaviour.enabled);
        }

        [Test]
        public void DisablesAlreadyDisabled()
        {
            behaviour.enabled = false;
            Assert.IsFalse(behaviour.enabled);

            subject.SetBehavioursEnabled(false);
            Assert.IsFalse(behaviour.enabled);
        }

        [Test]
        public void DisablesEnabled()
        {
            Assert.IsTrue(behaviour.enabled);

            subject.SetBehavioursEnabled(false);
            Assert.IsFalse(behaviour.enabled);
        }

        private class BehaviourEnabledSetterMock : BehaviourEnabledSetter
        {
            public bool wasSetBehavioursEnabledCalled;

            public override void SetBehavioursEnabled(bool state)
            {
                base.SetBehavioursEnabled(state);
                wasSetBehavioursEnabledCalled = true;
            }
        }
    }
}