using Zinnia.Pointer;
using Zinnia.Cast;

namespace Test.Zinnia.Pointer
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;

    public class PointerTest
    {
        private GameObject containingObject;
        private ObjectPointerMock subject;
        private PointerElement origin;
        private PointerElement segment;
        private PointerElement destination;

        private GameObject validOrigin;
        private GameObject invalidOrigin;
        private GameObject validSegment;
        private GameObject invalidSegment;
        private GameObject validDestination;
        private GameObject invalidDestination;

        [SetUp]
        public void SetUp()
        {
            Physics.autoSimulation = false;
            containingObject = new GameObject();
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<ObjectPointerMock>();

            validOrigin = new GameObject();
            invalidOrigin = new GameObject();
            validSegment = new GameObject();
            invalidSegment = new GameObject();
            validDestination = new GameObject();
            invalidDestination = new GameObject();

            origin = containingObject.AddComponent<PointerElement>();
            segment = containingObject.AddComponent<PointerElement>();
            destination = containingObject.AddComponent<PointerElement>();

            origin.ValidObject = validOrigin;
            origin.InvalidObject = invalidOrigin;
            subject.Origin = origin;

            segment.ValidObject = validSegment;
            segment.InvalidObject = invalidSegment;
            subject.RepeatedSegment = segment;

            segment.ValidObject = validDestination;
            segment.InvalidObject = invalidDestination;
            subject.Destination = destination;
            containingObject.SetActive(true);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);

            Object.DestroyImmediate(validOrigin);
            Object.DestroyImmediate(invalidOrigin);
            Object.DestroyImmediate(validSegment);
            Object.DestroyImmediate(invalidSegment);
            Object.DestroyImmediate(validDestination);
            Object.DestroyImmediate(invalidDestination);
            Physics.autoSimulation = true;
        }

        [Test]
        public void ActivateAndDeactivate()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            subject.ManualOnEnable();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

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

            Assert.IsTrue(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsTrue(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsTrue(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void Select()
        {
            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock selectListenerMock = new UnityEventListenerMock();

            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

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
            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock selectListenerMock = new UnityEventListenerMock();

            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

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
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            subject.ManualOnEnable();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.ManualOnDisable();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void OriginAlwaysVisible()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void SegmentAlwaysVisible()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void DestinationAlwaysVisible()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void SegmentAndDestinationAlwaysVisible()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void ElementsAlwaysVisible()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void OriginAlwaysHidden()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void SegmentAlwaysHidden()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void DestinationAlwaysHidden()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void SegmentAndDestinationAlwaysHidden()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void ElementsAlwaysHidden()
        {
            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Origin.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.RepeatedSegment.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            subject.Destination.ElementVisibility = PointerElement.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.Process();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void ActiveOnEnable()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();

            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

            subject.Activated.AddListener(activatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);

            subject.ManualOnEnable();
            subject.Activate();

            Assert.IsTrue(activatedListenerMock.Received);

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);
        }

        [Test]
        public void EnterExitHover()
        {
            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();

            subject.Origin.ValidObject = validOrigin;
            subject.Origin.InvalidObject = invalidOrigin;
            subject.RepeatedSegment.ValidObject = validSegment;
            subject.RepeatedSegment.InvalidObject = invalidSegment;
            subject.Destination.ValidObject = validDestination;
            subject.Destination.InvalidObject = invalidDestination;

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
            blocker.transform.position = Vector3.forward * 5f;

            List<Vector3> castPoints = new List<Vector3>
            {
                Vector3.zero,
                blocker.transform.position
            };

            PointsCast.EventData straightCast;

            straightCast = CastPoints(castPoints, true, true, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);

            //The target should be entered and be hovered over
            Assert.IsTrue(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsTrue(hoverListenerMock.Received);
            Assert.AreEqual(blocker.transform, subject.HoverTarget.Transform);

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