namespace VRTK.Core.Process
{
    using UnityEngine;
    using System;

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
        public Component[] targetComponents = Array.Empty<Component>();

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

        protected abstract void ProcessComponent(Component source, Component target);

        /// <summary>
        /// Processes the source <see cref="Component"/> against every target <see cref="Component"/> in the array.
        /// </summary>
        protected virtual void ProcessAllComponents()
        {
            foreach (Component currentComponent in targetComponents)
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
            foreach (Component currentComponent in targetComponents)
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