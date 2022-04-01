namespace Zinnia.Rule
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR;
    using Zinnia.Extension;
    using Zinnia.Tracking.CameraRig;

    /// <summary>
    /// Determines whether the selected <see cref="Controller"/> matches the current value set in the given <see cref="DominantControllerObserver"/>.
    /// </summary>
    public class DominantControllerRule : Rule
    {
        /// <summary>
        /// The controller types.
        /// </summary>
        public enum Controller
        {
            /// <summary>
            /// The headset as a controller
            /// </summary>
            Head,
            /// <summary>
            /// The left controller.
            /// </summary>
            LeftController,
            /// <summary>
            /// The right controller.
            /// </summary>
            RightController
        }

        [Tooltip("A source collection to get the first active current dominant controller from.")]
        [SerializeField]
        private List<DominantControllerObserver> sources = new List<DominantControllerObserver>();
        /// <summary>
        /// A source collection to get the first active current dominant controller from.
        /// </summary>
        public List<DominantControllerObserver> Sources
        {
            get
            {
                return sources;
            }
            set
            {
                sources = value;
            }
        }

        [Tooltip("The controller to check to see if the source matches.")]
        [SerializeField]
        private Controller toMatch;
        /// <summary>
        /// The controller to check to see if the source matches.
        /// </summary>
        public Controller ToMatch
        {
            get
            {
                return toMatch;
            }
            set
            {
                toMatch = value;
            }
        }

        /// <inheritdoc />
        public override bool Accepts(object _)
        {
            if (ShouldAutoRejectDueToState())
            {
                return false;
            }

            foreach (DominantControllerObserver Source in Sources)
            {
                if (!Source.gameObject.activeInHierarchy || !Source.enabled)
                {
                    continue;
                }

                switch (ToMatch)
                {
                    case Controller.Head:
                        return Source.DominantController == XRNode.Head;
                    case Controller.LeftController:
                        return Source.DominantController == XRNode.LeftHand;
                    case Controller.RightController:
                        return Source.DominantController == XRNode.RightHand;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the <see cref="ToMatch"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="Controller"/>.</param>
        public virtual void SetToMatch(int index)
        {
            ToMatch = EnumExtensions.GetByIndex<Controller>(index);
        }
    }
}