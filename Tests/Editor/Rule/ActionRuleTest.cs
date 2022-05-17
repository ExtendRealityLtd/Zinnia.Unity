using Zinnia.Action;
using Zinnia.Rule;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    public class ActionRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private ActionRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<ActionRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ActivatedState()
        {
            BooleanAction action = containingObject.AddComponent<BooleanAction>();
            subject.Action = action;
            Assert.IsFalse(subject.Accepts());
            Assert.IsFalse(subject.Accepts(action));
            Assert.IsFalse(action.IsActivated);
            action.Receive(true);
            Assert.IsTrue(subject.Accepts());
            Assert.IsTrue(subject.Accepts(action));
            Assert.IsTrue(action.IsActivated);
            action.Receive(false);
            Assert.IsFalse(subject.Accepts());
            Assert.IsFalse(subject.Accepts(action));
            Assert.IsFalse(action.IsActivated);
        }

        [Test]
        public void RefusesInactiveGameObject()
        {
            GameObject actionContainer = new GameObject();
            BooleanAction action = containingObject.AddComponent<BooleanAction>();
            subject.Action = action;
            action.Receive(true);
            subject.gameObject.SetActive(false);
            Assert.IsFalse(subject.Accepts());
            Object.DestroyImmediate(actionContainer);
        }

        [Test]
        public void RefusesInactiveComponent()
        {
            GameObject actionContainer = new GameObject();
            BooleanAction action = containingObject.AddComponent<BooleanAction>();
            subject.Action = action;
            action.Receive(true);
            subject.enabled = false;
            Assert.IsFalse(subject.Accepts());
            Object.DestroyImmediate(actionContainer);
        }

        [Test]
        public void AcceptInactiveGameObject()
        {
            GameObject actionContainer = new GameObject();
            BooleanAction action = actionContainer.AddComponent<BooleanAction>();
            subject.Action = action;
            action.Receive(true);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;
            subject.gameObject.SetActive(false);

            Assert.IsTrue(subject.Accepts());

            subject.enabled = false;

            Assert.IsFalse(subject.Accepts());

            Object.DestroyImmediate(actionContainer);
        }

        [Test]
        public void AcceptInactiveComponent()
        {
            GameObject actionContainer = new GameObject();
            BooleanAction action = containingObject.AddComponent<BooleanAction>();
            subject.Action = action;
            action.Receive(true);
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;
            subject.enabled = false;

            Assert.IsTrue(subject.Accepts());

            subject.gameObject.SetActive(false);

            Assert.IsFalse(subject.Accepts());

            Object.DestroyImmediate(actionContainer);
        }
    }
}