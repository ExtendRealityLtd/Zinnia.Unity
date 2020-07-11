using Zinnia.Event.Yield;

namespace Test.Zinnia.Event.Yield
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class WaitForSecondsYieldEmitterTest
    {
        private GameObject containingObject;
        private WaitForSecondsYieldEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<WaitForSecondsYieldEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator Yielded()
        {
            UnityEventListenerMock yieldedMock = new UnityEventListenerMock();
            UnityEventListenerMock cancelledMock = new UnityEventListenerMock();
            subject.Yielded.AddListener(yieldedMock.Listen);
            subject.Cancelled.AddListener(cancelledMock.Listen);

            subject.SecondsToWait = 1f;

            yield return null;

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            subject.Begin();

            while (subject.IsRunning)
            {
                yield return null;
            }

            Assert.IsTrue(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);
        }

        [UnityTest]
        public IEnumerator Cancelled()
        {
            UnityEventListenerMock yieldedMock = new UnityEventListenerMock();
            UnityEventListenerMock cancelledMock = new UnityEventListenerMock();
            subject.Yielded.AddListener(yieldedMock.Listen);
            subject.Cancelled.AddListener(cancelledMock.Listen);

            subject.SecondsToWait = 1f;

            yield return null;

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            subject.Begin();

            while (subject.IsRunning)
            {
                subject.Cancel();
                yield return null;
            }

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsTrue(cancelledMock.Received);
        }
    }
}