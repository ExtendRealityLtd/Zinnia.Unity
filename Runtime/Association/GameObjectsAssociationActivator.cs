namespace Zinnia.Association
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertySetterMethod;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Process;

    /// <summary>
    /// (De)activates <see cref="GameObjectsAssociation"/>s.
    /// </summary>
    public class GameObjectsAssociationActivator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The associations in order they will be activated if they match the currently expected state.
        /// </summary>
        [Serialized, Validated]
        [field: DocumentedByXml]
        public List<GameObjectsAssociation> Associations { get; set; } = new List<GameObjectsAssociation>();

        /// <summary>
        /// The currently activated association, or <see langword="null"/> if no association is activated.
        /// </summary>
        public GameObjectsAssociation CurrentAssociation { get; private set; }

        /// <summary>
        /// Activates the <see cref="GameObject"/>s that are part of the association if the association matches the currently expected state.
        /// </summary>
        public virtual void Activate()
        {
            GameObjectsAssociation desiredAssociation = Associations.FirstOrDefault(association => association.ShouldBeActive());
            if (desiredAssociation == null || CurrentAssociation == desiredAssociation)
            {
                return;
            }

            CurrentAssociation = desiredAssociation;

            IEnumerable<GameObjectsAssociation> otherAssociations = Associations.Except(
                new[]
                {
                    desiredAssociation
                });
            foreach (GameObject otherAssociationObject in otherAssociations.SelectMany(otherAssociation => otherAssociation.gameObjects))
            {
                otherAssociationObject.SetActive(false);
            }

            foreach (GameObject associationObject in desiredAssociation.gameObjects)
            {
                associationObject.SetActive(true);
            }
        }

        /// <summary>
        /// Deactivates the association that is currently activated and all other known associations.
        /// </summary>
        public virtual void Deactivate()
        {
            Deactivate(Associations);
        }

        public void Process()
        {
            Activate();
        }

        protected virtual void Awake()
        {
            if (Associations.Any(association => association.gameObjects.Any(associationObject => associationObject.activeInHierarchy)))
            {
                Debug.LogWarning($"At least one association object is active in the scene on {nameof(Awake)} of this {GetType().Name}. Having multiple association objects active at the same time will most likely lead to issues. Make sure to deactivate them all before you play or create a build.");
            }
        }

        protected virtual void OnEnable()
        {
            Activate();
        }

        protected virtual void OnDisable()
        {
            Deactivate();
        }

        /// <summary>
        /// Deactivates the association that is currently activated and all other known associations.
        /// </summary>
        /// <param name="associations">The associations to deactivate.</param>
        protected virtual void Deactivate(IEnumerable<GameObjectsAssociation> associations)
        {
            foreach (GameObject associationObject in associations
                .Append(CurrentAssociation)
                .Where(association => association != null)
                .SelectMany(association => association.gameObjects))
            {
                associationObject.SetActive(false);
            }

            CurrentAssociation = null;
        }

        /// <summary>
        /// Handles changes to <see cref="Associations"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        [CalledBySetter(nameof(Associations))]
        protected virtual void OnAssociationsChange(List<GameObjectsAssociation> previousValue, ref List<GameObjectsAssociation> newValue)
        {
            Deactivate(previousValue);
            Activate();
        }
    }
}