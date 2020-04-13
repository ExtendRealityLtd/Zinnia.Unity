using Zinnia.Data.Operation.Extraction;

namespace Test.Zinnia.Data.Operation.Extraction
{
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;
    public class ComponentRigidbodyExtractorTest
    {
        private GameObject containingObject;
        private ComponentRigidbodyExtractor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<ComponentRigidbodyExtractor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }


        [Test]
        public void ExtractFromSelf()
        {
            GameObject source = new GameObject();
            Rigidbody target = source.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = source.transform;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, target);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void ExtractFromDescendant()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);
            Rigidbody target = grandchild.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = child.transform;
            subject.SearchAlsoOn = ComponentRigidbodyExtractor.SearchCriteria.IncludeDescendants;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, target);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractFromAncestor()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);
            Rigidbody target = parent.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = child.transform;
            subject.SearchAlsoOn = ComponentRigidbodyExtractor.SearchCriteria.IncludeAncestors;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, target);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractFromDescendantOrAncestorOnAncestor()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);
            Rigidbody target = parent.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = child.transform;
            subject.SearchAlsoOn = ComponentRigidbodyExtractor.SearchCriteria.IncludeAncestors | ComponentRigidbodyExtractor.SearchCriteria.IncludeDescendants;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, target);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractFromDescendantOrAncestorOnDescendant()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);
            Rigidbody target = grandchild.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = child.transform;
            subject.SearchAlsoOn = ComponentRigidbodyExtractor.SearchCriteria.IncludeAncestors | ComponentRigidbodyExtractor.SearchCriteria.IncludeDescendants;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, target);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void ExtractFromSelfGameObject()
        {
            GameObject source = new GameObject();
            Rigidbody target = source.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.SetSource(source);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsTrue(extractedMock.Received);
            Assert.AreEqual(subject.Result, target);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void InvalidExtractFromDescendant()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);
            Rigidbody target = parent.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = child.transform;
            subject.SearchAlsoOn = ComponentRigidbodyExtractor.SearchCriteria.IncludeDescendants;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void InvalidExtractFromAncestor()
        {
            GameObject parent = new GameObject();
            GameObject child = new GameObject();
            GameObject grandchild = new GameObject();
            child.transform.SetParent(parent.transform);
            grandchild.transform.SetParent(child.transform);
            Rigidbody target = grandchild.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = child.transform;
            subject.SearchAlsoOn = ComponentRigidbodyExtractor.SearchCriteria.IncludeAncestors;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(parent);
        }

        [Test]
        public void InvalidExtractInactiveGameObject()
        {
            GameObject source = new GameObject();
            Rigidbody target = source.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = source.transform;
            subject.gameObject.SetActive(false);

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(source);
        }

        [Test]
        public void InvalidExtractInactiveComponent()
        {
            GameObject source = new GameObject();
            Rigidbody target = source.AddComponent<Rigidbody>();

            UnityEventListenerMock extractedMock = new UnityEventListenerMock();
            subject.Extracted.AddListener(extractedMock.Listen);

            subject.Source = source.transform;
            subject.enabled = false;

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            subject.Extract();

            Assert.IsFalse(extractedMock.Received);
            Assert.IsNull(subject.Result);

            Object.DestroyImmediate(source);
        }
    }
}