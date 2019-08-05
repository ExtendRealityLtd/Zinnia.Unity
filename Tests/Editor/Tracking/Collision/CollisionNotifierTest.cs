using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class CollisionNotifierTest
    {
        private GameObject containingObject;
        private CollisionNotifierMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<CollisionNotifierMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionStarted()
        {
            GameObject linkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier linkedNotifier = linkedContainer.AddComponent<CollisionNotifier>();

            GameObject unlinkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier unlinkedNotifier = unlinkedContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock collisionStartedListenerMock = new UnityEventListenerMock();
            subject.CollisionStarted.AddListener(collisionStartedListenerMock.Listen);

            UnityEventListenerMock linkedCollisionStartedListenerMock = new UnityEventListenerMock();
            linkedNotifier.CollisionStarted.AddListener(linkedCollisionStartedListenerMock.Listen);

            UnityEventListenerMock unlinkedCollisionStartedListenerMock = new UnityEventListenerMock();
            unlinkedNotifier.CollisionStarted.AddListener(unlinkedCollisionStartedListenerMock.Listen);

            subject.CollisionStartedMock(linkedContainer.GetComponent<Collider>());

            Assert.IsTrue(collisionStartedListenerMock.Received);
            Assert.IsTrue(linkedCollisionStartedListenerMock.Received);
            Assert.IsFalse(unlinkedCollisionStartedListenerMock.Received);

            Object.DestroyImmediate(linkedContainer);
            Object.DestroyImmediate(unlinkedContainer);
        }

        [Test]
        public void CollisionStopped()
        {
            GameObject linkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier linkedNotifier = linkedContainer.AddComponent<CollisionNotifier>();

            GameObject unlinkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier unlinkedNotifier = unlinkedContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock collisionStoppedListenerMock = new UnityEventListenerMock();
            subject.CollisionStopped.AddListener(collisionStoppedListenerMock.Listen);

            UnityEventListenerMock linkedCollisionStoppedListenerMock = new UnityEventListenerMock();
            linkedNotifier.CollisionStopped.AddListener(linkedCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock unlinkedCollisionStoppedListenerMock = new UnityEventListenerMock();
            unlinkedNotifier.CollisionStopped.AddListener(unlinkedCollisionStoppedListenerMock.Listen);

            subject.CollisionStoppedMock(linkedContainer.GetComponent<Collider>());

            Assert.IsTrue(collisionStoppedListenerMock.Received);
            Assert.IsTrue(linkedCollisionStoppedListenerMock.Received);
            Assert.IsFalse(unlinkedCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(linkedContainer);
            Object.DestroyImmediate(unlinkedContainer);
        }

        [Test]
        public void CollisionChanged()
        {
            GameObject linkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier linkedNotifier = linkedContainer.AddComponent<CollisionNotifier>();

            GameObject unlinkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier unlinkedNotifier = unlinkedContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock collisionChangedListenerMock = new UnityEventListenerMock();
            subject.CollisionChanged.AddListener(collisionChangedListenerMock.Listen);

            UnityEventListenerMock linkedCollisionChangedListenerMock = new UnityEventListenerMock();
            linkedNotifier.CollisionChanged.AddListener(linkedCollisionChangedListenerMock.Listen);

            UnityEventListenerMock unlinkedCollisionChangedListenerMock = new UnityEventListenerMock();
            unlinkedNotifier.CollisionChanged.AddListener(unlinkedCollisionChangedListenerMock.Listen);

            subject.CollisionChangedMock(linkedContainer.GetComponent<Collider>());

            Assert.IsTrue(collisionChangedListenerMock.Received);
            Assert.IsTrue(linkedCollisionChangedListenerMock.Received);
            Assert.IsFalse(unlinkedCollisionChangedListenerMock.Received);

            Object.DestroyImmediate(linkedContainer);
            Object.DestroyImmediate(unlinkedContainer);
        }

        [Test]
        public void CollisionIgnoreStarted()
        {
            GameObject linkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier linkedNotifier = linkedContainer.AddComponent<CollisionNotifier>();

            GameObject unlinkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier unlinkedNotifier = unlinkedContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock collisionStartedListenerMock = new UnityEventListenerMock();
            subject.CollisionStarted.AddListener(collisionStartedListenerMock.Listen);

            UnityEventListenerMock linkedCollisionStartedListenerMock = new UnityEventListenerMock();
            linkedNotifier.CollisionStarted.AddListener(linkedCollisionStartedListenerMock.Listen);

            UnityEventListenerMock unlinkedCollisionStartedListenerMock = new UnityEventListenerMock();
            unlinkedNotifier.CollisionStarted.AddListener(unlinkedCollisionStartedListenerMock.Listen);

            subject.StatesToProcess = CollisionNotifier.CollisionStates.Stay | CollisionNotifier.CollisionStates.Exit;

            subject.CollisionStartedMock(linkedContainer.GetComponent<Collider>());

            Assert.IsFalse(collisionStartedListenerMock.Received);
            Assert.IsFalse(linkedCollisionStartedListenerMock.Received);
            Assert.IsFalse(unlinkedCollisionStartedListenerMock.Received);

            Object.DestroyImmediate(linkedContainer);
            Object.DestroyImmediate(unlinkedContainer);
        }

        [Test]
        public void CollisionIgnoreStopped()
        {
            GameObject linkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier linkedNotifier = linkedContainer.AddComponent<CollisionNotifier>();

            GameObject unlinkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier unlinkedNotifier = unlinkedContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock collisionStoppedListenerMock = new UnityEventListenerMock();
            subject.CollisionStopped.AddListener(collisionStoppedListenerMock.Listen);

            UnityEventListenerMock linkedCollisionStoppedListenerMock = new UnityEventListenerMock();
            linkedNotifier.CollisionStopped.AddListener(linkedCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock unlinkedCollisionStoppedListenerMock = new UnityEventListenerMock();
            unlinkedNotifier.CollisionStopped.AddListener(unlinkedCollisionStoppedListenerMock.Listen);

            subject.StatesToProcess = CollisionNotifier.CollisionStates.Enter | CollisionNotifier.CollisionStates.Stay;

            subject.CollisionStoppedMock(linkedContainer.GetComponent<Collider>());

            Assert.IsFalse(collisionStoppedListenerMock.Received);
            Assert.IsFalse(linkedCollisionStoppedListenerMock.Received);
            Assert.IsFalse(unlinkedCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(linkedContainer);
            Object.DestroyImmediate(unlinkedContainer);
        }

        [Test]
        public void CollisionIgnoreChanged()
        {
            GameObject linkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier linkedNotifier = linkedContainer.AddComponent<CollisionNotifier>();

            GameObject unlinkedContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            CollisionNotifier unlinkedNotifier = unlinkedContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock collisionChangedListenerMock = new UnityEventListenerMock();
            subject.CollisionChanged.AddListener(collisionChangedListenerMock.Listen);

            UnityEventListenerMock linkedCollisionChangedListenerMock = new UnityEventListenerMock();
            linkedNotifier.CollisionChanged.AddListener(linkedCollisionChangedListenerMock.Listen);

            UnityEventListenerMock unlinkedCollisionChangedListenerMock = new UnityEventListenerMock();
            unlinkedNotifier.CollisionChanged.AddListener(unlinkedCollisionChangedListenerMock.Listen);

            subject.StatesToProcess = CollisionNotifier.CollisionStates.Enter | CollisionNotifier.CollisionStates.Exit;

            subject.CollisionChangedMock(linkedContainer.GetComponent<Collider>());

            Assert.IsFalse(collisionChangedListenerMock.Received);
            Assert.IsFalse(linkedCollisionChangedListenerMock.Received);
            Assert.IsFalse(unlinkedCollisionChangedListenerMock.Received);

            Object.DestroyImmediate(linkedContainer);
            Object.DestroyImmediate(unlinkedContainer);
        }

        [Test]
        public void EventDataEquals()
        {
            GameObject forwardSourceA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject forwardSourceB = GameObject.CreatePrimitive(PrimitiveType.Cube);

            CollisionNotifier.EventData eventAA = new CollisionNotifier.EventData();
            CollisionNotifier.EventData eventBB = new CollisionNotifier.EventData();
            CollisionNotifier.EventData eventAB = new CollisionNotifier.EventData();
            CollisionNotifier.EventData eventBA = new CollisionNotifier.EventData();

            eventAA.Set(forwardSourceA.GetComponent<Component>(), true, null, forwardSourceA.GetComponent<Collider>());
            eventBB.Set(forwardSourceB.GetComponent<Component>(), true, null, forwardSourceB.GetComponent<Collider>());
            eventAB.Set(forwardSourceA.GetComponent<Component>(), true, null, forwardSourceB.GetComponent<Collider>());
            eventBA.Set(forwardSourceB.GetComponent<Component>(), true, null, forwardSourceA.GetComponent<Collider>());

            Assert.IsTrue(eventAA.Equals(eventAA));
            Assert.IsTrue(eventBB.Equals(eventBB));
            Assert.IsTrue(eventAB.Equals(eventAB));
            Assert.IsTrue(eventBA.Equals(eventBA));

            Assert.IsFalse(eventAA.Equals(eventBB));
            Assert.IsFalse(eventAA.Equals(eventAB));
            Assert.IsFalse(eventAA.Equals(eventBA));

            Assert.IsFalse(eventBB.Equals(eventAB));
            Assert.IsFalse(eventBB.Equals(eventBA));

            Assert.IsFalse(eventAB.Equals(eventBA));

            Object.DestroyImmediate(forwardSourceA);
            Object.DestroyImmediate(forwardSourceB);
        }

        public class CollisionNotifierMock : CollisionNotifier
        {
            public virtual void CollisionStartedMock(Collider collider)
            {
                OnCollisionStarted(eventData.Set(this, false, null, collider));
            }

            public virtual void CollisionStoppedMock(Collider collider)
            {
                OnCollisionStopped(eventData.Set(this, false, null, collider));
            }

            public virtual void CollisionChangedMock(Collider collider)
            {
                OnCollisionChanged(eventData.Set(this, false, null, collider));
            }
        }
    }
}