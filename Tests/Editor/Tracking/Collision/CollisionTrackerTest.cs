using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class CollisionTrackerTest
    {
        [UnityTest]
        public IEnumerator CollisionStates()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTracker tracker = trackerContainer.AddComponent<CollisionTracker>();

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.one * 0.25f;
            notifierContainer.transform.position = Vector3.one * -0.25f;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.forward;
            notifierContainer.transform.position = Vector3.back;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsTrue(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionStatesIgnoreEnter()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTracker tracker = trackerContainer.AddComponent<CollisionTracker>();
            tracker.StatesToProcess = CollisionTracker.CollisionStates.Stay | CollisionTracker.CollisionStates.Exit;

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionStatesIgnoreStay()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTracker tracker = trackerContainer.AddComponent<CollisionTracker>();
            tracker.StatesToProcess = CollisionTracker.CollisionStates.Enter | CollisionTracker.CollisionStates.Exit;

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionStatesIgnoreExit()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTracker tracker = trackerContainer.AddComponent<CollisionTracker>();
            tracker.StatesToProcess = CollisionTracker.CollisionStates.Enter | CollisionTracker.CollisionStates.Stay;

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.forward;
            notifierContainer.transform.position = Vector3.back;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionEndsOnNotifierGameObjectDisable()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTracker tracker = trackerContainer.AddComponent<CollisionTracker>();

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            notifierContainer.SetActive(false);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsTrue(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionEndsOnTrackerGameObjectDisable()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTracker tracker = trackerContainer.AddComponent<CollisionTracker>();

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.SetActive(false);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsTrue(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionDoesNotEndOnNotifierGameObjectDisable()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTrackerMock tracker = trackerContainer.AddComponent<CollisionTrackerMock>();
            tracker.SetStopCollisionsOnDisable(false);

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            notifierContainer.SetActive(false);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        [UnityTest]
        public IEnumerator CollisionDoesNotEndOnTrackerGameObjectDisable()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            CollisionTrackerMock tracker = trackerContainer.AddComponent<CollisionTrackerMock>();
            tracker.SetStopCollisionsOnDisable(false);

            GameObject notifierContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            notifierContainer.GetComponent<Collider>().isTrigger = true;
            notifierContainer.AddComponent<Rigidbody>().isKinematic = true;
            notifierContainer.transform.position = Vector3.back;
            CollisionNotifier notifier = notifierContainer.AddComponent<CollisionNotifier>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionChanged.AddListener(trackerCollisionChangedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            UnityEventListenerMock notifierCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionChangedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock notifierCollisionStoppedListenerMock = new UnityEventListenerMock();
            notifier.CollisionStarted.AddListener(notifierCollisionStartedListenerMock.Listen);
            notifier.CollisionChanged.AddListener(notifierCollisionChangedListenerMock.Listen);
            notifier.CollisionStopped.AddListener(notifierCollisionStoppedListenerMock.Listen);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.zero;
            notifierContainer.transform.position = Vector3.zero;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsTrue(notifierCollisionStartedListenerMock.Received);
            Assert.IsTrue(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionChangedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            notifierCollisionStartedListenerMock.Reset();
            notifierCollisionChangedListenerMock.Reset();
            notifierCollisionStoppedListenerMock.Reset();

            trackerContainer.SetActive(false);

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionChangedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);

            Assert.IsFalse(notifierCollisionStartedListenerMock.Received);
            Assert.IsFalse(notifierCollisionChangedListenerMock.Received);
            Assert.IsFalse(notifierCollisionStoppedListenerMock.Received);

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(notifierContainer);
        }

        public class CollisionTrackerMock : CollisionTracker
        {
            public virtual void SetStopCollisionsOnDisable(bool state)
            {
                StopCollisionsOnDisable = state;
            }
        }
    }
}