namespace VRTK.Core.Tracking.Modification
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VRTK.Core.Data.Attribute;
    using VRTK.Core.Data.Type;
    using VRTK.Core.Extension;

    /// <summary>
    /// Enables or disables <see cref="Behaviour"/>s on a game object.
    /// </summary>
    public class BehaviourEnabledSetter : MonoBehaviour
    {
        /// <summary>
        /// The <see cref="Behaviour"/> types to enable or disable.
        /// </summary>
        [TypePicker(typeof(Behaviour))]
        [Tooltip("The behaviour types to enable or disable.")]
        public List<SerializableType> behaviourTypes = new List<SerializableType>();

        /// <summary>
        /// The target to search for <see cref="Behaviour"/>s.
        /// </summary>
        [Tooltip("The target to search for behaviours.")]
        public GameObject target;

        /// <summary>
        /// Enables or disables all matching <see cref="Behaviour"/>s found on <see cref="target"/>.
        /// </summary>
        /// <param name="state">The enabled state to apply.</param>
        public virtual void SetBehavioursEnabled(bool state)
        {
            if (!isActiveAndEnabled || target == null)
            {
                return;
            }

            IEnumerable<Behaviour> behaviours = behaviourTypes.EmptyIfNull()
                .SelectMany(serializableType => target.GetComponentsInChildren(serializableType, true))
                .Cast<Behaviour>();
            foreach (Behaviour behaviour in behaviours)
            {
                behaviour.enabled = state;
            }
        }
    }
}