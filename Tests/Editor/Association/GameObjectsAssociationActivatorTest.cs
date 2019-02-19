using Zinnia.Association;

namespace Test.Zinnia.Association
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
            foreach (GameObject gameObject in subject.Associations.SelectMany(association => association.gameObjects))
            {
                Object.DestroyImmediate(gameObject);
            }

            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ProcessActivates()
        {
            subject.wasActivateCalled = false;
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

            subject.Associations.Add(associationMock);

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

            subject.Associations.Add(associationMock);

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

            subject.Associations.Add(associationMock);

            LogAssert.Expect(LogType.Warning, new Regex("multiple association"));
            subject.ManualAwake();
        }

        [Test]
        public void OnEnableActivates()
        {
            subject.wasActivateCalled = false;
            subject.ManualOnEnable();
            Assert.IsTrue(subject.wasActivateCalled);
        }

        [Test]
        public void OnDisableDeactivates()
        {
            subject.wasDeactivateCalled = false;
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
