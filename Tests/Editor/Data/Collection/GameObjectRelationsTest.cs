using Zinnia.Data.Collection;

namespace Test.Zinnia.Data.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;

    public class GameObjectRelationsTest
    {
        private GameObject containingObject;
        private GameObjectRelations subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectRelations>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetValueByKey()
        {
            UnityEventListenerMock valueRetrievedListenerMock = new UnityEventListenerMock();
            subject.ValueRetreieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelations.Relation relationOne = new GameObjectRelations.Relation() { key = keyOne, value = valueOne };
            GameObjectRelations.Relation relationTwo = new GameObjectRelations.Relation() { key = keyTwo, value = valueTwo };
            GameObjectRelations.Relation relationThree = new GameObjectRelations.Relation() { key = keyThree, value = valueThree };

            subject.relations = new List<GameObjectRelations.Relation>() { relationOne, relationTwo, relationThree };

            Assert.IsFalse(valueRetrievedListenerMock.Received);

            Assert.AreEqual(valueTwo, subject.GetValue(keyTwo));

            Assert.IsTrue(valueRetrievedListenerMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(keyTwo);
            Object.DestroyImmediate(valueTwo);
            Object.DestroyImmediate(keyThree);
            Object.DestroyImmediate(valueThree);
        }

        [Test]
        public void GetValueByKeyNotFound()
        {
            UnityEventListenerMock valueRetrievedListenerMock = new UnityEventListenerMock();
            subject.ValueRetreieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelations.Relation relationOne = new GameObjectRelations.Relation() { key = keyOne, value = valueOne };
            GameObjectRelations.Relation relationThree = new GameObjectRelations.Relation() { key = keyThree, value = valueThree };

            subject.relations = new List<GameObjectRelations.Relation>() { relationOne, relationThree };

            Assert.IsFalse(valueRetrievedListenerMock.Received);

            Assert.IsNull(subject.GetValue(keyTwo));

            Assert.IsFalse(valueRetrievedListenerMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(keyTwo);
            Object.DestroyImmediate(valueTwo);
            Object.DestroyImmediate(keyThree);
            Object.DestroyImmediate(valueThree);
        }

        [Test]
        public void GetValueByIndex()
        {
            UnityEventListenerMock valueRetrievedListenerMock = new UnityEventListenerMock();
            subject.ValueRetreieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelations.Relation relationOne = new GameObjectRelations.Relation() { key = keyOne, value = valueOne };
            GameObjectRelations.Relation relationTwo = new GameObjectRelations.Relation() { key = keyTwo, value = valueTwo };
            GameObjectRelations.Relation relationThree = new GameObjectRelations.Relation() { key = keyThree, value = valueThree };

            subject.relations = new List<GameObjectRelations.Relation>() { relationOne, relationTwo, relationThree };

            Assert.IsFalse(valueRetrievedListenerMock.Received);

            Assert.AreEqual(valueTwo, subject.GetValue(1));

            Assert.IsTrue(valueRetrievedListenerMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(keyTwo);
            Object.DestroyImmediate(valueTwo);
            Object.DestroyImmediate(keyThree);
            Object.DestroyImmediate(valueThree);
        }

        [Test]
        public void GetValueByIndexNotFound()
        {
            UnityEventListenerMock valueRetrievedListenerMock = new UnityEventListenerMock();
            subject.ValueRetreieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelations.Relation relationOne = new GameObjectRelations.Relation() { key = keyOne, value = valueOne };
            GameObjectRelations.Relation relationThree = new GameObjectRelations.Relation() { key = keyThree, value = valueThree };

            subject.relations = new List<GameObjectRelations.Relation>() { relationOne, relationThree };

            Assert.IsFalse(valueRetrievedListenerMock.Received);

            Assert.IsNull(subject.GetValue(2));

            Assert.IsFalse(valueRetrievedListenerMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(keyTwo);
            Object.DestroyImmediate(valueTwo);
            Object.DestroyImmediate(keyThree);
            Object.DestroyImmediate(valueThree);
        }
    }
}