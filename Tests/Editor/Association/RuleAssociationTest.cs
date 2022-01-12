using Zinnia.Association;
using Zinnia.Pattern.Collection;
using Zinnia.Rule;

namespace Test.Zinnia.Association
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class RuleAssociationTest
    {
        private GameObject containingObject;
        private RuleAssociation subject;
        private RuleContainer ruleContainer;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<RuleAssociation>();
            ruleContainer = new RuleContainer();
            containingObject.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(containingObject);
        }

        [Test]
        public void ShouldBeActiveTrueFromPatternMatcherRule()
        {
            PatternMatcherObservableList patterns = containingObject.AddComponent<PatternMatcherObservableList>();
            PatternMatcherRule rule = containingObject.AddComponent<PatternMatcherRule>();
            rule.Patterns = patterns;
            ruleContainer.Interface = rule;
            subject.Rule = ruleContainer;

            GameObject patternOneContainer = new GameObject("pattern1");
            PatternMatcherStub patternOne = patternOneContainer.AddComponent<PatternMatcherStub>();
            patternOne.source = "tomatch";
            patternOne.Pattern = "^tomatch$";
            patterns.Add(patternOne);

            GameObject patternTwoContainer = new GameObject("pattern2");
            PatternMatcherStub patternTwo = patternTwoContainer.AddComponent<PatternMatcherStub>();
            patternTwo.source = "tomatch";
            patternTwo.Pattern = "^toma.*$";
            patterns.Add(patternTwo);

            GameObject patternThreeContainer = new GameObject("pattern3");
            PatternMatcherStub patternThree = patternThreeContainer.AddComponent<PatternMatcherStub>();
            patternThree.source = "tomatch";
            patternThree.Pattern = "";
            patterns.Add(patternThree);

            Assert.IsTrue(subject.ShouldBeActive());

            Object.Destroy(patternOneContainer);
            Object.Destroy(patternTwoContainer);
            Object.Destroy(patternThreeContainer);
        }

        [Test]
        public void ShouldBeActiveFalseFromPatternMatcherRule()
        {
            PatternMatcherObservableList patterns = containingObject.AddComponent<PatternMatcherObservableList>();
            PatternMatcherRule rule = containingObject.AddComponent<PatternMatcherRule>();
            rule.Patterns = patterns;
            ruleContainer.Interface = rule;
            subject.Rule = ruleContainer;

            GameObject patternOneContainer = new GameObject("pattern1");
            PatternMatcherStub patternOne = patternOneContainer.AddComponent<PatternMatcherStub>();
            patternOne.source = "tomatch";
            patternOne.Pattern = "^tomatch$";
            patterns.Add(patternOne);

            GameObject patternTwoContainer = new GameObject("pattern2");
            PatternMatcherStub patternTwo = patternTwoContainer.AddComponent<PatternMatcherStub>();
            patternTwo.source = "tomatch";
            patternTwo.Pattern = "wontmatch";
            patterns.Add(patternTwo);

            GameObject patternThreeContainer = new GameObject("pattern3");
            PatternMatcherStub patternThree = patternThreeContainer.AddComponent<PatternMatcherStub>();
            patternThree.source = "tomatch";
            patternThree.Pattern = "";
            patterns.Add(patternThree);

            Assert.IsFalse(subject.ShouldBeActive());

            Object.Destroy(patternOneContainer);
            Object.Destroy(patternTwoContainer);
            Object.Destroy(patternThreeContainer);
        }
    }
}