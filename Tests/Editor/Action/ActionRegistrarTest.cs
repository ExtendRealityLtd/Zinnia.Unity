using Zinnia.Action;

namespace Test.Zinnia.Action
{
    using UnityEngine;
    using NUnit.Framework;

    public class ActionRegistrarTest
    {
        private GameObject containingObject;
        private ActionRegistrarMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActionRegistrarMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void EnableSource()
        {
            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();
            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = false,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = false,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.registerOnEnable = false;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.IsFalse(subject.sources[0].enabled);
            Assert.IsFalse(subject.sources[1].enabled);

            subject.EnableSource(oneSourceActionObject);

            Assert.IsTrue(subject.sources[0].enabled);
            Assert.IsFalse(subject.sources[1].enabled);

            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void DisableSource()
        {
            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();
            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.registerOnEnable = false;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.IsTrue(subject.sources[0].enabled);
            Assert.IsTrue(subject.sources[1].enabled);

            subject.DisableSource(oneSourceActionObject);

            Assert.IsFalse(subject.sources[0].enabled);
            Assert.IsTrue(subject.sources[1].enabled);

            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void EnableAllSource()
        {
            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();
            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = false,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = false,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.registerOnEnable = false;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.IsFalse(subject.sources[0].enabled);
            Assert.IsFalse(subject.sources[1].enabled);

            subject.EnableAllSources();

            Assert.IsTrue(subject.sources[0].enabled);
            Assert.IsTrue(subject.sources[1].enabled);

            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void DisableAllSource()
        {
            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();
            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.registerOnEnable = false;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.IsTrue(subject.sources[0].enabled);
            Assert.IsTrue(subject.sources[1].enabled);

            subject.DisableAllSources();

            Assert.IsFalse(subject.sources[0].enabled);
            Assert.IsFalse(subject.sources[1].enabled);

            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void RegisterOnEnable()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.target = targetAction;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.AreEqual(0, targetAction.Sources.Count);

            subject.ManualOnEnable();

            Assert.AreEqual(2, targetAction.Sources.Count);
            Assert.IsNull(subject.SourceLimit);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void RegisterSpecific()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.registerOnEnable = false;
            subject.target = targetAction;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.AreEqual(0, targetAction.Sources.Count);

            subject.ManualOnEnable();
            subject.Register(twoSourceActionObject);

            Assert.AreEqual(1, targetAction.Sources.Count);
            Assert.AreEqual(twoSourceAction, targetAction.Sources[0]);
            Assert.AreEqual(twoSourceActionObject, subject.SourceLimit);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void RegisterOnlyEnabled()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = false,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.target = targetAction;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);

            Assert.AreEqual(0, targetAction.Sources.Count);

            subject.ManualOnEnable();

            Assert.AreEqual(1, targetAction.Sources.Count);
            Assert.IsNull(subject.SourceLimit);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void Unregister()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.target = targetAction;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);
            subject.ManualOnEnable();

            Assert.AreEqual(2, targetAction.Sources.Count);

            subject.Unregister(oneSourceActionObject);

            Assert.AreEqual(1, targetAction.Sources.Count);
            Assert.AreEqual(twoSourceAction, targetAction.Sources[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void UnregisterEvenIfDisabled()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.target = targetAction;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);
            subject.ManualOnEnable();

            Assert.AreEqual(2, targetAction.Sources.Count);

            oneActionSource.enabled = false;

            subject.Unregister(oneSourceActionObject);

            Assert.AreEqual(1, targetAction.Sources.Count);
            Assert.AreEqual(twoSourceAction, targetAction.Sources[0]);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }

        [Test]
        public void Clear()
        {
            GameObject targetActionObject = new GameObject();
            BooleanAction targetAction = targetActionObject.AddComponent<BooleanAction>();

            GameObject oneSourceActionObject = new GameObject();
            BooleanAction oneSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            GameObject twoSourceActionObject = new GameObject();
            BooleanAction twoSourceAction = oneSourceActionObject.AddComponent<BooleanAction>();

            ActionRegistrar.ActionSource oneActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = oneSourceActionObject,
                action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                enabled = true,
                container = twoSourceActionObject,
                action = twoSourceAction
            };

            subject.target = targetAction;
            subject.sources.Add(oneActionSource);
            subject.sources.Add(twoActionSource);
            subject.ManualOnEnable();

            Assert.AreEqual(2, targetAction.Sources.Count);

            subject.Clear();

            Assert.AreEqual(0, targetAction.Sources.Count);

            Object.DestroyImmediate(targetActionObject);
            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }
    }

    public class ActionRegistrarMock : ActionRegistrar
    {
        public virtual void ManualOnEnable()
        {
            OnEnable();
        }
    }
}