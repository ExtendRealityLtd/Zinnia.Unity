using VRTK.Core.Association;
using VRTK.Core.Extension;

namespace Test.VRTK.Core.Association
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class GameObjectsAssociationActivatorTest
    {
        private GameObject containingObject;
        private GameObjectsAssociationActivatorMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<GameObjectsAssociationActivatorMock>();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject gameObject in subject.associations.EmptyIfNull()
                .SelectMany(association => association.gameObjects.EmptyIfNull()))
            {
                Object.DestroyImmediate(gameObject);
            }

            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ProcessActivates()
        {
            Assert.IsFalse(subject.wasActivateCalled);
            subject.Process();
            Assert.IsTrue(subject.wasActivateCalled);
        }

        [Test]
        public void SetsGameObjectsActivationState()
        {
            GameObject gameObject = new GameObject();
            gameObject.SetActive(false);

            GameObjectsAssociationMock associationMock = containingObject.AddComponent<GameObjectsAssociationMock>();
            associationMock.gameObjects.Add(gameObject);
            associationMock.shouldBeActive = true;

            subject.associations.Add(associationMock);

            subject.Activate();
            Assert.IsTrue(gameObject.activeSelf);

            subject.Deactivate();
            Assert.IsFalse(gameObject.activeSelf);
        }

        [Test]
        public void CurrentAssociationUpdatesProperly()
        {
            Assert.IsNull(subject.CurrentAssociation);

            GameObjectsAssociationMock associationMock = containingObject.AddComponent<GameObjectsAssociationMock>();
            associationMock.shouldBeActive = true;

            subject.associations.Add(associationMock);

            subject.Activate();
            Assert.AreEqual(associationMock, subject.CurrentAssociation);

            subject.Deactivate();
            Assert.IsNull(subject.CurrentAssociation);
        }

        [Test]
        public void AwakeLogsWarningForMultipleActiveGameObjects()
        {
            GameObjectsAssociationMock associationMock = containingObject.AddComponent<GameObjectsAssociationMock>();
            associationMock.gameObjects.Add(new GameObject());
            associationMock.gameObjects.Add(new GameObject());

            subject.associations.Add(associationMock);

            LogAssert.Expect(LogType.Warning, new Regex("multiple association"));
            subject.ManualAwake();
        }

        [Test]
        public void OnEnableActivates()
        {
            Assert.IsFalse(subject.wasActivateCalled);
            subject.ManualOnEnable();
            Assert.IsTrue(subject.wasActivateCalled);
        }

        [Test]
        public void OnDisableDeactivates()
        {
            Assert.IsFalse(subject.wasDeactivateCalled);
            subject.ManualOnDisable();
            Assert.IsTrue(subject.wasDeactivateCalled);
        }
    }

    public class GameObjectsAssociationActivatorMock : GameObjectsAssociationActivator
    {
        public bool wasActivateCalled;
        public bool wasDeactivateCalled;

        public void ManualAwake()
        {
            Awake();
        }

        public void ManualOnEnable()
        {
            OnEnable();
        }

        public void ManualOnDisable()
        {
            OnDisable();
        }

        public override void Activate()
        {
            base.Activate();
            wasActivateCalled = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            wasDeactivateCalled = true;
        }
    }

    public class GameObjectsAssociationMock : GameObjectsAssociation
    {
        public bool shouldBeActive;

        public override bool ShouldBeActive()
        {
            return shouldBeActive;
        }
    }
}
