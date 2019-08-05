using Zinnia.Rule;
using Zinnia.Data.Collection.List;
using Zinnia.Process.Component;

namespace Test.Zinnia.Process.Component
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectSourceTargetProcessorTest
    {
        private GameObject containingObject;
        private GameObjectSourceTargetProcessorMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectSourceTargetProcessorMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator AddSource()
        {
            GameObject source = new GameObject("source");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            Assert.AreEqual(0, subject.Sources.NonSubscribableElements.Count);

            subject.Sources.Add(source);

            Assert.AreEqual(1, subject.Sources.NonSubscribableElements.Count);
            Assert.AreEqual(source, subject.Sources.NonSubscribableElements[0]);

            Object.DestroyImmediate(source);
        }

        [UnityTest]
        public IEnumerator RemoveSource()
        {
            GameObject source = new GameObject("source");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source);

            Assert.AreEqual(1, subject.Sources.NonSubscribableElements.Count);
            Assert.AreEqual(source, subject.Sources.NonSubscribableElements[0]);

            subject.Sources.Remove(source);

            Assert.AreEqual(0, subject.Sources.NonSubscribableElements.Count);

            Object.DestroyImmediate(source);
        }

        [UnityTest]
        public IEnumerator SetSourceAtCurrentIndex()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject newSource1 = new GameObject("new source1");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source1);
            subject.Sources.Add(source2);

            Assert.AreEqual(2, subject.Sources.NonSubscribableElements.Count);

            subject.Sources.CurrentIndex = 0;

            subject.Sources.SetAtCurrentIndex(newSource1);

            Assert.AreEqual(2, subject.Sources.NonSubscribableElements.Count);
            Assert.AreEqual(newSource1, subject.Sources.NonSubscribableElements[0]);
            Assert.AreEqual(source2, subject.Sources.NonSubscribableElements[1]);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(source2);
            Object.DestroyImmediate(newSource1);
        }

        [UnityTest]
        public IEnumerator ClearSources()
        {
            GameObject source = new GameObject("source");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source);

            Assert.AreEqual(1, subject.Sources.NonSubscribableElements.Count);
            Assert.AreEqual(source, subject.Sources.NonSubscribableElements[0]);

            subject.Sources.Clear();

            Assert.AreEqual(0, subject.Sources.NonSubscribableElements.Count);

            Object.DestroyImmediate(source);
        }

        [UnityTest]
        public IEnumerator AddTarget()
        {
            GameObject target = new GameObject("target");

            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            Assert.AreEqual(0, subject.Targets.NonSubscribableElements.Count);

            subject.Targets.Add(target);

            Assert.AreEqual(1, subject.Targets.NonSubscribableElements.Count);
            Assert.AreEqual(target, subject.Targets.NonSubscribableElements[0]);

            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator RemoveTarget()
        {
            GameObject target = new GameObject("target");
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Targets.Add(target);

            Assert.AreEqual(1, subject.Targets.NonSubscribableElements.Count);
            Assert.AreEqual(target, subject.Targets.NonSubscribableElements[0]);

            subject.Targets.Remove(target);

            Assert.AreEqual(0, subject.Targets.NonSubscribableElements.Count);

            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator SetTargetAtCurrentIndex()
        {
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject newTarget1 = new GameObject("new target1");
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Targets.Add(target1);
            subject.Targets.Add(target2);

            Assert.AreEqual(2, subject.Targets.NonSubscribableElements.Count);

            subject.Targets.CurrentIndex = 0;

            subject.Targets.SetAtCurrentIndex(newTarget1);

            Assert.AreEqual(2, subject.Targets.NonSubscribableElements.Count);
            Assert.AreEqual(newTarget1, subject.Targets.NonSubscribableElements[0]);
            Assert.AreEqual(target2, subject.Targets.NonSubscribableElements[1]);

            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(newTarget1);
        }

        [UnityTest]
        public IEnumerator ClearTargets()
        {
            GameObject target = new GameObject("target");
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Targets.Add(target);

            Assert.AreEqual(1, subject.Targets.NonSubscribableElements.Count);
            Assert.AreEqual(target, subject.Targets.NonSubscribableElements[0]);

            subject.Targets.Clear();

            Assert.AreEqual(0, subject.Targets.NonSubscribableElements.Count);

            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator ProcessAllTargetsAgainstSource()
        {
            GameObject source1 = new GameObject("source1");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source1);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("target2", target2.name);
            Assert.AreEqual("target3", target3.name);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source1", target1.name);
            Assert.AreEqual("source1", target2.name);
            Assert.AreEqual("source1", target3.name);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
        }

        [UnityTest]
        public IEnumerator ProcessFirstActiveSourceAgainstTargetThenCease()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject source3 = new GameObject("source3");
            GameObject target1 = new GameObject("target1");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.CeaseAfterFirstSourceProcessed = true;

            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.SourceValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            subject.Sources.Add(source1);
            subject.Sources.Add(source2);
            subject.Sources.Add(source3);
            subject.Targets.Add(target1);

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("target1", target1.name);

            source1.SetActive(false);
            source2.SetActive(true);
            source3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source2", target1.name);

            source1.SetActive(false);
            source2.SetActive(false);
            source3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source3", target1.name);

            source1.SetActive(true);
            source2.SetActive(true);
            source3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source1", target1.name);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(source2);
            Object.DestroyImmediate(source3);
            Object.DestroyImmediate(target1);
        }

        [UnityTest]
        public IEnumerator ProcessFirstActiveSourceAgainstTargetThenContinue()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject source3 = new GameObject("source3");
            GameObject target1 = new GameObject("target1");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.CeaseAfterFirstSourceProcessed = false;

            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.SourceValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            subject.Sources.Add(source1);
            subject.Sources.Add(source2);
            subject.Sources.Add(source3);
            subject.Targets.Add(target1);

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("target1", target1.name);

            source1.SetActive(false);
            source2.SetActive(true);
            source3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source3", target1.name);

            source1.SetActive(false);
            source2.SetActive(false);
            source3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source3", target1.name);

            source1.SetActive(true);
            source2.SetActive(true);
            source3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source3", target1.name);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(source2);
            Object.DestroyImmediate(source3);
            Object.DestroyImmediate(target1);
        }

        [UnityTest]
        public IEnumerator ProcessFirstActiveSourceAgainstValidTargetThenCease()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject source3 = new GameObject("source3");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.CeaseAfterFirstSourceProcessed = true;

            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.SourceValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };
            subject.TargetValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            subject.Sources.Add(source1);
            subject.Sources.Add(source2);
            subject.Sources.Add(source3);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("target2", target2.name);
            Assert.AreEqual("target3", target3.name);

            source1.SetActive(false);
            source2.SetActive(true);
            source3.SetActive(true);

            target1.SetActive(false);
            target2.SetActive(true);
            target3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("source2", target2.name);
            Assert.AreEqual("source2", target3.name);

            source1.SetActive(false);
            source2.SetActive(false);
            source3.SetActive(true);

            target1.SetActive(false);
            target2.SetActive(true);
            target3.SetActive(false);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("source3", target2.name);
            Assert.AreEqual("source2", target3.name);

            source1.SetActive(true);
            source2.SetActive(true);
            source3.SetActive(true);

            target1.SetActive(true);
            target2.SetActive(true);
            target3.SetActive(true);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("source2", source2.name);
            Assert.AreEqual("source3", source3.name);
            Assert.AreEqual("source1", target1.name);
            Assert.AreEqual("source1", target2.name);
            Assert.AreEqual("source1", target3.name);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(source2);
            Object.DestroyImmediate(source3);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
        }

        [UnityTest]
        public IEnumerator ProcessInactiveGameObject()
        {
            GameObject source1 = new GameObject("source1");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source1);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("target2", target2.name);
            Assert.AreEqual("target3", target3.name);

            subject.gameObject.SetActive(false);

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("target2", target2.name);
            Assert.AreEqual("target3", target3.name);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
        }

        [UnityTest]
        public IEnumerator ProcessInactiveComponent()
        {
            GameObject source1 = new GameObject("source1");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source1);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("target2", target2.name);
            Assert.AreEqual("target3", target3.name);

            subject.enabled = false;

            subject.Process();

            Assert.AreEqual("source1", source1.name);
            Assert.AreEqual("target1", target1.name);
            Assert.AreEqual("target2", target2.name);
            Assert.AreEqual("target3", target3.name);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
        }
    }

    public class GameObjectSourceTargetProcessorMock : GameObjectSourceTargetProcessor
    {
        protected override void ApplySourceToTarget(GameObject source, GameObject target)
        {
            target.name = source.name;
        }
    }
}