using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active;
using Zinnia.Tracking.Collision.Active.Operation;

namespace Test.Zinnia.Tracking.Collision.Active.Operation
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;
    using Test.Zinnia.Utility.Helper;
    using Assert = UnityEngine.Assertions.Assert;

    public class OrderReverserTest
    {
        private GameObject containingObject;
        private OrderReverser subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<OrderReverser>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Reverse()
        {
            UnityEventListenerMock reversedMock = new UnityEventListenerMock();
            subject.Reversed.AddListener(reversedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(reversedMock.Received);

            ActiveCollisionsContainer.EventData reversedList = subject.Reverse(eventData);

            Assert.IsTrue(reversedMock.Received);
            Assert.AreEqual("three,two,one", ActiveCollisionsHelper.GetNamesOfActiveCollisions(reversedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }

        [Test]
        public void ReverseEmptyList()
        {
            UnityEventListenerMock reversedMock = new UnityEventListenerMock();
            subject.Reversed.AddListener(reversedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(reversedMock.Received);

            ActiveCollisionsContainer.EventData reversedList = subject.Reverse(eventData);

            Assert.IsTrue(reversedMock.Received);
            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(reversedList));
        }

        [Test]
        public void ReverseInactiveGameObject()
        {
            UnityEventListenerMock reversedMock = new UnityEventListenerMock();
            subject.Reversed.AddListener(reversedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.gameObject.SetActive(false);

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(reversedMock.Received);

            ActiveCollisionsContainer.EventData reversedList = subject.Reverse(eventData);

            Assert.IsFalse(reversedMock.Received);
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(reversedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }

        [Test]
        public void ReverseInactiveComponent()
        {
            UnityEventListenerMock reversedMock = new UnityEventListenerMock();
            subject.Reversed.AddListener(reversedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.enabled = false;

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(reversedMock.Received);

            ActiveCollisionsContainer.EventData reversedList = subject.Reverse(eventData);

            Assert.IsFalse(reversedMock.Received);
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(reversedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }
    }
}