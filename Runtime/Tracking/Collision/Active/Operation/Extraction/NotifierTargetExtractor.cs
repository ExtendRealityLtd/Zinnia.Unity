namespace Zinnia.Tracking.Collision.Active.Operation.Extraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Data.Operation.Extraction;
    using Zinnia.Extension;

    /// <summary>
    /// Extracts the <see cref="GameObject"/> of the <see cref="Collider"/> from a given <see cref="CollisionNotifier.EventData"/>.
    /// </summary>
    public class NotifierTargetExtractor : GameObjectExtractor<CollisionNotifier.EventData, NotifierTargetExtractor.UnityEvent>
    {
        /// <summary>
        /// Defines the event with the specified <see cref="GameObject"/>.
        /// </summary>
        [Serializable]
        public class UnityEvent : UnityEvent<GameObject> { }

        [Tooltip("Whether to extract the collder's compound parent, which is the GameObject that contains the collision Rigidbody.")]
        [SerializeField]
        private bool extractCompoundParent = true;
        /// <summary>
        /// Whether to extract the collder's compound parent, which is the <see cref="GameObject"/> that contains the collision <see cref="Rigidbody"/>.
        /// </summary>
        public bool ExtractCompoundParent
        {
            get
            {
                return extractCompoundParent;
            }
            set
            {
                extractCompoundParent = value;
            }
        }

        /// <inheritdoc />
        protected override GameObject ExtractValue()
        {
            Transform containingTransform = Source != null ? (ExtractCompoundParent ? Source.ColliderData.GetContainingTransform() : Source.ColliderData.TryGetTransform()) : null;
            return containingTransform != null ? containingTransform.gameObject : null;
        }
    }
}