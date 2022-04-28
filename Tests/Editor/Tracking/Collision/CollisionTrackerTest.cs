using Zinnia.Rule;
using Zinnia.Tracking.Collision;

namespace Test.Zinnia.Tracking.Collision
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
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

        [UnityTest]
        public IEnumerator CollisionCheckExitEnterOnKinematicChange()
        {
            WaitForFixedUpdate yieldInstruction = new WaitForFixedUpdate();

            GameObject trackerContainer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trackerContainer.name = "TrackerContainer";
            trackerContainer.GetComponent<Collider>().isTrigger = true;
            trackerContainer.AddComponent<Rigidbody>().isKinematic = true;
            trackerContainer.transform.position = Vector3.forward;
            trackerContainer.transform.localScale = Vector3.one * 0.1f;
            CollisionTrackerMock tracker = trackerContainer.AddComponent<CollisionTrackerMock>();

            UnityEventListenerMock trackerCollisionStartedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock trackerCollisionStoppedListenerMock = new UnityEventListenerMock();
            tracker.CollisionStarted.AddListener(trackerCollisionStartedListenerMock.Listen);
            tracker.CollisionStopped.AddListener(trackerCollisionStoppedListenerMock.Listen);

            GameObject targetContainer = new GameObject("TargetContainer");
            targetContainer.transform.localPosition = Vector3.zero;
            targetContainer.transform.localScale = Vector3.one;

            GameObject childOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childOne.name = "childOne";
            childOne.transform.SetParent(targetContainer.transform);
            childOne.transform.localPosition = Vector3.zero;
            childOne.transform.localScale = Vector3.one * 0.2f;

            GameObject childTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childTwo.name = "childTwo";
            childTwo.transform.SetParent(targetContainer.transform);
            childTwo.transform.localPosition = Vector3.left * 0.2f;
            childTwo.transform.localScale = Vector3.one * 0.2f;

            Rigidbody targetRigidbody = targetContainer.AddComponent<Rigidbody>();
            targetRigidbody.useGravity = false;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 1. First do a simple touch childOne to check started is called.

            trackerContainer.transform.position = childOne.transform.position;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childOne.transform, tracker.LatestStartedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStartedEventData.ColliderData.attachedRigidbody);
            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStoppedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 1. Then untouch childOne to check stopped is called

            trackerContainer.transform.position = Vector3.forward;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childOne.transform, tracker.LatestStoppedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStoppedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStartedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            yield return yieldInstruction;

            /// 1. Now touch childOne, check start is called, then switch target to kinematic

            trackerContainer.transform.position = childOne.transform.position;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            tracker.PrepareKinematicStateChange(targetRigidbody);
            targetRigidbody.isKinematic = true;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childOne.transform, tracker.LatestStartedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStartedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStoppedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 1. Then untouch childOne but remove kinematic state first

            tracker.PrepareKinematicStateChange(targetRigidbody);
            targetRigidbody.isKinematic = false;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.forward;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childOne.transform, tracker.LatestStoppedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStoppedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStartedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 2. Now do a simple touch childTwo to check started is called.

            trackerContainer.transform.position = childTwo.transform.position;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childTwo.transform, tracker.LatestStartedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStartedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStoppedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 2. Then untouch childTwo to check stopped is called

            trackerContainer.transform.position = Vector3.forward;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childTwo.transform, tracker.LatestStoppedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStoppedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStartedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 2. Now touch childTwo, check start is called, then switch target to kinematic

            trackerContainer.transform.position = childTwo.transform.position;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            tracker.PrepareKinematicStateChange(targetRigidbody);
            targetRigidbody.isKinematic = true;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childTwo.transform, tracker.LatestStartedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStartedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStoppedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 2. Then untouch childTwo but remove kinematic state first

            tracker.PrepareKinematicStateChange(targetRigidbody);
            targetRigidbody.isKinematic = false;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            trackerContainer.transform.position = Vector3.forward;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childTwo.transform, tracker.LatestStoppedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStoppedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStartedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 3. Now touch childTwo, but when kinematic is changed, in the same frame move the touch to childOne

            trackerContainer.transform.position = childTwo.transform.position;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            Assert.IsFalse(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childTwo.transform, tracker.LatestStartedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStartedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStoppedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            tracker.PrepareKinematicStateChange(targetRigidbody);
            targetRigidbody.isKinematic = true;
            trackerContainer.transform.position = childOne.transform.position;

            yield return yieldInstruction;

            Assert.IsTrue(trackerCollisionStartedListenerMock.Received);
            /// Wait a frame for the trigger exit fix in 2019
            yield return yieldInstruction;
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childOne.transform, tracker.LatestStartedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStartedEventData.ColliderData.attachedRigidbody);
            Assert.AreEqual(childTwo.transform, tracker.LatestStoppedEventData.ColliderData.transform);

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// 3. Then untouch childOne but remove kinematic state first

            tracker.PrepareKinematicStateChange(targetRigidbody);
            targetRigidbody.isKinematic = false;
            trackerContainer.transform.position = Vector3.forward;

            yield return yieldInstruction;

            Assert.IsFalse(trackerCollisionStartedListenerMock.Received);
            /// Wait a frame for the trigger exit fix in 2019
            yield return yieldInstruction;
            Assert.IsTrue(trackerCollisionStoppedListenerMock.Received);
            trackerCollisionStartedListenerMock.Reset();
            trackerCollisionStoppedListenerMock.Reset();

            Assert.AreEqual(childOne.transform, tracker.LatestStoppedEventData.ColliderData.transform);
            Assert.AreEqual(targetRigidbody, tracker.LatestStoppedEventData.ColliderData.attachedRigidbody);

            Assert.IsTrue(tracker.IsEventDataEmpty(tracker.LatestStartedEventData));

            tracker.LatestStartedEventData.Clear();
            tracker.LatestStoppedEventData.Clear();

            /// Clean up

            Object.DestroyImmediate(trackerContainer);
            Object.DestroyImmediate(targetContainer);
        }

        [Test]
        public void ClearColliderValidity()
        {
            GameObject containingObject = new GameObject();
            CollisionTracker subject = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.ColliderValidity);
            RuleContainer rule = new RuleContainer();
            subject.ColliderValidity = rule;
            Assert.AreEqual(rule, subject.ColliderValidity);
            subject.ClearColliderValidity();
            Assert.IsNull(subject.ColliderValidity);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearColliderValidityInactiveGameObject()
        {
            GameObject containingObject = new GameObject();
            CollisionTracker subject = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.ColliderValidity);
            RuleContainer rule = new RuleContainer();
            subject.ColliderValidity = rule;
            Assert.AreEqual(rule, subject.ColliderValidity);
            subject.gameObject.SetActive(false);
            subject.ClearColliderValidity();
            Assert.AreEqual(rule, subject.ColliderValidity);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearColliderValidityInactiveComponent()
        {
            GameObject containingObject = new GameObject();
            CollisionTracker subject = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.ColliderValidity);
            RuleContainer rule = new RuleContainer();
            subject.ColliderValidity = rule;
            Assert.AreEqual(rule, subject.ColliderValidity);
            subject.enabled = false;
            subject.ClearColliderValidity();
            Assert.AreEqual(rule, subject.ColliderValidity);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearContainingTransformValidity()
        {
            GameObject containingObject = new GameObject();
            CollisionTracker subject = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.ContainingTransformValidity);
            RuleContainer rule = new RuleContainer();
            subject.ContainingTransformValidity = rule;
            Assert.AreEqual(rule, subject.ContainingTransformValidity);
            subject.ClearContainingTransformValidity();
            Assert.IsNull(subject.ContainingTransformValidity);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearContainingTransformValidityInactiveGameObject()
        {
            GameObject containingObject = new GameObject();
            CollisionTracker subject = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.ContainingTransformValidity);
            RuleContainer rule = new RuleContainer();
            subject.ContainingTransformValidity = rule;
            Assert.AreEqual(rule, subject.ContainingTransformValidity);
            subject.gameObject.SetActive(false);
            subject.ClearContainingTransformValidity();
            Assert.AreEqual(rule, subject.ContainingTransformValidity);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearContainingTransformValidityInactiveComponent()
        {
            GameObject containingObject = new GameObject();
            CollisionTracker subject = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.ContainingTransformValidity);
            RuleContainer rule = new RuleContainer();
            subject.ContainingTransformValidity = rule;
            Assert.AreEqual(rule, subject.ContainingTransformValidity);
            subject.enabled = false;
            subject.ClearContainingTransformValidity();
            Assert.AreEqual(rule, subject.ContainingTransformValidity);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionTrackerDisabledObserver_ClearSource()
        {
            GameObject containingObject = new GameObject();
            CollisionTrackerDisabledObserver subject = containingObject.AddComponent<CollisionTrackerDisabledObserver>();
            CollisionTracker tracker = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.Source);
            subject.Source = tracker;
            Assert.AreEqual(tracker, subject.Source);
            subject.ClearSource();
            Assert.IsNull(subject.Source);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionTrackerDisabledObserver_ClearSourceInactiveGameObject()
        {
            GameObject containingObject = new GameObject();
            CollisionTrackerDisabledObserver subject = containingObject.AddComponent<CollisionTrackerDisabledObserver>();
            CollisionTracker tracker = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.Source);
            subject.Source = tracker;
            Assert.AreEqual(tracker, subject.Source);
            subject.gameObject.SetActive(false);
            subject.ClearSource();
            Assert.AreEqual(tracker, subject.Source);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionTrackerDisabledObserver_ClearSourceInactiveComponent()
        {
            GameObject containingObject = new GameObject();
            CollisionTrackerDisabledObserver subject = containingObject.AddComponent<CollisionTrackerDisabledObserver>();
            CollisionTracker tracker = containingObject.AddComponent<CollisionTracker>();

            Assert.IsNull(subject.Source);
            subject.Source = tracker;
            Assert.AreEqual(tracker, subject.Source);
            subject.enabled = false;
            subject.ClearSource();
            Assert.AreEqual(tracker, subject.Source);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionTrackerDisabledObserver_ClearTarget()
        {
            GameObject containingObject = new GameObject();
            CollisionTrackerDisabledObserver subject = containingObject.AddComponent<CollisionTrackerDisabledObserver>();
            BoxCollider collider = containingObject.AddComponent<BoxCollider>();

            Assert.IsNull(subject.Target);
            subject.Target = collider;
            Assert.AreEqual(collider, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionTrackerDisabledObserver_ClearTargetInactiveGameObject()
        {
            GameObject containingObject = new GameObject();
            CollisionTrackerDisabledObserver subject = containingObject.AddComponent<CollisionTrackerDisabledObserver>();
            BoxCollider collider = containingObject.AddComponent<BoxCollider>();

            Assert.IsNull(subject.Target);
            subject.Target = collider;
            Assert.AreEqual(collider, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(collider, subject.Target);

            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void CollisionTrackerDisabledObserver_ClearTargetInactiveComponent()
        {
            GameObject containingObject = new GameObject();
            CollisionTrackerDisabledObserver subject = containingObject.AddComponent<CollisionTrackerDisabledObserver>();
            BoxCollider collider = containingObject.AddComponent<BoxCollider>();

            Assert.IsNull(subject.Target);
            subject.Target = collider;
            Assert.AreEqual(collider, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(collider, subject.Target);

            Object.DestroyImmediate(containingObject);
        }

        public class CollisionTrackerMock : CollisionTracker
        {
            public EventData LatestStartedEventData { get; set; } = new EventData();
            public EventData LatestStoppedEventData { get; set; } = new EventData();

            public virtual void SetStopCollisionsOnDisable(bool state)
            {
                StopCollisionsOnDisable = state;
            }

            public override void OnCollisionStarted(EventData data)
            {
                base.OnCollisionStarted(data);
                LatestStartedEventData = new EventData();
                LatestStartedEventData.Set(data);
            }

            public override void OnCollisionStopped(EventData data)
            {
                base.OnCollisionStopped(data);
                LatestStoppedEventData = new EventData();
                LatestStoppedEventData.Set(data);
            }

            public virtual bool IsEventDataEmpty(EventData data)
            {
                return (data.ForwardSource == default && data.IsTrigger == default && data.CollisionData == default && data.ColliderData == default);
            }
        }
    }
}