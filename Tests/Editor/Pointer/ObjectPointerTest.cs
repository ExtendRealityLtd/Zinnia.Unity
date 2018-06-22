using VRTK.Core.Pointer;
using VRTK.Core.Cast;

namespace Test.VRTK.Core.Pointer
{
    using UnityEngine;
    using NUnit.Framework;
    using Test.VRTK.Core.Utility.Mock;

    public class PointerTest
    {
        private GameObject containingObject;
        private ObjectPointerMock subject;

        private GameObject validOrigin;
        private GameObject invalidOrigin;
        private GameObject validSegment;
        private GameObject invalidSegment;
        private GameObject validDestination;
        private GameObject invalidDestination;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ObjectPointerMock>();

            validOrigin = new GameObject();
            invalidOrigin = new GameObject();
            validSegment = new GameObject();
            invalidSegment = new GameObject();
            validDestination = new GameObject();
            invalidDestination = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);

            Object.DestroyImmediate(validOrigin);
            Object.DestroyImmediate(invalidOrigin);
            Object.DestroyImmediate(validSegment);
            Object.DestroyImmediate(invalidSegment);
            Object.DestroyImmediate(validDestination);
            Object.DestroyImmediate(invalidDestination);
        }

        [Test]
        public void ActivateAndDeactivate()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

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
            subject.ManualUpdate();

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

            Vector3[] castPoints = new Vector3[]
            {
                Vector3.zero,
                blocker.transform.position
            };

            PointsCast.EventData straightCast = CastPoints(castPoints);

            subject.HandleData(straightCast);
            subject.ManualUpdate();

            Assert.IsTrue(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsTrue(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsTrue(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            activatedListenerMock.Reset();
            deactivatedListenerMock.Reset();
            subject.Deactivate();
            subject.ManualUpdate();

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
            UnityEventListenerMock selectListenerMock = new UnityEventListenerMock();

            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.Selected.AddListener(selectListenerMock.Listen);

            subject.ManualOnEnable();

            Assert.IsFalse(selectListenerMock.Received);

            subject.Activate();
            subject.ManualUpdate();
            subject.Select();

            Assert.IsTrue(selectListenerMock.Received);
            Assert.IsNull(subject.HoverTarget);

            selectListenerMock.Reset();

            //Now add a valid target that can be selected
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            Vector3[] castPoints = new Vector3[]
            {
                Vector3.zero,
                blocker.transform.position
            };

            PointsCast.EventData straightCast = CastPoints(castPoints);

            subject.HandleData(straightCast);
            subject.ManualUpdate();
            subject.Select();

            Assert.IsTrue(selectListenerMock.Received);
            Assert.IsNotNull(subject.HoverTarget);

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
            Assert.IsFalse(subject.ActivationState);
        }

        [Test]
        public void DeactivateOnDisableComponent()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

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
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.origin.visibility = ObjectPointer.Element.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.repeatedSegment.visibility = ObjectPointer.Element.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.destination.visibility = ObjectPointer.Element.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.repeatedSegment.visibility = ObjectPointer.Element.Visibility.AlwaysOn;
            subject.destination.visibility = ObjectPointer.Element.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.origin.visibility = ObjectPointer.Element.Visibility.AlwaysOn;
            subject.repeatedSegment.visibility = ObjectPointer.Element.Visibility.AlwaysOn;
            subject.destination.visibility = ObjectPointer.Element.Visibility.AlwaysOn;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.origin.visibility = ObjectPointer.Element.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.repeatedSegment.visibility = ObjectPointer.Element.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsTrue(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.destination.visibility = ObjectPointer.Element.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsTrue(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.repeatedSegment.visibility = ObjectPointer.Element.Visibility.AlwaysOff;
            subject.destination.visibility = ObjectPointer.Element.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsTrue(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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
            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.origin.visibility = ObjectPointer.Element.Visibility.AlwaysOff;
            subject.repeatedSegment.visibility = ObjectPointer.Element.Visibility.AlwaysOff;
            subject.destination.visibility = ObjectPointer.Element.Visibility.AlwaysOff;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validOrigin.activeInHierarchy);
            Assert.IsFalse(invalidOrigin.activeInHierarchy);
            Assert.IsFalse(validSegment.activeInHierarchy);
            Assert.IsFalse(invalidSegment.activeInHierarchy);
            Assert.IsFalse(validDestination.activeInHierarchy);
            Assert.IsFalse(invalidDestination.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

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

            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.activateOnEnable = true;

            subject.Activated.AddListener(activatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);

            subject.ManualOnEnable();

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

            subject.origin.validObject = validOrigin;
            subject.origin.invalidObject = invalidOrigin;
            subject.repeatedSegment.validObject = validSegment;
            subject.repeatedSegment.invalidObject = invalidSegment;
            subject.destination.validObject = validDestination;
            subject.destination.invalidObject = invalidDestination;

            subject.Entered.AddListener(enterListenerMock.Listen);
            subject.Exited.AddListener(exitListenerMock.Listen);
            subject.Hovering.AddListener(hoverListenerMock.Listen);

            subject.ManualOnEnable();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);

            subject.Activate();
            subject.ManualUpdate();

            //No valid target so still should be false
            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);
            Assert.IsNull(subject.HoverTarget);

            //Place an object in the way to make a valid target
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            Vector3[] castPoints = new Vector3[]
            {
                Vector3.zero,
                blocker.transform.position
            };

            PointsCast.EventData straightCast;

            straightCast = CastPoints(castPoints, true, new Ray(Vector3.zero, Vector3.forward));

            subject.HandleData(straightCast);

            //The target should be entered and be hovered over
            Assert.IsTrue(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsTrue(hoverListenerMock.Received);
            Assert.AreEqual(blocker.transform, subject.HoverTarget.transform);

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

        protected PointsCast.EventData CastPoints(Vector3[] points, bool validHit = true, Ray? realRay = null)
        {
            if (validHit)
            {
                RaycastHit hit = new RaycastHit();
                if (realRay != null)
                {
                    Physics.Raycast((Ray)realRay, out hit);
                }
                return new PointsCast.EventData()
                {
                    points = points,
                    targetHit = hit
                };
            }
            else
            {
                return new PointsCast.EventData()
                {
                    points = points
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

        public void ManualUpdate()
        {
            Update();
        }
    }
}