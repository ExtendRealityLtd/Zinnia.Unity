using Zinnia.Rule;
using Zinnia.Data.Collection.List;
using Zinnia.Tracking.Follow;
using Zinnia.Tracking.Follow.Modifier;

namespace Test.Zinnia.Tracking.Follow
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;
    using Assert = UnityEngine.Assertions.Assert;

    public class ObjectFollowerTest
    {
        private GameObject containingObject;
        private ObjectFollower subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ObjectFollower>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator AddTargetOffset()
        {
            GameObject offset = new GameObject("offset");
            subject.TargetOffsets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            Assert.AreEqual(0, subject.TargetOffsets.NonSubscribableElements.Count);
            subject.TargetOffsets.Add(offset);
            Assert.AreEqual(1, subject.TargetOffsets.NonSubscribableElements.Count);
            Assert.AreEqual(offset, subject.TargetOffsets.NonSubscribableElements[0]);
            Object.DestroyImmediate(offset);
        }

        [UnityTest]
        public IEnumerator RemoveTargetOffset()
        {
            GameObject offset = new GameObject("offset");
            subject.TargetOffsets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.TargetOffsets.Add(offset);
            Assert.AreEqual(1, subject.TargetOffsets.NonSubscribableElements.Count);
            Assert.AreEqual(offset, subject.TargetOffsets.NonSubscribableElements[0]);

            subject.TargetOffsets.Remove(offset);
            Assert.AreEqual(0, subject.TargetOffsets.NonSubscribableElements.Count);
            Object.DestroyImmediate(offset);
        }

        [UnityTest]
        public IEnumerator SetTargetOffsetAtCurrentIndex()
        {
            GameObject offset1 = new GameObject("offset1");
            GameObject offset2 = new GameObject("offset2");
            GameObject newOffset1 = new GameObject("new offset1");

            subject.TargetOffsets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.TargetOffsets.Add(offset1);
            subject.TargetOffsets.Add(offset2);
            Assert.AreEqual(2, subject.TargetOffsets.NonSubscribableElements.Count);

            subject.TargetOffsets.CurrentIndex = 0;

            subject.TargetOffsets.SetAtCurrentIndex(newOffset1);

            Assert.AreEqual(2, subject.TargetOffsets.NonSubscribableElements.Count);
            Assert.AreEqual(newOffset1, subject.TargetOffsets.NonSubscribableElements[0]);
            Assert.AreEqual(offset2, subject.TargetOffsets.NonSubscribableElements[1]);

            Object.DestroyImmediate(offset1);
            Object.DestroyImmediate(offset2);
            Object.DestroyImmediate(newOffset1);
        }

        [UnityTest]
        public IEnumerator ClearTargetOffsets()
        {
            GameObject offset = new GameObject("offset");

            subject.TargetOffsets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.TargetOffsets.Add(offset);
            Assert.AreEqual(1, subject.TargetOffsets.NonSubscribableElements.Count);
            Assert.AreEqual(offset, subject.TargetOffsets.NonSubscribableElements[0]);

            subject.TargetOffsets.Clear();
            Assert.AreEqual(0, subject.TargetOffsets.NonSubscribableElements.Count);
            Object.DestroyImmediate(offset);
        }

        [UnityTest]
        public IEnumerator AllTargetsFollowSourceNoOffsets()
        {
            // The play area alias moves and all SDK play areas follow it.

            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("playAreaAlias");

            GameObject target1 = new GameObject("SDK1PlayArea");
            GameObject target2 = new GameObject("SDK2PlayArea");
            GameObject target3 = new GameObject("SDK3PlayArea");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            source.transform.position = Vector3.one;

            target1.transform.position = Vector3.one * 2f;
            target2.transform.position = Vector3.one * 3f;
            target3.transform.position = Vector3.one * 4f;

            subject.Sources.Add(source);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.FollowModifier = followModifierMock;

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);
            Assert.AreEqual(source.transform.position, target1.transform.position);
            Assert.AreEqual(source.transform.position, target2.transform.position);
            Assert.AreEqual(source.transform.position, target3.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
        }

        [UnityTest]
        public IEnumerator TargetFollowsFirstActiveSourceNoOffsets()
        {
            // The first active SDK HMD moves and the target HMD alias follows

            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source1 = new GameObject("SDK1HMD");
            GameObject source2 = new GameObject("SDK2HMD");
            GameObject source3 = new GameObject("SDK3HMD");
            GameObject target = new GameObject("HMDAlias");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            source1.transform.position = Vector3.one;
            source2.transform.position = Vector3.one * 2f;
            source3.transform.position = Vector3.one * 3f;

            target.transform.position = Vector3.zero;

            subject.Sources.Add(source1);
            subject.Sources.Add(source2);
            subject.Sources.Add(source3);
            subject.Targets.Add(target);

            subject.CeaseAfterFirstSourceProcessed = true;
            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.SourceValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            source1.SetActive(false);
            source2.SetActive(true);
            source3.SetActive(true);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.FollowModifier = followModifierMock;

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);

            Assert.AreNotEqual(source1.transform.position, target.transform.position);
            Assert.AreEqual(source2.transform.position, target.transform.position);
            Assert.AreNotEqual(source3.transform.position, target.transform.position);

            Object.DestroyImmediate(source1);
            Object.DestroyImmediate(source2);
            Object.DestroyImmediate(source3);
            Object.DestroyImmediate(target);
        }

        [UnityTest]
        public IEnumerator OnlActiveTargetsFollowSourceNoOffsets()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("playAreaAlias");

            GameObject target1 = new GameObject("SDK1PlayArea");
            GameObject target2 = new GameObject("SDK2PlayArea");
            GameObject target3 = new GameObject("SDK3PlayArea");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            source.transform.position = Vector3.one;

            target1.transform.position = Vector3.one * 2f;
            target2.transform.position = Vector3.one * 3f;
            target3.transform.position = Vector3.one * 4f;

            subject.Sources.Add(source);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.TargetValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            target1.SetActive(false);
            target2.SetActive(true);
            target3.SetActive(true);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.FollowModifier = followModifierMock;

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);
            Assert.AreNotEqual(source.transform.position, target1.transform.position);
            Assert.AreEqual(source.transform.position, target2.transform.position);
            Assert.AreEqual(source.transform.position, target3.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
        }

        [UnityTest]
        public IEnumerator AllTargetsFollowSourceWithOffsets()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");

            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");

            GameObject targetOffset1 = new GameObject("targetOffset1");
            GameObject targetOffset2 = new GameObject("targetOffset2");
            GameObject targetOffset3 = new GameObject("targetOffset3");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.TargetOffsets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            targetOffset1.transform.SetParent(target1.transform);
            targetOffset2.transform.SetParent(target2.transform);
            targetOffset3.transform.SetParent(target3.transform);

            source.transform.position = Vector3.one * 10f;

            target1.transform.position = Vector3.one * 2f;
            target2.transform.position = Vector3.one * 3f;
            target3.transform.position = Vector3.one * 4f;

            targetOffset1.transform.localPosition = Vector3.one;
            targetOffset2.transform.localPosition = Vector3.one * 2f;
            targetOffset3.transform.localPosition = Vector3.one * 3f;

            subject.Sources.Add(source);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            subject.TargetOffsets.Add(targetOffset1);
            subject.TargetOffsets.Add(targetOffset2);
            subject.TargetOffsets.Add(targetOffset3);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.FollowModifier = followModifierMock;

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);

            Assert.AreEqual(source.transform.position - targetOffset1.transform.localPosition, target1.transform.position);
            Assert.AreEqual(source.transform.position - targetOffset2.transform.localPosition, target2.transform.position);
            Assert.AreEqual(source.transform.position - targetOffset3.transform.localPosition, target3.transform.position);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
            Object.DestroyImmediate(targetOffset1);
            Object.DestroyImmediate(targetOffset2);
            Object.DestroyImmediate(targetOffset3);
        }

        [UnityTest]
        public IEnumerator TargetOffsetNotChildException()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");

            GameObject target1 = new GameObject("target1");
            GameObject target2 = new GameObject("target2");
            GameObject target3 = new GameObject("target3");

            GameObject targetOffset1 = new GameObject("targetOffset1");
            GameObject targetOffset2 = new GameObject("targetOffset2");
            GameObject targetOffset3 = new GameObject("targetOffset3");

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();
            subject.TargetOffsets = containingObject.AddComponent<GameObjectObservableList>();
            yield return null;

            subject.Sources.Add(source);
            subject.Targets.Add(target1);
            subject.Targets.Add(target2);
            subject.Targets.Add(target3);

            subject.TargetOffsets.Add(targetOffset1);
            subject.TargetOffsets.Add(targetOffset2);
            subject.TargetOffsets.Add(targetOffset3);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.FollowModifier = followModifierMock;

            NUnit.Framework.Assert.Throws<System.ArgumentException>(() => subject.Process());

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsFalse(processedMock.Received);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target1);
            Object.DestroyImmediate(target2);
            Object.DestroyImmediate(target3);
            Object.DestroyImmediate(targetOffset1);
            Object.DestroyImmediate(targetOffset2);
            Object.DestroyImmediate(targetOffset3);
            }

        public class FollowModifierMock : FollowModifier
        {
            public override void Modify(GameObject source, GameObject target, GameObject offset = null)
            {
                target.transform.position = source.transform.position - (offset != null ? offset.transform.localPosition : Vector3.zero);
            }
        }
    }
}
