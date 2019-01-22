using Zinnia.Tracking.Follow;
using Zinnia.Tracking.Follow.Modifier;
using Zinnia.Rule;

namespace Test.Zinnia.Tracking.Follow
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Stub;

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

        [Test]
        public void AddTargetOffset()
        {
            GameObject offset = new GameObject("offset");

            Assert.IsEmpty(subject.targetOffsets);
            subject.AddTargetOffset(offset);
            Assert.AreEqual(1, subject.targetOffsets.Count);
            Assert.AreEqual(offset, subject.targetOffsets[0]);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void RemoveTargetOffset()
        {
            GameObject offset = new GameObject("offset");

            subject.AddTargetOffset(offset);
            Assert.AreEqual(1, subject.targetOffsets.Count);
            Assert.AreEqual(offset, subject.targetOffsets[0]);

            subject.RemoveTargetOffset(offset);
            Assert.IsEmpty(subject.targetOffsets);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void SetTargetOffsetAtCurrentIndex()
        {
            GameObject offset1 = new GameObject("offset1");
            GameObject offset2 = new GameObject("offset2");
            GameObject newOffset1 = new GameObject("new offset1");

            subject.AddTargetOffset(offset1);
            subject.AddTargetOffset(offset2);
            Assert.AreEqual(2, subject.targetOffsets.Count);

            subject.CurrentTargetOffsetsIndex = 0;

            subject.SetTargetOffsetAtCurrentIndex(newOffset1);

            Assert.AreEqual(2, subject.targetOffsets.Count);
            Assert.AreEqual(newOffset1, subject.targetOffsets[0]);
            Assert.AreEqual(offset2, subject.targetOffsets[1]);

            Object.DestroyImmediate(offset1);
            Object.DestroyImmediate(offset2);
            Object.DestroyImmediate(newOffset1);
        }

        [Test]
        public void ClearTargetOffsets()
        {
            GameObject offset = new GameObject("offset");

            subject.AddTargetOffset(offset);
            Assert.AreEqual(1, subject.targetOffsets.Count);
            Assert.AreEqual(offset, subject.targetOffsets[0]);

            subject.ClearTargetOffsets();
            Assert.IsEmpty(subject.targetOffsets);
            Object.DestroyImmediate(offset);
        }

        [Test]
        public void AllTargetsFollowSourceNoOffsets()
        {
            /// The play area alias moves and all SDK play areas follow it.

            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("playAreaAlias");

            GameObject target1 = new GameObject("SDK1PlayArea");
            GameObject target2 = new GameObject("SDK2PlayArea");
            GameObject target3 = new GameObject("SDK3PlayArea");

            source.transform.position = Vector3.one;

            target1.transform.position = Vector3.one * 2f;
            target2.transform.position = Vector3.one * 3f;
            target3.transform.position = Vector3.one * 4f;

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.followModifier = followModifierMock;

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

        [Test]
        public void TargetFollowsFirstActiveSourceNoOffsets()
        {
            /// The first active SDK HMD moves and the target HMD alias follows

            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source1 = new GameObject("SDK1HMD");
            GameObject source2 = new GameObject("SDK2HMD");
            GameObject source3 = new GameObject("SDK3HMD");
            GameObject target = new GameObject("HMDAlias");

            source1.transform.position = Vector3.one;
            source2.transform.position = Vector3.one * 2f;
            source3.transform.position = Vector3.one * 3f;

            target.transform.position = Vector3.zero;

            subject.AddSource(source1);
            subject.AddSource(source2);
            subject.AddSource(source3);
            subject.AddTarget(target);

            subject.ceaseAfterFirstSourceProcessed = true;
            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.sourceValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            source1.SetActive(false);
            source2.SetActive(true);
            source3.SetActive(true);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.followModifier = followModifierMock;

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

        [Test]
        public void OnlActiveTargetsFollowSourceNoOffsets()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("playAreaAlias");

            GameObject target1 = new GameObject("SDK1PlayArea");
            GameObject target2 = new GameObject("SDK2PlayArea");
            GameObject target3 = new GameObject("SDK3PlayArea");

            source.transform.position = Vector3.one;

            target1.transform.position = Vector3.one * 2f;
            target2.transform.position = Vector3.one * 3f;
            target3.transform.position = Vector3.one * 4f;

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);

            subject.gameObject.AddComponent<RuleStub>();
            ActiveInHierarchyRule activeInHierarchyRule = subject.gameObject.AddComponent<ActiveInHierarchyRule>();
            subject.targetValidity = new RuleContainer
            {
                Interface = activeInHierarchyRule
            };

            target1.SetActive(false);
            target2.SetActive(true);
            target3.SetActive(true);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.followModifier = followModifierMock;

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

        [Test]
        public void AllTargetsFollowSourceWithOffsets()
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

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);
            subject.AddTargetOffset(targetOffset1);
            subject.AddTargetOffset(targetOffset2);
            subject.AddTargetOffset(targetOffset3);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.followModifier = followModifierMock;

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

        [Test]
        public void TargetOffsetNotChildException()
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

            subject.AddSource(source);
            subject.AddTarget(target1);
            subject.AddTarget(target2);
            subject.AddTarget(target3);
            subject.AddTargetOffset(targetOffset1);
            subject.AddTargetOffset(targetOffset2);
            subject.AddTargetOffset(targetOffset3);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();
            subject.followModifier = followModifierMock;

            Assert.Throws<System.ArgumentException>(() => subject.Process());

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
