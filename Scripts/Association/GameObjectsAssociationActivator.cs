namespace VRTK.Core.Association
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using VRTK.Core.Extension;
    using VRTK.Core.Process;

    /// <summary>
    /// (De)activates <see cref="GameObjectsAssociation"/>s.
    /// </summary>
    public class GameObjectsAssociationActivator : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// The associations in order they will be activated if they match the currently expected state.
        /// </summary>
        [Tooltip("The associations in order they will be activated if they match the currently expected state.")]
        public List<GameObjectsAssociation> associations = new List<GameObjectsAssociation>();

        /// <summary>
        /// The currently activated association, or <see langword="null"/> if no association is activated.
        /// </summary>
        public GameObjectsAssociation CurrentAssociation { get; private set; }

        /// <summary>
        /// Activates the <see cref="GameObject"/>s that are part of the association if the association matches the currently expected state.
        /// </summary>
        public virtual void Activate()
        {
            GameObjectsAssociation desiredAssociation = associations.EmptyIfNull().FirstOrDefault(association => association.ShouldBeActive());
            if (desiredAssociation == null || CurrentAssociation == desiredAssociation)
            {
                return;
            }

            CurrentAssociation = desiredAssociation;

            IEnumerable<GameObjectsAssociation> otherAssociations = associations.EmptyIfNull()
                .Except(
                    new[]
                    {
                        desiredAssociation
                    });
            foreach (GameObject otherAssociationObject in otherAssociations.SelectMany(otherAssociation => otherAssociation.gameObjects.EmptyIfNull()))
            {
                otherAssociationObject.SetActive(false);
            }

            foreach (GameObject associationObject in desiredAssociation.gameObjects.EmptyIfNull())
            {
                associationObject.SetActive(true);
            }
        }

        /// <summary>
        /// Deactivates the association that is currently activated and all other known associations.
        /// </summary>
        public virtual void Deactivate()
        {
            foreach (GameObject associationObject in associations.EmptyIfNull()
                .Append(CurrentAssociation)
                .Where(association => association != null)
                .SelectMany(association => association.gameObjects.EmptyIfNull()))
            {
                associationObject.SetActive(false);
            }

            CurrentAssociation = null;
        }

        public void Process()
        {
            Activate();
        }

        protected virtual void Awake()
        {
            if (associations.EmptyIfNull().Any(association => association.gameObjects.EmptyIfNull().Any(associationObject => associationObject.activeInHierarchy)))
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
    }
}