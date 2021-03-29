using Zinnia.Data.Collection.List;
using Zinnia.Visual;

namespace Test.Zinnia.Visual
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using Test.Zinnia.Utility.Mock;
    using UnityEngine;

    public class MeshStateModifierTest
    {
        private GameObject containingObject;
        private MeshStateModifier subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<MeshStateModifier>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void ShowMeshFromGameObjectWithMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(containingObject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshFromGameObjectWithSkinnedMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            SkinnedMeshRenderer meshRenderer = containingObject.AddComponent<SkinnedMeshRenderer>();
            meshRenderer.enabled = false;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(containingObject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshFromComponentWithMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(subject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshFromComponentWithSkinnedMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            SkinnedMeshRenderer meshRenderer = containingObject.AddComponent<SkinnedMeshRenderer>();
            meshRenderer.enabled = false;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(subject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshExcludeMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            subject.MeshesToModifiy = MeshStateModifier.MeshTypes.SkinnedMeshRenderer;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(containingObject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshExcludeSkinnedMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            SkinnedMeshRenderer meshRenderer = containingObject.AddComponent<SkinnedMeshRenderer>();
            meshRenderer.enabled = false;

            subject.MeshesToModifiy = MeshStateModifier.MeshTypes.MeshRenderer;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(containingObject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInactiveGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            subject.gameObject.SetActive(false);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(containingObject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInactiveComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            subject.enabled = false;

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMesh(containingObject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshFromGameObjectWithMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = true;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(containingObject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshFromGameObjectWithSkinnedMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            SkinnedMeshRenderer meshRenderer = containingObject.AddComponent<SkinnedMeshRenderer>();
            meshRenderer.enabled = true;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(containingObject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshFromComponentWithMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = true;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(subject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshFromComponentWithSkinnedMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            SkinnedMeshRenderer meshRenderer = containingObject.AddComponent<SkinnedMeshRenderer>();
            meshRenderer.enabled = true;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(subject);

            Assert.IsFalse(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshExcludeMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = true;

            subject.MeshesToModifiy = MeshStateModifier.MeshTypes.SkinnedMeshRenderer;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(containingObject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshExcludeSkinnedMeshRenderer()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            SkinnedMeshRenderer meshRenderer = containingObject.AddComponent<SkinnedMeshRenderer>();
            meshRenderer.enabled = true;

            subject.MeshesToModifiy = MeshStateModifier.MeshTypes.MeshRenderer;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(containingObject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInactiveGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = true;

            subject.gameObject.SetActive(false);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(containingObject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInactiveComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            MeshRenderer meshRenderer = containingObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = true;

            subject.enabled = false;

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMesh(containingObject);

            Assert.IsTrue(meshRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInChildrenFromGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInChildren(containingObject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInChildrenFromComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInChildren(subject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInChildrenInactiveGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            subject.gameObject.SetActive(false);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInChildren(containingObject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInChildrenInactiveComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            subject.enabled = false;

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInChildren(containingObject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInChildrenFromGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInChildren(containingObject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInChildrenFromComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInChildren(subject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInChildrenInactiveGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            subject.gameObject.SetActive(false);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInChildren(containingObject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInChildrenInactiveComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            subject.enabled = false;

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInChildren(containingObject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInCollectionsFromGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInCollections(containingObject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInCollectionsFromComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInCollections(subject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsTrue(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInCollectionsInactiveGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);
            subject.gameObject.SetActive(false);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInCollections(containingObject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void ShowMeshInCollectionsInactiveComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();
            childARenderer.enabled = false;

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();
            childBRenderer.enabled = false;

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);
            subject.enabled = false;

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.ShowMeshInCollections(containingObject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsFalse(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInCollectionsFromGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInCollections(containingObject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInCollectionsFromComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInCollections(subject);

            Assert.IsFalse(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsTrue(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInCollectionsInactiveGameObject()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);
            subject.gameObject.SetActive(false);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInCollections(containingObject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }

        [Test]
        public void HideMeshInCollectionsInactiveComponent()
        {
            UnityEventListenerMock shownMock = new UnityEventListenerMock();
            UnityEventListenerMock hiddenMock = new UnityEventListenerMock();
            subject.Shown.AddListener(shownMock.Listen);
            subject.Hidden.AddListener(hiddenMock.Listen);

            subject.MeshCollections = containingObject.AddComponent<GameObjectMultiRelationObservableList>();

            GameObject childA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childA.transform.SetParent(containingObject.transform);
            MeshRenderer childARenderer = childA.GetComponent<MeshRenderer>();

            GameObject childB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            childB.transform.SetParent(containingObject.transform);
            MeshRenderer childBRenderer = childB.GetComponent<MeshRenderer>();

            GameObjectMultiRelationObservableList.MultiRelation relationA = new GameObjectMultiRelationObservableList.MultiRelation
            {
                Key = containingObject,
                Values = new List<GameObject>() { childA }
            };

            subject.MeshCollections.Add(relationA);
            subject.enabled = false;

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);

            subject.HideMeshInCollections(containingObject);

            Assert.IsTrue(childARenderer.enabled);
            Assert.IsTrue(childBRenderer.enabled);
            Assert.IsFalse(shownMock.Received);
            Assert.IsFalse(hiddenMock.Received);
        }
    }
}