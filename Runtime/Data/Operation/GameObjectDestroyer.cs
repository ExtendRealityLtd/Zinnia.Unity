namespace Zinnia.Data.Operation
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Destroys the <see cref="Target"/> <see cref="GameObject"/>.
    /// </summary>
    public class GameObjectDestroyer : MonoBehaviour
    {
        [Tooltip("The object to destroy.")]
        [SerializeField]
        private GameObject target;
        /// <summary>
        /// The object to destroy.
        /// </summary>
        public GameObject Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }
        [Tooltip("Whether to destroy the GameObject immediately or at the end of the frame.")]
        [SerializeField]
        private bool destroyAtEndOfFrame = true;
        /// <summary>
        /// Whether to destroy the <see cref="GameObject"/> immediately or at the end of the frame
        /// </summary>
        public bool DestroyAtEndOfFrame
        {
            get
            {
                return destroyAtEndOfFrame;
            }
            set
            {
                destroyAtEndOfFrame = value;
            }
        }

        /// <summary>
        /// Sets the <see cref="Target"/> to the given <see cref="GameObject"/> and then destroys it.
        /// </summary>
        /// <param name="givenTarget">The object to destroy.</param>
        public virtual void DoDestroy(GameObject givenTarget)
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = givenTarget;
            DoDestroy();
        }

        /// <summary>
        /// Destroys the <see cref="Target"/> <see cref="GameObject"/>.
        /// </summary>
        public virtual void DoDestroy()
        {
            if (!this.IsValidState())
            {
                return;
            }

            if (DestroyAtEndOfFrame)
            {
                Destroy(Target);
            }
            else
            {
                DestroyImmediate(Target);
            }
        }
    }
}