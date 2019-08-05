using Zinnia.Utility;

namespace Test.Zinnia.Utility
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class CountdownTimerTest
    {
        private GameObject containingObject;
        private CountdownTimer subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<CountdownTimer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(containingObject);
        }

        [UnityTest]
        public IEnumerator TimerComplete()
        {
            UnityEventListenerMock timerStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerStillRunningMock = new UnityEventListenerMock();
            UnityEventListenerMock timerNotRunningMock = new UnityEventListenerMock();

            subject.Started.AddListener(timerStartedMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.StillRunning.AddListener(timerStillRunningMock.Listen);
            subject.NotRunning.AddListener(timerNotRunningMock.Listen);

            subject.StartTime = 0.1f;

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);
            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            subject.Begin();

            Assert.IsTrue(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitStatus();

            Assert.IsTrue(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            yield return new WaitForSeconds(0.1f);

            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsTrue(timerCompleteMock.Received);

            timerStillRunningMock.Reset();
            timerNotRunningMock.Reset();

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsTrue(timerNotRunningMock.Received);
        }

        [UnityTest]
        public IEnumerator TimerCancelled()
        {
            UnityEventListenerMock timerStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerStillRunningMock = new UnityEventListenerMock();
            UnityEventListenerMock timerNotRunningMock = new UnityEventListenerMock();

            subject.Started.AddListener(timerStartedMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.StillRunning.AddListener(timerStillRunningMock.Listen);
            subject.NotRunning.AddListener(timerNotRunningMock.Listen);

            subject.StartTime = 0.2f;

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.Begin();

            Assert.IsTrue(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitStatus();

            Assert.IsTrue(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            yield return new WaitForSeconds(0.1f);

            subject.Cancel();

            Assert.IsTrue(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            timerStillRunningMock.Reset();
            timerNotRunningMock.Reset();

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsTrue(timerNotRunningMock.Received);
        }

        [UnityTest]
        public IEnumerator TimerDoesNotCompleteOnInactiveGameObject()
        {
            UnityEventListenerMock timerStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerStillRunningMock = new UnityEventListenerMock();
            UnityEventListenerMock timerNotRunningMock = new UnityEventListenerMock();

            subject.Started.AddListener(timerStartedMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.StillRunning.AddListener(timerStillRunningMock.Listen);
            subject.NotRunning.AddListener(timerNotRunningMock.Listen);

            subject.gameObject.SetActive(false);

            subject.StartTime = 0.1f;

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);
            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            subject.Begin();

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            yield return new WaitForSeconds(0.1f);

            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            timerStillRunningMock.Reset();
            timerNotRunningMock.Reset();

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);
        }

        [UnityTest]
        public IEnumerator TimerDoesNotCompleteOnInactiveComponent()
        {
            UnityEventListenerMock timerStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerStillRunningMock = new UnityEventListenerMock();
            UnityEventListenerMock timerNotRunningMock = new UnityEventListenerMock();

            subject.Started.AddListener(timerStartedMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.StillRunning.AddListener(timerStillRunningMock.Listen);
            subject.NotRunning.AddListener(timerNotRunningMock.Listen);

            subject.enabled = false;

            subject.StartTime = 0.1f;

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);
            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            subject.Begin();

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            yield return new WaitForSeconds(0.1f);

            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            timerStillRunningMock.Reset();
            timerNotRunningMock.Reset();

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);
        }

        [UnityTest]
        public IEnumerator TimerCancelledOnDisableGameObject()
        {
            UnityEventListenerMock timerStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerStillRunningMock = new UnityEventListenerMock();
            UnityEventListenerMock timerNotRunningMock = new UnityEventListenerMock();

            subject.Started.AddListener(timerStartedMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.StillRunning.AddListener(timerStillRunningMock.Listen);
            subject.NotRunning.AddListener(timerNotRunningMock.Listen);

            subject.StartTime = 0.2f;

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.Begin();

            Assert.IsTrue(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitStatus();

            Assert.IsTrue(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            yield return new WaitForSeconds(0.1f);

            subject.gameObject.SetActive(false);

            Assert.IsTrue(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            timerStillRunningMock.Reset();
            timerNotRunningMock.Reset();

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);
        }

        [UnityTest]
        public IEnumerator TimerCancelledOnDisableComponent()
        {
            UnityEventListenerMock timerStartedMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerStillRunningMock = new UnityEventListenerMock();
            UnityEventListenerMock timerNotRunningMock = new UnityEventListenerMock();

            subject.Started.AddListener(timerStartedMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.StillRunning.AddListener(timerStillRunningMock.Listen);
            subject.NotRunning.AddListener(timerNotRunningMock.Listen);

            subject.StartTime = 0.2f;

            Assert.IsFalse(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.Begin();

            Assert.IsTrue(timerStartedMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitStatus();

            Assert.IsTrue(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);

            yield return new WaitForSeconds(0.1f);

            subject.enabled = false;

            Assert.IsTrue(timerCancelledMock.Received);
            Assert.IsFalse(timerCompleteMock.Received);

            timerStillRunningMock.Reset();
            timerNotRunningMock.Reset();

            subject.EmitStatus();

            Assert.IsFalse(timerStillRunningMock.Received);
            Assert.IsFalse(timerNotRunningMock.Received);
        }
    }
}
