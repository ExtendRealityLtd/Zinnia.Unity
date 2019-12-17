using Zinnia.Rule;
using Zinnia.Extension;
using Zinnia.Data.Collection.List;
using BaseRule = Zinnia.Rule.Rule;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class StringInListRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private StringInListRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            container = new RuleContainer();
            subject = containingObject.AddComponent<StringInListRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AcceptsFound()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = toFind;

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseEmpty()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            subject.InListPattern = toFind;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseNoList()
        {
            string toFind = "found";
            subject.InListPattern = toFind;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseNotFound()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = "different";

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseNotFoundExact()
        {
            string toFind = "founds";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = "$found$";

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptsFoundAny()
        {
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add("one");
            list.Add("two");
            list.Add("three");
            subject.InListPattern = "two";

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptsFoundFuzzy()
        {
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add("one two three");
            list.Add("two three one");
            list.Add("three two one");
            subject.InListPattern = "^three.*$";

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseNotFoundFuzzy()
        {
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add("one two three");
            list.Add("two three one");
            list.Add("three two one");
            subject.InListPattern = "^.*two$";

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseInactiveGameObject()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = toFind;
            subject.gameObject.SetActive(false);

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefuseInactiveComponent()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = toFind;
            subject.enabled = false;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptsInactiveGameObject()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = toFind;
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleComponentIsDisabled;
            subject.gameObject.SetActive(false);

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void AcceptsInactiveComponent()
        {
            string toFind = "found";
            StringObservableList list = containingObject.AddComponent<StringObservableList>();
            list.Add(toFind);
            subject.InListPattern = toFind;
            subject.AutoRejectStates = BaseRule.RejectRuleStates.RuleGameObjectIsNotActiveInHierarchy;
            subject.enabled = false;

            Assert.IsTrue(container.Accepts(containingObject));
        }
    }
}
