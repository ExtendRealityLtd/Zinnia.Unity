namespace VRTK.Core.Process
{
    using UnityEngine;

    /// <summary>
    /// The SourceTargetProcessor implements a Process that runs a set method on a source Component against an array of target Components.
    /// </summary>
    public abstract class SourceTargetProcessor : MonoBehaviour, IProcessable
    {
        [Header("Processor Component Settings")]

        [Tooltip("The source component to apply against the source within the process.")]
        public Component sourceComponent;
        [Tooltip("The target components to apply the source to within the process.")]
        public Component[] targetComponents;

        /// <summary>
        /// Tjhe Component that is currently the active target for the process.
        /// </summary>
        public Component ActiveTargetComponent
        {
            get;
            protected set;
        }

        /// <summary>
        /// The Process method executes the relevant process to apply between the source and target Component.
        /// </summary>
        public abstract void Process();

        protected abstract void ProcessComponent(Component source, Component target);

        /// <summary>
        /// The ProcessAllComponents method processes the source Component against every target Component in the array.
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
        /// The ProcessFirstActiveComponent method processes the source Component against the first active target Component in the array.
        /// </summary>
        protected virtual void ProcessFirstActiveComponent()
        {
            ActiveTargetComponent = null;
            foreach (Component currentComponent in targetComponents)
            {
                if (sourceComponent != null && currentComponent.gameObject.activeInHierarchy)
                {
                    ProcessComponent(sourceComponent, currentComponent);
                    ActiveTargetComponent = currentComponent;
                    break;
                }
            }
        }
    }
}