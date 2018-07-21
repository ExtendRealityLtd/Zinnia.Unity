namespace VRTK.Core.Tracking.Follow.Modifier.Property
{
    using UnityEngine;
    using VRTK.Core.Tracking.Follow;

    public abstract class PropertyModifier : MonoBehaviour
    {
        public bool applyOffset = true;

        /// <summary>
        /// Emitted before the property is modified.
        /// </summary>
        public ObjectFollow.UnityEvent Premodified = new ObjectFollow.UnityEvent();
        /// <summary>
        /// Emitted after the property is modified.
        /// </summary>
        public ObjectFollow.UnityEvent Modified = new ObjectFollow.UnityEvent();

        protected ObjectFollow.EventData eventData = new ObjectFollow.EventData();

        /// <summary>
        /// Determines whether the offset will be applied on the modification.
        /// </summary>
        /// <param name="applyOffset"><see langword="true"/> will apply the offset during modification.</param>
        public virtual void ApplyOffset(bool applyOffset)
        {
            this.applyOffset = applyOffset;
        }

        /// <summary>
        /// Attempts modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        public virtual void Modify(GameObject source, GameObject target, GameObject offset = null)
        {
            if (!isActiveAndEnabled || source == null || target == null)
            {
                return;
            }

            offset = (applyOffset ? offset : null);

            Premodified?.Invoke(eventData.Set(source, target, offset));
            DoModify(source, target, offset);
            Modified?.Invoke(eventData.Set(source, target, offset));
        }

        /// <summary>
        /// Attempts modify the target.
        /// </summary>
        /// <param name="source">The source to utilize in the modification.</param>
        /// <param name="target">The target to modify.</param>
        /// <param name="offset">The offset of the target against the source when modifying.</param>
        protected abstract void DoModify(GameObject source, GameObject target, GameObject offset = null);
    }
}