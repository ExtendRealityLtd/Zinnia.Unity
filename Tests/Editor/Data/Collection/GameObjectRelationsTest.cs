using Zinnia.Data.Collection;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Data.Collection
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

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
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void GetValueByKey()
        {
            UnityEventListenerMock valueRetrievedListenerMock = new UnityEventListenerMock();
            subject.ValueRetrieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelationObservableList relations = containingObject.AddComponent<GameObjectRelationObservableList>();
            subject.Relations = relations;

            GameObjectRelationObservableList.Relation relationOne = new GameObjectRelationObservableList.Relation() { Key = keyOne, Value = valueOne };
            GameObjectRelationObservableList.Relation relationTwo = new GameObjectRelationObservableList.Relation() { Key = keyTwo, Value = valueTwo };
            GameObjectRelationObservableList.Relation relationThree = new GameObjectRelationObservableList.Relation() { Key = keyThree, Value = valueThree };


            subject.Relations.Add(relationOne);
            subject.Relations.Add(relationTwo);
            subject.Relations.Add(relationThree);

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
            subject.ValueRetrieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelationObservableList relations = containingObject.AddComponent<GameObjectRelationObservableList>();
            subject.Relations = relations;

            GameObjectRelationObservableList.Relation relationOne = new GameObjectRelationObservableList.Relation() { Key = keyOne, Value = valueOne };
            GameObjectRelationObservableList.Relation relationThree = new GameObjectRelationObservableList.Relation() { Key = keyThree, Value = valueThree };

            subject.Relations.Add(relationOne);
            subject.Relations.Add(relationThree);

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
            subject.ValueRetrieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelationObservableList relations = containingObject.AddComponent<GameObjectRelationObservableList>();
            subject.Relations = relations;

            GameObjectRelationObservableList.Relation relationOne = new GameObjectRelationObservableList.Relation() { Key = keyOne, Value = valueOne };
            GameObjectRelationObservableList.Relation relationTwo = new GameObjectRelationObservableList.Relation() { Key = keyTwo, Value = valueTwo };
            GameObjectRelationObservableList.Relation relationThree = new GameObjectRelationObservableList.Relation() { Key = keyThree, Value = valueThree };

            subject.Relations.Add(relationOne);
            subject.Relations.Add(relationTwo);
            subject.Relations.Add(relationThree);

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
            subject.ValueRetrieved.AddListener(valueRetrievedListenerMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject keyTwo = new GameObject();
            GameObject valueTwo = new GameObject();
            GameObject keyThree = new GameObject();
            GameObject valueThree = new GameObject();

            GameObjectRelationObservableList relations = containingObject.AddComponent<GameObjectRelationObservableList>();
            subject.Relations = relations;

            GameObjectRelationObservableList.Relation relationOne = new GameObjectRelationObservableList.Relation() { Key = keyOne, Value = valueOne };
            GameObjectRelationObservableList.Relation relationThree = new GameObjectRelationObservableList.Relation() { Key = keyThree, Value = valueThree };

            subject.Relations.Add(relationOne);
            subject.Relations.Add(relationThree);

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