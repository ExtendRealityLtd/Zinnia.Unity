namespace Zinnia.Action
{
    using System.Linq;
    using System.Collections.Generic;
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
        [DocumentedByXml]
        public List<Action> actions = new List<Action>();

        protected virtual void Update()
        {
            bool areAnyActionsActivated = actions.EmptyIfNull().Any(action => action.IsActivated);
            if (areAnyActionsActivated != IsActivated)
            {
                Receive(areAnyActionsActivated);
            }
        }
    }
}