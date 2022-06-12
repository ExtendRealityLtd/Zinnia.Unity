using Zinnia.Action;
using Zinnia.Action.Collection;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Action
{
    using NUnit.Framework;
    using System.Collections;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActionRegistrarTest
    {
        private GameObject containingObject;
        private ActionRegistrar subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);

            ActionRegistrarSourceObservableList sources = containingObject.AddComponent<ActionRegistrarSourceObservableList>();
            GameObjectObservableList limits = containingObject.AddComponent<GameObjectObservableList>();

            subject = containingObject.AddComponent<ActionRegistrar>();
            subject.enabled = false;
            subject.Sources = sources;
            subject.SourceLimits = limits;

            containingObject.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator RegisterOnEnable()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            subject.Target = targetAction;
            subject.Sources.Add(oneActionSource);
            subject.Sources.Add(twoActionSource);
            subject.SourceLimits.Add(null);

            Assert.AreEqual(0, targetAction.ReadOnlySources.Count);

            subject.enabled = true;
            yield return null;

            Assert.AreEqual(2, targetAction.ReadOnlySources.Count);
            Assert.AreEqual(1, subject.SourceLimits.SubscribableElements.Count);
            Assert.IsNull(subject.SourceLimits.SubscribableElements[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [UnityTest]
        public IEnumerator RegisterSpecific()
        {
            subject.enabled = true;
            yield return null;

            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            subject.Target = targetAction;
            subject.Sources.Add(oneActionSource);
            subject.Sources.Add(twoActionSource);

            Assert.AreEqual(0, targetAction.ReadOnlySources.Count);

            subject.SourceLimits.Add(twoSourceActionObject);

            Assert.AreEqual(1, targetAction.ReadOnlySources.Count);
            Assert.AreEqual(twoSourceAction, targetAction.ReadOnlySources[0]);
            Assert.AreEqual(twoSourceActionObject, subject.SourceLimits.SubscribableElements[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [UnityTest]
        public IEnumerator RegisterOnlyEnabled()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = false,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            subject.Target = targetAction;
            subject.Sources.Add(oneActionSource);
            subject.Sources.Add(twoActionSource);
            subject.SourceLimits.Add(null);

            Assert.AreEqual(0, targetAction.ReadOnlySources.Count);

            subject.enabled = true;
            yield return null;

            Assert.AreEqual(1, targetAction.ReadOnlySources.Count);
            Assert.AreEqual(1, subject.SourceLimits.SubscribableElements.Count);
            Assert.IsNull(subject.SourceLimits.SubscribableElements[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [UnityTest]
        public IEnumerator Unregister()
        {
            UnityEventListenerMock actionAddedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock actionRemovedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock limitRegisteredListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock limitUnregisteredListenerMock = new UnityEventListenerMock();

            subject.ActionAdded.AddListener(actionAddedListenerMock.Listen);
            subject.ActionRemoved.AddListener(actionRemovedListenerMock.Listen);
            subject.LimitRegistered.AddListener(limitRegisteredListenerMock.Listen);
            subject.LimitUnregistered.AddListener(limitUnregisteredListenerMock.Listen);

            subject.enabled = true;
            yield return null;

            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            Assert.IsFalse(actionAddedListenerMock.Received);
            Assert.IsFalse(actionRemovedListenerMock.Received);
            Assert.IsFalse(limitRegisteredListenerMock.Received);
            Assert.IsFalse(limitUnregisteredListenerMock.Received);

            subject.Target = targetAction;
            subject.Sources.Add(oneActionSource);
            subject.Sources.Add(twoActionSource);

            subject.SourceLimits.Add(oneSourceActionObject);
            subject.SourceLimits.Add(twoSourceActionObject);

            Assert.IsTrue(actionAddedListenerMock.Received);
            Assert.IsFalse(actionRemovedListenerMock.Received);
            Assert.IsTrue(limitRegisteredListenerMock.Received);
            Assert.IsFalse(limitUnregisteredListenerMock.Received);

            actionAddedListenerMock.Reset();
            actionRemovedListenerMock.Reset();
            limitRegisteredListenerMock.Reset();
            limitUnregisteredListenerMock.Reset();

            Assert.AreEqual(2, targetAction.ReadOnlySources.Count);

            subject.SourceLimits.Remove(oneSourceActionObject);

            Assert.IsFalse(actionAddedListenerMock.Received);
            Assert.IsTrue(actionRemovedListenerMock.Received);
            Assert.IsFalse(limitRegisteredListenerMock.Received);
            Assert.IsTrue(limitUnregisteredListenerMock.Received);

            Assert.AreEqual(1, targetAction.ReadOnlySources.Count);
            Assert.AreEqual(twoSourceAction, targetAction.ReadOnlySources[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [UnityTest]
        public IEnumerator UnregisterEvenIfDisabled()
        {
            subject.enabled = true;
            yield return null;

            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = true,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            subject.Target = targetAction;
            subject.Sources.Add(oneActionSource);
            subject.Sources.Add(twoActionSource);
            subject.SourceLimits.Add(oneSourceActionObject);
            subject.SourceLimits.Add(twoSourceActionObject);

            Assert.AreEqual(2, targetAction.ReadOnlySources.Count);

            oneActionSource.Enabled = false;

            subject.SourceLimits.Remove(oneSourceActionObject);

            Assert.AreEqual(1, targetAction.ReadOnlySources.Count);
            Assert.AreEqual(twoSourceAction, targetAction.ReadOnlySources[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }
    }
}