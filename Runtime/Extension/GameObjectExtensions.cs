namespace Zinnia.Extension
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extended methods for the <see cref="GameObject"/> Type.
    /// </summary>
    public static class GameObjectExtensions
    {
#if UNITY_EDITOR
        private static class ComponentCache<T> where T : Component
        {
            public static readonly List<T> List = new List<T>();
        }
#endif

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
        /// Attempts to retrieve the component or if one is not found then optionally search children then search parents for the component.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve.</typeparam>
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

#if UNITY_EDITOR
            gameObject.GetComponents(ComponentCache<T>.List);
            T foundComponent = ComponentCache<T>.List.Count > 0 ? ComponentCache<T>.List[0] : null;
#else
            T foundComponent = gameObject.GetComponent<T>();
#endif

            if (foundComponent == null && searchDescendants)
            {
#if UNITY_EDITOR
                gameObject.GetComponentsInChildren(ComponentCache<T>.List);
                foundComponent = ComponentCache<T>.List.Count > 0 ? ComponentCache<T>.List[0] : null;
#else
                foundComponent = gameObject.GetComponentInChildren<T>();
#endif
            }

            if (foundComponent == null && searchAncestors)
            {
#if UNITY_EDITOR
                gameObject.GetComponentsInParent(false, ComponentCache<T>.List);
                foundComponent = ComponentCache<T>.List.Count > 0 ? ComponentCache<T>.List[0] : null;
#else
                foundComponent = gameObject.GetComponentInParent<T>();
#endif
            }

            return foundComponent;
        }

        /// <summary>
        /// Attempts to retrieve the component or if one is not found then optionally search children then search parents for the component.
        /// </summary>
        /// <param name="gameObject">The reference <see cref="GameObject"/> to search on.</param>
        /// <param name="searchAncestors">Optionally searches all ancestors in the hierarchy for the component.</param>
        /// <param name="type">The </param>
        /// <param name="searchDescendants">Optionally searches all descendants in the hierarchy for component.</param>
        /// <returns>The component if one exists.</returns>
        public static Component TryGetComponent(this GameObject gameObject, Type type, bool searchDescendants = false, bool searchAncestors = false)
        {
            if (gameObject == null)
            {
                return default;
            }

#if UNITY_EDITOR
            gameObject.GetComponents(type, ComponentCache<Component>.List);
            Component foundComponent = ComponentCache<Component>.List.Count > 0 ? ComponentCache<Component>.List[0] : null;
#else
            Component foundComponent = gameObject.GetComponent(type);
#endif

            if (foundComponent == null && searchDescendants)
            {
#if UNITY_EDITOR
                // Unity doesn't offer an overload that works with a non-generic List and passing the type...
                gameObject.GetComponentsInChildren(ComponentCache<Component>.List);

                // ...so let's find it ourselves.
                foreach (Component component in ComponentCache<Component>.List)
                {
                    if (type.IsInstanceOfType(component))
                    {
                        foundComponent = component;
                        break;
                    }
                }
#else
                foundComponent = gameObject.GetComponentInChildren(type);
#endif
            }

            if (foundComponent == null && searchAncestors)
            {
#if UNITY_EDITOR
                // Unity doesn't offer an overload that works with a non-generic List and passing the type...
                gameObject.GetComponentsInParent(false, ComponentCache<Component>.List);

                // ...so let's find it ourselves.
                foreach (Component component in ComponentCache<Component>.List)
                {
                    if (type.IsInstanceOfType(component))
                    {
                        foundComponent = component;
                        break;
                    }
                }
#else
                foundComponent = gameObject.GetComponentInParent(type);
#endif
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
