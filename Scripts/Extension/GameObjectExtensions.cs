namespace Zinnia.Extension
{
    using UnityEngine;

    /// <summary>
    /// Extended methods for the <see cref="GameObject"/> Type.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Attempts to set the active state.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/> to set the active state on.</param>
        /// <param name="state">The new state.</param>
        public static void TrySetActive(this GameObject gameObject, bool state)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(state);
            }
        }

        /// <summary>
        /// Attempts to retreive the component or if one is not found then optionally search children then search parents for the component.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to search on.</param>
        /// <param name="searchAncestors">Optionally searches all ancestors in the hierarchy for the component.</param>
        /// <param name="searchDescendants">Optionally searches all descendants in the hierarchy for component.</param>
        /// <returns>The component if one exists.</returns>
        public static T TryGetComponent<T>(this GameObject gameObject, bool searchDescendants = false, bool searchAncestors = false) where T : Component
        {
            if (gameObject == null)
            {
                return default;
            }

            T foundComponent = gameObject.GetComponent<T>();

            if (foundComponent == null && searchDescendants)
            {
                foundComponent = gameObject.GetComponentInChildren<T>();
            }

            if (foundComponent == null && searchAncestors)
            {
                foundComponent = gameObject.GetComponentInParent<T>();
            }

            return foundComponent;
        }

        /// <summary>
        /// Attempts to retrieve the position of the <see cref="GameObject.transform"/>.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to retrieve for.</param>
        /// <param name="getLocal">Determines whether to get the local or world position.</param>
        /// <returns>The position of the <see cref="GameObject.transform"/>.</returns>
        public static Vector3 TryGetPosition(this GameObject gameObject, bool getLocal = false)
        {
            return (gameObject != null ? (getLocal ? gameObject.transform.localPosition : gameObject.transform.position) : Vector3.zero);
        }

        /// <summary>
        /// Attempts to retrieve the rotation of the <see cref="GameObject.transform"/>.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to retrieve for.</param>
        /// <param name="getLocal">Determines whether to get the local or world rotation.</param>
        /// <returns>The rotation of the <see cref="GameObject.transform"/>.</returns>
        public static Quaternion TryGetRotation(this GameObject gameObject, bool getLocal = false)
        {
            return (gameObject != null ? (getLocal ? gameObject.transform.localRotation : gameObject.transform.rotation) : Quaternion.identity);
        }

        /// <summary>
        /// Attempts to retrieve the euler rotation of the <see cref="GameObject.transform"/>.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to retrieve for.</param>
        /// <param name="getLocal">Determines whether to get the local or world euler rotation.</param>
        /// <returns>The euler rotation of the <see cref="GameObject.transform"/>.</returns>
        public static Vector3 TryGetEulerRotation(this GameObject gameObject, bool getLocal = false)
        {
            return (gameObject != null ? (getLocal ? gameObject.transform.localEulerAngles : gameObject.transform.eulerAngles) : Vector3.zero);
        }

        /// <summary>
        /// Attempts to retrieve the scale of the <see cref="GameObject.transform"/>.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to retrieve for.</param>
        /// <param name="getLocal">Determines whether to get the local or world scale.</param>
        /// <returns>The scale of the <see cref="GameObject.transform"/>.</returns>
        public static Vector3 TryGetScale(this GameObject gameObject, bool getLocal = false)
        {
            return (gameObject != null ? (getLocal ? gameObject.transform.localScale : gameObject.transform.lossyScale) : Vector3.zero);
        }
    }
}
