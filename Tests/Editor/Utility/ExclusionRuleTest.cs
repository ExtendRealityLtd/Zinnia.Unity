using VRTK.Core.Utility;

namespace Test.VRTK.Core.Utility
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;

    public class ExclusionRuleTest
    {
        private GameObject containingObject;
        private ExclusionRule subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ExclusionRule>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CheckIgnoreTag()
        {
            string tag = GetValidTag();
            subject.operation = ExclusionRule.OperationType.Ignore;
            subject.checkType = ExclusionRule.CheckTypes.Tag;
            subject.identifiers = new List<string>() { tag };

            GameObject includeObject = new GameObject();
            GameObject ignoreObject = new GameObject
            {
                tag = tag
            };

            //The PolicyExclusionList is an Ignore type so anything not in the ignore list shouldn't be excluded so will return false.
            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            //And anything in the ignore list should be excluded so will return true.
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        [Test]
        public void CheckIncludeTag()
        {
            string tag = GetValidTag();
            subject.operation = ExclusionRule.OperationType.Include;
            subject.checkType = ExclusionRule.CheckTypes.Tag;
            subject.identifiers = new List<string>() { tag };

            GameObject ignoreObject = new GameObject();
            GameObject includeObject = new GameObject
            {
                tag = tag
            };

            //The PolicyExclusionList is an Include type so anything in the list shouldn't be excluded so will return false.
            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            //And anything not in the list should be excluded so will return true.
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        [Test]
        public void CheckIgnoreTags()
        {
            string tag1 = GetValidTag(1);
            string tag2 = GetValidTag(2);

            subject.operation = ExclusionRule.OperationType.Ignore;
            subject.checkType = ExclusionRule.CheckTypes.Tag;
            subject.identifiers = new List<string>() { tag1, tag2 };

            GameObject includeObject = new GameObject();
            GameObject ignoreObject1 = new GameObject
            {
                tag = tag1
            };
            GameObject ignoreObject2 = new GameObject
            {
                tag = tag2
            };

            //The PolicyExclusionList is an Ignore type so anything not in the ignore list shouldn't be excluded so will return false.
            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            //And anything in the ignore list should be excluded so will return true.
            Assert.IsTrue(subject.ShouldExclude(ignoreObject1));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject1, subject));
            Assert.IsTrue(subject.ShouldExclude(ignoreObject2));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject2, subject));
        }

        [Test]
        public void CheckIncludeTags()
        {
            string tag1 = GetValidTag(1);
            string tag2 = GetValidTag(2);

            subject.operation = ExclusionRule.OperationType.Include;
            subject.checkType = ExclusionRule.CheckTypes.Tag;
            subject.identifiers = new List<string>() { tag1, tag2 };

            GameObject ignoreObject = new GameObject();
            GameObject includeObject1 = new GameObject
            {
                tag = tag1
            };
            GameObject includeObject2 = new GameObject
            {
                tag = tag2
            };

            //The PolicyExclusionList is an Include type so anything in the list shouldn't be excluded so will return false.
            Assert.IsFalse(subject.ShouldExclude(includeObject1));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject1, subject));
            Assert.IsFalse(subject.ShouldExclude(includeObject2));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject2, subject));
            //And anything not in the list should be excluded so will return true.
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        [Test]
        public void CheckIgnoreScript()
        {

            subject.operation = ExclusionRule.OperationType.Ignore;
            subject.checkType = ExclusionRule.CheckTypes.Script;
            subject.identifiers = new List<string>() { "PolicyExclusionTestScript" };

            GameObject includeObject = new GameObject();
            GameObject ignoreObject = new GameObject();
            ignoreObject.AddComponent<PolicyExclusionTestScript>();

            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        [Test]
        public void CheckIncludeScript()
        {
            subject.operation = ExclusionRule.OperationType.Include;
            subject.checkType = ExclusionRule.CheckTypes.Script;
            subject.identifiers = new List<string>() { "PolicyExclusionTestScript" };

            GameObject includeObject = new GameObject();
            includeObject.AddComponent<PolicyExclusionTestScript>();
            GameObject ignoreObject = new GameObject();

            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        [Test]
        public void CheckIgnoreLayer()
        {
            string layer = GetValidLayer();
            subject.operation = ExclusionRule.OperationType.Ignore;
            subject.checkType = ExclusionRule.CheckTypes.Layer;
            subject.identifiers = new List<string>() { layer };

            GameObject includeObject = new GameObject();
            GameObject ignoreObject = new GameObject
            {
                layer = LayerMask.NameToLayer(layer)
            };

            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        [Test]
        public void CheckIncludeLayer()
        {
            string layer = GetValidLayer();
            subject.operation = ExclusionRule.OperationType.Include;
            subject.checkType = ExclusionRule.CheckTypes.Layer;
            subject.identifiers = new List<string>() { layer };

            GameObject ignoreObject = new GameObject();
            GameObject includeObject = new GameObject
            {
                layer = LayerMask.NameToLayer(layer)
            };

            Assert.IsFalse(subject.ShouldExclude(includeObject));
            Assert.IsFalse(ExclusionRule.ShouldExclude(includeObject, subject));
            Assert.IsTrue(subject.ShouldExclude(ignoreObject));
            Assert.IsTrue(ExclusionRule.ShouldExclude(ignoreObject, subject));
        }

        private string GetValidTag(int index = 1)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags.Length > index)
            {
                return UnityEditorInternal.InternalEditorUtility.tags[index];
            }
            else
            {
                throw new MissingReferenceException("No Unity Tags have been defined.");
            }
        }

        private string GetValidLayer(int index = 1)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers.Length > index)
            {
                return UnityEditorInternal.InternalEditorUtility.layers[index];
            }
            else
            {
                throw new MissingReferenceException("No Unity Layers have been defined.");
            }
        }
    }

    public class PolicyExclusionTestScript : MonoBehaviour
    {
    }
}