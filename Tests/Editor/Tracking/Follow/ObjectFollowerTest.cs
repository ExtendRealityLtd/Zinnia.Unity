using VRTK.Core.Tracking.Follow;
using VRTK.Core.Tracking.Follow.Modifier;

namespace Test.VRTK.Core.Tracking.Follow
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

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
        public void SetFollowOffset()
        {
            GameObject offset = new GameObject("offset");

            Assert.IsNull(subject.followOffset);
            subject.SetFollowOffset(offset);
            Assert.AreEqual(offset, subject.followOffset);

            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ClearFollowOffset()
        {
            GameObject offset = new GameObject("offset");

            Assert.IsNull(subject.followOffset);
            subject.SetFollowOffset(offset);
            Assert.AreEqual(offset, subject.followOffset);
            subject.ClearFollowOffset();
            Assert.IsNull(subject.followOffset);

            Object.DestroyImmediate(offset);
        }

        [Test]
        public void ProcessFirstActiveTargetAllActive()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");
            GameObject[] targets = new GameObject[]
            {
                new GameObject("target1"),
                new GameObject("target2"),
                new GameObject("target3")
            };

            subject.sourceComponent = source.transform;
            subject.targetComponents.Add(targets[0].transform);
            subject.targetComponents.Add(targets[1].transform);
            subject.targetComponents.Add(targets[2].transform);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();

            subject.modificationType = ObjectFollower.ModificationType.ModifySourceUsingTarget;
            subject.processTarget = ObjectFollower.ProcessTarget.FirstActive;
            subject.followModifier = followModifierMock;

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);
            Assert.AreEqual(targets[0].transform, source.transform);
            Assert.AreEqual(targets[1].transform, targets[1].transform);
            Assert.AreEqual(targets[2].transform, targets[2].transform);
            Assert.AreEqual(source, followModifierMock.finalTarget);

            Object.DestroyImmediate(source);
            foreach (GameObject target in targets)
            {
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void ProcessFirstActiveTargetOnlyLastActive()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");
            GameObject[] targets = new GameObject[]
            {
                new GameObject("target1"),
                new GameObject("target2"),
                new GameObject("target3")
            };

            subject.sourceComponent = source.transform;
            subject.targetComponents.Add(targets[0].transform);
            subject.targetComponents.Add(targets[1].transform);
            subject.targetComponents.Add(targets[2].transform);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();

            subject.modificationType = ObjectFollower.ModificationType.ModifySourceUsingTarget;
            subject.processTarget = ObjectFollower.ProcessTarget.FirstActive;
            subject.followModifier = followModifierMock;

            targets[0].SetActive(false);
            targets[1].SetActive(false);

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);
            Assert.AreEqual(targets[0].transform, targets[0].transform);
            Assert.AreEqual(targets[1].transform, targets[1].transform);
            Assert.AreEqual(targets[2].transform, source.transform);
            Assert.AreEqual(source, followModifierMock.finalTarget);

            Object.DestroyImmediate(source);
            foreach (GameObject target in targets)
            {
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void ProcessAllTargets()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");
            GameObject[] targets = new GameObject[]
            {
                new GameObject("target1"),
                new GameObject("target2"),
                new GameObject("target3")
            };

            subject.sourceComponent = source.transform;
            subject.targetComponents.Add(targets[0].transform);
            subject.targetComponents.Add(targets[1].transform);
            subject.targetComponents.Add(targets[2].transform);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();

            subject.modificationType = ObjectFollower.ModificationType.ModifyTargetUsingSource;
            subject.processTarget = ObjectFollower.ProcessTarget.All;
            subject.followModifier = followModifierMock;

            subject.Process();

            Assert.IsTrue(preprocessedMock.Received);
            Assert.IsTrue(processedMock.Received);
            Assert.AreEqual(source.transform, targets[0].transform);
            Assert.AreEqual(source.transform, targets[1].transform);
            Assert.AreEqual(source.transform, targets[2].transform);
            Assert.AreEqual(source, followModifierMock.finalSource);

            Object.DestroyImmediate(source);
            foreach (GameObject target in targets)
            {
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void ProcessNoFollowModifier()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");
            GameObject[] targets = new GameObject[]
            {
                new GameObject("target1"),
                new GameObject("target2"),
                new GameObject("target3")
            };

            subject.sourceComponent = source.transform;
            subject.targetComponents.Add(targets[0].transform);
            subject.targetComponents.Add(targets[1].transform);
            subject.targetComponents.Add(targets[2].transform);

            subject.modificationType = ObjectFollower.ModificationType.ModifyTargetUsingSource;
            subject.processTarget = ObjectFollower.ProcessTarget.All;

            subject.Process();

            Assert.IsFalse(preprocessedMock.Received);
            Assert.IsFalse(processedMock.Received);
            Assert.AreEqual(targets[0].transform, targets[0].transform);
            Assert.AreEqual(targets[2].transform, targets[1].transform);
            Assert.AreEqual(targets[2].transform, targets[2].transform);

            Object.DestroyImmediate(source);
            foreach (GameObject target in targets)
            {
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");
            GameObject[] targets = new GameObject[]
            {
                new GameObject("target1"),
                new GameObject("target2"),
                new GameObject("target3")
            };

            subject.sourceComponent = source.transform;
            subject.targetComponents.Add(targets[0].transform);
            subject.targetComponents.Add(targets[1].transform);
            subject.targetComponents.Add(targets[2].transform);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();

            subject.modificationType = ObjectFollower.ModificationType.ModifyTargetUsingSource;
            subject.processTarget = ObjectFollower.ProcessTarget.All;
            subject.followModifier = followModifierMock;

            subject.gameObject.SetActive(false);
            subject.Process();

            Assert.IsFalse(preprocessedMock.Received);
            Assert.IsFalse(processedMock.Received);
            Assert.AreEqual(targets[0].transform, targets[0].transform);
            Assert.AreEqual(targets[2].transform, targets[1].transform);
            Assert.AreEqual(targets[2].transform, targets[2].transform);
            Assert.IsNull(followModifierMock.finalSource);
            Assert.IsNull(followModifierMock.finalTarget);

            Object.DestroyImmediate(source);
            foreach (GameObject target in targets)
            {
                Object.DestroyImmediate(target);
            }
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            UnityEventListenerMock preprocessedMock = new UnityEventListenerMock();
            UnityEventListenerMock processedMock = new UnityEventListenerMock();
            subject.Preprocessed.AddListener(preprocessedMock.Listen);
            subject.Processed.AddListener(processedMock.Listen);

            GameObject source = new GameObject("source");
            GameObject[] targets = new GameObject[]
            {
                new GameObject("target1"),
                new GameObject("target2"),
                new GameObject("target3")
            };

            subject.sourceComponent = source.transform;
            subject.targetComponents.Add(targets[0].transform);
            subject.targetComponents.Add(targets[1].transform);
            subject.targetComponents.Add(targets[2].transform);

            FollowModifierMock followModifierMock = containingObject.AddComponent<FollowModifierMock>();

            subject.modificationType = ObjectFollower.ModificationType.ModifyTargetUsingSource;
            subject.processTarget = ObjectFollower.ProcessTarget.All;
            subject.followModifier = followModifierMock;

            subject.enabled = false;
            subject.Process();

            Assert.IsFalse(preprocessedMock.Received);
            Assert.IsFalse(processedMock.Received);
            Assert.AreEqual(targets[0].transform, targets[0].transform);
            Assert.AreEqual(targets[2].transform, targets[1].transform);
            Assert.AreEqual(targets[2].transform, targets[2].transform);
            Assert.IsNull(followModifierMock.finalSource);
            Assert.IsNull(followModifierMock.finalTarget);

            Object.DestroyImmediate(source);
            foreach (GameObject target in targets)
            {
                Object.DestroyImmediate(target);
            }
        }

        public class FollowModifierMock : FollowModifier
        {
            public GameObject finalSource;
            public GameObject finalTarget;

            public override void Modify(GameObject source, GameObject target, GameObject offset = null)
            {
                finalSource = source;
                finalTarget = target;
            }

            public virtual void ResetMock()
            {
                finalSource = null;
                finalTarget = null;
            }
        }
    }
}
