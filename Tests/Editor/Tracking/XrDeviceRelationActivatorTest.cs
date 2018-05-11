namespace VRTK.Core.Tracking
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class XrDeviceRelationActivatorTest
    {
        private GameObject containingObject;
        private XrDeviceRelationActivatorMock subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<XrDeviceRelationActivatorMock>();
        }

        [TearDown]
        public void TearDown()
        {
            if (subject.relations != null)
            {
                foreach (GameObject gameObject in subject.relations.SelectMany(relation => relation.gameObjects))
                {
                    UnityEngine.Object.DestroyImmediate(gameObject);
                }
            }

            UnityEngine.Object.DestroyImmediate(subject);
            UnityEngine.Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void AwakeLogsWarningForMultipleActiveGameObjects()
        {
            const string xrDeviceName = "XR Device";

            subject.xrDeviceName = xrDeviceName;
            subject.relations = new[]
            {
                new XrDeviceRelationActivator.XrDeviceRelation
                {
                    xrDeviceName = xrDeviceName,
                    gameObjects = new[]
                    {
                        new GameObject()
                    }
                },
                new XrDeviceRelationActivator.XrDeviceRelation
                {
                    xrDeviceName = xrDeviceName,
                    gameObjects = new[]
                    {
                        new GameObject()
                    }
                }
            };

            LogAssert.Expect(LogType.Warning, new Regex("multiple relation"));
            subject.ManualAwake();
        }

        [Test]
        public void OnDisableDeactivates()
        {
            Assert.IsFalse(subject.wasDeactivateCalled);
            subject.ManualOnDisable();
            Assert.IsTrue(subject.wasDeactivateCalled);
        }

        [Test]
        public void ActivatesFirstListEntryThatMatchesDevice()
        {
            const string expectedXrDeviceName = "Expected Device";
            const string unexpectedXrDeviceName = "Unexpected Device";
            XrDeviceRelationActivator.XrDeviceRelation unexpectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = unexpectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };
            XrDeviceRelationActivator.XrDeviceRelation expectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = expectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = expectedXrDeviceName;
            subject.relations = new[]
            {
                unexpectedRelation,
                expectedRelation
            };

            Assert.AreEqual(subject.CurrentRelation, null);

            subject.ManualUpdate();

            Assert.AreEqual(subject.CurrentRelation, expectedRelation);
            Assert.AreNotEqual(subject.CurrentRelation, unexpectedRelation);

            Assert.IsTrue(subject.CurrentRelation.gameObjects.All(relationObject => relationObject.activeInHierarchy));
            Assert.IsTrue(
                subject.relations.Except(
                        new[]
                        {
                            subject.CurrentRelation
                        })
                    .All(relation => relation.gameObjects.All(relationObject => !relationObject.activeInHierarchy)));
        }

        [Test]
        public void ManualActivateOfValidListEntry()
        {
            const string xrDeviceName = "XR Device";
            XrDeviceRelationActivator.XrDeviceRelation unexpectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = xrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };
            XrDeviceRelationActivator.XrDeviceRelation expectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = xrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = xrDeviceName;
            subject.relations = new[]
            {
                unexpectedRelation,
                expectedRelation
            };

            subject.ManualUpdate();
            subject.Activate(expectedRelation);

            Assert.AreEqual(subject.CurrentRelation, expectedRelation);
            Assert.AreNotEqual(subject.CurrentRelation, unexpectedRelation);
        }

        [Test]
        public void ManualActivateOfInvalidListEntry()
        {
            const string expectedXrDeviceName = "Expected Device";
            const string unexpectedXrDeviceName = "Unexpected Device";
            XrDeviceRelationActivator.XrDeviceRelation unexpectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = unexpectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };
            XrDeviceRelationActivator.XrDeviceRelation expectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = expectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = expectedXrDeviceName;
            subject.relations = new[]
            {
                unexpectedRelation,
                expectedRelation
            };

            subject.ManualUpdate();

            Assert.Throws<ArgumentException>(() => subject.Activate(unexpectedRelation));
            Assert.AreEqual(subject.CurrentRelation, expectedRelation);
            Assert.AreNotEqual(subject.CurrentRelation, unexpectedRelation);
        }

        [Test]
        public void ManualActivateOfValidUnknownRelation()
        {
            const string xrDeviceName = "XR Device";
            XrDeviceRelationActivator.XrDeviceRelation unexpectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = xrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };
            XrDeviceRelationActivator.XrDeviceRelation expectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = xrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = xrDeviceName;
            subject.relations = new[]
            {
                unexpectedRelation
            };

            subject.ManualUpdate();
            subject.Activate(expectedRelation);

            Assert.AreEqual(subject.CurrentRelation, expectedRelation);
            Assert.AreNotEqual(subject.CurrentRelation, unexpectedRelation);
        }

        [Test]
        public void ManualActivateOfInvalidUnknownRelation()
        {
            const string expectedXrDeviceName = "Expected Device";
            const string unexpectedXrDeviceName = "Unexpected Device";
            XrDeviceRelationActivator.XrDeviceRelation unexpectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = unexpectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };
            XrDeviceRelationActivator.XrDeviceRelation expectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = expectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = expectedXrDeviceName;
            subject.relations = new[]
            {
                expectedRelation
            };

            subject.ManualUpdate();

            Assert.Throws<ArgumentException>(() => subject.Activate(unexpectedRelation));
            Assert.AreEqual(subject.CurrentRelation, expectedRelation);
            Assert.AreNotEqual(subject.CurrentRelation, unexpectedRelation);
        }

        [Test]
        public void ActivatesFirstListEntryThatMatchesDeviceAfterLoadedXrDeviceChanged()
        {
            const string expectedXrDeviceName = "Expected Device";
            const string unexpectedXrDeviceName = "Unexpected Device";
            XrDeviceRelationActivator.XrDeviceRelation unexpectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = unexpectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };
            XrDeviceRelationActivator.XrDeviceRelation expectedRelation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = expectedXrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = unexpectedXrDeviceName;
            subject.relations = new[]
            {
                unexpectedRelation,
                expectedRelation
            };

            subject.ManualUpdate();
            subject.xrDeviceName = expectedXrDeviceName;
            subject.ManualUpdate();

            Assert.AreEqual(subject.CurrentRelation, expectedRelation);
            Assert.AreNotEqual(subject.CurrentRelation, unexpectedRelation);
        }

        [Test]
        public void DeactivateDeactivatesAllListEntries()
        {
            const string xrDeviceName = "XR Device";
            XrDeviceRelationActivator.XrDeviceRelation relation = new XrDeviceRelationActivator.XrDeviceRelation
            {
                xrDeviceName = xrDeviceName,
                gameObjects = new[]
                {
                    new GameObject()
                }
            };

            subject.xrDeviceName = xrDeviceName;
            subject.relations = new[]
            {
                relation
            };

            subject.ManualUpdate();
            subject.ManualOnDisable();

            Assert.AreEqual(subject.CurrentRelation, null);
            Assert.AreNotEqual(subject.CurrentRelation, relation);
            Assert.IsTrue(subject.relations.All(deviceRelation => deviceRelation.gameObjects.All(gameObject => !gameObject.activeInHierarchy)));
        }
    }

    public class XrDeviceRelationActivatorMock : XrDeviceRelationActivator
    {
        public string xrDeviceName;
        public bool wasDeactivateCalled;

        public XrDeviceRelation CurrentRelation
        {
            get
            {
                return currentRelation;
            }
        }

        public void ManualAwake()
        {
            Awake();
        }

        public void ManualOnDisable()
        {
            OnDisable();
        }

        public void ManualUpdate()
        {
            Update();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            wasDeactivateCalled = true;
        }

        protected override string GetLoadedDeviceName()
        {
            return xrDeviceName;
        }
    }
}
