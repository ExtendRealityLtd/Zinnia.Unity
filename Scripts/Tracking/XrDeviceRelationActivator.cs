namespace VRTK.Core.Tracking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.XR;

    /// <summary>
    /// The XrDeviceRelationActivator activates and deactivates GameObjects based on the currently loaded XR device automatically and allows to override the active GameObject manually.
    /// </summary>
    public class XrDeviceRelationActivator : MonoBehaviour
    {
        /// <summary>
        /// The XrDeviceRelation allows specifying a GameObject that will be (de)activated based on the XR device name.
        /// </summary>
        [Serializable]
        public class XrDeviceRelation
        {
            [Tooltip("The name of the XR device that needs to be loaded for this relation to be activated.")]
            public string xrDeviceName;
            [Tooltip("The GameObjects to (de)activate.")]
            public GameObject[] gameObjects;
        }

        [Tooltip("The relations in order they will be activated if their XR device name matches the currently loaded one.")]
        public XrDeviceRelation[] relations = Array.Empty<XrDeviceRelation>();

        protected XrDeviceRelation currentRelation;

        /// <summary>
        /// The Activate method activates the GameObject that is part of the relation if the XR device name matches the currently loaded one.
        /// </summary>
        /// <param name="relation">The relation to try to activate.</param>
        public virtual void Activate(XrDeviceRelation relation)
        {
            if (currentRelation == relation)
            {
                return;
            }

            string loadedDeviceName = GetLoadedDeviceName();
            if (relation.xrDeviceName != loadedDeviceName)
            {
                throw new ArgumentException($"The specified XR device named '{relation.xrDeviceName}' isn't loaded. The currently loaded XR device is named '{loadedDeviceName}' instead.", nameof(relation));
            }

            currentRelation = relation;

            IEnumerable<XrDeviceRelation> otherRelations = relations.Except(
                new[]
                {
                    relation
                });
            foreach (GameObject relationObject in otherRelations.SelectMany(otherRelation => otherRelation.gameObjects))
            {
                relationObject.SetActive(false);
            }

            foreach (GameObject relationObject in relation.gameObjects)
            {
                relationObject.SetActive(true);
            }
        }

        /// <summary>
        /// Deactivates the relation that is currently activated in addition to all other known relations.
        /// </summary>
        public virtual void Deactivate()
        {
            foreach (GameObject relationObject in relations.Append(currentRelation).Where(relation => relation != null).SelectMany(relation => relation.gameObjects).Where(relationObject => relationObject != null))
            {
                relationObject.SetActive(false);
            }

            currentRelation = null;
        }

        protected virtual void Awake()
        {
            if (relations.Any(relation => relation.gameObjects.Any(relationObject => relationObject.activeInHierarchy)))
            {
                Debug.LogWarning($"At least one relation object is active in the scene on {nameof(Awake)} of this {nameof(XrDeviceRelationActivator)}. Having multiple relation objects active at the same time will most likely lead to issues. Make sure to deactivate them all before you play or create a build.");
            }
        }

        protected virtual void OnDisable()
        {
            Deactivate();
        }

        protected virtual void Update()
        {
            string loadedDeviceName = GetLoadedDeviceName();
            XrDeviceRelation desiredRelation = relations.FirstOrDefault(relation => relation.xrDeviceName == loadedDeviceName);
            if (desiredRelation != null)
            {
                Activate(desiredRelation);
            }
        }

        protected virtual string GetLoadedDeviceName()
        {
            return XRSettings.loadedDeviceName;
        }
    }
}
