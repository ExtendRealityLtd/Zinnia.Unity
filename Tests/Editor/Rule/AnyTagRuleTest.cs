using Zinnia.Rule;
using Zinnia.Utility;
using Zinnia.Extension;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Rule
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

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

        [UnityTest]
        public IEnumerator AcceptsMatch()
        {
            StringObservableList tags = containingObject.AddComponent<StringObservableList>();
            yield return null;
            subject.Tags = tags;
            tags.Add(validTag);

            Assert.IsTrue(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesEmpty()
        {
            StringObservableList tags = containingObject.AddComponent<StringObservableList>();
            yield return null;
            subject.Tags = tags;

            Assert.IsFalse(container.Accepts(containingObject));
        }

        [Test]
        public void RefusesNullTags()
        {
            Assert.IsFalse(container.Accepts(containingObject));
        }

        [UnityTest]
        public IEnumerator RefusesDifferent()
        {
            StringObservableList tags = containingObject.AddComponent<StringObservableList>();
            yield return null;
            subject.Tags = tags;
            tags.Add(invalidTag);

            Assert.IsFalse(container.Accepts(containingObject));
        }
    }
}