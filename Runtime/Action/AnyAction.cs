namespace Zinnia.Action
{
    using System.Linq;
    using System.Collections.Generic;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.PropertySetterMethod;
    using Malimbe.PropertyValidationMethod;
    using Malimbe.XmlDocumentationAttribute;
    using Zinnia.Extension;

    /// <summary>
    /// Emits a <see cref="bool"/> value when any given actions are in their active state.
    /// </summary>
    public class AnyAction : BooleanAction
    {
        /// <summary>
        /// Actions to check the active state on.
        /// </summary>
        [Serialized, Validated]
        [field: DocumentedByXml]
        public List<Action> Actions { get; set; } = new List<Action>();

        protected virtual void Update()
        {
            bool areAnyActionsActivated = Actions.EmptyIfNull().Any(action => action.IsActivated);
            if (areAnyActionsActivated != IsActivated)
            {
                Receive(areAnyActionsActivated);
            }
        }

        /// <summary>
        /// Handles changes to <see cref="Actions"/>.
        /// </summary>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        [CalledBySetter(nameof(Actions))]
        protected virtual void OnActionsChange(List<Action> previousValue, ref List<Action> newValue)
        {
            Update();
        }
    }
}