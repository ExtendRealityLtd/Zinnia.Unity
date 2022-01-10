using Zinnia.Extension;
using Zinnia.Pattern;
using Zinnia.Pattern.Collection;
using Zinnia.Rule;

namespace Test.Zinnia.Rule
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class PatternMatcherRuleTest
    {
        private GameObject containingObject;
        private RuleContainer container;
        private PatternMatcherRule subject;
        private PatternMatcherObservableList patternMatcherList;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            container = new RuleContainer();
            subject = containingObject.AddComponent<PatternMatcherRule>();
            container.Interface = subject;

            patternMatcherList = containingObject.AddComponent<PatternMatcherObservableList>();
            subject.Patterns = patternMatcherList;
            containingObject.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (PatternMatcher associatedPattern in subject.Patterns.NonSubscribableElements)
            {
                Object.Destroy(associatedPattern.gameObject);
            }

            Object.Destroy(containingObject);
        }

        [Test]
        public void AcceptsTrue()
        {
            GameObject patternOneContainer = new GameObject("pattern1");
            PatternMatcherStub patternOne = patternOneContainer.AddComponent<PatternMatcherStub>();
            patternOne.source = "tomatch";
            patternOne.Pattern = "^tomatch$";
            patternOne.MockEnable();
            patternMatcherList.Add(patternOne);

            Assert.AreEqual("tomatch", patternOne.SourceValue);

            GameObject patternTwoContainer = new GameObject("pattern2");
            PatternMatcherStub patternTwo = patternTwoContainer.AddComponent<PatternMatcherStub>();
            patternTwo.source = "tomatch";
            patternTwo.Pattern = "^toma.*$";
            patternTwo.MockEnable();
            patternMatcherList.Add(patternTwo);

            Assert.AreEqual("tomatch", patternTwo.SourceValue);

            GameObject patternThreeContainer = new GameObject("pattern3");
            PatternMatcherStub patternThree = patternThreeContainer.AddComponent<PatternMatcherStub>();
            patternThree.source = "tomatch";
            patternThree.Pattern = "";
            patternThree.MockEnable();
            patternMatcherList.Add(patternThree);

            Assert.AreEqual("tomatch", patternThree.SourceValue);

            Assert.IsTrue(container.Accepts(null));

            Assert.AreEqual("tomatch", patternOne.SourceValue);
            Assert.AreEqual("tomatch", patternTwo.SourceValue);
            Assert.AreEqual("tomatch", patternThree.SourceValue);
        }

        [Test]
        public void AcceptsFalse()
        {
            GameObject patternOneContainer = new GameObject("pattern1");
            PatternMatcherStub patternOne = patternOneContainer.AddComponent<PatternMatcherStub>();
            patternOne.source = "tomatch";
            patternOne.Pattern = "^tomatch$";
            patternOne.MockEnable();
            patternMatcherList.Add(patternOne);

            Assert.AreEqual("tomatch", patternOne.SourceValue);

            GameObject patternTwoContainer = new GameObject("pattern2");
            PatternMatcherStub patternTwo = patternTwoContainer.AddComponent<PatternMatcherStub>();
            patternTwo.source = "tomatch";
            patternTwo.Pattern = "wontmatch";
            patternTwo.MockEnable();
            patternMatcherList.Add(patternTwo);

            Assert.AreEqual("tomatch", patternTwo.SourceValue);

            GameObject patternThreeContainer = new GameObject("pattern3");
            PatternMatcherStub patternThree = patternThreeContainer.AddComponent<PatternMatcherStub>();
            patternThree.source = "tomatch";
            patternThree.Pattern = "";
            patternThree.MockEnable();
            patternMatcherList.Add(patternThree);

            Assert.AreEqual("tomatch", patternThree.SourceValue);

            Assert.IsFalse(container.Accepts(null));

            Assert.AreEqual("tomatch", patternOne.SourceValue);
            Assert.AreEqual("tomatch", patternTwo.SourceValue);
            Assert.AreEqual("tomatch", patternThree.SourceValue);
        }
    }
}