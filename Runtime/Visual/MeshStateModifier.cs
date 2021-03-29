namespace Zinnia.Visual
{
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Modifies the enabled state of the mesh associated with given <see cref="GameObject"/>.
    /// </summary>
    public class MeshStateModifier : MonoBehaviour
    {
        /// <summary>
        /// Defines the event with the <see cref="Renderer"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<Renderer> { }

        /// <summary>
        /// Mesh types that can be modified.
        /// </summary>
        [Flags]
        public enum MeshTypes
        {
            /// <summary>
            /// The <see cref="MeshRenderer"/> component.
            /// </summary>
            MeshRenderer = 1 << 0,
            /// <summary>
            /// The <see cref="SkinnedMeshRenderer"/> component.
            /// </summary>
            SkinnedMeshRenderer = 1 << 1
        }

        #region Mesh Settings
        /// <summary>
        /// The mesh components to modify.
        /// </summary>
        [Serialized]
        [field: Header("Mesh Settings"), DocumentedByXml]
        public MeshTypes MeshesToModifiy { get; set; } = (MeshTypes)(-1);

        /// <summary>
        /// A relationship connection between a given <see cref="GameObject"/> and any meshes to modify upon matching.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObjectMultiRelationObservableList MeshCollections { get; set; }
        #endregion

        #region Visibility Events
        /// <summary>
        /// Emitted when the mesh is shown.
        /// </summary>
        [Header("Visibility Events"), DocumentedByXml]
        public UnityEvent Shown = new UnityEvent();
        /// <summary>
        /// Emitted when the mesh is hidden.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Hidden = new UnityEvent();
        #endregion

        /// <summary>
        /// Shows the mesh found on the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="container">The <see cref="GameObject"/> to search for a mesh on.</param>
        [RequiresBehaviourState]
        public virtual void ShowMesh(GameObject container)
        {
            ToggleMesh(container, true);
        }

        /// <summary>
        /// Shows the mesh found on the given <see cref="Component"/>.
        /// </summary>
        /// <param name="container">The <see cref="Component"/> to search for a mesh on.</param>
        [RequiresBehaviourState]
        public virtual void ShowMesh(Component container)
        {
            if (container == null)
            {
                return;
            }

            ShowMesh(container.gameObject);
        }

        /// <summary>
        /// Shows any meshes found on any children of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="container">The <see cref="GameObject"/> to begin the descendant search from.</param>
        [RequiresBehaviourState]
        public virtual void ShowMeshInChildren(GameObject container)
        {
            ToggleAllMeshesInChildren(container, true);
        }

        /// <summary>
        /// Shows any meshes found on any children of the given <see cref="Component"/>.
        /// </summary>
        /// <param name="container">The <see cref="Component"/> to begin the descendant search from.</param>
        [RequiresBehaviourState]
        public virtual void ShowMeshInChildren(Component container)
        {
            if (container == null)
            {
                return;
            }

            ShowMeshInChildren(container.gameObject);
        }

        /// <summary>
        /// Shows the meshes found for any relation matched to the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="key">The key to match on the <see cref="MeshCollections"/>.</param>
        [RequiresBehaviourState]
        public virtual void ShowMeshInCollections(GameObject key)
        {
            ToggleMeshInCollections(key, true);
        }

        /// <summary>
        /// Shows the meshes found for any relation matched to the given <see cref="Component"/>.
        /// </summary>
        /// <param name="key">The key to match on the <see cref="MeshCollections"/>.</param>
        [RequiresBehaviourState]
        public virtual void ShowMeshInCollections(Component key)
        {
            if (key == null)
            {
                return;
            }

            ShowMeshInCollections(key.gameObject);
        }

        /// <summary>
        /// Hides the mesh found on the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="container">The <see cref="GameObject"/> to search for a mesh on.</param>
        [RequiresBehaviourState]
        public virtual void HideMesh(GameObject container)
        {
            ToggleMesh(container, false);
        }

        /// <summary>
        /// Hides the mesh found on the given <see cref="Component"/>.
        /// </summary>
        /// <param name="container">The <see cref="Component"/> to search for a mesh on.</param>
        [RequiresBehaviourState]
        public virtual void HideMesh(Component container)
        {
            if (container == null)
            {
                return;
            }

            HideMesh(container.gameObject);
        }

        /// <summary>
        /// Hides any meshes found on any children of the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="container">The <see cref="GameObject"/> to begin the descendant search from.</param>
        [RequiresBehaviourState]
        public virtual void HideMeshInChildren(GameObject container)
        {
            ToggleAllMeshesInChildren(container, false);
        }

        /// <summary>
        /// Hides any meshes found on any children of the given <see cref="Component"/>.
        /// </summary>
        /// <param name="container">The <see cref="Component"/> to begin the descendant search from.</param>
        [RequiresBehaviourState]
        public virtual void HideMeshInChildren(Component container)
        {
            if (container == null)
            {
                return;
            }

            HideMeshInChildren(container.gameObject);
        }

        /// <summary>
        /// Hides the meshes found for any relation matched to the given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="key">The key to match on the <see cref="MeshCollections"/>.</param>
        [RequiresBehaviourState]
        public virtual void HideMeshInCollections(GameObject key)
        {
            ToggleMeshInCollections(key, false);
        }

        /// <summary>
        /// Hides the meshes found for any relation matched to the given <see cref="Component"/>.
        /// </summary>
        /// <param name="key">The key to match on the <see cref="MeshCollections"/>.</param>
        [RequiresBehaviourState]
        public virtual void HideMeshInCollections(Component key)
        {
            if (key == null)
            {
                return;
            }

            HideMeshInCollections(key.gameObject);
        }

        /// <summary>
        /// Toggles the state of the meshes in the collection for the matching key.
        /// </summary>
        /// <param name="key">The key to match in the <see cref="MeshCollections"/>.</param>
        /// <param name="state">The state to toggle the mesh to.</param>
        protected virtual void ToggleMeshInCollections(GameObject key, bool state)
        {
            if (MeshCollections.HasRelationship(key, out List<GameObject> relations))
            {
                if (relations.Count == 0)
                {
                    ToggleAllMeshesInChildren(key, state);
                }
                else
                {
                    foreach (GameObject relation in relations)
                    {
                        ToggleMesh(relation, state);
                    }
                }
            }
        }

        /// <summary>
        /// Toggles all meshes found in the container and the children.
        /// </summary>
        /// <param name="container">The container to begin the search from.</param>
        /// <param name="state">The state to toggle the mesh to.</param>
        protected virtual void ToggleAllMeshesInChildren(GameObject container, bool state)
        {
            if ((MeshesToModifiy & MeshTypes.MeshRenderer) != 0)
            {
                foreach (MeshRenderer mesh in container.GetComponentsInChildren<MeshRenderer>())
                {
                    mesh.enabled = state;
                    EmitEvent(mesh, state);
                }
            }

            if ((MeshesToModifiy & MeshTypes.SkinnedMeshRenderer) != 0)
            {
                foreach (SkinnedMeshRenderer mesh in container.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    mesh.enabled = state;
                    EmitEvent(mesh, state);
                }
            }
        }

        /// <summary>
        /// Toggles all meshes found on the given container.
        /// </summary>
        /// <param name="container">The container to search on.</param>
        /// <param name="state">The state to toggle the mesh to.</param>
        protected virtual void ToggleMesh(GameObject container, bool state)
        {
            MeshRenderer meshRenderer = (MeshesToModifiy & MeshTypes.MeshRenderer) != 0 ? container.GetComponent<MeshRenderer>() : null;
            SkinnedMeshRenderer skinnedMeshRenderer = (MeshesToModifiy & MeshTypes.SkinnedMeshRenderer) != 0 ? container.GetComponent<SkinnedMeshRenderer>() : null;

            if (meshRenderer != null)
            {
                meshRenderer.enabled = state;
                EmitEvent(meshRenderer, state);
            }

            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = state;
                EmitEvent(skinnedMeshRenderer, state);
            }
        }

        /// <summary>
        /// Emits the appropriate state event.
        /// </summary>
        /// <param name="renderer">The <see cref="Renderer"/> to emit with the event.</param>
        /// <param name="state">The event type to raise.</param>
        protected virtual void EmitEvent(Renderer renderer, bool state)
        {
            if (state)
            {
                Shown?.Invoke(renderer);
            }
            else
            {
                Hidden?.Invoke(renderer);
            }
        }
    }
}