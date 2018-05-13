namespace VRTK.Core.Prefabs.Pointer
{
    using UnityEngine;
    using NUnit.Framework;
    using VRTK.Core.Utility.Mock;

    public class StraightPointerTest
    {
        private GameObject containingObject;
        private StraightPointerMock subject;

        private GameObject validTracer;
        private GameObject validCursor;
        private GameObject invalidTracer;
        private GameObject invalidCursor;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<StraightPointerMock>();

            validTracer = new GameObject();
            validCursor = new GameObject();
            invalidTracer = new GameObject();
            invalidCursor = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);

            Object.DestroyImmediate(validTracer);
            Object.DestroyImmediate(validCursor);
            Object.DestroyImmediate(invalidTracer);
            Object.DestroyImmediate(invalidCursor);
        }

        [Test]
        public void ActivateAndDeactivate()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock deactivatedListenerMock = new UnityEventListenerMock();

            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.cursorVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.activateOnEnable = false;

            subject.Activated.AddListener(activatedListenerMock.Listen);
            subject.Deactivated.AddListener(deactivatedListenerMock.Listen);

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            subject.ManualOnEnable();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsTrue(activatedListenerMock.Received);
            Assert.IsFalse(deactivatedListenerMock.Received);

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            subject.ManualUpdate();

            Assert.IsTrue(validTracer.activeInHierarchy);
            Assert.IsTrue(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            activatedListenerMock.Reset();
            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(activatedListenerMock.Received);
            Assert.IsTrue(deactivatedListenerMock.Received);

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void Select()
        {
            UnityEventListenerMock selectListenerMock = new UnityEventListenerMock();

            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.cursorVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.activateOnEnable = false;

            subject.Selected.AddListener(selectListenerMock.Listen);

            subject.ManualOnEnable();

            Assert.IsFalse(selectListenerMock.Received);

            subject.Activate();
            subject.ManualUpdate();
            subject.Select();

            //Even though select is called, it isn't hitting a valid object so should be ignored.
            Assert.IsFalse(selectListenerMock.Received);

            //Place an object in the way to make a valid target
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            subject.ManualUpdate();
            subject.Select();

            //Now there is a valid object, select should be true.
            Assert.IsTrue(selectListenerMock.Received);

            Object.DestroyImmediate(blocker);
        }

        [Test]
        public void TracerAlwaysVisible()
        {
            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.AlwaysOn;
            subject.cursorVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.activateOnEnable = false;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            //Before activate is called the tracer should be true (not the valid one as there is no valid target).
            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);
        }

        [Test]
        public void CursorAlwaysVisible()
        {
            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.cursorVisibility = BasePointer.VisibilityType.AlwaysOn;
            subject.activateOnEnable = false;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);
        }

        [Test]
        public void TracerAndCursorAlwaysVisible()
        {
            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.AlwaysOn;
            subject.cursorVisibility = BasePointer.VisibilityType.AlwaysOn;
            subject.activateOnEnable = false;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);
        }

        [Test]
        public void TracerAlwaysHidden()
        {
            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.AlwaysOff;
            subject.cursorVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.activateOnEnable = false;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);
        }

        [Test]
        public void CursorAlwaysHidden()
        {
            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.cursorVisibility = BasePointer.VisibilityType.AlwaysOff;
            subject.activateOnEnable = false;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);
        }

        [Test]
        public void TracerAndCursorAlwaysHidden()
        {
            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.AlwaysOff;
            subject.cursorVisibility = BasePointer.VisibilityType.AlwaysOff;
            subject.activateOnEnable = false;

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Activate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);

            subject.Deactivate();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsFalse(invalidTracer.activeInHierarchy);
            Assert.IsFalse(invalidCursor.activeInHierarchy);
        }

        [Test]
        public void ActiveOnEnable()
        {
            UnityEventListenerMock activatedListenerMock = new UnityEventListenerMock();

            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.cursorVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.activateOnEnable = true;

            subject.Activated.AddListener(activatedListenerMock.Listen);

            subject.ManualOnEnable();
            subject.ManualUpdate();

            Assert.IsFalse(validTracer.activeInHierarchy);
            Assert.IsFalse(validCursor.activeInHierarchy);
            Assert.IsTrue(invalidTracer.activeInHierarchy);
            Assert.IsTrue(invalidCursor.activeInHierarchy);

            Assert.IsTrue(activatedListenerMock.Received);
        }

        [Test]
        public void EnterExit()
        {
            UnityEventListenerMock enterListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock exitListenerMock = new UnityEventListenerMock();
            UnityEventListenerMock hoverListenerMock = new UnityEventListenerMock();

            subject.maximumLength = 100f;
            subject.validTracer = validTracer;
            subject.validCursor = validCursor;
            subject.invalidTracer = invalidTracer;
            subject.invalidCursor = invalidCursor;
            subject.tracerVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.cursorVisibility = BasePointer.VisibilityType.OnWhenActive;
            subject.activateOnEnable = false;

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

            //Place an object in the way to make a valid target
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.transform.position = Vector3.forward * 5f;

            subject.ManualUpdate();

            //The target should be entered and be hovered over
            Assert.IsTrue(enterListenerMock.Received);
            Assert.IsFalse(exitListenerMock.Received);
            Assert.IsTrue(hoverListenerMock.Received);
            Assert.AreEqual(blocker.transform, subject.CollisionData.transform);

            enterListenerMock.Reset();
            hoverListenerMock.Reset();

            //Move the target
            blocker.transform.position = Vector3.left * 10f;

            subject.ManualUpdate();

            Assert.IsFalse(enterListenerMock.Received);
            Assert.IsTrue(exitListenerMock.Received);
            Assert.IsFalse(hoverListenerMock.Received);

            Assert.AreEqual(null, subject.CollisionData.transform);

            Object.DestroyImmediate(blocker);
        }
    }

    public class StraightPointerMock : StraightPointer
    {
        public void ManualOnEnable()
        {
            OnEnable();
        }

        public void ManualUpdate()
        {
            Update();
        }
    }
}