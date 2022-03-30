using Zinnia.Pointer;

namespace Test.Zinnia.Pointer
{
    using NUnit.Framework;
    using UnityEngine;

    public class PointerElementTest
    {
        private GameObject containingObject;
        private PointerElement subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("containingObject");
            subject = containingObject.AddComponent<PointerElement>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ClearValidElementContainer()
        {
            Assert.IsNull(subject.ValidElementContainer);
            subject.ValidElementContainer = containingObject;
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
            subject.ClearValidElementContainer();
            Assert.IsNull(subject.ValidElementContainer);
        }

        [Test]
        public void ClearValidElementContainerInactiveGameObject()
        {
            Assert.IsNull(subject.ValidElementContainer);
            subject.ValidElementContainer = containingObject;
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
            subject.gameObject.SetActive(false);
            subject.ClearValidElementContainer();
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
        }

        [Test]
        public void ClearValidElementContainerInactiveComponent()
        {
            Assert.IsNull(subject.ValidElementContainer);
            subject.ValidElementContainer = containingObject;
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
            subject.enabled = false;
            subject.ClearValidElementContainer();
            Assert.AreEqual(containingObject, subject.ValidElementContainer);
        }

        [Test]
        public void ClearValidMeshContainer()
        {
            Assert.IsNull(subject.ValidMeshContainer);
            subject.ValidMeshContainer = containingObject;
            Assert.AreEqual(containingObject, subject.ValidMeshContainer);
            subject.ClearValidMeshContainer();
            Assert.IsNull(subject.ValidMeshContainer);
        }

        [Test]
        public void ClearValidMeshContainerInactiveGameObject()
        {
            Assert.IsNull(subject.ValidMeshContainer);
            subject.ValidMeshContainer = containingObject;
            Assert.AreEqual(containingObject, subject.ValidMeshContainer);
            subject.gameObject.SetActive(false);
            subject.ClearValidMeshContainer();
            Assert.AreEqual(containingObject, subject.ValidMeshContainer);
        }

        [Test]
        public void ClearValidMeshContainerInactiveComponent()
        {
            Assert.IsNull(subject.ValidMeshContainer);
            subject.ValidMeshContainer = containingObject;
            Assert.AreEqual(containingObject, subject.ValidMeshContainer);
            subject.enabled = false;
            subject.ClearValidMeshContainer();
            Assert.AreEqual(containingObject, subject.ValidMeshContainer);
        }

        [Test]
        public void ClearInvalidElementContainer()
        {
            Assert.IsNull(subject.InvalidElementContainer);
            subject.InvalidElementContainer = containingObject;
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
            subject.ClearInvalidElementContainer();
            Assert.IsNull(subject.InvalidElementContainer);
        }

        [Test]
        public void ClearInvalidElementContainerInactiveGameObject()
        {
            Assert.IsNull(subject.InvalidElementContainer);
            subject.InvalidElementContainer = containingObject;
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
            subject.gameObject.SetActive(false);
            subject.ClearInvalidElementContainer();
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
        }

        [Test]
        public void ClearInvalidElementContainerInactiveComponent()
        {
            Assert.IsNull(subject.InvalidElementContainer);
            subject.InvalidElementContainer = containingObject;
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
            subject.enabled = false;
            subject.ClearInvalidElementContainer();
            Assert.AreEqual(containingObject, subject.InvalidElementContainer);
        }

        [Test]
        public void ClearInvalidMeshContainer()
        {
            Assert.IsNull(subject.InvalidMeshContainer);
            subject.InvalidMeshContainer = containingObject;
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
            subject.ClearInvalidMeshContainer();
            Assert.IsNull(subject.InvalidMeshContainer);
        }

        [Test]
        public void ClearInvalidMeshContainerInactiveGameObject()
        {
            Assert.IsNull(subject.InvalidMeshContainer);
            subject.InvalidMeshContainer = containingObject;
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
            subject.gameObject.SetActive(false);
            subject.ClearInvalidMeshContainer();
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
        }

        [Test]
        public void ClearInvalidMeshContainerInactiveComponent()
        {
            Assert.IsNull(subject.InvalidMeshContainer);
            subject.InvalidMeshContainer = containingObject;
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
            subject.enabled = false;
            subject.ClearInvalidMeshContainer();
            Assert.AreEqual(containingObject, subject.InvalidMeshContainer);
        }

        [Test]
        public void SetElementVisibility()
        {
            Assert.AreEqual(PointerElement.Visibility.OnWhenPointerActivated, subject.ElementVisibility);
            subject.ElementVisibility = PointerElement.Visibility.AlwaysOn;
            Assert.AreEqual(PointerElement.Visibility.AlwaysOn, subject.ElementVisibility);
            subject.ElementVisibility = PointerElement.Visibility.AlwaysOff;
            Assert.AreEqual(PointerElement.Visibility.AlwaysOff, subject.ElementVisibility);
        }
    }
}