using Zinnia.Action;
using Zinnia.Action.Collection;

namespace Test.Zinnia.Action.Collection
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class ActionRegistrarSourceObservableListTest
    {
        private GameObject containingObject;
        private ActionRegistrarSourceObservableList subject;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ActionRegistrarSourceObservableList>();
            yield return null;
        }

        [TearDown]
        public void TearDown()
        {
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
                Enabled = false,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = false,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            subject.Add(oneActionSource);
            subject.Add(twoActionSource);

            Assert.IsFalse(subject.NonSubscribableElements[0].Enabled);
            Assert.IsFalse(subject.NonSubscribableElements[1].Enabled);

            subject.EnableSource(oneSourceActionObject);

            Assert.IsTrue(subject.NonSubscribableElements[0].Enabled);
            Assert.IsFalse(subject.NonSubscribableElements[1].Enabled);

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

            subject.Add(oneActionSource);
            subject.Add(twoActionSource);

            Assert.IsTrue(subject.NonSubscribableElements[0].Enabled);
            Assert.IsTrue(subject.NonSubscribableElements[1].Enabled);

            subject.DisableSource(oneSourceActionObject);

            Assert.IsFalse(subject.NonSubscribableElements[0].Enabled);
            Assert.IsTrue(subject.NonSubscribableElements[1].Enabled);

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
                Enabled = false,
                Container = oneSourceActionObject,
                Action = oneSourceAction
            };

            ActionRegistrar.ActionSource twoActionSource = new ActionRegistrar.ActionSource
            {
                Enabled = false,
                Container = twoSourceActionObject,
                Action = twoSourceAction
            };

            subject.Add(oneActionSource);
            subject.Add(twoActionSource);

            Assert.IsFalse(subject.NonSubscribableElements[0].Enabled);
            Assert.IsFalse(subject.NonSubscribableElements[1].Enabled);

            subject.EnableAllSources();

            Assert.IsTrue(subject.NonSubscribableElements[0].Enabled);
            Assert.IsTrue(subject.NonSubscribableElements[1].Enabled);

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

            subject.Add(oneActionSource);
            subject.Add(twoActionSource);

            Assert.IsTrue(subject.NonSubscribableElements[0].Enabled);
            Assert.IsTrue(subject.NonSubscribableElements[1].Enabled);

            subject.DisableAllSources();

            Assert.IsFalse(subject.NonSubscribableElements[0].Enabled);
            Assert.IsFalse(subject.NonSubscribableElements[1].Enabled);

            Object.DestroyImmediate(oneSourceActionObject);
            Object.DestroyImmediate(twoSourceActionObject);
        }
    }
}