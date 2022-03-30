using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

    public class DirectionModifierTest
    {
        private GameObject containingObject;
        private DirectionModifier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<DirectionModifier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(new Quaternion(-0.6f, 0.0f, 0.0f, 0.8f).ToString(), target.transform.rotation.ToString());

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoLookAt()
        {
            GameObject target = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.Pivot = pivot;

            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoPivot()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;

            lookAt.transform.position = Vector3.up * 2f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.gameObject.SetActive(false);

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.gameObject.SetActive(false);

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ResetOrientation()
        {
            UnityEventListenerMock orientationResetMock = new UnityEventListenerMock();
            subject.OrientationReset.AddListener(orientationResetMock.Listen);

            GameObject target = new GameObject();
            GameObject lookAt = new GameObject();
            GameObject pivot = new GameObject();

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.ResetOrientationSpeed = 0f;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);

            subject.Process();

            Assert.AreEqual(new Quaternion(-0.6f, 0.0f, 0f, 0.8f).ToString(), target.transform.rotation.ToString());

            subject.ResetOrientation();

            Assert.AreEqual(Quaternion.identity, target.transform.rotation);
            Assert.IsTrue(orientationResetMock.Received);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void CancelResetOrientation()
        {
            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subject.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);
            subject.CancelResetOrientation();
            Assert.IsTrue(orientationResetCancelledMock.Received);
        }

        [Test]
        public void ClearTarget()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            Assert.IsNull(subject.Target);
            subject.Target = containingObject;
            Assert.AreEqual(containingObject, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(containingObject, subject.Target);
        }

        [Test]
        public void ClearLookAt()
        {
            Assert.IsNull(subject.LookAt);
            subject.LookAt = containingObject;
            Assert.AreEqual(containingObject, subject.LookAt);
            subject.ClearLookAt();
            Assert.IsNull(subject.LookAt);
        }

        [Test]
        public void ClearLookAtInactiveGameObject()
        {
            Assert.IsNull(subject.LookAt);
            subject.LookAt = containingObject;
            Assert.AreEqual(containingObject, subject.LookAt);
            subject.gameObject.SetActive(false);
            subject.ClearLookAt();
            Assert.AreEqual(containingObject, subject.LookAt);
        }

        [Test]
        public void ClearLookAtInactiveComponent()
        {
            Assert.IsNull(subject.LookAt);
            subject.LookAt = containingObject;
            Assert.AreEqual(containingObject, subject.LookAt);
            subject.enabled = false;
            subject.ClearLookAt();
            Assert.AreEqual(containingObject, subject.LookAt);
        }

        [Test]
        public void ClearPivotProperty()
        {
            Assert.IsNull(subject.Pivot);
            subject.Pivot = containingObject;
            Assert.AreEqual(containingObject, subject.Pivot);
            subject.ClearPivotProperty();
            Assert.IsNull(subject.Pivot);
        }

        [Test]
        public void ClearPivotPropertyInactiveGameObject()
        {
            Assert.IsNull(subject.Pivot);
            subject.Pivot = containingObject;
            Assert.AreEqual(containingObject, subject.Pivot);
            subject.gameObject.SetActive(false);
            subject.ClearPivotProperty();
            Assert.AreEqual(containingObject, subject.Pivot);
        }

        [Test]
        public void ClearPivotPropertyInactiveComponent()
        {
            Assert.IsNull(subject.Pivot);
            subject.Pivot = containingObject;
            Assert.AreEqual(containingObject, subject.Pivot);
            subject.enabled = false;
            subject.ClearPivotProperty();
            Assert.AreEqual(containingObject, subject.Pivot);
        }

        [Test]
        public void ClearTargetOffset()
        {
            Assert.IsNull(subject.TargetOffset);
            subject.TargetOffset = containingObject;
            Assert.AreEqual(containingObject, subject.TargetOffset);
            subject.ClearTargetOffset();
            Assert.IsNull(subject.TargetOffset);
        }

        [Test]
        public void ClearTargetOffsetInactiveGameObject()
        {
            Assert.IsNull(subject.TargetOffset);
            subject.TargetOffset = containingObject;
            Assert.AreEqual(containingObject, subject.TargetOffset);
            subject.gameObject.SetActive(false);
            subject.ClearTargetOffset();
            Assert.AreEqual(containingObject, subject.TargetOffset);
        }

        [Test]
        public void ClearTargetOffsetInactiveComponent()
        {
            Assert.IsNull(subject.TargetOffset);
            subject.TargetOffset = containingObject;
            Assert.AreEqual(containingObject, subject.TargetOffset);
            subject.enabled = false;
            subject.ClearTargetOffset();
            Assert.AreEqual(containingObject, subject.TargetOffset);
        }

        [Test]
        public void ClearPivotOffset()
        {
            Assert.IsNull(subject.PivotOffset);
            subject.PivotOffset = containingObject;
            Assert.AreEqual(containingObject, subject.PivotOffset);
            subject.ClearPivotOffset();
            Assert.IsNull(subject.PivotOffset);
        }

        [Test]
        public void ClearPivotOffsetInactiveGameObject()
        {
            Assert.IsNull(subject.PivotOffset);
            subject.PivotOffset = containingObject;
            Assert.AreEqual(containingObject, subject.PivotOffset);
            subject.gameObject.SetActive(false);
            subject.ClearPivotOffset();
            Assert.AreEqual(containingObject, subject.PivotOffset);
        }

        [Test]
        public void ClearPivotOffsetInactiveComponent()
        {
            Assert.IsNull(subject.PivotOffset);
            subject.PivotOffset = containingObject;
            Assert.AreEqual(containingObject, subject.PivotOffset);
            subject.enabled = false;
            subject.ClearPivotOffset();
            Assert.AreEqual(containingObject, subject.PivotOffset);
        }

        [Test]
        public void ClearPivotNullPivot()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            subjectMock.ClearPivot();

            Assert.AreEqual(Quaternion.identity, subjectMock.PivotReleaseRotation);
        }

        [Test]
        public void ClearPivotWithPivot()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            subjectMock.ClearPivot();

            Assert.AreEqual(Quaternion.Euler(2f, 2f, 2f), subjectMock.PivotReleaseRotation);

            Object.DestroyImmediate(pivotPoint);
        }

        [Test]
        public void SaveOrientationWithTargetWithPivotDoCancelReset()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            GameObject target = new GameObject("target");
            GameObject pivot = new GameObject("pivot");

            target.transform.eulerAngles = Vector3.one * 2f;
            pivot.transform.eulerAngles = Vector3.one * 2f;

            subjectMock.Target = target;
            subjectMock.Pivot = pivot;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.AreEqual(Quaternion.Euler(2f, 2f, 2f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(2f, 2f, 2f), subjectMock.PivotInitialRotation);
            Assert.IsTrue(orientationResetCancelledMock.Received);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void SaveOrientationWithTargetWithNoPivotDoCancelReset()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            GameObject target = new GameObject("target");

            target.transform.eulerAngles = Vector3.one * 2f;

            subjectMock.Target = target;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.AreEqual(Quaternion.Euler(2f, 2f, 2f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.identity, subjectMock.PivotInitialRotation);
            Assert.IsTrue(orientationResetCancelledMock.Received);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SaveOrientationWithNoTargetWithPivotDoCancelReset()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            GameObject pivot = new GameObject("pivot");

            pivot.transform.eulerAngles = Vector3.one * 2f;

            subjectMock.Pivot = pivot;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.AreEqual(Quaternion.identity, subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(2f, 2f, 2f), subjectMock.PivotInitialRotation);
            Assert.IsTrue(orientationResetCancelledMock.Received);

            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void SaveOrientationWithNoTargetWithNoPivotDoCancelReset()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.AreEqual(Quaternion.identity, subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.identity, subjectMock.PivotInitialRotation);
            Assert.IsTrue(orientationResetCancelledMock.Received);
        }

        [Test]
        public void SaveOrientationWithNoTargetWithNoPivotDontCancelReset()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation(false);

            Assert.AreEqual(Quaternion.identity, subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.identity, subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);
        }

        [Test]
        public void SaveOrientationInactiveGameObject()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.ignoreOnDisable = true;
            subjectMock.gameObject.SetActive(false);
            subjectMock.SaveOrientation();

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);
        }

        [Test]
        public void SaveOrientationInactiveComponent()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.ignoreOnDisable = true;
            subjectMock.enabled = false;
            subjectMock.SaveOrientation();

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.TargetInitialRotation);
            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotInitialRotation);
            Assert.IsFalse(orientationResetCancelledMock.Received);
        }

        [Test]
        public void ResetOrientationWithPivot()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            subjectMock.ResetOrientation();

            Assert.AreEqual(Quaternion.Euler(2f, 2f, 2f), subjectMock.PivotReleaseRotation);

            Object.DestroyImmediate(pivotPoint);
        }

        [Test]
        public void ResetOrientationNullPivot()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            subjectMock.ResetOrientation();

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);
        }

        [Test]
        public void ResetOrientationInactiveGameObject()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            subjectMock.gameObject.SetActive(false);
            subjectMock.ResetOrientation();

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            Object.DestroyImmediate(pivotPoint);
        }

        [Test]
        public void ResetOrientationInactiveComponent()
        {
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            subjectMock.enabled = false;
            subjectMock.ResetOrientation();

            Assert.AreEqual(Quaternion.Euler(1f, 1f, 1f), subjectMock.PivotReleaseRotation);

            Object.DestroyImmediate(pivotPoint);
        }

        protected class DirectionModifierMock : DirectionModifier
        {
            public Quaternion TargetInitialRotation
            {
                get
                {
                    return targetInitialRotation;
                }
                set
                {
                    targetInitialRotation = value;
                }
            }

            public Quaternion PivotInitialRotation
            {
                get
                {
                    return pivotInitialRotation;
                }
                set
                {
                    pivotInitialRotation = value;
                }
            }

            public Quaternion PivotReleaseRotation
            {
                get
                {
                    return pivotReleaseRotation;
                }
                set
                {
                    pivotReleaseRotation = value;
                }
            }

            public bool ignoreOnDisable;

            protected override void OnDisable()
            {
                if (!ignoreOnDisable)
                {
                    base.OnDisable();
                }
            }
        }
    }
}