namespace Zinnia.Rule
{
    using UnityEngine;
    using Malimbe.BehaviourStateRequirementMethod;

    /// <summary>
    /// Determines whether a <see cref="Behaviour"/> is active in the scene hierarchy.
    /// </summary>
    public class IsActiveAndEnabledRule : MonoBehaviour, IRule
    {
        /// <inheritdoc />
        [RequiresBehaviourState]
        public bool Accepts(object target)
        {
            Behaviour behaviour = target as Behaviour;
            return behaviour != null && behaviour.isActiveAndEnabled;
        }
    }
}