using Zinnia.Cast;
using Zinnia.Pointer;

namespace Test.Zinnia.Pointer
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
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
        private PointerElement origin;
        private PointerElement segment;
        private PointerElement destination;

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

            origin = containingObject.AddComponent<PointerElement>();
            segment = containingObject.AddComponent<PointerElement>();
            destination = containingObject.AddComponent<PointerElement>();

            origin.ValidElementContainer = validOriginContainer;
            origin.ValidMeshContainer = validOriginMesh;
            origin.InvalidElementContainer = invalidOriginContainer;
            origin.InvalidMeshContainer = invalidOriginMesh;

            segment.ValidElementContainer = validSegmentContainer;
            segment.ValidMeshContainer = validSegmentMesh;
            segment.InvalidElementContainer = invalidSegmentContainer;
            segment.InvalidMeshContainer = invalidSegmentMesh;

            destination.ValidElementContainer = validDestinationContainer;
            destination.ValidMeshContainer = validDestinationMesh;
            destination.InvalidElementContainer = invalidDestinationContainer;
            destination.InvalidMeshContainer = invalidDestinationMesh;

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

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

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

            Assert.IsTrue(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsTrue(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsTrue(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

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
            Assert.AreEqual(subject.transform, subject.HoverTarget.Transform);
            Assert.AreEqual(blocker, subject.HoverTarget.CollisionData.transform.gameObject);
            Assert.AreEqual(blocker, subject.SelectedTarget.CollisionData.transform.gameObject);
            Assert.AreEqual("{ Transform = containingObject (UnityEngine.Transform) | UseLocalValues = False | PositionOverride = (0.0, 0.0, 0.0) | RotationOverride = (0.0, 0.0, 0.0, 1.0) | ScaleOverride = (1.0, 1.0, 1.0) | Origin = (0.0, 0.0, 0.0) | Direction = (0.0, 0.0, 1.0) | CollisionData = { barycentricCoordinate = (1.0, 0.0, 0.0) | Collider = Cube (UnityEngine.BoxCollider) | Distance = 4.5 | Lightmap Coord = (0.0, 0.0) | Normal = (0.0, 0.0, -1.0) | Point = (0.0, 0.0, 4.5) | Rigidbody = [null] | Texture Coord = (0.0, 0.0) | Texture Coord2 = (0.0, 0.0) | Transform = Cube (UnityEngine.Transform) | Triangle Index = -1 } | IsCurrentlyActive = True | IsCurrentlyHovering = True | CurrentHoverDuration = " + subject.SelectedTarget.CurrentHoverDuration + " | CurrentPointsCastData = { HitData = { barycentricCoordinate = (1.0, 0.0, 0.0) | Collider = Cube (UnityEngine.BoxCollider) | Distance = 4.5 | Lightmap Coord = (0.0, 0.0) | Normal = (0.0, 0.0, -1.0) | Point = (0.0, 0.0, 4.5) | Rigidbody = [null] | Texture Coord = (0.0, 0.0) | Texture Coord2 = (0.0, 0.0) | Transform = Cube (UnityEngine.Transform) | Triangle Index = -1 } | IsValid = True } }", subject.SelectedTarget.ToString());

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void SelectOriginTransformOverride()
        {
            SetUpElements();

            GameObject originTransformOverride = new GameObject();
            subject.EventDataOriginTransformOverride = originTransformOverride;

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
            Assert.AreEqual(originTransformOverride.transform, subject.HoverTarget.Transform);
            Assert.AreEqual(blocker, subject.HoverTarget.CollisionData.transform.gameObject);
            Assert.AreEqual(blocker, subject.SelectedTarget.CollisionData.transform.gameObject);

            Object.DestroyImmediate(blocker);
            Object.DestroyImmediate(originTransformOverride);
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

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.ManualOnDisable();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void OriginAlwaysVisible()
        {
            SetUpElements();

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
        }


        [Test]
        public void SegmentAlwaysVisible()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void DestinationAlwaysVisible()
        {
            SetUpElements();

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void SegmentAndDestinationAlwaysVisible()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);
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

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void OriginAlwaysHidden()
        {
            SetUpElements();

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void SegmentAlwaysHidden()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void DestinationAlwaysHidden()
        {
            SetUpElements();

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
        }

        [Test]
        public void SegmentAndDestinationAlwaysHidden()
        {
            SetUpElements();

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
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

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsFalse(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsFalse(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);
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

            Assert.IsFalse(validOriginMesh.activeInHierarchy);
            Assert.IsTrue(invalidOriginMesh.activeInHierarchy);
            Assert.IsFalse(validSegmentMesh.activeInHierarchy);
            Assert.IsTrue(invalidSegmentMesh.activeInHierarchy);
            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);
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

        [Test]
        public void HideDestinationOnNoCollision()
        {
            SetUpElements();

            List<Vector3> castPoints = new List<Vector3>();
            PointsCast.EventData straightCast = CastPoints(castPoints, false);

            subject.EnableDestinationOnNoCollision = false;
            subject.ManualOnEnable();
            subject.Activate();
            subject.HandleData(new PointsCast.EventData());
            subject.Process();

            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            //Now add a valid target
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;
            castPoints = new List<Vector3>()
            {
                Vector3.zero,
                blocker.transform.position
            };

            straightCast = CastPoints(castPoints, true, true, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);
            subject.Process();

            Assert.IsTrue(validDestinationMesh.activeInHierarchy);
            Assert.IsFalse(invalidDestinationMesh.activeInHierarchy);

            //Now make it so the target is invalid
            straightCast = CastPoints(castPoints, true, false, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);
            subject.Process();

            Assert.IsFalse(validDestinationMesh.activeInHierarchy);
            Assert.IsTrue(invalidDestinationMesh.activeInHierarchy);

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void EventDataComparison()
        {
            Transform subject = new GameObject().transform;

            ObjectPointer.EventData subjectA = new ObjectPointer.EventData(subject);
            ObjectPointer.EventData subjectB = new ObjectPointer.EventData(subject);

            Assert.IsFalse(subjectA == subjectB);
            Assert.IsTrue(subjectA.Equals(subjectB));
            Assert.AreEqual(subjectA, subjectB);

            subjectA.IsCurrentlyActive = true;
            subjectB.IsCurrentlyActive = false;

            Assert.IsFalse(subjectA == subjectB);
            Assert.IsFalse(subjectA.Equals(subjectB));
            Assert.AreNotEqual(subjectA, subjectB);

            Object.DestroyImmediate(subject.gameObject);
        }

        [Test]
        public void IsVisible()
        {
            SetUpElements();

            Assert.IsFalse(subject.IsVisible);

            subject.Activate();

            Assert.IsTrue(subject.IsVisible);

            subject.Deactivate();

            Assert.IsFalse(subject.IsVisible);
        }

        [Test]
        public void IsVisibleInactiveComponent()
        {
            SetUpElements();

            Assert.IsFalse(subject.IsVisible);

            subject.Activate();
            subject.enabled = false;

            Assert.IsFalse(subject.IsVisible);
        }

        [Test]
        public void IsVisibleAlwaysOff()
        {
            SetUpElements();

            Assert.IsFalse(subject.IsVisible);

            subject.Activate();

            Assert.IsTrue(subject.IsVisible);

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            Assert.IsTrue(subject.IsVisible);

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            Assert.IsTrue(subject.IsVisible);

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            Assert.IsFalse(subject.IsVisible);
        }

        [Test]
        public void IsVisibleAlwaysOn()
        {
            SetUpElements();

            Assert.IsFalse(subject.IsVisible);

            subject.Activate();

            Assert.IsTrue(subject.IsVisible);

            subject.Deactivate();

            Assert.IsFalse(subject.IsVisible);

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            Assert.IsTrue(subject.IsVisible);

            subject.Origin.ElementVisibility = PointerElement.Visibility.OnWhenPointerActivated;

            Assert.IsFalse(subject.IsVisible);

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            Assert.IsTrue(subject.IsVisible);

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.OnWhenPointerActivated;

            Assert.IsFalse(subject.IsVisible);

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            Assert.IsTrue(subject.IsVisible);

            subject.Destination.ElementVisibility = PointerElement.Visibility.OnWhenPointerActivated;

            Assert.IsFalse(subject.IsVisible);
        }

        [Test]
        public void ClearOrigin()
        {
            Assert.IsNull(subject.Origin);
            SetUpElements();
            Assert.AreEqual(origin, subject.Origin);
            subject.ClearOrigin();
            Assert.IsNull(subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveGameObject()
        {
            Assert.IsNull(subject.Origin);
            SetUpElements();
            Assert.AreEqual(origin, subject.Origin);
            subject.gameObject.SetActive(false);
            subject.ClearOrigin();
            Assert.AreEqual(origin, subject.Origin);
        }

        [Test]
        public void ClearOriginInactiveComponent()
        {
            Assert.IsNull(subject.Origin);
            SetUpElements();
            Assert.AreEqual(origin, subject.Origin);
            subject.enabled = false;
            subject.ClearOrigin();
            Assert.AreEqual(origin, subject.Origin);
        }

        [Test]
        public void ClearRepeatedSegment()
        {
            Assert.IsNull(subject.RepeatedSegment);
            SetUpElements();
            Assert.AreEqual(segment, subject.RepeatedSegment);
            subject.ClearRepeatedSegment();
            Assert.IsNull(subject.RepeatedSegment);
        }

        [Test]
        public void ClearRepeatedSegmentInactiveGameObject()
        {
            Assert.IsNull(subject.RepeatedSegment);
            SetUpElements();
            Assert.AreEqual(segment, subject.RepeatedSegment);
            subject.gameObject.SetActive(false);
            subject.ClearRepeatedSegment();
            Assert.AreEqual(segment, subject.RepeatedSegment);
        }

        [Test]
        public void ClearRepeatedSegmentInactiveComponent()
        {
            Assert.IsNull(subject.RepeatedSegment);
            SetUpElements();
            Assert.AreEqual(segment, subject.RepeatedSegment);
            subject.enabled = false;
            subject.ClearRepeatedSegment();
            Assert.AreEqual(segment, subject.RepeatedSegment);
        }

        [Test]
        public void ClearDestination()
        {
            Assert.IsNull(subject.Destination);
            SetUpElements();
            Assert.AreEqual(destination, subject.Destination);
            subject.ClearDestination();
            Assert.IsNull(subject.Destination);
        }

        [Test]
        public void ClearDestinationInactiveGameObject()
        {
            Assert.IsNull(subject.Destination);
            SetUpElements();
            Assert.AreEqual(destination, subject.Destination);
            subject.gameObject.SetActive(false);
            subject.ClearDestination();
            Assert.AreEqual(destination, subject.Destination);
        }

        [Test]
        public void ClearDestinationInactiveComponent()
        {
            Assert.IsNull(subject.Destination);
            SetUpElements();
            Assert.AreEqual(destination, subject.Destination);
            subject.enabled = false;
            subject.ClearDestination();
            Assert.AreEqual(destination, subject.Destination);
        }

        [Test]
        public void ClearEventDataOriginTransformOverride()
        {
            Assert.IsNull(subject.EventDataOriginTransformOverride);

            SetUpElements();
            GameObject originTransformOverride = new GameObject();
            subject.EventDataOriginTransformOverride = originTransformOverride;

            Assert.AreEqual(originTransformOverride, subject.EventDataOriginTransformOverride);

            subject.ClearEventDataOriginTransformOverride();

            Assert.IsNull(subject.EventDataOriginTransformOverride);

            Object.DestroyImmediate(originTransformOverride);
        }

        [Test]
        public void ClearEventDataOriginTransformOverrideInactiveGameObject()
        {
            Assert.IsNull(subject.EventDataOriginTransformOverride);

            SetUpElements();
            GameObject originTransformOverride = new GameObject();
            subject.EventDataOriginTransformOverride = originTransformOverride;

            Assert.AreEqual(originTransformOverride, subject.EventDataOriginTransformOverride);

            subject.gameObject.SetActive(false);
            subject.ClearEventDataOriginTransformOverride();

            Assert.AreEqual(originTransformOverride, subject.EventDataOriginTransformOverride);

            Object.DestroyImmediate(originTransformOverride);
        }

        [Test]
        public void ClearEventDataOriginTransformOverrideInactiveComponent()
        {
            Assert.IsNull(subject.EventDataOriginTransformOverride);

            SetUpElements();
            GameObject originTransformOverride = new GameObject();
            subject.EventDataOriginTransformOverride = originTransformOverride;

            Assert.AreEqual(originTransformOverride, subject.EventDataOriginTransformOverride);

            subject.enabled = false;
            subject.ClearEventDataOriginTransformOverride();

            Assert.AreEqual(originTransformOverride, subject.EventDataOriginTransformOverride);

            Object.DestroyImmediate(originTransformOverride);
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
        public EventData GetEventData()
        {
            return eventData;
        }

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