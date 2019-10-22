namespace Zinnia.Extension
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Extended methods for <see cref="Behaviour"/>.
    /// </summary>
    public static class BehaviourExtensions
    {
        /// <summary>
        /// The active state of a GameObject.
        /// </summary>
        public enum GameObjectActivity
        {
            /// <summary>
            /// The GameObject active state is of no interest.
            /// </summary>
            None,
            /// <summary>
            /// The GameObject itself needs to be active, the state of parent GameObjects is ignored.
            /// </summary>
            Self,
            /// <summary>
            /// The GameObject is active in the scene because it is active itself and all parent GameObjects are, too.
            /// </summary>
            InHierarchy
        }

        /// <summary>
        /// Executes the given <see cref="Action"/> when the <see cref="Behaviour"/> becomes active and enabled or immediately runs if the <see cref="Behaviour"/> is already active and enabled.
        /// </summary>
        /// <param name="behaviour">The <see cref="Behaviour"/> to check against.</param>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        public static void RunWhenActiveAndEnabled(this Behaviour behaviour, Action action)
        {
            void OnBeforeRender()
            {
                if (behaviour == null)
                {
                    Application.onBeforeRender -= OnBeforeRender;
                    return;
                }

                if (!behaviour.isActiveAndEnabled)
                {
                    return;
                }

                Application.onBeforeRender -= OnBeforeRender;
                action();
            }

            if (behaviour.isActiveAndEnabled)
            {
                action();
                return;
            }

            Application.onBeforeRender += OnBeforeRender;
        }

        /// <summary>
        /// Indicates if the <see cref="Behaviour"/> is considered valid if the active scene state of the containing <see cref="GameObject"/> and the component enabled state are matched.
        /// </summary>
        /// <param name="behaviour">The <see cref="Behaviour"/> to check against.</param>
        /// <param name="gameObjectActivity">The required active state of the <see cref="GameObject"/> that the component the method is on is added to.</param>
        /// <param name="behaviourNeedsToBeEnabled">The required state of the <see cref="Behaviour"/>.</param>
        /// <returns>Whether the <see cref="Behaviour"/> state is valid.</returns>
        public static bool IsValidState(this Behaviour behaviour, GameObjectActivity gameObjectActivity = GameObjectActivity.InHierarchy, bool behaviourNeedsToBeEnabled = true)
        {
            return
                (!behaviourNeedsToBeEnabled || behaviour.enabled)
                &&
                ((gameObjectActivity & GameObjectActivity.None) != 0
                || ((gameObjectActivity & GameObjectActivity.Self) != 0 && behaviour.gameObject.activeSelf)
                || ((gameObjectActivity & GameObjectActivity.InHierarchy) != 0 && behaviour.gameObject.activeInHierarchy));
        }
    }
}