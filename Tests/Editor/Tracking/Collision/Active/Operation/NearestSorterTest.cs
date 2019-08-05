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

    public class NearestSorterTest
    {
        private GameObject containingObject;
        private NearestSorter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<NearestSorter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Sort()
        {
            UnityEventListenerMock sortedMock = new UnityEventListenerMock();
            subject.Sorted.AddListener(sortedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer, Vector3.one * 5f);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer, Vector3.one * 2f);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer, Vector3.one * 3f);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.Source = containingObject;

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(sortedMock.Received);

            ActiveCollisionsContainer.EventData sortedList = subject.Sort(eventData);

            Assert.IsTrue(sortedMock.Received);
            Assert.AreEqual("two,three,one", ActiveCollisionsHelper.GetNamesOfActiveCollisions(sortedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }

        [Test]
        public void SortEmptyList()
        {
            UnityEventListenerMock sortedMock = new UnityEventListenerMock();
            subject.Sorted.AddListener(sortedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.Source = containingObject;

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(sortedMock.Received);

            ActiveCollisionsContainer.EventData sortedList = subject.Sort(eventData);

            Assert.IsTrue(sortedMock.Received);
            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(sortedList));
        }

        [Test]
        public void SortInactiveGameObject()
        {
            UnityEventListenerMock sortedMock = new UnityEventListenerMock();
            subject.Sorted.AddListener(sortedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer, Vector3.one * 5f);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer, Vector3.one * 2f);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer, Vector3.one * 3f);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.Source = containingObject;
            subject.gameObject.SetActive(false);

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(sortedMock.Received);

            ActiveCollisionsContainer.EventData sortedList = subject.Sort(eventData);

            Assert.IsFalse(sortedMock.Received);
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(sortedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }

        [Test]
        public void SortInactiveComponent()
        {
            UnityEventListenerMock sortedMock = new UnityEventListenerMock();
            subject.Sorted.AddListener(sortedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer, Vector3.one * 5f);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer, Vector3.one * 2f);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer, Vector3.one * 3f);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.Source = containingObject;
            subject.enabled = false;

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(sortedMock.Received);

            ActiveCollisionsContainer.EventData sortedList = subject.Sort(eventData);

            Assert.IsFalse(sortedMock.Received);
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(sortedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }

        [Test]
        public void SortNoSource()
        {
            UnityEventListenerMock sortedMock = new UnityEventListenerMock();
            subject.Sorted.AddListener(sortedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            GameObject oneContainer;
            CollisionNotifier.EventData oneData = CollisionNotifierHelper.GetEventData(out oneContainer, Vector3.one * 5f);
            oneContainer.name = "one";
            collisionList.Add(oneData);

            GameObject twoContainer;
            CollisionNotifier.EventData twoData = CollisionNotifierHelper.GetEventData(out twoContainer, Vector3.one * 2f);
            twoContainer.name = "two";
            collisionList.Add(twoData);

            GameObject threeContainer;
            CollisionNotifier.EventData threeData = CollisionNotifierHelper.GetEventData(out threeContainer, Vector3.one * 3f);
            threeContainer.name = "three";
            collisionList.Add(threeData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));
            Assert.IsFalse(sortedMock.Received);

            ActiveCollisionsContainer.EventData sortedList = subject.Sort(eventData);

            Assert.IsFalse(sortedMock.Received);
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(sortedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }
    }
}
