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
    }
}