using Zinnia.Utility;

namespace Test.Zinnia.Utility
{
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class CountdownTimerTest
    {
        private const float Tolerance = 0.01f;
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

        [UnityTest]
        public IEnumerator TimerEmitTime()
        {
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventValueListenerMock<float> timerElapsedTimeMock = new UnityEventValueListenerMock<float>();
            UnityEventValueListenerMock<float> timerRemainingTimeMock = new UnityEventValueListenerMock<float>();

            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.ElapsedTimeEmitted.AddListener(timerElapsedTimeMock.Listen);
            subject.RemainingTimeEmitted.AddListener(timerRemainingTimeMock.Listen);

            float beginTime = Time.time;
            float startTime = 0.5f;

            subject.StartTime = startTime;

            Assert.IsFalse(timerElapsedTimeMock.Received);
            Assert.IsFalse(timerRemainingTimeMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0f, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0.5f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            subject.Begin();

            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0f, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0.5f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            yield return new WaitForSeconds(0.2f);

            float elapsedTime = Time.time - beginTime;
            float remainingTime = startTime + (beginTime - Time.time);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreEqual(timerElapsedTimeMock.Value, elapsedTime);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreEqual(timerRemainingTimeMock.Value, remainingTime);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            yield return new WaitForSeconds(0.3f);

            Assert.IsTrue(timerCompleteMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0.5f, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0f, Tolerance);
        }

        [UnityTest]
        public IEnumerator TimerEmitTimeUnchangedAfterComplete()
        {
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventValueListenerMock<float> timerElapsedTimeMock = new UnityEventValueListenerMock<float>();
            UnityEventValueListenerMock<float> timerRemainingTimeMock = new UnityEventValueListenerMock<float>();

            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.ElapsedTimeEmitted.AddListener(timerElapsedTimeMock.Listen);
            subject.RemainingTimeEmitted.AddListener(timerRemainingTimeMock.Listen);

            float beginTime = Time.time;
            float startTime = 0.5f;
            subject.StartTime = startTime;

            Assert.IsFalse(timerElapsedTimeMock.Received);
            Assert.IsFalse(timerRemainingTimeMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0.5f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            subject.Begin();

            Assert.IsFalse(timerCompleteMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0.5f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            yield return new WaitForSeconds(0.5f);

            Assert.IsTrue(timerCompleteMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0.5f, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            yield return new WaitForSeconds(0.5f);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0.5f, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0f, Tolerance);
        }

        [UnityTest]
        public IEnumerator TimerEmitTimeUnchangedAfterCancelled()
        {
            UnityEventListenerMock timerCompleteMock = new UnityEventListenerMock();
            UnityEventListenerMock timerCancelledMock = new UnityEventListenerMock();
            UnityEventValueListenerMock<float> timerElapsedTimeMock = new UnityEventValueListenerMock<float>();
            UnityEventValueListenerMock<float> timerRemainingTimeMock = new UnityEventValueListenerMock<float>();

            subject.Completed.AddListener(timerCompleteMock.Listen);
            subject.Cancelled.AddListener(timerCancelledMock.Listen);
            subject.ElapsedTimeEmitted.AddListener(timerElapsedTimeMock.Listen);
            subject.RemainingTimeEmitted.AddListener(timerRemainingTimeMock.Listen);

            float beginTime = Time.time;
            float startTime = 0.5f;
            subject.StartTime = startTime;

            Assert.IsFalse(timerElapsedTimeMock.Received);
            Assert.IsFalse(timerRemainingTimeMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0.5f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            subject.Begin();

            Assert.IsFalse(timerCompleteMock.Received);
            Assert.IsFalse(timerCancelledMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreApproximatelyEqual(timerElapsedTimeMock.Value, 0, Tolerance);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreApproximatelyEqual(timerRemainingTimeMock.Value, 0.5f, Tolerance);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            yield return new WaitForSeconds(0.2f);

            float elapsedTime = Time.time - beginTime;
            float remainingTime = startTime + (beginTime - Time.time);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreEqual(timerElapsedTimeMock.Value, elapsedTime);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreEqual(timerRemainingTimeMock.Value, remainingTime);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            subject.Cancel();
            Assert.IsFalse(timerCompleteMock.Received);
            Assert.IsTrue(timerCancelledMock.Received);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreEqual(timerElapsedTimeMock.Value, elapsedTime);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreEqual(timerRemainingTimeMock.Value, remainingTime);

            timerElapsedTimeMock.Reset();
            timerRemainingTimeMock.Reset();

            yield return new WaitForSeconds(0.5f);

            subject.EmitElapsedTime();
            Assert.IsTrue(timerElapsedTimeMock.Received);
            Assert.AreEqual(timerElapsedTimeMock.Value, elapsedTime);

            subject.EmitRemainingTime();
            Assert.IsTrue(timerRemainingTimeMock.Received);
            Assert.AreEqual(timerRemainingTimeMock.Value, remainingTime);
        }
    }
}
