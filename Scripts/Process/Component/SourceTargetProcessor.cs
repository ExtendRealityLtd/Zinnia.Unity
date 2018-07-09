﻿namespace VRTK.Core.Process.Component
{
    using UnityEngine;
    using System.Collections.Generic;
    using VRTK.Core.Extension;

    /// <summary>
    /// An <see cref="IProcessable"/> that runs a set method on a source <see cref="Component"/> against an array of target <see cref="Component"/>s.
    /// </summary>
    public abstract class SourceTargetProcessor : MonoBehaviour, IProcessable
    {
        [Header("Processor Component Settings")]

        /// <summary>
        /// The source <see cref="Component"/> to apply against the source within the process.
        /// </summary>
        [Tooltip("The source Component to apply against the source within the process.")]
        public Component sourceComponent;
        /// <summary>
        /// The target <see cref="Component"/>s to apply the source to within the process.
        /// </summary>
        [Tooltip("The target Components to apply the source to within the process.")]
        public List<Component> targetComponents = new List<Component>();

        /// <summary>
        /// The <see cref="Component"/> that is currently the active target for the process.
        /// </summary>
        public Component ActiveTargetComponent
        {
            get;
            protected set;
        }

        /// <summary>
        /// Executes the relevant process to apply between the source and target <see cref="Component"/>.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Sets the source component to the the given <see cref="GameObject"/> component.
        /// </summary>
        /// <param name="source">The new source.</param>
        public virtual void SetSource(GameObject source)
        {
            sourceComponent = source == null ? null : source.GetComponent<Component>();
        }

        /// <summary>
        /// Adds a new target to the target components from a given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The target to add.</param>
        public virtual void AddTarget(GameObject target)
        {
            Component addTarget = target == null ? null : target.GetComponent<Component>();
            if (addTarget != null)
            {
                targetComponents.Add(addTarget);
            }
        }

        /// <summary>
        /// Removes an existing target from the target components from a given <see cref="GameObject"/>.
        /// </summary>
        /// <param name="target">The target to remove.</param>
        public virtual void RemoveTarget(GameObject target)
        {
            Component removeTarget = target == null ? null : target.GetComponent<Component>();
            if (removeTarget != null)
            {
                targetComponents.Remove(removeTarget);
            }
        }

        /// <summary>
        /// Clears the existing target components.
        /// </summary>
        public virtual void ClearTargets()
        {
            targetComponents.Clear();
        }

        protected abstract void ProcessComponent(Component source, Component target);

        /// <summary>
        /// Processes the source <see cref="Component"/> against every target <see cref="Component"/> in the array.
        /// </summary>
        protected virtual void ProcessAllComponents()
        {
            foreach (Component currentComponent in targetComponents.EmptyIfNull())
            {
                if (sourceComponent != null)
                {
                    ProcessComponent(sourceComponent, currentComponent);
                }
            }
        }

        /// <summary>
        /// Processes the source <see cref="Component"/> against the first active target <see cref="Component"/> in the array.
        /// </summary>
        protected virtual void ProcessFirstActiveComponent()
        {
            ActiveTargetComponent = null;
            foreach (Component currentComponent in targetComponents.EmptyIfNull())
            {
                if (sourceComponent != null && currentComponent != null && currentComponent.gameObject.activeInHierarchy)
                {
                    ProcessComponent(sourceComponent, currentComponent);
                    ActiveTargetComponent = currentComponent;
                    break;
                }
            }
        }
    }
}