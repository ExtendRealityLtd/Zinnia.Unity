using Zinnia.Rule;
using Zinnia.Data.Collection;
using Zinnia.Process.Component;

namespace Test.Zinnia.Process.Component
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Stub;

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

        [Test]
        public void AddSource()
        {
            GameObject source = new GameObject("source");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();

            Assert.IsEmpty(subject.Sources.ReadOnlyElements);

            subject.Sources.Add(source);

            Assert.AreEqual(1, subject.Sources.ReadOnlyElements.Count);
            Assert.AreEqual(source, subject.Sources.ReadOnlyElements[0]);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void RemoveSource()
        {
            GameObject source = new GameObject("source");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();

            subject.Sources.Add(source);

            Assert.AreEqual(1, subject.Sources.ReadOnlyElements.Count);
            Assert.AreEqual(source, subject.Sources.ReadOnlyElements[0]);

            subject.Sources.Remove(source);

            Assert.IsEmpty(subject.Sources.ReadOnlyElements);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void SetSourceAtCurrentIndex()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject newSource1 = new GameObject("new source1");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();

            subject.Sources.Add(source1);
            subject.Sources.Add(source2);

            Assert.AreEqual(2, subject.Sources.ReadOnlyElements.Count);

            subject.Sources.CurrentIndex = 0;

            subject.Sources.SetAtCurrentIndex(newSource1);

            Assert.AreEqual(2, subject.Sources.ReadOnlyElements.Count);
            Assert.AreEqual(newSource1, subject.Sources.ReadOnlyElements[0]);
            Assert.AreEqual(source2, subject.Sources.ReadOnlyElements[1]);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(source2);
            Object.DestroyImmediate(newSource1);
        }

        [Test]
        public void ClearSources()
        {
            GameObject source = new GameObject("source");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();

            subject.Sources.Add(source);

            Assert.AreEqual(1, subject.Sources.ReadOnlyElements.Count);
            Assert.AreEqual(source, subject.Sources.ReadOnlyElements[0]);

            subject.Sources.Clear(false);

            Assert.IsEmpty(subject.Sources.ReadOnlyElements);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void AddTarget()
        {
            GameObject target = new GameObject("target");

            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

            Assert.IsEmpty(subject.Targets.ReadOnlyElements);

            subject.Targets.Add(target);

            Assert.AreEqual(1, subject.Targets.ReadOnlyElements.Count);
            Assert.AreEqual(target, subject.Targets.ReadOnlyElements[0]);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void RemoveTarget()
        {
            GameObject target = new GameObject("target");
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

            subject.Targets.Add(target);

            Assert.AreEqual(1, subject.Targets.ReadOnlyElements.Count);
            Assert.AreEqual(target, subject.Targets.ReadOnlyElements[0]);

            subject.Targets.Remove(target);

            Assert.IsEmpty(subject.Targets.ReadOnlyElements);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SetTargetAtCurrentIndex()
        {
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject newTarget1 = new GameObject("new target1");
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

            subject.Targets.Add(target1);
            subject.Targets.Add(target2);

            Assert.AreEqual(2, subject.Targets.ReadOnlyElements.Count);

            subject.Targets.CurrentIndex = 0;

            subject.Targets.SetAtCurrentIndex(newTarget1);

            Assert.AreEqual(2, subject.Targets.ReadOnlyElements.Count);
            Assert.AreEqual(newTarget1, subject.Targets.ReadOnlyElements[0]);
            Assert.AreEqual(target2, subject.Targets.ReadOnlyElements[1]);

            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(newTarget1);
        }

        [Test]
        public void ClearTargets()
        {
            GameObject target = new GameObject("target");
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

            subject.Targets.Add(target);

            Assert.AreEqual(1, subject.Targets.ReadOnlyElements.Count);
            Assert.AreEqual(target, subject.Targets.ReadOnlyElements[0]);

            subject.Targets.Clear(false);

            Assert.IsEmpty(subject.Targets.ReadOnlyElements);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void ProcessAllTargetsAgainstSource()
        {
            GameObject source1 = new GameObject("source1");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

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

        [Test]
        public void ProcessFirstActiveSourceAgainstTargetThenCease()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject source3 = new GameObject("source3");
            GameObject target1 = new GameObject("target1");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

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

        [Test]
        public void ProcessFirstActiveSourceAgainstTargetThenContinue()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject source3 = new GameObject("source3");
            GameObject target1 = new GameObject("target1");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

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

        [Test]
        public void ProcessFirstActiveSourceAgainstValidTargetThenCease()
        {
            GameObject source1 = new GameObject("source1");
            GameObject source2 = new GameObject("source2");
            GameObject source3 = new GameObject("source3");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

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

        [Test]
        public void ProcessInactiveGameObject()
        {
            GameObject source1 = new GameObject("source1");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

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

        [Test]
        public void ProcessInactiveComponent()
        {
            GameObject source1 = new GameObject("source1");
            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");
            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

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