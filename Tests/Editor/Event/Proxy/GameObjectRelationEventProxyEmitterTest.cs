using Zinnia.Data.Collection.List;
using Zinnia.Event.Proxy;
using Zinnia.Rule;

namespace Test.Zinnia.Event.Proxy
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class GameObjectRelationEventProxyEmitterTest
    {
        private GameObject containingObject;
        private GameObjectRelationEventProxyEmitter subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("GameObjectRelationEventProxyEmitterTest");
            subject = containingObject.AddComponent<GameObjectRelationEventProxyEmitter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Receive()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            GameObject relationKey = new GameObject("GameObjectRelationEventProxyEmitterTest_Key");
            GameObject relationValue = new GameObject("GameObjectRelationEventProxyEmitterTest_Value");
            GameObjectRelationObservableList.Relation relation = new GameObjectRelationObservableList.Relation();
            relation.Key = relationKey;
            relation.Value = relationValue;

            Assert.IsFalse(emittedMock.Received);
            subject.Receive(relation);
            Assert.AreEqual(relation, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            Object.DestroyImmediate(relationKey);
            Object.DestroyImmediate(relationValue);
        }

        [Test]
        public void ReceiveWithRuleRestrictions()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            GameObject validRelationKey = new GameObject("GameObjectRelationEventProxyEmitterTest_Key");
            GameObject validRelationValue = new GameObject("GameObjectRelationEventProxyEmitterTest_Value");
            GameObjectRelationObservableList.Relation validRelation = new GameObjectRelationObservableList.Relation();
            validRelation.Key = validRelationKey;
            validRelation.Value = validRelationValue;

            GameObject invalidRelationKey = new GameObject("GameObjectRelationEventProxyEmitterTest_Key");
            GameObject invalidRelationValue = new GameObject("GameObjectRelationEventProxyEmitterTest_Value");
            GameObjectRelationObservableList.Relation invalidRelation = new GameObjectRelationObservableList.Relation();
            invalidRelation.Key = invalidRelationKey;
            invalidRelation.Value = invalidRelationValue;

            ListContainsRule rule = subject.gameObject.AddComponent<ListContainsRule>();
            UnityObjectObservableList objects = containingObject.AddComponent<UnityObjectObservableList>();
            rule.Objects = objects;

            objects.Add(validRelationKey);
            subject.ReceiveValidity = new RuleContainer
            {
                Interface = rule
            };

            subject.RuleSource = GameObjectRelationEventProxyEmitter.RuleSourceType.Key;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(validRelation);

            Assert.AreEqual(validRelation, subject.Payload);
            Assert.IsTrue(emittedMock.Received);

            emittedMock.Reset();

            Assert.IsFalse(emittedMock.Received);

            subject.Receive(invalidRelation);

            Assert.AreEqual(validRelation, subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(validRelationKey);
            Object.DestroyImmediate(validRelationValue);
            Object.DestroyImmediate(invalidRelationKey);
            Object.DestroyImmediate(invalidRelationValue);
        }

        [Test]
        public void ReceiveInactiveGameObject()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            GameObject relationKey = new GameObject("GameObjectRelationEventProxyEmitterTest_Key");
            GameObject relationValue = new GameObject("GameObjectRelationEventProxyEmitterTest_Value");
            GameObjectRelationObservableList.Relation relation = new GameObjectRelationObservableList.Relation();
            relation.Key = relationKey;
            relation.Value = relationValue;

            subject.gameObject.SetActive(false);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(relation);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(relationKey);
            Object.DestroyImmediate(relationValue);
        }

        [Test]
        public void ReceiveInactiveComponent()
        {
            UnityEventListenerMock emittedMock = new UnityEventListenerMock();
            subject.Emitted.AddListener(emittedMock.Listen);
            GameObject relationKey = new GameObject("GameObjectRelationEventProxyEmitterTest_Key");
            GameObject relationValue = new GameObject("GameObjectRelationEventProxyEmitterTest_Value");
            GameObjectRelationObservableList.Relation relation = new GameObjectRelationObservableList.Relation();
            relation.Key = relationKey;
            relation.Value = relationValue;

            subject.enabled = false;

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            subject.Receive(relation);

            Assert.IsNull(subject.Payload);
            Assert.IsFalse(emittedMock.Received);

            Object.DestroyImmediate(relationKey);
            Object.DestroyImmediate(relationValue);
        }
    }
}