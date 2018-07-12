using VRTK.Core.Extension;
using VRTK.Core.Rule;
using VRTK.Core.Utility;

namespace Test.VRTK.Core.Rule
{
    using UnityEngine;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class AnyTagRuleTest
    {
        private const string validTag = "TestTag";
        private const string invalidTag = "WrongTestTag";
        private static readonly string[] tags =
        {
            validTag,
            invalidTag
        };
        private readonly List<string> tagsToRemove = new List<string>();

        private GameObject containingObject;
        private RuleContainer container;
        private AnyTagRule subject;

        [OneTimeSetUp]
        public void SetUpTags()
        {
            tagsToRemove.AddRange(EditorHelper.AddTags(tags));
        }

        [OneTimeTearDown]
        public void TearDownTags()
        {
            EditorHelper.RemoveTags(tagsToRemove.ToArray());
            tagsToRemove.Clear();
        }

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject
            {
                tag = validTag
            };
            container = new RuleContainer();
            subject = containingObject.AddComponent<AnyTagRule>();
            container.Interface = subject;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AcceptsMatch()
        {
            subject.tags.Add(validTag);
            Assert.IsTrue(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesEmpty()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesDifferent()
        {
            subject.tags.Add(invalidTag);
            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}