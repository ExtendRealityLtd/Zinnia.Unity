using Zinnia.Association;
using Zinnia.Association.Collection;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Association
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using NUnit.Framework;
    using System.Text.RegularExpressions;
    using Assert = UnityEngine.Assertions.Assert;

    public class GameObjectsAssociationActivatorTest
    {
        private GameObject containingObject;
        private GameObjectsAssociationActivatorMock subject;
        private GameObjectsAssociationObservableList associations;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<GameObjectsAssociationActivatorMock>();
            associations = containingObject.AddComponent<GameObjectsAssociationObservableList>();
            subject.Associations = associations;
            containingObject.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (GameObjectsAssociation association in subject.Associations.NonSubscribableElements)
            {
                foreach (GameObject associatedObject in association.GameObjects.NonSubscribableElements)
                {
                    Object.Destroy(associatedObject);
                }
            }

            Object.Destroy(containingObject);
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
            associationMock.GameObjects = containingObject.AddComponent<GameObjectObservableList>();
            associationMock.GameObjects.Add(gameObject);
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
            associationMock.GameObjects = containingObject.AddComponent<GameObjectObservableList>();
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
            associationMock.GameObjects = containingObject.AddComponent<GameObjectObservableList>();
            associationMock.GameObjects.Add(new GameObject());
            associationMock.GameObjects.Add(new GameObject());

            subject.Associations.Add(associationMock);

            Debug.Log("This test is expecting a warning to be logged next.");
            LogAssert.Expect(LogType.Warning, new Regex("multiple association"));
            subject.ManualAwake();
            Debug.Log("Warning log recognized, the test is successful.");
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
