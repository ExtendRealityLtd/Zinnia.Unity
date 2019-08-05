using Zinnia.Pointer;
using Zinnia.Cast;

namespace Test.Zinnia.Pointer
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class ObjectPointerTest
    {
        private GameObject containingObject;
        private GameObject validOriginContainer;
        private GameObject validOriginMesh;
        private GameObject invalidOriginContainer;
        private GameObject invalidOriginMesh;
        private GameObject validSegmentContainer;
        private GameObject validSegmentMesh;
        private GameObject invalidSegmentContainer;
        private GameObject invalidSegmentMesh;
        private GameObject validDestinationContainer;
        private GameObject validDestinationMesh;
        private GameObject invalidDestinationContainer;
        private GameObject invalidDestinationMesh;
        private ObjectPointerMock subject;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject("containingObject");
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<ObjectPointerMock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(validOriginContainer);
            Object.DestroyImmediate(invalidOriginContainer);
            Object.DestroyImmediate(validSegmentContainer);
            Object.DestroyImmediate(invalidSegmentContainer);
            Object.DestroyImmediate(validDestinationContainer);
            Object.DestroyImmediate(invalidDestinationContainer);
            Physics.autoSimulation = true;
        }

        protected virtual void SetUpElements()
        {
            validOriginContainer = new GameObject("validOriginContainer");
            validOriginMesh = new GameObject("validOriginMesh");
            validOriginMesh.transform.SetParent(validOriginContainer.transform);

            invalidOriginContainer = new GameObject("invalidOriginContainer");
            invalidOriginMesh = new GameObject("invalidOriginMesh");
            invalidOriginMesh.transform.SetParent(invalidOriginContainer.transform);

            validSegmentContainer = new GameObject("validSegmentContainer");
            validSegmentMesh = new GameObject("validSegmentMesh");
            validSegmentMesh.transform.SetParent(validSegmentContainer.transform);

            invalidSegmentContainer = new GameObject("invalidSegmentContainer");
            invalidSegmentMesh = new GameObject("invalidSegmentMesh");
            invalidSegmentMesh.transform.SetParent(invalidSegmentContainer.transform);

            validDestinationContainer = new GameObject("validDestinationContainer");
            validDestinationMesh = new GameObject("validDestinationMesh");
            validDestinationMesh.transform.SetParent(validDestinationContainer.transform);

            invalidDestinationContainer = new GameObject("invalidDestinationContainer");
            invalidDestinationMesh = new GameObject("invalidDestinationMesh");
            invalidDestinationMesh.transform.SetParent(invalidDestinationContainer.transform);

            PointerElement origin = containingObject.AddComponent<PointerElement>();
            PointerElement segment = containingObject.AddComponent<PointerElement>();
            PointerElement destination = containingObject.AddComponent<PointerElement>();

            origin.ValidElementContainer = validOriginContainer;
            origin.ValidMeshContainer = validOriginMesh;
            origin.InvalidMeshContainer = invalidOriginContainer;
            origin.InvalidElementContainer = invalidOriginMesh;

            segment.ValidElementContainer = validSegmentContainer;
            segment.ValidMeshContainer = validSegmentMesh;
            segment.InvalidMeshContainer = invalidSegmentContainer;
            segment.InvalidElementContainer = invalidSegmentMesh;

            destination.ValidElementContainer = validDestinationContainer;
            destination.ValidMeshContainer = validDestinationMesh;
            destination.InvalidMeshContainer = invalidDestinationContainer;
            destination.InvalidElementContainer = invalidDestinationMesh;

            subject.Origin = origin;
            subject.RepeatedSegment = segment;
            subject.Destination = destination;
            containingObject.SetActive(true);
        }

        [Test]
        public void ActivateAndDeactivate()
        {
            SetUpElements();

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            subject.ManualOnEnable();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            List<Vector3> castPoints = new List<Vector3>
            {
                Vector3.zero,
                blocker.transform.position
            };

            PointsCast.EventData straightCast = CastPoints(castPoints);

            subject.HandleData(straightCast);
            subject.Process();

            Assert.IsTrue(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsTrue(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsTrue(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            Object.DestroyImmediate(blocker);
        }


        [Test]
        public void Select()
        {
            SetUpElements();

            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock selectListenerMock = new UnityEventListenerMock();

            subject.Entered.AddListener(enterListenerMock.Listen);
            subject.Exited.AddListener(exitListenerMock.Listen);
            subject.Hovering.AddListener(hoverListenerMock.Listen);
            subject.Selected.AddListener(selectListenerMock.Listen);

            subject.ManualOnEnable();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsFalse(selectListenerMock.Received);

            subject.Activate();
            subject.Process();
            subject.Select();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsTrue(selectListenerMock.Received);
            Assert.IsNull(subject.HoverTarget);

            enterListenerMock.Reset();
            exitListenerMock.Reset();
            hoverListenerMock.Reset();
            selectListenerMock.Reset();

            //Now add a valid target that can be selected
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            List<Vector3> castPoints = new List<Vector3>
            {
                Vector3.zero,
                blocker.transform.position
            };

            PointsCast.EventData straightCast = CastPoints(castPoints, true, true, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);
            subject.Process();
            subject.Select();

            Assert.IsTrue(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsTrue(hoverListenerMock.Received);
            Assert.IsTrue(selectListenerMock.Received);
            Assert.AreEqual(blocker, subject.HoverTarget.CollisionData.transform.gameObject);
            Assert.AreEqual(blocker, subject.SelectedTarget.CollisionData.transform.gameObject);

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void NoSelectOnInvalidTarget()
        {
            SetUpElements();

            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock selectListenerMock = new UnityEventListenerMock();

            subject.Entered.AddListener(enterListenerMock.Listen);
            subject.Exited.AddListener(exitListenerMock.Listen);
            subject.Hovering.AddListener(hoverListenerMock.Listen);
            subject.Selected.AddListener(selectListenerMock.Listen);

            subject.ManualOnEnable();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsFalse(selectListenerMock.Received);

            subject.Activate();
            subject.Process();
            subject.Select();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsTrue(selectListenerMock.Received);
            Assert.IsNull(subject.HoverTarget);

            enterListenerMock.Reset();
            exitListenerMock.Reset();
            hoverListenerMock.Reset();
            selectListenerMock.Reset();

            //Now add a valid target that can be selected
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            List<Vector3> castPoints = new List<Vector3> { Vector3.zero, blocker.transform.position };
            PointsCast.EventData straightCast = CastPoints(castPoints, true, false, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);
            subject.Process();
            subject.Select();

            Assert.IsTrue(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsTrue(hoverListenerMock.Received);
            Assert.IsTrue(selectListenerMock.Received);
            Assert.AreEqual(blocker, subject.HoverTarget.CollisionData.transform.gameObject);
            Assert.IsNull(subject.SelectedTarget);

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void NoActivateOnDisabledComponent()
        {
            SetUpElements();

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.ManualOnEnable();
            subject.enabled = false;
            subject.Activate();
            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(subject.IsActivated);
        }

        [Test]
        public void DeactivateOnDisableComponent()
        {
            SetUpElements();

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            subject.ManualOnEnable();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.ManualOnDisable();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void OriginAlwaysVisible()
        {
            SetUpElements();

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }


        [Test]
        public void SegmentAlwaysVisible()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void DestinationAlwaysVisible()
        {
            SetUpElements();

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void SegmentAndDestinationAlwaysVisible()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void ElementsAlwaysVisible()
        {
            SetUpElements();

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void OriginAlwaysHidden()
        {
            SetUpElements();

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void SegmentAlwaysHidden()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void DestinationAlwaysHidden()
        {
            SetUpElements();

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void SegmentAndDestinationAlwaysHidden()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void ElementsAlwaysHidden()
        {
            SetUpElements();

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsFalse(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsFalse(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsFalse(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void ActiveOnEnable()
        {
            SetUpElements();

            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            subject.Activated.AddListener(activatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);

            subject.ManualOnEnable();
            subject.Activate();

            Assert.IsTrue(activatedListenerMock.Received);

            Assert.IsFalse(validOriginContainer.activeInHierarchy);
            Assert.IsTrue(invalidOriginContainer.activeInHierarchy);
            Assert.IsFalse(validSegmentContainer.activeInHierarchy);
            Assert.IsTrue(invalidSegmentContainer.activeInHierarchy);
            Assert.IsFalse(validDestinationContainer.activeInHierarchy);
            Assert.IsTrue(invalidDestinationContainer.activeInHierarchy);
        }

        [Test]
        public void EnterExitHover()
        {
            SetUpElements();

            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();

            subject.Entered.AddListener(enterListenerMock.Listen);
            subject.Exited.AddListener(exitListenerMock.Listen);
            subject.Hovering.AddListener(hoverListenerMock.Listen);

            subject.ManualOnEnable();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);

            subject.Activate();
            subject.Process();

            //No valid target so still should be false
            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsNull(subject.HoverTarget);

            //Place an object in the way to make a valid target
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.name = "blocker";
            blocker.transform.position = Vector3.forward * 5f;

            List<Vector3> castPoints = new List<Vector3> { Vector3.zero, blocker.transform.position };

            PointsCast.EventData straightCast;

            straightCast = CastPoints(castPoints, true, true, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);

            //The target should be entered and be hovered over
            Assert.IsTrue(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsTrue(hoverListenerMock.Received);
            Assert.AreEqual(blocker, subject.HoverTarget.CollisionData.transform.gameObject);

            enterListenerMock.Reset();
            hoverListenerMock.Reset();

            //Move the target
            blocker.transform.position = Vector3.left * 10f;

            straightCast = CastPoints(castPoints, false);

            subject.HandleData(straightCast);

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsTrue(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsNull(subject.HoverTarget);

            Object.DestroyImmediate(blocker);
        }

        protected static PointsCast.EventData CastPoints(List<Vector3> points, bool doesCollisionOccur = true, bool validHit = true, Ray? realRay = null)
        {
            if (doesCollisionOccur)
            {
                RaycastHit hit = new RaycastHit();
                if (realRay != null)
                {
                    Physics.autoSimulation = false;
                    Physics.Simulate(Time.fixedDeltaTime);
                    Physics.Raycast((Ray)realRay, out hit);
                    Physics.autoSimulation = true;
                }

                return new PointsCast.EventData
                {
                    HitData = hit,
                    IsValid = validHit,
                    Points = points
                };
            }
            else
            {
                return new PointsCast.EventData
                {
                    Points = points
                };
            }
        }
    }

    public class ObjectPointerMock : ObjectPointer
    {
        public void ManualOnEnable()
        {
            enabled = true;
            OnEnable();
        }

        public void ManualOnDisable()
        {
            enabled = false;
            OnDisable();
        }
    }
}