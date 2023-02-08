using Zinnia.Event.Yield;

namespace Test.Zinnia.Event.Yield
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class WaitForEndOfFrameYieldEmitterTest
    {
        private GameObject containingObject;
        private WaitForEndOfFrameYieldEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("WaitForEndOfFrameYieldEmitterTest");
            subject = containingObject.AddComponent<WaitForEndOfFrameYieldEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator Yielded()
        {
            UnityEventListenerMock yieldedMock = new UnityEventListenerMock();
            UnityEventListenerMock cancelledMock = new UnityEventListenerMock();
            subject.Yielded.AddListener(yieldedMock.Listen);
            subject.Cancelled.AddListener(cancelledMock.Listen);

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
        public IEnumerator WaitFor2Frames()
        {
            UnityEventListenerMock yieldedMock = new UnityEventListenerMock();
            UnityEventListenerMock cancelledMock = new UnityEventListenerMock();
            subject.Yielded.AddListener(yieldedMock.Listen);
            subject.Cancelled.AddListener(cancelledMock.Listen);

            subject.FramesUntilYield = 2;

            yield return null;

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            subject.Begin();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            yield return new WaitForEndOfFrame();

            Assert.IsTrue(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);
        }

        [UnityTest]
        public IEnumerator WaitFor4Frames()
        {
            UnityEventListenerMock yieldedMock = new UnityEventListenerMock();
            UnityEventListenerMock cancelledMock = new UnityEventListenerMock();
            subject.Yielded.AddListener(yieldedMock.Listen);
            subject.Cancelled.AddListener(cancelledMock.Listen);

            subject.FramesUntilYield = 4;

            yield return null;

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            subject.Begin();

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(yieldedMock.Received);
            Assert.IsFalse(cancelledMock.Received);

            yield return new WaitForEndOfFrame();

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