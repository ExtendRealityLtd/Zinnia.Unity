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

    public class SlicerTest
    {
        private GameObject containingObject;
        private Slicer subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<Slicer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void SliceFirstElement()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 0;
            subject.Length = 1;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("one", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceLastElement()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = -1;
            subject.Length = 1;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three,four,five", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceMiddleElement()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 2;
            subject.Length = 1;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceMiddleTwoElements()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 2;
            subject.Length = 2;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("three,four", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceFirstThreeElements()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 0;
            subject.Length = 3;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceLastThreeElements()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 3;
            subject.Length = 3;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceLastThreeActualElements()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 4;
            subject.Length = 3;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three,four", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceLastThreeElementsNegativeStartIndex()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = -3;
            subject.Length = 3;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }


        [Test]
        public void SliceLastThreeActualElementsNegativeStartIndex()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = -2;
            subject.Length = 3;

            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three,four", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void StartIndexExceedsCount()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = eventData.ActiveCollisions.Count + 1;
            subject.Length = 1;

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void LengthIsZero()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            GameObject fourContainer;
            CollisionNotifier.EventData fourData = CollisionNotifierHelper.GetEventData(out fourContainer);
            fourContainer.name = "four";
            collisionList.Add(fourData);

            GameObject fiveContainer;
            CollisionNotifier.EventData fiveData = CollisionNotifierHelper.GetEventData(out fiveContainer);
            fiveContainer.name = "five";
            collisionList.Add(fiveData);

            GameObject sixContainer;
            CollisionNotifier.EventData sixData = CollisionNotifierHelper.GetEventData(out sixContainer);
            sixContainer.name = "six";
            collisionList.Add(sixData);

            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 0;
            subject.Length = 0;

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three,four,five,six", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
            Object.DestroyImmediate(fourContainer);
            Object.DestroyImmediate(fiveContainer);
            Object.DestroyImmediate(sixContainer);
        }

        [Test]
        public void SliceEmptyList()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

            List<CollisionNotifier.EventData> collisionList = new List<CollisionNotifier.EventData>();
            ActiveCollisionsContainer.EventData eventData = new ActiveCollisionsContainer.EventData().Set(collisionList);

            subject.StartIndex = 0;
            subject.Length = 1;

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsTrue(slicedMock.Received);
            Assert.IsTrue(remainedMock.Received);

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));
        }

        [Test]
        public void SliceInactiveGameObject()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            subject.StartIndex = 0;
            subject.Length = 1;
            subject.gameObject.SetActive(false);

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }

        [Test]
        public void SliceInactiveComponent()
        {
            UnityEventListenerMock slicedMock = new UnityEventListenerMock();
            subject.Sliced.AddListener(slicedMock.Listen);
            UnityEventListenerMock remainedMock = new UnityEventListenerMock();
            subject.Remained.AddListener(remainedMock.Listen);

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

            subject.StartIndex = 0;
            subject.Length = 1;
            subject.enabled = false;

            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(eventData));

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            ActiveCollisionsContainer.EventData remainedList;
            ActiveCollisionsContainer.EventData slicedList = subject.Slice(eventData, out remainedList);

            Assert.IsFalse(slicedMock.Received);
            Assert.IsFalse(remainedMock.Received);

            Assert.AreEqual("", ActiveCollisionsHelper.GetNamesOfActiveCollisions(slicedList));
            Assert.AreEqual("one,two,three", ActiveCollisionsHelper.GetNamesOfActiveCollisions(remainedList));

            Object.DestroyImmediate(oneContainer);
            Object.DestroyImmediate(twoContainer);
            Object.DestroyImmediate(threeContainer);
        }
    }
}
