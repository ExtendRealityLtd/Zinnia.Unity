using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Data.Collection.List
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class GameObjectMultiRelationObservableListTest
    {
        private GameObject containingObject;
        private GameObjectMultiRelationObservableList subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectMultiRelationObservableList>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void HasRelationship()
        {
            UnityEventListenerMock relationshipFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock relationshipNotFoundMock = new UnityEventListenerMock();
            subject.RelationshipFound.AddListener(relationshipFoundMock.Listen);
            subject.RelationshipNotFound.AddListener(relationshipNotFoundMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject valueTwo = new GameObject();

            GameObject keyTwo = new GameObject();
            GameObject valueThree = new GameObject();

            GameObject keyThree = new GameObject();

            GameObjectMultiRelationObservableList.MultiRelation relationOne = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = keyOne,
                Values = new List<GameObject>() { valueOne, valueTwo }
            };

            GameObjectMultiRelationObservableList.MultiRelation relationTwo = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = keyTwo,
                Values = new List<GameObject>() { valueThree }
            };

            subject.Add(relationOne);
            subject.Add(relationTwo);

            Assert.IsFalse(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            Assert.IsTrue(subject.HasRelationship(keyOne, out List<GameObject> resultsOne));
            Assert.AreEqual(valueOne, resultsOne[0]);
            Assert.AreEqual(valueTwo, resultsOne[1]);
            Assert.IsTrue(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            relationshipFoundMock.Reset();
            relationshipNotFoundMock.Reset();

            Assert.IsTrue(subject.HasRelationship(keyTwo, out List<GameObject> resultsTwo));
            Assert.AreEqual(valueThree, resultsTwo[0]);
            Assert.IsTrue(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            relationshipFoundMock.Reset();
            relationshipNotFoundMock.Reset();

            Assert.IsFalse(subject.HasRelationship(keyThree, out List<GameObject> resultsThree));
            Assert.IsNull(resultsThree);
            Assert.IsFalse(relationshipFoundMock.Received);
            Assert.IsTrue(relationshipNotFoundMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(valueTwo);
            Object.DestroyImmediate(keyTwo);
            Object.DestroyImmediate(valueThree);
            Object.DestroyImmediate(keyThree);
        }

        [Test]
        public void HasRelationshipInactiveGameObject()
        {
            UnityEventListenerMock relationshipFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock relationshipNotFoundMock = new UnityEventListenerMock();
            subject.RelationshipFound.AddListener(relationshipFoundMock.Listen);
            subject.RelationshipNotFound.AddListener(relationshipNotFoundMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject valueTwo = new GameObject();

            GameObjectMultiRelationObservableList.MultiRelation relationOne = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = keyOne,
                Values = new List<GameObject>() { valueOne, valueTwo }
            };

            subject.Add(relationOne);

            subject.gameObject.SetActive(false);

            Assert.IsFalse(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            Assert.IsTrue(subject.HasRelationship(keyOne, out List<GameObject> resultsOne));
            Assert.AreEqual(valueOne, resultsOne[0]);
            Assert.AreEqual(valueTwo, resultsOne[1]);
            Assert.IsFalse(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(valueTwo);
        }

        [Test]
        public void HasRelationshipInactiveComponent()
        {
            UnityEventListenerMock relationshipFoundMock = new UnityEventListenerMock();
            UnityEventListenerMock relationshipNotFoundMock = new UnityEventListenerMock();
            subject.RelationshipFound.AddListener(relationshipFoundMock.Listen);
            subject.RelationshipNotFound.AddListener(relationshipNotFoundMock.Listen);

            GameObject keyOne = new GameObject();
            GameObject valueOne = new GameObject();
            GameObject valueTwo = new GameObject();

            GameObjectMultiRelationObservableList.MultiRelation relationOne = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = keyOne,
                Values = new List<GameObject>() { valueOne, valueTwo }
            };

            subject.Add(relationOne);

            subject.enabled = false;

            Assert.IsFalse(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            Assert.IsTrue(subject.HasRelationship(keyOne, out List<GameObject> resultsOne));
            Assert.AreEqual(valueOne, resultsOne[0]);
            Assert.AreEqual(valueTwo, resultsOne[1]);
            Assert.IsFalse(relationshipFoundMock.Received);
            Assert.IsFalse(relationshipNotFoundMock.Received);

            Object.DestroyImmediate(keyOne);
            Object.DestroyImmediate(valueOne);
            Object.DestroyImmediate(valueTwo);
        }
    }
}