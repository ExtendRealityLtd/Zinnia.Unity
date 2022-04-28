using Zinnia.Pointer;
using Zinnia.Pointer.Operation.Mutation;

namespace Test.Zinnia.Pointer.Operation.Mutation
{
    using NUnit.Framework;
    using UnityEngine;

    public class PointerElementPropertyMutatorTest
    {
        private GameObject containingObject;
        private PointerElementPropertyMutator subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PointerElementPropertyMutator>();

        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearTarget()
        {
            PointerElement pointer = containingObject.AddComponent<PointerElement>();
            subject.Target = pointer;

            Assert.AreEqual(pointer, subject.Target);
            subject.ClearTarget();
            Assert.IsNull(subject.Target);
        }

        [Test]
        public void ClearTargetInactiveGameObject()
        {
            PointerElement pointer = containingObject.AddComponent<PointerElement>();
            subject.Target = pointer;

            Assert.AreEqual(pointer, subject.Target);
            subject.gameObject.SetActive(false);
            subject.ClearTarget();
            Assert.AreEqual(pointer, subject.Target);
        }

        [Test]
        public void ClearTargetInactiveComponent()
        {
            PointerElement pointer = containingObject.AddComponent<PointerElement>();
            subject.Target = pointer;

            Assert.AreEqual(pointer, subject.Target);
            subject.enabled = false;
            subject.ClearTarget();
            Assert.AreEqual(pointer, subject.Target);
        }

        [Test]
        public void ClearValidElementContainer()
        {
            subject.ValidElementContainer = containingObject;

            Assert.AreEqual(containingObject, subject.ValidElementContainer);
            subject.ClearValidElementContainer();
            Assert.IsNull(subject.ValidElementContainer);
        }

        [Test]
        public void ClearValidElementContainerInactiveGameObject()
        {
            subject.ValidElementContainer = containingObject;

            Assert.AreEqual(containingObject, subject.ValidElementContainer);
            subject.gameObject.SetActive(false);
            subject.ClearValidElementContainer();
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
        }

        [Test]
        public void ClearValidElementContainerInactiveComponent()
        {
            subject.ValidElementContainer = containingObject;

            Assert.AreEqual(containingObject, subject.ValidElementContainer);
            subject.enabled = false;
            subject.ClearValidElementContainer();
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
        }

        [Test]
        public void ClearInvalidElementContainer()
        {
            subject.InvalidElementContainer = containingObject;

            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
            subject.ClearInvalidElementContainer();
            Assert.IsNull(subject.InvalidElementContainer);
        }

        [Test]
        public void ClearInvalidElementContainerInactiveGameObject()
        {
            subject.InvalidElementContainer = containingObject;

            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
            subject.gameObject.SetActive(false);
            subject.ClearInvalidElementContainer();
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
        }

        [Test]
        public void ClearInvalidElementContainerInactiveComponent()
        {
            subject.InvalidElementContainer = containingObject;

            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
            subject.enabled = false;
            subject.ClearInvalidElementContainer();
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
        }

        [Test]
        public void ClearInvalidMeshContainer()
        {
            subject.InvalidMeshContainer = containingObject;

            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
            subject.ClearInvalidMeshContainer();
            Assert.IsNull(subject.InvalidMeshContainer);
        }

        [Test]
        public void ClearInvalidMeshContainerInactiveGameObject()
        {
            subject.InvalidMeshContainer = containingObject;

            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
            subject.gameObject.SetActive(false);
            subject.ClearInvalidMeshContainer();
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
        }

        [Test]
        public void ClearInvalidMeshContainerInactiveComponent()
        {
            subject.InvalidMeshContainer = containingObject;

            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
            subject.enabled = false;
            subject.ClearInvalidMeshContainer();
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
        }

        [Test]
        public void SetTarget()
        {
            GameObject pointerContainer = new GameObject();
            PointerElement pointer = pointerContainer.AddComponent<PointerElement>();

            Assert.IsNull(subject.Target);

            subject.SetTarget(pointerContainer);

            Assert.AreEqual(pointer, subject.Target);

            Object.DestroyImmediate(pointerContainer);
        }

        [Test]
        public void SetTargetInChild()
        {
            GameObject pointerParent = new GameObject();
            GameObject pointerContainer = new GameObject();
            PointerElement pointer = pointerContainer.AddComponent<PointerElement>();
            pointerContainer.transform.SetParent(pointerParent.transform);

            Assert.IsNull(subject.Target);

            subject.SetTarget(pointerParent);

            Assert.AreEqual(pointer, subject.Target);

            Object.DestroyImmediate(pointerContainer);
            Object.DestroyImmediate(pointerParent);
        }

        [Test]
        public void SetTargetInParent()
        {
            GameObject pointerContainer = new GameObject();
            GameObject pointerChild = new GameObject();
            PointerElement pointer = pointerContainer.AddComponent<PointerElement>();
            pointerChild.transform.SetParent(pointerContainer.transform);

            Assert.IsNull(subject.Target);

            subject.SetTarget(pointerChild);

            Assert.AreEqual(pointer, subject.Target);

            Object.DestroyImmediate(pointerChild);
            Object.DestroyImmediate(pointerContainer);
        }

        [Test]
        public void SetTargetInactiveGameObject()
        {
            GameObject pointerContainer = new GameObject();
            PointerElement pointer = pointerContainer.AddComponent<PointerElement>();

            Assert.IsNull(subject.Target);

            subject.gameObject.SetActive(false);
            subject.SetTarget(pointerContainer);

            Assert.IsNull(subject.Target);

            Object.DestroyImmediate(pointerContainer);
        }

        [Test]
        public void SetTargetInactiveComponent()
        {
            GameObject pointerContainer = new GameObject();
            PointerElement pointer = pointerContainer.AddComponent<PointerElement>();

            Assert.IsNull(subject.Target);

            subject.enabled = false;
            subject.SetTarget(pointerContainer);

            Assert.IsNull(subject.Target);

            Object.DestroyImmediate(pointerContainer);
        }

        [Test]
        public void SetTargetNullParameter()
        {
            GameObject pointerContainer = new GameObject();
            PointerElement pointer = pointerContainer.AddComponent<PointerElement>();

            Assert.IsNull(subject.Target);

            subject.SetTarget(pointerContainer);

            Assert.AreEqual(pointer, subject.Target);

            subject.SetTarget(null);

            Assert.AreEqual(pointer, subject.Target);

            Object.DestroyImmediate(pointerContainer);
        }

        [Test]
        public void SetElementVisibility()
        {
            Assert.AreEqual(PointerElement.Visibility.OnWhenPointerActivated, subject.ElementVisibility);

            subject.SetElementVisibility(1);

            Assert.AreEqual(PointerElement.Visibility.AlwaysOn, subject.ElementVisibility);

            subject.SetElementVisibility(2);

            Assert.AreEqual(PointerElement.Visibility.AlwaysOff, subject.ElementVisibility);

            subject.SetElementVisibility(0);

            Assert.AreEqual(PointerElement.Visibility.OnWhenPointerActivated, subject.ElementVisibility);
        }
    }
}