namespace Zinnia.Tracking.Collision
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Extension;

    /// <summary>
    /// Ignores the collisions between the source <see cref="GameObject"/> colliders and the target <see cref="GameObject"/> colliders.
    /// </summary>
    public class CollisionIgnorer : MonoBehaviour
    {
        [Tooltip("The sources to ignore colliders from.")]
        [SerializeField]
        private GameObjectObservableList sources;
        /// <summary>
        /// The sources to ignore colliders from.
        /// </summary>
        public GameObjectObservableList Sources
        {
            get
            {
                return sources;
            }
            set
            {
                if (this.IsMemberChangeAllowed())
                {
                    OnBeforeSourcesChange();
                }
                sources = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterSourcesChange();
                }
            }
        }
        [Tooltip("The targets to ignore colliders with.")]
        [SerializeField]
        private GameObjectObservableList targets;
        /// <summary>
        /// The targets to ignore colliders with.
        /// </summary>
        public GameObjectObservableList Targets
        {
            get
            {
                return targets;
            }
            set
            {
                if (this.IsMemberChangeAllowed())
                {
                    OnBeforeTargetsChange();
                }
                targets = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterTargetsChange();
                }
            }
        }
        [Tooltip("Whether to process inactive GameObjects when ignoring or resuming collisions.")]
        [SerializeField]
        private bool processInactiveGameObjects;
        /// <summary>
        /// Whether to process inactive <see cref="GameObject"/>s when ignoring or resuming collisions.
        /// </summary>
        public bool ProcessInactiveGameObjects
        {
            get
            {
                return processInactiveGameObjects;
            }
            set
            {
                processInactiveGameObjects = value;
            }
        }

        /// <summary>
        /// A reused instance to store the <see cref="Collider"/> collection belonging to the <see cref="Sources"/>.
        /// </summary>
        protected List<Collider> sourceColliders = new List<Collider>();
        /// <summary>
        /// A reused instance to store the <see cref="Collider"/> collection belonging to the <see cref="Targets"/>.
        /// </summary>
        protected List<Collider> targetColliders = new List<Collider>();

        protected virtual void OnEnable()
        {
            RegisterSourceListeners();
            RegisterTargetListeners();
            ToggleCollisions(true);
        }

        protected virtual void OnDisable()
        {
            UnregisterSourceListeners();
            UnregisterTargetListeners();
            ToggleCollisions(false);
        }

        /// <summary>
        /// Registers the listeners for elements that are added or removed from <see cref="Sources"/>.
        /// </summary>
        protected virtual void RegisterSourceListeners()
        {
            if (Sources == null)
            {
                return;
            }

            Sources.Added.AddListener(OnSourceAdded);
            Sources.Removed.AddListener(OnSourceRemoved);
        }

        /// <summary>
        /// Unregisters the listeners for elements that are added or removed from <see cref="Sources"/>.
        /// </summary>
        protected virtual void UnregisterSourceListeners()
        {
            if (Sources == null)
            {
                return;
            }

            Sources.Added.RemoveListener(OnSourceAdded);
            Sources.Removed.RemoveListener(OnSourceRemoved);
        }

        /// <summary>
        /// Registers the listeners for elements that are added or removed from <see cref="Targets"/>.
        /// </summary>
        protected virtual void RegisterTargetListeners()
        {
            if (Targets == null)
            {
                return;
            }

            Targets.Added.AddListener(OnTargetAdded);
            Targets.Removed.AddListener(OnTargetRemoved);
        }

        /// <summary>
        /// Unregisters the listeners for elements that are added or removed from <see cref="Targets"/>.
        /// </summary>
        protected virtual void UnregisterTargetListeners()
        {
            if (Targets == null)
            {
                return;
            }

            Targets.Added.RemoveListener(OnTargetAdded);
            Targets.Removed.RemoveListener(OnTargetRemoved);
        }

        /// <summary>
        /// Responds to a <see cref="GameObject"/> being added to <see cref="Sources"/> and ignores all collisions against <see cref="Targets"/>.
        /// </summary>
        /// <param name="source">The source to ignore collisions from.</param>
        protected virtual void OnSourceAdded(GameObject source)
        {
            ToggleCollisions(source, Sources, Targets, true);
        }

        /// <summary>
        /// Responds to a <see cref="GameObject"/> being removed from <see cref="Sources"/> and resumes all collisions against <see cref="Targets"/>.
        /// </summary>
        /// <param name="source">The source to restore collisions with.</param>
        protected virtual void OnSourceRemoved(GameObject source)
        {
            ToggleCollisions(source, Sources, Targets, false);
        }

        /// <summary>
        /// Responds to a <see cref="GameObject"/> being added to <see cref="Targets"/> and ignores all collisions against <see cref="Sources"/>.
        /// </summary>
        /// <param name="target">The target to ignore collisions on.</param>
        protected virtual void OnTargetAdded(GameObject target)
        {
            ToggleCollisions(target, Targets, Sources, true);
        }

        /// <summary>
        /// Responds to a <see cref="GameObject"/> being removed from <see cref="Targets"/> and resumes all collisions against <see cref="Sources"/>.
        /// </summary>
        /// <param name="target">The target to restore collisions on.</param>
        protected virtual void OnTargetRemoved(GameObject target)
        {
            ToggleCollisions(target, Targets, Sources, false);
        }

        /// <summary>
        /// Sets the collision state between <see cref="Sources"/> and <see cref="Targets"/>.
        /// </summary>
        /// <param name="state">Whether to ignore collisions or not.</param>
        protected virtual void ToggleCollisions(bool state)
        {
            foreach (GameObject source in Sources.SubscribableElements)
            {
                ToggleCollisions(source, Sources, Targets, state);
            }
        }

        /// <summary>
        /// Sets the collision state between the source and targets.
        /// </summary>
        /// <param name="source">The source to set the collision state on.</param>
        /// <param name="sources">A collection of sources to check if the given <see cref="source"/> belongs to.</param>
        /// <param name="targets">A collection of targets to set the collision state on.</param>
        /// <param name="state">Whether to ignore collisions or not.</param>
        protected virtual void ToggleCollisions(GameObject source, GameObjectObservableList sources, GameObjectObservableList targets, bool state)
        {
            if (source == null || (!state && isActiveAndEnabled && sources.Contains(source)))
            {
                return;
            }

            source.GetComponentsInChildren(ProcessInactiveGameObjects, sourceColliders);

            foreach (GameObject target in targets.SubscribableElements)
            {
                if (target == null)
                {
                    continue;
                }

                target.GetComponentsInChildren(ProcessInactiveGameObjects, targetColliders);

                foreach (Collider sourceCollider in sourceColliders)
                {
                    foreach (Collider targetCollider in targetColliders)
                    {
                        Physics.IgnoreCollision(sourceCollider, targetCollider, state);
                    }
                }
            }
        }

        /// <summary>
        /// Called before <see cref="Sources"/> has been changed.
        /// </summary>
        protected virtual void OnBeforeSourcesChange()
        {
            if (Sources != null)
            {
                UnregisterSourceListeners();
                ToggleCollisions(false);
            }
        }

        /// <summary>
        /// Called after <see cref="Sources"/> has been changed.
        /// </summary>
        protected virtual void OnAfterSourcesChange()
        {
            if (Sources != null)
            {
                RegisterSourceListeners();
                ToggleCollisions(true);
            }
        }

        /// <summary>
        /// Called before <see cref="Targets"/> has been changed.
        /// </summary>
        protected virtual void OnBeforeTargetsChange()
        {
            if (Targets != null)
            {
                UnregisterTargetListeners();
                ToggleCollisions(false);
            }
        }

        /// <summary>
        /// Called after <see cref="Targets"/> has been changed.
        /// </summary>
        protected virtual void OnAfterTargetsChange()
        {
            if (Targets != null)
            {
                RegisterTargetListeners();
                ToggleCollisions(true);
            }
        }
    }
}