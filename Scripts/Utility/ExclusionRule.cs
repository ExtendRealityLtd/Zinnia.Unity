namespace VRTK.Core.Utility
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using VRTK.Core.Data.Attribute;

    /// <summary>
    /// The ExclusionRule allows to create a list of either tag names, script names or layer names that can be checked against to see if another operation should be excluded or permitted.
    /// </summary>
    public class ExclusionRule : MonoBehaviour
    {
        /// <summary>
        /// The operation to apply on the list of identifiers.
        /// </summary>
        public enum OperationType
        {
            /// <summary>
            /// Will ignore any GameObjects that contain the CheckType that is included in the identifiers list.
            /// </summary>
            Ignore,
            /// <summary>
            /// Will only include any GameObjects that contain the CheckType that is included in the identifiers list.
            /// </summary>
            Include
        }

        /// <summary>
        /// The types of element that can be checked against.
        /// </summary>
        [Flags]
        public enum CheckTypes
        {
            /// <summary>
            /// The tag applied to the GameObject.
            /// </summary>
            Tag = 1 << 0,
            /// <summary>
            /// A script component added to the GameObject.
            /// </summary>
            Script = 1 << 1,
            /// <summary>
            /// A layer applied to the GameObject.
            /// </summary>
            Layer = 1 << 2
        }

        [Tooltip("The operation to apply on the list of identifiers.")]
        public OperationType operation = OperationType.Ignore;
        [UnityFlag]
        [Tooltip("The element type on the GameObject to check against.")]
        public CheckTypes checkType = CheckTypes.Tag;
        [Tooltip("A list of identifiers to check against the given check type.")]
        public List<string> identifiers = new List<string>();

        /// <summary>
        /// The ShouldExclude method is used to determine if a GameObject should be considered excluded due to the set rules.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <param name="rule">The ExclusionRule to use for checking.</param>
        /// <returns>Returns `true` if the given GameObject should be excluded based on the set rules.</returns>
        public static bool ShouldExclude(GameObject obj, ExclusionRule rule)
        {
            if (rule != null)
            {
                return rule.ShouldExclude(obj);
            }
            return false;
        }

        /// <summary>
        /// The ShouldExclude method is used to determine if a GameObject should be considered excluded due to the set rules.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <returns>Returns `true` if the given GameObject should be excluded based on the set rules.</returns>
        public virtual bool ShouldExclude(GameObject obj)
        {
            if (operation == OperationType.Ignore)
            {
                return TypeCheck(obj, true);
            }
            else
            {
                return TypeCheck(obj, false);
            }
        }

        /// <summary>
        /// The ScriptCheck method determines if the given GameObject has a script named after one of the identifiers.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <param name="returnState">The current state of the check.</param>
        /// <returns>Returns `true` if the GameObject does have a script named after one of the identifiers.</returns>
        protected virtual bool ScriptCheck(GameObject obj, bool returnState)
        {
            for (int i = 0; i < identifiers.Count; i++)
            {
                if (obj.GetComponent(identifiers[i]))
                {
                    return returnState;
                }
            }
            return !returnState;
        }

        /// <summary>
        /// The TagCheck method determines if the given GameObject has a tag named after one of the identifiers.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <param name="returnState">The current state of the check.</param>
        /// <returns>Returns `true` if the GameObject does have a tag named after one of the identifiers.</returns>
        protected virtual bool TagCheck(GameObject obj, bool returnState)
        {
            if (returnState)
            {
                return identifiers.Contains(obj.tag);
            }
            else
            {
                return !identifiers.Contains(obj.tag);
            }
        }

        /// <summary>
        /// The LayerCheck method determines if the given GameObject is on a layer named after one of the identifiers.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <param name="returnState">The current state of the check.</param>
        /// <returns>Returns `true` if the GameObject is on a layer named after one of the identifiers.</returns>
        protected virtual bool LayerCheck(GameObject obj, bool returnState)
        {
            if (returnState)
            {
                return identifiers.Contains(LayerMask.LayerToName(obj.layer));
            }
            else
            {
                return !identifiers.Contains(LayerMask.LayerToName(obj.layer));
            }
        }

        /// <summary>
        /// The TypeCheck method determines the mechanism for checking the GameObject matches the appropriate identifiers.
        /// </summary>
        /// <param name="obj">The GameObject to check.</param>
        /// <param name="returnState">The current state of the check.</param>
        /// <returns>Returns `true` if the GameObject matches an appropriate identifier.</returns>
        protected virtual bool TypeCheck(GameObject obj, bool returnState)
        {
            int selection = 0;

            if (checkType.HasFlag(CheckTypes.Tag))
            {
                selection += 1;
            }
            if (checkType.HasFlag(CheckTypes.Script))
            {
                selection += 2;
            }
            if (checkType.HasFlag(CheckTypes.Layer))
            {
                selection += 4;
            }

            switch (selection)
            {
                case 1:
                    return TagCheck(obj, returnState);
                case 2:
                    return ScriptCheck(obj, returnState);
                case 3:
                    if ((returnState && TagCheck(obj, returnState)) || (!returnState && !TagCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    if ((returnState && ScriptCheck(obj, returnState)) || (!returnState && !ScriptCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    break;
                case 4:
                    return LayerCheck(obj, returnState);
                case 5:
                    if ((returnState && TagCheck(obj, returnState)) || (!returnState && !TagCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    if ((returnState && LayerCheck(obj, returnState)) || (!returnState && !LayerCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    break;
                case 6:
                    if ((returnState && ScriptCheck(obj, returnState)) || (!returnState && !ScriptCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    if ((returnState && LayerCheck(obj, returnState)) || (!returnState && !LayerCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    break;
                case 7:
                    if ((returnState && TagCheck(obj, returnState)) || (!returnState && !TagCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    if ((returnState && ScriptCheck(obj, returnState)) || (!returnState && !ScriptCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    if ((returnState && LayerCheck(obj, returnState)) || (!returnState && !LayerCheck(obj, returnState)))
                    {
                        return returnState;
                    }
                    break;
            }

            return !returnState;
        }
    }
}