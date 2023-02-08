using Zinnia.Tracking.Modification;

namespace Test.Zinnia.Tracking.Modification
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

    public class DirectionModifierTest
    {
        private GameObject containingObject;
        private DirectionModifier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("DirectionModifierTest");
            subject = containingObject.AddComponent<DirectionModifier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("DirectionModifierTest");
            GameObject lookAt = new GameObject("DirectionModifierTest");
            GameObject pivot = new GameObject("DirectionModifierTest");

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            subject.Process();

            Assert.That(target.transform.rotation, Is.EqualTo(new Quaternion(-0.6f, 0.0f, 0.0f, 0.8f)).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoLookAt()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("DirectionModifierTest");
            GameObject pivot = new GameObject("DirectionModifierTest");

            subject.Target = target;
            subject.Pivot = pivot;

            pivot.transform.position = Vector3.back * 0.5f;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            subject.Process();

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessNoPivot()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("DirectionModifierTest");
            GameObject lookAt = new GameObject("DirectionModifierTest");

            subject.Target = target;
            subject.LookAt = lookAt;

            lookAt.transform.position = Vector3.up * 2f;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            subject.Process();

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
        }

        [Test]
        public void ProcessInactiveGameObject()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("DirectionModifierTest");
            GameObject lookAt = new GameObject("DirectionModifierTest");
            GameObject pivot = new GameObject("DirectionModifierTest");

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.gameObject.SetActive(false);

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            subject.Process();

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ProcessInactiveComponent()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            GameObject target = new GameObject("DirectionModifierTest");
            GameObject lookAt = new GameObject("DirectionModifierTest");
            GameObject pivot = new GameObject("DirectionModifierTest");

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.gameObject.SetActive(false);

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            subject.Process();

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(lookAt);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void ResetOrientation()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            UnityEventListenerMock orientationResetMock = new UnityEventListenerMock();
            subject.OrientationReset.AddListener(orientationResetMock.Listen);

            GameObject target = new GameObject("DirectionModifierTest");
            GameObject lookAt = new GameObject("DirectionModifierTest");
            GameObject pivot = new GameObject("DirectionModifierTest");

            subject.Target = target;
            subject.LookAt = lookAt;
            subject.Pivot = pivot;
            subject.ResetOrientationSpeed = 0f;

            lookAt.transform.position = Vector3.up * 2f;
            pivot.transform.position = Vector3.back * 0.5f;

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));

            subject.Process();

            Assert.That(target.transform.rotation, Is.EqualTo(new Quaternion(-0.6f, 0.0f, 0f, 0.8f)).Using(comparer));

            subject.ResetOrientation();

            Assert.That(target.transform.rotation, Is.EqualTo(Quaternion.identity).Using(comparer));
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
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            subjectMock.ClearPivot();

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
        }

        [Test]
        public void ClearPivotWithPivot()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            subjectMock.ClearPivot();

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(2f, 2f, 2f)).Using(comparer));

            Object.DestroyImmediate(pivotPoint);
        }

        [Test]
        public void SaveOrientationWithTargetWithPivotDoCancelReset()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
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

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(2f, 2f, 2f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(2f, 2f, 2f)).Using(comparer));
            Assert.IsTrue(orientationResetCancelledMock.Received);

            Object.DestroyImmediate(target);
            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void SaveOrientationWithTargetWithNoPivotDoCancelReset()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            GameObject target = new GameObject("target");

            target.transform.eulerAngles = Vector3.one * 2f;

            subjectMock.Target = target;

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(2f, 2f, 2f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            Assert.IsTrue(orientationResetCancelledMock.Received);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void SaveOrientationWithNoTargetWithPivotDoCancelReset()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            GameObject pivot = new GameObject("pivot");

            pivot.transform.eulerAngles = Vector3.one * 2f;

            subjectMock.Pivot = pivot;

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(2f, 2f, 2f)).Using(comparer));
            Assert.IsTrue(orientationResetCancelledMock.Received);

            Object.DestroyImmediate(pivot);
        }

        [Test]
        public void SaveOrientationWithNoTargetWithNoPivotDoCancelReset()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation();

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            Assert.IsTrue(orientationResetCancelledMock.Received);
        }

        [Test]
        public void SaveOrientationWithNoTargetWithNoPivotDontCancelReset()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.SaveOrientation(false);

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.identity).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);
        }

        [Test]
        public void SaveOrientationInactiveGameObject()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.ignoreOnDisable = true;
            subjectMock.gameObject.SetActive(false);
            subjectMock.SaveOrientation();

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);
        }

        [Test]
        public void SaveOrientationInactiveComponent()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();

            UnityEventListenerMock orientationResetCancelledMock = new UnityEventListenerMock();
            subjectMock.OrientationResetCancelled.AddListener(orientationResetCancelledMock.Listen);

            subjectMock.TargetInitialRotation = Quaternion.Euler(1f, 1f, 1f);
            subjectMock.PivotInitialRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);

            subjectMock.ignoreOnDisable = true;
            subjectMock.enabled = false;
            subjectMock.SaveOrientation();

            Assert.That(subjectMock.TargetInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.That(subjectMock.PivotInitialRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
            Assert.IsFalse(orientationResetCancelledMock.Received);
        }

        [Test]
        public void ResetOrientationWithPivot()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            subjectMock.ResetOrientation();

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(2f, 2f, 2f)).Using(comparer));

            Object.DestroyImmediate(pivotPoint);
        }

        [Test]
        public void ResetOrientationNullPivot()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            subjectMock.ResetOrientation();

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));
        }

        [Test]
        public void ResetOrientationInactiveGameObject()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            subjectMock.gameObject.SetActive(false);
            subjectMock.ResetOrientation();

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            Object.DestroyImmediate(pivotPoint);
        }

        [Test]
        public void ResetOrientationInactiveComponent()
        {
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);
            DirectionModifierMock subjectMock = containingObject.AddComponent<DirectionModifierMock>();
            subjectMock.PivotReleaseRotation = Quaternion.Euler(1f, 1f, 1f);
            GameObject pivotPoint = new GameObject("pivotPoint");
            pivotPoint.transform.eulerAngles = Vector3.one * 2f;
            subjectMock.Pivot = pivotPoint;

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

            subjectMock.enabled = false;
            subjectMock.ResetOrientation();

            Assert.That(subjectMock.PivotReleaseRotation, Is.EqualTo(Quaternion.Euler(1f, 1f, 1f)).Using(comparer));

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