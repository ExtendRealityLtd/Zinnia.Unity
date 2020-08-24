namespace Zinnia.Data.Type
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using Object = UnityEngine.Object;

    /// <summary>
    ///     Displays a custom reorderable inspector collection in a collapsible drawer.
    /// </summary>
    [CustomPropertyDrawer(typeof(UnityEventBase), true), CustomPropertyDrawer(typeof(UnityEvent), true), CustomPropertyDrawer(typeof(UnityEvent<>), true),
     CustomPropertyDrawer(typeof(UnityEvent<BaseEventData>), true)]
    public class ReorderableUnityEventDrawer : PropertyDrawer
    {
        #region Data Types

        /// <summary>
        ///     Container for data related to a listener function.
        /// </summary>
        protected class FunctionData
        {
            public FunctionData(SerializedProperty listener, Object target = null, MethodInfo method = null, PersistentListenerMode mode = PersistentListenerMode.EventDefined)
            {
                listenerElement = listener;
                targetObject = target;
                targetMethod = method;
                listenerMode = mode;
            }

            public SerializedProperty listenerElement;
            public Object targetObject;
            public MethodInfo targetMethod;
            public PersistentListenerMode listenerMode;
        }

        /// <summary>
        ///     Container for storing current state of <see cref="ReorderableUnityEventDrawer"/>.
        /// </summary>
        protected class DrawerState
        {
            public ReorderableList reorderableList;
            public int lastSelectedIndex;

            // Invoke field tracking
            public string currentInvokeStrArg = "";
            public int currentInvokeIntArg;
            public float currentInvokeFloatArg;
            public bool currentInvokeBoolArg;
            public Object currentInvokeObjectArg;
        }

        /// <summary>
        ///     TODO: docs
        /// </summary>
        private class ComponentTypeCount
        {
            public int TotalCount;
            public int CurrentCount = 1;
        }

        #endregion

        #region Static Fields

        private static ReorderableUnityEventHandler.ReorderableUnityEventSettings cachedSettings;

        #endregion

        #region Static Methods

        /// <summary>
        ///     Replaces the default inspector drawer with this custom drawer.
        /// </summary>
        [InitializeOnLoadMethod]
        public static void ReplaceDefaultDrawer()
        {
            var utilityType = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");
            MethodInfo buildMethod = utilityType.GetMethod(
                "BuildDrawerTypeForTypeDictionary",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                null,
                Type.EmptyTypes,
                null);
            FieldInfo dictionaryField = utilityType.GetField(
                "s_DrawerTypeForType",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo drawerField = utilityType
                .GetNestedType("DrawerKeySet", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetField("drawer", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Ensure the key set is populated.
            buildMethod.Invoke(null, null);

            var dictionary = (IDictionary) dictionaryField.GetValue(null);
            Type[] types = dictionary.Keys.OfType<Type>()
                .Where(
                    type => typeof(UnityEventBase).IsAssignableFrom(type)
                            && typeof(UnityEventDrawer) == (Type) drawerField.GetValue(dictionary[type]))
                .ToArray();

            foreach (Type type in types)
            {
                object keySet = dictionary[type];
                drawerField.SetValue(keySet, typeof(ReorderableUnityEventDrawer));

                // DrawerKeySet is a struct, so set it again after modifying it.
                dictionary[type] = keySet;
            }
        }

        #endregion

        #region Static Methods (UnityEvent Logic)

        /// <summary>
        ///    Invokes provided method (<see cref="MethodInfo"/>). 
        /// </summary>
        /// <remarks>
        ///    This invoke call respects "Runtime Only"/"Editor and Runtime"/"Off" setting on the <see cref="UnityEvent"/> listener which is being invoked.
        /// </remarks>
        /// <param name="method"><see cref="MethodInfo"/> of the method to invoke.</param>
        /// <param name="targets">Objects to invoke method on (<see cref="UnityEvent"/> listeners).</param>
        /// <param name="argValue">Value of the argument provided to invoke call.</param>
        private static void InvokeOnTargetEvents(MethodInfo method, IEnumerable<object> targets, object argValue)
        {
            foreach (object target in targets)
            {
                if (argValue != null)
                    method.Invoke(target, new[] {argValue});
                else
                    method.Invoke(target, new object[] { });
            }
        }

        /// <summary>
        ///     Where the event data actually gets added when you choose a function.
        ///     TODO: adequate docs
        /// </summary>
        /// <param name="functionUserData">TODO: docs</param>
        protected static void SetEventFunctionCallback(object functionUserData)
        {
            var functionData = functionUserData as FunctionData;

            SerializedProperty serializedElement = functionData.listenerElement;

            SerializedProperty serializedTarget = serializedElement.FindPropertyRelative("m_Target");
            SerializedProperty serializedMethodName = serializedElement.FindPropertyRelative("m_MethodName");
            SerializedProperty serializedArgs = serializedElement.FindPropertyRelative("m_Arguments");
            SerializedProperty serializedMode = serializedElement.FindPropertyRelative("m_Mode");

            SerializedProperty serializedArgAssembly = serializedArgs.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
            SerializedProperty serializedArgObjectValue = serializedArgs.FindPropertyRelative("m_ObjectArgument");

            serializedTarget.objectReferenceValue = functionData.targetObject;
            serializedMethodName.stringValue = functionData.targetMethod.Name;
            serializedMode.enumValueIndex = (int) functionData.listenerMode;

            if (functionData.listenerMode == PersistentListenerMode.Object)
            {
                ParameterInfo[] methodParams = functionData.targetMethod.GetParameters();
                if (methodParams.Length == 1 && typeof(Object).IsAssignableFrom(methodParams[0].ParameterType))
                    serializedArgAssembly.stringValue = methodParams[0].ParameterType.AssemblyQualifiedName;
                else
                    serializedArgAssembly.stringValue = typeof(Object).AssemblyQualifiedName;
            }
            else
            {
                serializedArgAssembly.stringValue = typeof(Object).AssemblyQualifiedName;
                serializedArgObjectValue.objectReferenceValue = null;
            }

            Type argType = ReorderableUnityEventHandler.FindTypeInAllAssemblies(serializedArgAssembly.stringValue);
            if (!typeof(Object).IsAssignableFrom(argType) || !argType.IsInstanceOfType(serializedArgObjectValue.objectReferenceValue))
                serializedArgObjectValue.objectReferenceValue = null;

            functionData.listenerElement.serializedObject.ApplyModifiedProperties();
        }

        protected static void ClearEventFunctionCallback(object functionUserData)
        {
            var functionData = functionUserData as FunctionData;

            functionData.listenerElement.FindPropertyRelative("m_Mode").enumValueIndex = (int) PersistentListenerMode.Void;
            functionData.listenerElement.FindPropertyRelative("m_MethodName").stringValue = null;
            functionData.listenerElement.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///     Resets the state of the given <see cref="UnityEvent" /> listener.
        /// </summary>
        /// <param name="serialiedListener">Listener to reset the state of.</param>
        protected virtual void ResetEventState(SerializedProperty serialiedListener)
        {
            SerializedProperty serializedCallState = serialiedListener.FindPropertyRelative("m_CallState");
            SerializedProperty serializedTarget = serialiedListener.FindPropertyRelative("m_Target");
            SerializedProperty serializedMethodName = serialiedListener.FindPropertyRelative("m_MethodName");
            SerializedProperty serializedMode = serialiedListener.FindPropertyRelative("m_Mode");
            SerializedProperty serializedArgs = serialiedListener.FindPropertyRelative("m_Arguments");

            serializedCallState.enumValueIndex = (int) UnityEventCallState.RuntimeOnly;
            serializedTarget.objectReferenceValue = null;
            serializedMethodName.stringValue = null;
            serializedMode.enumValueIndex = (int) PersistentListenerMode.Void;

            serializedArgs.FindPropertyRelative("m_IntArgument").intValue = 0;
            serializedArgs.FindPropertyRelative("m_FloatArgument").floatValue = 0f;
            serializedArgs.FindPropertyRelative("m_BoolArgument").boolValue = false;
            serializedArgs.FindPropertyRelative("m_StringArgument").stringValue = null;
            serializedArgs.FindPropertyRelative("m_ObjectArgument").objectReferenceValue = null;
            serializedArgs.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue = null;
        }

        #endregion

        #region Static Methods (Utility)

        /// <summary>
        ///     Returns <see cref="Rect" />s used for drawing a single listener of <see cref="UnityEvent" />.
        /// </summary>
        /// <param name="rect">Initial rect. TODO: docs</param>
        /// <returns>
        ///     An array of 4 <see cref="Rect" />s in the following order: enabled field, GameObject field, function field and
        ///     argument field.
        /// </returns>
        protected static Rect[] GetEventListenerRects(Rect rect)
        {
            var rects = new Rect[4];

            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2;

            // enabled field
            rects[0] = rect;
            rects[0].width *= 0.3f;

            // game object field
            rects[1] = rects[0];
            rects[1].x += 1;
            rects[1].width -= 2;
            rects[1].y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // function field
            rects[2] = rect;
            rects[2].xMin = rects[1].xMax + 5;

            // argument field
            rects[3] = rects[2];
            rects[3].y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return rects;
        }

#if UNITY_2018_4_OR_NEWER
        /// <summary>
        /// TODO: docs
        /// </summary>
        /// <param name="property">TODO: docs</param>
        /// <returns>TODO: docs</returns>
        private static UnityEventBase GetDummyEvent(SerializedProperty property)
        {
            Object targetObject = property.serializedObject.targetObject;
            if (targetObject == null)
                return new UnityEvent();

            UnityEventBase dummyEvent = null;
            Type targetType = targetObject.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            do
            {
                dummyEvent = GetDummyEventStep(property.propertyPath, targetType, bindingFlags);
                bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                targetType = targetType.BaseType;
            } while (dummyEvent == null && targetType != null);

            return dummyEvent ?? new UnityEvent();
        }

        /// <summary>
        /// TODO: docs
        /// </summary>
        /// <param name="propertyPath">TODO: docs</param>
        /// <param name="propertyType">TODO: docs</param>
        /// <param name="bindingFlags">TODO: docs</param>
        /// <returns>TODO: docs</returns>
        private static UnityEventBase GetDummyEventStep(string propertyPath, Type propertyType, BindingFlags bindingFlags)
        {
            UnityEventBase dummyEvent = null;

            while (propertyPath.Length > 0)
            {
                if (propertyPath.StartsWith("."))
                    propertyPath = propertyPath.Substring(1);

                string[] splitPath = propertyPath.Split(new[] {'.'}, 2);

                FieldInfo newField = propertyType.GetField(splitPath[0], bindingFlags);

                if (newField == null)
                    break;

                propertyType = newField.FieldType;
                if (propertyType.IsArray)
                    propertyType = propertyType.GetElementType();
                else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>)) propertyType = propertyType.GetGenericArguments()[0];

                if (splitPath.Length == 1)
                    break;

                propertyPath = splitPath[1];
                if (propertyPath.StartsWith("Array.data["))
                    propertyPath = propertyPath.Split(new[] {']'}, 2)[1];
            }

            if (propertyType.IsSubclassOf(typeof(UnityEventBase)))
                dummyEvent = Activator.CreateInstance(propertyType) as UnityEventBase;

            return dummyEvent;
        }
#endif

        /// <summary>
        ///     Gets object type corresponding to the given <see cref="PersistentListenerMode" />.
        /// </summary>
        /// <param name="listenerMode">Listener mode to get the type for.</param>
        /// <returns><see cref="Type" /> corresponding to the provided <see cref="PersistentListenerMode" />.</returns>
        protected static Type[] GetTypeForListenerMode(PersistentListenerMode listenerMode)
        {
            switch (listenerMode)
            {
                case PersistentListenerMode.EventDefined:
                case PersistentListenerMode.Void:
                    return new Type[] { };
                case PersistentListenerMode.Object:
                    return new[] {typeof(Object)};
                case PersistentListenerMode.Int:
                    return new[] {typeof(int)};
                case PersistentListenerMode.Float:
                    return new[] {typeof(float)};
                case PersistentListenerMode.String:
                    return new[] {typeof(string)};
                case PersistentListenerMode.Bool:
                    return new[] {typeof(bool)};
            }

            return new Type[] { };
        }

        /// <summary>
        ///     Finds all methods on a given Object which TODO: docs
        /// </summary>
        /// <remarks>
        ///     Values provided by this method are used to populate a dropdown list of functions displayed when selecting a
        ///     function in UnityEvent listener Inspector.
        /// </remarks>
        /// <param name="targetObject">Object to get methods from.</param>
        /// <param name="listenerMode">Mode of UnityEvent listener.</param>
        /// <param name="methodInfos">This list will be populated with <see cref="FunctionData"/> for each valid method found.</param>
        /// <param name="customArgTypes">TODO: docs</param>
        protected static void FindValidMethods(Object targetObject, PersistentListenerMode listenerMode, List<FunctionData> methodInfos, Type[] customArgTypes = null)
        {
            Type objectType = targetObject.GetType();

            Type[] argTypes;

            if (listenerMode == PersistentListenerMode.EventDefined && customArgTypes != null)
                argTypes = customArgTypes;
            else
                argTypes = GetTypeForListenerMode(listenerMode);

            var foundMethods = new List<MethodInfo>();

            // For some reason BindingFlags.FlattenHierarchy does not seem to work, so we manually traverse the base types instead
            while (objectType != null)
            {
                MethodInfo[] foundMethodsOnType =
                    objectType.GetMethods(BindingFlags.Public | (cachedSettings.privateMembersShown ? BindingFlags.NonPublic : BindingFlags.Default) | BindingFlags.Instance);

                foundMethods.AddRange(foundMethodsOnType);

                objectType = objectType.BaseType;
            }

            foreach (MethodInfo methodInfo in foundMethods)
            {
                // Sadly we can only use functions with void return type since C# throws an error
                if (methodInfo.ReturnType != typeof(void))
                    continue;

                ParameterInfo[] methodParams = methodInfo.GetParameters();
                if (methodParams.Length != argTypes.Length)
                    continue;

                bool isValidParamMatch = true;
                for (int i = 0; i < methodParams.Length; i++)
                {
                    if (!methodParams[i].ParameterType.IsAssignableFrom(argTypes[i]) /* && (argTypes[i] != typeof(int) || !methodParams[i].ParameterType.IsEnum)*/) isValidParamMatch = false;
                    if (listenerMode == PersistentListenerMode.Object && argTypes[i].IsAssignableFrom(methodParams[i].ParameterType)) isValidParamMatch = true;
                }

                if (!isValidParamMatch)
                    continue;

                if (!cachedSettings.privateMembersShown && methodInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
                    continue;


                var foundMethodData = new FunctionData(null, targetObject, methodInfo, listenerMode);

                methodInfos.Add(foundMethodData);
            }
        }

        /// <summary>
        ///     Returns a string name of the given <see cref="Type" />.
        /// </summary>
        /// <remarks>
        ///     Used instead of direct Type.Name call because it returns prettier values for built-in types (i.e. int instead of
        ///     System.Int32).
        /// </remarks>
        /// <param name="typeToName"><see cref="Type" /> to get the name for.</param>
        /// <returns>String name of the given <see cref="Type" />.</returns>
        protected static string GetTypeName(Type typeToName)
        {
            if (typeToName == typeof(float))
                return "float";
            if (typeToName == typeof(bool))
                return "bool";
            if (typeToName == typeof(int))
                return "int";
            if (typeToName == typeof(string))
                return "string";

            return typeToName.Name;
        }

        #endregion

        #region Fields

        /// <summary>
        ///     The height of the header.
        /// </summary>
        protected const float headerHeight = 20f;

        /// <summary>
        ///     The padding for the drawer.
        /// </summary>
        protected const float padding = 6f;

        /// <summary>
        ///     The offset of the drawer height.
        /// </summary>
        protected const float heightOffset = 2f;

        /// <summary>
        ///     The header background style.
        /// </summary>
        protected readonly GUIStyle headerBackground = new GUIStyle("RL Header");

        // /// <summary>
        // /// Whether the `base.OnGUI` was called.
        // /// </summary>
        // protected bool wasBaseOnGuiCalled = false;

        /// <summary>
        ///     TODO: docs
        /// </summary>
        protected readonly Dictionary<string, DrawerState> drawerStates = new Dictionary<string, DrawerState>();

        /// <summary>
        ///     TODO: docs
        /// </summary>
        protected DrawerState currentState;

        /// <summary>
        ///     TODO: docs
        /// </summary>
        protected string currentLabelText;

        /// <summary>
        ///     TODO: docs
        /// </summary>
        protected SerializedProperty currentProperty;

        /// <summary>
        ///     Array of listeners of this <see cref="UnityEvent"/>.
        /// </summary>
        protected SerializedProperty listenerArray;

        /// <summary>
        ///     TODO: docs
        /// </summary>
        protected UnityEventBase dummyEvent;

        /// <summary>
        ///     TODO: docs
        /// </summary>
        protected MethodInfo cachedFindMethodInfo;

        // /// <summary>
        // ///     Draws the drawer header.
        // /// </summary>
        // /// <param name="position">The position to draw at.</param>
        // // TODO: move into ReorderableList header
        // protected virtual void DrawHeader(Rect position)
        // {
        //     if (Event.current.type == EventType.Repaint) headerBackground.Draw(position, false, false, false, false);
        //
        //     var headerRect = new Rect(position.x, position.y, position.width, headerHeight);
        //     headerRect.xMin += padding;
        //     headerRect.xMax -= padding;
        //     headerRect.height -= heightOffset;
        //     headerRect.y += heightOffset * 0.5f;
        //     DrawEventHeader(headerRect);
        // }

        #endregion

        #region Methods (PropertyDrawer Overrides)

        // /// <inheritdoc/>
        // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        // {
        //     if (!wasBaseOnGuiCalled)
        //     {
        //         base.OnGUI(position, property, label);
        //         wasBaseOnGuiCalled = true;
        //         return;
        //     }
        //
        //     Rect foldoutPosition = new Rect(position.x, position.y + heightOffset, position.width, headerHeight);
        //     property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, GUIContent.none, true);
        //
        //     if (property.isExpanded)
        //     {
        //         base.OnGUI(position, property, label);
        //     }
        //     else
        //     {
        //         DrawHeader(position);
        //     }
        // }

        // /// <inheritdoc/>
        // public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
        //     property.isExpanded
        //         ? base.GetPropertyHeight(property, label)
        //         : EditorGUIUtility.singleLineHeight + heightOffset * 2f;

        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            currentLabelText = label.text;
            PrepareState(property);

            HandleKeyboardShortcuts();

            if (dummyEvent == null)
                return;

            if (currentState.reorderableList != null)
            {
                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                currentState.reorderableList.DoList(position);
                EditorGUI.indentLevel = oldIndent;
            }
        }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Don't draw contents if not expanded
            // if (property.isExpanded) return EditorGUIUtility.singleLineHeight + heightOffset * 2f;

            PrepareState(property);

            float height = 0f;
            if (currentState.reorderableList != null)
                height = currentState.reorderableList.GetHeight();

            return height;
        }

        #endregion

        #region Methods (Drawing ReorderableList)

        // This region contains methods which actually draw the collapsible UnityEvent
        // and a reorderable list of all its listeners in the Inspector.

        /// <summary>
        ///     Draws the event invoke button of the UnityEvent field.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="headerStartOffset"></param>
        private void DrawInvokeField(Rect position, float headerStartOffset)
        {
            Rect buttonPos = position;
            buttonPos.height *= 0.9f;
            buttonPos.width = 51;
            buttonPos.x += headerStartOffset + 2;

            Rect textPos = buttonPos;
            textPos.x += 6;
            textPos.width -= 12;

            Rect inputFieldPos = position;
            inputFieldPos.height = buttonPos.height;
            inputFieldPos.width = position.width - buttonPos.width - 3 - headerStartOffset;
            inputFieldPos.x = buttonPos.x + buttonPos.width + 2;
            inputFieldPos.y += 1;

            Rect inputFieldTextPlaceholder = inputFieldPos;

            Type[] eventInvokeArgs = GetEventParams(dummyEvent);

            GUIStyle textStyle = EditorStyles.miniLabel;
            textStyle.alignment = TextAnchor.MiddleLeft;

            MethodInfo invokeMethod = InvokeFindMethod("Invoke", dummyEvent, dummyEvent, PersistentListenerMode.EventDefined);
            FieldInfo serializedField = currentProperty.serializedObject.targetObject.GetType().GetField(currentProperty.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            object[] invokeTargets = currentProperty.serializedObject.targetObjects.Select(target => target == null || serializedField == null ? null : serializedField.GetValue(target))
                .Where(f => f != null).ToArray();

            // TODO: add object state to disable conditions
            // TODO: add warning when trying to Invoke in Edit Mode (tell that only listener with "Editor and Runtime" invocation mode will be invoked)
            EditorGUI.BeginDisabledGroup(invokeTargets.Length == 0 || invokeMethod == null);

            bool executeInvoke = GUI.Button(buttonPos, "", EditorStyles.miniButton);
            GUI.Label(textPos, "Invoke" /* + " (" + string.Join(", ", eventInvokeArgs.Select(e => e.Name).ToArray()) + ")"*/, textStyle);

            if (eventInvokeArgs.Length > 0)
            {
                Type argType = eventInvokeArgs[0];

                if (argType == typeof(string))
                {
                    currentState.currentInvokeStrArg = EditorGUI.TextField(inputFieldPos, currentState.currentInvokeStrArg);

                    // Draw placeholder text
                    if (currentState.currentInvokeStrArg.Length == 0)
                    {
                        GUIStyle placeholderLabelStyle = EditorStyles.centeredGreyMiniLabel;
                        placeholderLabelStyle.alignment = TextAnchor.UpperLeft;

                        GUI.Label(inputFieldTextPlaceholder, "String argument...", placeholderLabelStyle);
                    }

                    if (executeInvoke)
                        InvokeOnTargetEvents(invokeMethod, invokeTargets, currentState.currentInvokeStrArg);
                }
                else if (argType == typeof(int))
                {
                    currentState.currentInvokeIntArg = EditorGUI.IntField(inputFieldPos, currentState.currentInvokeIntArg);

                    if (executeInvoke)
                        InvokeOnTargetEvents(invokeMethod, invokeTargets, currentState.currentInvokeIntArg);
                }
                else if (argType == typeof(float))
                {
                    currentState.currentInvokeFloatArg = EditorGUI.FloatField(inputFieldPos, currentState.currentInvokeFloatArg);

                    if (executeInvoke)
                        InvokeOnTargetEvents(invokeMethod, invokeTargets, currentState.currentInvokeFloatArg);
                }
                else if (argType == typeof(bool))
                {
                    currentState.currentInvokeBoolArg = EditorGUI.Toggle(inputFieldPos, currentState.currentInvokeBoolArg);

                    if (executeInvoke)
                        InvokeOnTargetEvents(invokeMethod, invokeTargets, currentState.currentInvokeBoolArg);
                }
                else if (argType == typeof(Object))
                {
                    currentState.currentInvokeObjectArg = EditorGUI.ObjectField(inputFieldPos, currentState.currentInvokeObjectArg, argType, true);

                    if (executeInvoke)
                        invokeMethod.Invoke(currentProperty.serializedObject.targetObject, new object[] {currentState.currentInvokeObjectArg});
                }
            }
            else if (executeInvoke) // No input arg
            {
                InvokeOnTargetEvents(invokeMethod, invokeTargets, null);
            }

            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        ///     Draws the header element of the UnityEvent field.
        /// </summary>
        /// <param name="headerRect">Element rect.</param>
        private void DrawHeaderCallback(Rect headerRect)
        {
            // We need to know where to position the invoke field based on the length of the title in the UI
            var headerTitle = new GUIContent(string.IsNullOrEmpty(currentLabelText) ? "Event" : currentLabelText + " " + GetEventParamsStr(dummyEvent));
            float headerStartOffset = EditorStyles.label.CalcSize(headerTitle).x;

            GUI.Label(headerRect, headerTitle);

            if (cachedSettings.invokeButtonShown)
                DrawInvokeField(headerRect, headerStartOffset);
        }

        /// <summary>
        ///     Draws a single element of the UnityEvent listeners list.
        /// </summary>
        /// <param name="rect">Element rect.</param>
        /// <param name="index">List index of the element being drawn.</param>
        /// <param name="active">Is this list element currently active?</param>
        /// <param name="focused">Is this list element currently active?</param>
        protected virtual void DrawEventListenerCallback(Rect rect, int index, bool active, bool focused)
        {
            SerializedProperty element = listenerArray.GetArrayElementAtIndex(index);

            rect.y++;
            Rect[] rects = GetEventListenerRects(rect);

            Rect enabledRect = rects[0];
            Rect gameObjectRect = rects[1];
            Rect functionRect = rects[2];
            Rect argRect = rects[3];

            SerializedProperty serializedCallState = element.FindPropertyRelative("m_CallState");
            SerializedProperty serializedMode = element.FindPropertyRelative("m_Mode");
            SerializedProperty serializedArgs = element.FindPropertyRelative("m_Arguments");
            SerializedProperty serializedTarget = element.FindPropertyRelative("m_Target");
            SerializedProperty serializedMethod = element.FindPropertyRelative("m_MethodName");

            Color oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.white;

            EditorGUI.PropertyField(enabledRect, serializedCallState, GUIContent.none);

            EditorGUI.BeginChangeCheck();

            Object oldTargetObject = serializedTarget.objectReferenceValue;

            GUI.Box(gameObjectRect, GUIContent.none);
            EditorGUI.PropertyField(gameObjectRect, serializedTarget, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                Object newTargetObject = serializedTarget.objectReferenceValue;

                // Attempt to maintain the function pointer and component pointer if someone changes the target object and it has the correct component type on it.
                if (oldTargetObject != null && newTargetObject != null)
                {
                    if (oldTargetObject.GetType() != newTargetObject.GetType()) // If not an asset, if it is an asset and the same type we don't do anything
                    {
                        // If these are Unity components then the game object that they are attached to may have multiple copies of the same component type so attempt to match the count
                        if (typeof(Component).IsAssignableFrom(oldTargetObject.GetType()) && newTargetObject.GetType() == typeof(GameObject))
                        {
                            GameObject oldParentObject = ((Component) oldTargetObject).gameObject;
                            var newParentObject = (GameObject) newTargetObject;

                            Component[] oldComponentList = oldParentObject.GetComponents(oldTargetObject.GetType());

                            int componentLocationOffset = 0;
                            for (int i = 0; i < oldComponentList.Length; ++i)
                            {
                                if (oldComponentList[i] == oldTargetObject)
                                    break;

                                if (oldComponentList[i].GetType() == oldTargetObject.GetType()
                                ) // Only take exact matches for component type since I don't want to do redo the reflection to find the methods at the moment.
                                    componentLocationOffset++;
                            }

                            Component[] newComponentList = newParentObject.GetComponents(oldTargetObject.GetType());

                            int newComponentIndex = 0;
                            int componentCount = -1;
                            for (int i = 0; i < newComponentList.Length; ++i)
                            {
                                if (componentCount == componentLocationOffset)
                                    break;

                                if (newComponentList[i].GetType() == oldTargetObject.GetType())
                                {
                                    newComponentIndex = i;
                                    componentCount++;
                                }
                            }

                            if (newComponentList.Length > 0 && newComponentList[newComponentIndex].GetType() == oldTargetObject.GetType())
                                serializedTarget.objectReferenceValue = newComponentList[newComponentIndex];
                            else
                                serializedMethod.stringValue = null;
                        }
                        else { serializedMethod.stringValue = null; }
                    }
                }
                else { serializedMethod.stringValue = null; }
            }

            var mode = (PersistentListenerMode) serializedMode.enumValueIndex;

            SerializedProperty argument;
            if (serializedTarget.objectReferenceValue == null || string.IsNullOrEmpty(serializedMethod.stringValue))
                mode = PersistentListenerMode.Void;

            switch (mode)
            {
                case PersistentListenerMode.Object:
                case PersistentListenerMode.String:
                case PersistentListenerMode.Bool:
                case PersistentListenerMode.Float:
                    argument = serializedArgs.FindPropertyRelative("m_" + Enum.GetName(typeof(PersistentListenerMode), mode) + "Argument");
                    break;
                default:
                    argument = serializedArgs.FindPropertyRelative("m_IntArgument");
                    break;
            }

            string argTypeName = serializedArgs.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue;
            Type argType = typeof(Object);
            if (!string.IsNullOrEmpty(argTypeName))
                argType = ReorderableUnityEventHandler.FindTypeInAllAssemblies(argTypeName) ?? typeof(Object);

            if (mode == PersistentListenerMode.Object)
            {
                EditorGUI.BeginChangeCheck();
                Object result = EditorGUI.ObjectField(argRect, GUIContent.none, argument.objectReferenceValue, argType, true);
                if (EditorGUI.EndChangeCheck())
                    argument.objectReferenceValue = result;
            }
            else if (mode != PersistentListenerMode.Void && mode != PersistentListenerMode.EventDefined) { EditorGUI.PropertyField(argRect, argument, GUIContent.none); }

            EditorGUI.BeginDisabledGroup(serializedTarget.objectReferenceValue == null);
            {
                EditorGUI.BeginProperty(functionRect, GUIContent.none, serializedMethod);

                GUIContent buttonContent;

                if (EditorGUI.showMixedValue) { buttonContent = new GUIContent("\u2014", "Mixed Values"); }
                else
                {
                    if (serializedTarget.objectReferenceValue == null || string.IsNullOrEmpty(serializedMethod.stringValue))
                        buttonContent = new GUIContent("No Function");
                    else
                        buttonContent = new GUIContent(GetFunctionDisplayName(serializedTarget, serializedMethod, mode, argType, cachedSettings.argumentTypeDisplayed));
                }

                if (GUI.Button(functionRect, buttonContent, EditorStyles.popup)) BuildPopupMenu(serializedTarget.objectReferenceValue, element/*, argType*/).DropDown(functionRect);

                EditorGUI.EndProperty();
            }
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        ///     Called when a listener of the UnityEvent becomes selected.
        /// </summary>
        /// <param name="list">Reorderable list which sent the callback.</param>
        protected virtual void SelectEventListenerCallback(ReorderableList list)
        {
            currentState.lastSelectedIndex = list.index;
        }

        /// <summary>
        ///     Called when a new listener gets added to UnityEvent.
        /// </summary>
        /// <param name="list">Reorderable list which sent the callback.</param>
        protected virtual void AddEventListenerCallback(ReorderableList list)
        {
            if (listenerArray.hasMultipleDifferentValues)
            {
                foreach (Object targetObj in listenerArray.serializedObject.targetObjects)
                {
                    var tempSerializedObject = new SerializedObject(targetObj);
                    SerializedProperty listenerArrayProperty = tempSerializedObject.FindProperty(listenerArray.propertyPath);
                    listenerArrayProperty.arraySize += 1;
                    tempSerializedObject.ApplyModifiedProperties();
                }

                listenerArray.serializedObject.SetIsDifferentCacheDirty();
                listenerArray.serializedObject.Update();
                list.index = list.serializedProperty.arraySize - 1;
            }
            else { ReorderableList.defaultBehaviours.DoAddButton(list); }

            currentState.lastSelectedIndex = list.index;

            // Init default state
            SerializedProperty serialiedListener = listenerArray.GetArrayElementAtIndex(list.index);
            ResetEventState(serialiedListener);
        }

        /// <summary>
        ///     Called when listeners of UnityEvent get reordered.
        /// </summary>
        /// <param name="list">Reorderable list which sent the callback.</param>
        protected virtual void ReorderCallback(ReorderableList list)
        {
            currentState.lastSelectedIndex = list.index;
        }

        /// <summary>
        ///     Called when a listener gets removed from UnityEvent.
        /// </summary>
        /// <param name="list">Reorderable list which sent the callback.</param>
        protected virtual void RemoveCallback(ReorderableList list)
        {
            if (currentState.reorderableList.count > 0)
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
                currentState.lastSelectedIndex = list.index;
            }
        }

        #endregion

        #region Methods (Keyboard Shortcuts)

        // This region contains methods which handle keyboard shortcuts (like Ctrl+C, Ctrl+V)
        // for copying, cutting and pasting event listeners between UnityEvents.

        /// <summary>
        ///     Data container for storing copied event in clipboard.
        /// </summary>
        private static class EventClipboardStorage
        {
            public static SerializedObject copiedEventProperty;
            public static int copiedEventIndex;
        }

        /// <summary>
        ///     Handles all keyboard shortcuts.
        /// </summary>
        private void HandleKeyboardShortcuts()
        {
            if (!cachedSettings.hotkeysEnabled)
                return;

            Event currentEvent = Event.current;

            if (!currentState.reorderableList.HasKeyboardControl())
                return;

            if (currentEvent.type == EventType.ValidateCommand)
            {
                if (currentEvent.commandName == "Copy" ||
                    currentEvent.commandName == "Paste" ||
                    currentEvent.commandName == "Cut" ||
                    currentEvent.commandName == "Duplicate" ||
                    currentEvent.commandName == "Delete" ||
                    currentEvent.commandName == "SoftDelete" /*|| // NOTE: no more using Ctrl+A to add new, as it's an obscure shortcut (Ctrl+A is usually used to select all).
                currentEvent.commandName == "SelectAll"*/)
                    currentEvent.Use();
            }
            else if (currentEvent.type == EventType.ExecuteCommand)
            {
                if (currentEvent.commandName == "Copy")
                {
                    HandleCopy();
                    currentEvent.Use();
                }
                else if (currentEvent.commandName == "Paste")
                {
                    HandlePaste();
                    currentEvent.Use();
                }
                else if (currentEvent.commandName == "Cut")
                {
                    HandleCut();
                    currentEvent.Use();
                }
                else if (currentEvent.commandName == "Duplicate")
                {
                    HandleDuplicate();
                    currentEvent.Use();
                }
                else if (currentEvent.commandName == "Delete" || currentEvent.commandName == "SoftDelete")
                {
                    RemoveCallback(currentState.reorderableList);
                    currentEvent.Use();
                }

                // NOTE: no more using Ctrl+A to add new, as it's an obscure shortcut (Ctrl+A is usually used to select all).
                // else if (currentEvent.commandName == "SelectAll") // Use Ctrl+A for add, since Ctrl+N isn't usable using command names
                // {
                //     HandleAdd();
                //     currentEvent.Use();
                // }
            }
        }

        /// <summary>
        ///     Handles copying currently selected listener to clipboard.
        /// </summary>
        private void HandleCopy()
        {
            var serializedEvent = new SerializedObject(listenerArray.GetArrayElementAtIndex(currentState.reorderableList.index).serializedObject.targetObject);

            EventClipboardStorage.copiedEventProperty = serializedEvent;
            EventClipboardStorage.copiedEventIndex = currentState.reorderableList.index;
        }

        /// <summary>
        ///     Handles pasting listener from clipboard to the end of this UnityEvent listeners list.
        /// </summary>
        /// <remarks>
        ///     New listener is pasted in the list right after currently selected listener.
        /// </remarks>
        private void HandlePaste()
        {
            if (EventClipboardStorage.copiedEventProperty == null)
                return;

            SerializedProperty iterator = EventClipboardStorage.copiedEventProperty.GetIterator();

            if (iterator == null)
                return;

            while (iterator.NextVisible(true))
            {
                if (iterator != null && iterator.name == "m_PersistentCalls")
                {
                    iterator = iterator.FindPropertyRelative("m_Calls");
                    break;
                }
            }

            if (iterator.arraySize < EventClipboardStorage.copiedEventIndex + 1)
                return;

            SerializedProperty sourceProperty = iterator.GetArrayElementAtIndex(EventClipboardStorage.copiedEventIndex);

            if (sourceProperty == null)
                return;

            int targetArrayIdx = currentState.reorderableList.count > 0 ? currentState.reorderableList.index : 0;
            currentState.reorderableList.serializedProperty.InsertArrayElementAtIndex(targetArrayIdx);

            SerializedProperty targetProperty =
                currentState.reorderableList.serializedProperty.GetArrayElementAtIndex((currentState.reorderableList.count > 0 ? currentState.reorderableList.index : 0) + 1);
            ResetEventState(targetProperty);

            targetProperty.FindPropertyRelative("m_CallState").enumValueIndex = sourceProperty.FindPropertyRelative("m_CallState").enumValueIndex;
            targetProperty.FindPropertyRelative("m_Target").objectReferenceValue = sourceProperty.FindPropertyRelative("m_Target").objectReferenceValue;
            targetProperty.FindPropertyRelative("m_MethodName").stringValue = sourceProperty.FindPropertyRelative("m_MethodName").stringValue;
            targetProperty.FindPropertyRelative("m_Mode").enumValueIndex = sourceProperty.FindPropertyRelative("m_Mode").enumValueIndex;

            SerializedProperty targetArgs = targetProperty.FindPropertyRelative("m_Arguments");
            SerializedProperty sourceArgs = sourceProperty.FindPropertyRelative("m_Arguments");

            targetArgs.FindPropertyRelative("m_IntArgument").intValue = sourceArgs.FindPropertyRelative("m_IntArgument").intValue;
            targetArgs.FindPropertyRelative("m_FloatArgument").floatValue = sourceArgs.FindPropertyRelative("m_FloatArgument").floatValue;
            targetArgs.FindPropertyRelative("m_BoolArgument").boolValue = sourceArgs.FindPropertyRelative("m_BoolArgument").boolValue;
            targetArgs.FindPropertyRelative("m_StringArgument").stringValue = sourceArgs.FindPropertyRelative("m_StringArgument").stringValue;
            targetArgs.FindPropertyRelative("m_ObjectArgument").objectReferenceValue = sourceArgs.FindPropertyRelative("m_ObjectArgument").objectReferenceValue;
            targetArgs.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue = sourceArgs.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName").stringValue;

            currentState.reorderableList.index++;
            currentState.lastSelectedIndex++;

            targetProperty.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///     Handles cutting currently selected listener to clipboard.
        /// </summary>
        private void HandleCut()
        {
            HandleCopy();
            RemoveCallback(currentState.reorderableList);
        }


        /// <summary>
        ///     Handles duplicating currently selected listener in this UnityEvent listeners list.
        /// </summary>
        /// <remarks>
        ///     Duplicated listener is pasted in the list right after the original listener.
        /// </remarks>
        private void HandleDuplicate()
        {
            if (currentState.reorderableList.count == 0)
                return;

            SerializedProperty listProperty = currentState.reorderableList.serializedProperty;

            SerializedProperty eventProperty = listProperty.GetArrayElementAtIndex(currentState.reorderableList.index);

            eventProperty.DuplicateCommand();

            currentState.reorderableList.index++;
            currentState.lastSelectedIndex++;
        }

        #endregion

        #region Methods (Utility)

        /// <summary>
        ///     TODO: docs
        /// </summary>
        /// <param name="propertyForState"></param>
        private void PrepareState(SerializedProperty propertyForState)
        {
            DrawerState state;

            if (!drawerStates.TryGetValue(propertyForState.propertyPath, out state))
            {
                state = new DrawerState();

                SerializedProperty persistentListeners = propertyForState.FindPropertyRelative("m_PersistentCalls.m_Calls");

                // The fun thing is that if Unity just made the first bool arg true internally, this whole thing would be unnecessary.
                state.reorderableList = new ReorderableList(propertyForState.serializedObject, persistentListeners, true, true, true, true);
                state.reorderableList.elementHeight = 43; // todo: actually find proper constant for this. 
                state.reorderableList.drawHeaderCallback += DrawHeaderCallback;
                state.reorderableList.drawElementCallback += DrawEventListenerCallback;
                state.reorderableList.onSelectCallback += SelectEventListenerCallback;
                state.reorderableList.onRemoveCallback += ReorderCallback;
                state.reorderableList.onAddCallback += AddEventListenerCallback;
                state.reorderableList.onRemoveCallback += RemoveCallback;

                state.lastSelectedIndex = 0;

                drawerStates.Add(propertyForState.propertyPath, state);
            }

            currentProperty = propertyForState;

            currentState = state;
            currentState.reorderableList.index = currentState.lastSelectedIndex;
            listenerArray = state.reorderableList.serializedProperty;

            // Setup dummy event
#if UNITY_2018_4_OR_NEWER
            dummyEvent = GetDummyEvent(propertyForState);
#else
        string eventTypeName = currentProperty.FindPropertyRelative("m_TypeName").stringValue;
        System.Type eventType = ReorderableUnityEventHandler.FindTypeInAllAssemblies(eventTypeName);
        if (eventType == null)
            dummyEvent = new UnityEvent();
        else
            dummyEvent = System.Activator.CreateInstance(eventType) as UnityEventBase;
#endif

            cachedSettings = ReorderableUnityEventHandler.GetEditorSettings();
        }

        /// <summary>
        /// TODO: docs
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="targetObject"></param>
        /// <param name="eventObject"></param>
        /// <param name="listenerMode"></param>
        /// <param name="argType"></param>
        /// <returns></returns>
        private MethodInfo InvokeFindMethod(string functionName, object targetObject, UnityEventBase eventObject, PersistentListenerMode listenerMode, Type argType = null)
        {
            MethodInfo findMethod = cachedFindMethodInfo;

            if (findMethod == null)
            {
                // Rather not reinvent the wheel considering this function calls different functions depending on the number of args the event has...
                findMethod = eventObject.GetType().GetMethod("FindMethod", BindingFlags.NonPublic | BindingFlags.Instance, null,
                    new[]
                    {
                        typeof(string),
                        typeof(object),
                        typeof(PersistentListenerMode),
                        typeof(Type)
                    },
                    null);

                cachedFindMethodInfo = findMethod;
            }

            if (findMethod == null)
            {
                Debug.LogError("Could not find FindMethod function!");
                return null;
            }

            return findMethod.Invoke(eventObject, new[] {functionName, targetObject, listenerMode, argType}) as MethodInfo;
        }

        private Type[] GetEventParams(UnityEventBase eventIn)
        {
            MethodInfo methodInfo = InvokeFindMethod("Invoke", eventIn, eventIn, PersistentListenerMode.EventDefined);
            return methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();
        }

        protected string GetEventParamsStr(UnityEventBase eventIn)
        {
            var builder = new StringBuilder();
            Type[] methodTypes = GetEventParams(eventIn);

            builder.Append("(");
            builder.Append(string.Join(", ", methodTypes.Select(val => val.Name).ToArray()));
            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        ///     Returns pre-formatted a string of the arguments of the given function.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="targetObject"></param>
        /// <param name="listenerMode"></param>
        /// <param name="argType"></param>
        /// <returns></returns>
        protected string GetFunctionArgStr(string functionName, object targetObject, PersistentListenerMode listenerMode, Type argType = null)
        {
            MethodInfo methodInfo = InvokeFindMethod(functionName, targetObject, dummyEvent, listenerMode, argType);

            if (methodInfo == null)
                return "";

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (parameterInfos.Length == 0)
                return "";

            return GetTypeName(parameterInfos[0].ParameterType);
        }

        /// <summary>
        /// Returns a display name for the given listener callback method.
        /// </summary>
        /// <param name="objectProperty">Object which contains the method.</param>
        /// <param name="methodProperty">Method to get the name of.</param>
        /// <param name="listenerMode"><see cref="PersistentListenerMode"/> of the listener.</param>
        /// <param name="argType">Method argument type.</param>
        /// <param name="showArg">If true, method arguments will be added to </param>
        /// <returns></returns>
        protected string GetFunctionDisplayName(SerializedProperty objectProperty, SerializedProperty methodProperty, PersistentListenerMode listenerMode, Type argType, bool showArg)
        {
            string methodNameOut = "No Function";

            if (objectProperty.objectReferenceValue == null || methodProperty.stringValue == "")
                return methodNameOut;

            MethodInfo methodInfo = InvokeFindMethod(methodProperty.stringValue, objectProperty.objectReferenceValue, dummyEvent, listenerMode, argType);
            string funcName = methodProperty.stringValue.StartsWith("set_") ? methodProperty.stringValue.Substring(4) : methodProperty.stringValue;

            if (methodInfo == null)
            {
                methodNameOut = $"<Missing {objectProperty.objectReferenceValue.GetType().Name}.{funcName}>";
                return methodNameOut;
            }

            string objectTypeName = objectProperty.objectReferenceValue.GetType().Name;
            var objectComponent = objectProperty.objectReferenceValue as Component;

            if (!cachedSettings.sameComponentTypesGrouped && objectComponent != null)
            {
                Type objectType = objectProperty.objectReferenceValue.GetType();

                Component[] components = objectComponent.GetComponents(objectType);

                if (components.Length > 1)
                {
                    int componentID = 0;
                    for (int i = 0; i < components.Length; i++)
                    {
                        if (components[i] == objectComponent)
                        {
                            componentID = i + 1;
                            break;
                        }
                    }

                    objectTypeName += $"({componentID})";
                }
            }

            if (showArg)
            {
                string functionArgStr = GetFunctionArgStr(methodProperty.stringValue, objectProperty.objectReferenceValue, listenerMode, argType);
                methodNameOut = $"{objectTypeName}.{funcName} ({functionArgStr})";
            }
            else { methodNameOut = $"{objectTypeName}.{funcName}"; }


            return methodNameOut;
        }

        protected void AddFunctionToMenu(string contentPath, SerializedProperty elementProperty, FunctionData methodData, GenericMenu menu, int componentCount, bool dynamicCall = false)
        {
            string functionName = methodData.targetMethod.Name.StartsWith("set_") ? methodData.targetMethod.Name.Substring(4) : methodData.targetMethod.Name;
            string argStr = string.Join(", ", methodData.targetMethod.GetParameters().Select(param => GetTypeName(param.ParameterType)).ToArray());

            if (dynamicCall) // Cut out the args from the dynamic variation to match Unity, and the menu item won't be created if it's not unique.
            {
                contentPath += functionName;
            }
            else
            {
                if (methodData.targetMethod.Name.StartsWith("set_")) // If it's a property add the arg before the name
                    contentPath += argStr + " " + functionName;
                else
                    contentPath += functionName + " (" + argStr + ")"; // Add arguments
            }

            if (!methodData.targetMethod.IsPublic)
                contentPath += " " + (methodData.targetMethod.IsPrivate ? "<private>" : "<internal>");

            if (methodData.targetMethod.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
                contentPath += " <obsolete>";

            methodData.listenerElement = elementProperty;

            SerializedProperty serializedTargetObject = elementProperty.FindPropertyRelative("m_Target");
            SerializedProperty serializedMethodName = elementProperty.FindPropertyRelative("m_MethodName");
            SerializedProperty serializedMode = elementProperty.FindPropertyRelative("m_Mode");

            bool itemOn = serializedTargetObject.objectReferenceValue == methodData.targetObject &&
                          serializedMethodName.stringValue == methodData.targetMethod.Name &&
                          serializedMode.enumValueIndex == (int) methodData.listenerMode;

            menu.AddItem(new GUIContent(contentPath), itemOn, SetEventFunctionCallback, methodData);
        }

        /// <summary>
        /// Builds a popup menu for selecting TODO: docs
        /// </summary>
        /// <param name="targetObj">TODO: docs</param>
        /// <param name="elementProperty">TODO: docs</param>
        /// <returns>TODO: docs</returns>
        protected GenericMenu BuildPopupMenu(Object targetObj, SerializedProperty elementProperty)
        {
            var menu = new GenericMenu();

            string currentMethodName = elementProperty.FindPropertyRelative("m_MethodName").stringValue;

            menu.AddItem(new GUIContent("No Function"), string.IsNullOrEmpty(currentMethodName), ClearEventFunctionCallback, new FunctionData(elementProperty));
            menu.AddSeparator("");

            if (targetObj is Component) { targetObj = (targetObj as Component).gameObject; }
            else if (!(targetObj is GameObject))
            {
                // Function menu for asset objects and such
                BuildMenuForObject(targetObj, elementProperty, menu);
                return menu;
            }

            // GameObject menu
            BuildMenuForObject(targetObj, elementProperty, menu);

            Component[] components = (targetObj as GameObject).GetComponents<Component>();
            var componentTypeCounts = new Dictionary<Type, ComponentTypeCount>();

            // Only get the first instance of each component type
            if (cachedSettings.sameComponentTypesGrouped)
                components = components.GroupBy(comp => comp.GetType()).Select(group => group.First()).ToArray();
            else // Otherwise we need to know if there are multiple components of a given type before we start going through the components since we only need numbers on component types with multiple instances.
                foreach (Component component in components)
                {
                    ComponentTypeCount typeCount;
                    if (!componentTypeCounts.TryGetValue(component.GetType(), out typeCount))
                    {
                        typeCount = new ComponentTypeCount();
                        componentTypeCounts.Add(component.GetType(), typeCount);
                    }

                    typeCount.TotalCount++;
                }

            foreach (Component component in components)
            {
                int componentCount = 0;

                if (!cachedSettings.sameComponentTypesGrouped)
                {
                    ComponentTypeCount typeCount = componentTypeCounts[component.GetType()];
                    if (typeCount.TotalCount > 1)
                        componentCount = typeCount.CurrentCount++;
                }

                BuildMenuForObject(component, elementProperty, menu, componentCount);
            }

            return menu;
        }

        protected void BuildMenuForObject(Object targetObject, SerializedProperty elementProperty, GenericMenu menu, int componentCount = 0)
        {
            var methodInfos = new List<FunctionData>();
            string contentPath = targetObject.GetType().Name + (componentCount > 0 ? string.Format("({0})", componentCount) : "") + "/";

            FindValidMethods(targetObject, PersistentListenerMode.Void, methodInfos);
            FindValidMethods(targetObject, PersistentListenerMode.Int, methodInfos);
            FindValidMethods(targetObject, PersistentListenerMode.Float, methodInfos);
            FindValidMethods(targetObject, PersistentListenerMode.String, methodInfos);
            FindValidMethods(targetObject, PersistentListenerMode.Bool, methodInfos);
            FindValidMethods(targetObject, PersistentListenerMode.Object, methodInfos);

            methodInfos = methodInfos.OrderBy(method1 => method1.targetMethod.Name.StartsWith("set_") ? 0 : 1).ThenBy(method1 => method1.targetMethod.Name).ToList();

            // Get event args to determine if we can do a pass through of the arg to the parameter
            Type[] eventArgs = dummyEvent.GetType().GetMethod("Invoke").GetParameters().Select(p => p.ParameterType).ToArray();

            bool dynamicBinding = false;

            if (eventArgs.Length > 0)
            {
                var dynamicMethodInfos = new List<FunctionData>();
                FindValidMethods(targetObject, PersistentListenerMode.EventDefined, dynamicMethodInfos, eventArgs);

                if (dynamicMethodInfos.Count > 0)
                {
                    dynamicMethodInfos = dynamicMethodInfos.OrderBy(m => m.targetMethod.Name.StartsWith("set") ? 0 : 1).ThenBy(m => m.targetMethod.Name).ToList();

                    dynamicBinding = true;

                    // Add dynamic header
                    menu.AddDisabledItem(new GUIContent(contentPath + $"Dynamic {GetTypeName(eventArgs[0])}"));
                    menu.AddSeparator(contentPath);

                    foreach (FunctionData dynamicMethod in dynamicMethodInfos) { AddFunctionToMenu(contentPath, elementProperty, dynamicMethod, menu, 0, true); }
                }
            }

            // Add static header if we have dynamic bindings
            if (dynamicBinding)
            {
                menu.AddDisabledItem(new GUIContent(contentPath + "Static Parameters"));
                menu.AddSeparator(contentPath);
            }

            foreach (FunctionData method in methodInfos) { AddFunctionToMenu(contentPath, elementProperty, method, menu, componentCount); }
        }

        #endregion
    }

    [InitializeOnLoad]
    public class ReorderableUnityEventHandler
    {
        #region Data Types

        /// <summary>
        /// Container for settings for <see cref="ReorderableUnityEventDrawer"/>.
        /// </summary>
        public class ReorderableUnityEventSettings
        {
            /// <summary>
            /// Whether <see cref="ReorderableUnityEventDrawer"/> is applied at all.
            /// </summary>
            public bool eventDrawerEnabled;
            /// <summary>
            /// If enabled, private methods and properties will be exposed for event listeners when selecting a callback function from the dropdown.
            /// </summary>
            public bool privateMembersShown;
            /// <summary>
            /// If enabled, an "Invoke" button will be displayed in <see cref="UnityEvent"/> header which allows to invoke this <see cref="UnityEvent"/> directly from Inspector.
            /// </summary>
            public bool invokeButtonShown;
            /// <summary>
            /// If enabled, argument types will be shown alongside the method names in <see cref="UnityEvent"/>s.  
            /// </summary>
            public bool argumentTypeDisplayed;
            /// <summary>
            /// TODO: docs
            /// </summary>
            public bool sameComponentTypesGrouped;
            /// <summary>
            /// If enabled, selected <see cref="UnityEvent"/> listeners can be cut, copied, pasted and duplicated using default Unity keyboard shortcuts.
            /// </summary>
            public bool hotkeysEnabled;
        }

        #endregion
        
        #region Constants

        private const string OverrideEventDrawerKey = "Zinnia.Data.Type.ReorderableUnityEvent.overrideEventDrawer";
        private const string ShowPrivateMembersKey = "Zinnia.Data.Type.ReorderableUnityEvent.showPrivateMembers";
        private const string ShowInvokeFieldKey = "Zinnia.Data.Type.ReorderableUnityEvent.showInvokeField";
        private const string DisplayArgumentTypeKey = "Zinnia.Data.Type.ReorderableUnityEvent.displayArgumentType";
        private const string GroupSameComponentTypeKey = "Zinnia.Data.Type.ReorderableUnityEvent.groupSameComponentType";
        private const string UseHotkeys = "Zinnia.Data.Type.ReorderableUnityEvent.usehotkeys";

        #endregion

        #region Static Fields
        
        private static bool DrawerPatchApplied = false;
        private static FieldInfo InternalDrawerTypeMap = null;
        private static System.Type AttributeUtilityType = null;
        
        #endregion

        #region Constructor
        
        static ReorderableUnityEventHandler()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        #endregion

        // https://stackoverflow.com/questions/12898282/type-gettype-not-working 
        public static System.Type FindTypeInAllAssemblies(string qualifiedTypeName)
        {
            System.Type t = System.Type.GetType(qualifiedTypeName);

            if (t != null) { return t; }
            else
            {
                foreach (System.Reflection.Assembly asm in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    t = asm.GetType(qualifiedTypeName);
                    if (t != null)
                        return t;
                }

                return null;
            }
        }

        private static void OnEditorUpdate()
        {
            ApplyEventPropertyDrawerPatch();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            ApplyEventPropertyDrawerPatch(true);
        }

        internal static FieldInfo GetDrawerTypeMap()
        {
            // We already have the map so skip all the reflection
            if (InternalDrawerTypeMap != null) { return InternalDrawerTypeMap; }

            System.Type scriptAttributeUtilityType = FindTypeInAllAssemblies("UnityEditor.ScriptAttributeUtility");

            if (scriptAttributeUtilityType == null)
            {
                Debug.LogError("Could not find ScriptAttributeUtility in assemblies!");
                return null;
            }

            // Save for later in case we need to lookup the function to populate the attributes
            AttributeUtilityType = scriptAttributeUtilityType;

            FieldInfo info = scriptAttributeUtilityType.GetField("s_DrawerTypeForType", BindingFlags.NonPublic | BindingFlags.Static);

            if (info == null)
            {
                Debug.LogError("Could not find drawer type map!");
                return null;
            }

            InternalDrawerTypeMap = info;

            return InternalDrawerTypeMap;
        }

        private static void ClearPropertyCaches()
        {
            if (AttributeUtilityType == null)
            {
                Debug.LogError("UnityEditor.ScriptAttributeUtility type is null! Make sure you have called GetDrawerTypeMap() to ensure this is cached!");
                return;
            }

            // Nuke handle caches so they can find our modified drawer
            MethodInfo clearCacheFunc = AttributeUtilityType.GetMethod("ClearGlobalCache", BindingFlags.NonPublic | BindingFlags.Static);

            if (clearCacheFunc == null)
            {
                Debug.LogError("Could not find cache clear method!");
                return;
            }

            clearCacheFunc.Invoke(null, new object[] { });

            FieldInfo currentCacheField = AttributeUtilityType.GetField("s_CurrentCache", BindingFlags.NonPublic | BindingFlags.Static);

            if (currentCacheField == null)
            {
                Debug.LogError("Could not find CurrentCache field!");
                return;
            }

            object currentCacheValue = currentCacheField.GetValue(null);

            if (currentCacheValue != null)
            {
                MethodInfo clearMethod = currentCacheValue.GetType().GetMethod("Clear", BindingFlags.Public | BindingFlags.Instance);

                if (clearMethod == null)
                {
                    Debug.LogError("Could not find clear function for current cache!");
                    return;
                }

                clearMethod.Invoke(currentCacheValue, new object[] { });
            }

            System.Type inspectorWindowType = FindTypeInAllAssemblies("UnityEditor.InspectorWindow");

            if (inspectorWindowType == null)
            {
                Debug.LogError("Could not find inspector window type!");
                return;
            }

            FieldInfo trackerField = inspectorWindowType.GetField("m_Tracker", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo propertyHandleCacheField = typeof(Editor).GetField("m_PropertyHandlerCache", BindingFlags.NonPublic | BindingFlags.Instance);

            if (trackerField == null || propertyHandleCacheField == null)
            {
                Debug.LogError("Could not find tracker field!");
                return;
            }

            //FieldInfo trackerEditorsField = trackerField.GetType().GetField("")

            System.Type propertyHandlerCacheType = FindTypeInAllAssemblies("UnityEditor.PropertyHandlerCache");

            if (propertyHandlerCacheType == null)
            {
                Debug.LogError("Could not find type of PropertyHandlerCache");
                return;
            }

            // Secondary nuke because Unity is great and keeps a cached copy of the events for every Editor in addition to a global cache we cleared earlier.
            EditorWindow[] editorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            foreach (EditorWindow editor in editorWindows)
            {
                if (editor.GetType() == inspectorWindowType || editor.GetType().IsSubclassOf(inspectorWindowType))
                {
                    ActiveEditorTracker activeEditorTracker = trackerField.GetValue(editor) as ActiveEditorTracker;

                    if (activeEditorTracker != null)
                    {
                        foreach (Editor activeEditor in activeEditorTracker.activeEditors)
                        {
                            if (activeEditor != null)
                            {
                                propertyHandleCacheField.SetValue(activeEditor, System.Activator.CreateInstance(propertyHandlerCacheType));
                                activeEditor.Repaint(); // Force repaint to get updated drawing of property
                            }
                        }
                    }
                }
            }
        }

        // Applies patch to Unity's builtin tracking for Drawers to redirect any drawers for Unity Events to our EasyEventDrawer instead.
        private static void ApplyEventDrawerPatch(bool enableOverride)
        {
            // Call here to find the scriptAttributeUtilityType in case it's needed for when overrides are disabled
            FieldInfo drawerTypeMap = GetDrawerTypeMap();

            if (enableOverride)
            {
                System.Type[] mapArgs = drawerTypeMap.FieldType.GetGenericArguments();

                System.Type keyType = mapArgs[0];
                System.Type valType = mapArgs[1];

                if (keyType == null || valType == null)
                {
                    Debug.LogError("Could not retrieve dictionary types!");
                    return;
                }

                FieldInfo drawerField = valType.GetField("drawer", BindingFlags.Public | BindingFlags.Instance);
                FieldInfo typeField = valType.GetField("type", BindingFlags.Public | BindingFlags.Instance);

                if (drawerField == null || typeField == null)
                {
                    Debug.LogError("Could not retrieve dictionary value fields!");
                    return;
                }

                IDictionary drawerTypeMapDict = drawerTypeMap.GetValue(null) as IDictionary;

                if (drawerTypeMapDict == null)
                {
                    MethodInfo popAttributesFunc = AttributeUtilityType.GetMethod("BuildDrawerTypeForTypeDictionary", BindingFlags.NonPublic | BindingFlags.Static);

                    if (popAttributesFunc == null)
                    {
                        Debug.LogError("Could not populate attributes for override!");
                        return;
                    }

                    popAttributesFunc.Invoke(null, new object[] { });

                    // Try again now that this should be populated
                    drawerTypeMapDict = drawerTypeMap.GetValue(null) as IDictionary;
                    if (drawerTypeMapDict == null)
                    {
                        Debug.LogError("Could not get dictionary for drawer types!");
                        return;
                    }
                }

                // Replace EventDrawer handles with our custom drawer
                List<object> keysToRecreate = new List<object>();

                foreach (DictionaryEntry entry in drawerTypeMapDict)
                {
                    System.Type drawerType = (System.Type) drawerField.GetValue(entry.Value);

                    if (drawerType.Name == "UnityEventDrawer" || drawerType.Name == "CollapsibleUnityEventDrawer") { keysToRecreate.Add(entry.Key); }
                }

                foreach (object keyToKill in keysToRecreate) { drawerTypeMapDict.Remove(keyToKill); }

                // Recreate these key-value pairs since they are structs
                foreach (object keyToRecreate in keysToRecreate)
                {
                    object newValMapping = System.Activator.CreateInstance(valType);
                    typeField.SetValue(newValMapping, (System.Type) keyToRecreate);
                    drawerField.SetValue(newValMapping, typeof(ReorderableUnityEventDrawer));

                    drawerTypeMapDict.Add(keyToRecreate, newValMapping);
                }
            }
            else
            {
                MethodInfo popAttributesFunc = AttributeUtilityType.GetMethod("BuildDrawerTypeForTypeDictionary", BindingFlags.NonPublic | BindingFlags.Static);

                if (popAttributesFunc == null)
                {
                    Debug.LogError("Could not populate attributes for override!");
                    return;
                }

                // Just force the editor to repopulate the drawers without nuking afterwards.
                popAttributesFunc.Invoke(null, new object[] { });
            }

            // Clear caches to force event drawers to refresh immediately.
            ClearPropertyCaches();
        }

        public static void ApplyEventPropertyDrawerPatch(bool forceApply = false)
        {
            ReorderableUnityEventSettings settings = GetEditorSettings();

            if (!DrawerPatchApplied || forceApply)
            {
                ApplyEventDrawerPatch(settings.eventDrawerEnabled);
                DrawerPatchApplied = true;
            }
        }

        // TODO: store configuration in project instead of Editor
        public static ReorderableUnityEventSettings GetEditorSettings()
        {
            var settings = new ReorderableUnityEventSettings
            {
                eventDrawerEnabled = EditorPrefs.GetBool(OverrideEventDrawerKey, true),
                privateMembersShown = EditorPrefs.GetBool(ShowPrivateMembersKey, true),
                invokeButtonShown = EditorPrefs.GetBool(ShowInvokeFieldKey, true),
                argumentTypeDisplayed = EditorPrefs.GetBool(DisplayArgumentTypeKey, true),
                sameComponentTypesGrouped = EditorPrefs.GetBool(GroupSameComponentTypeKey, false),
                hotkeysEnabled = EditorPrefs.GetBool(UseHotkeys, true),
            };

            return settings;
        }

        // TODO: store configuration in project instead of Editor
        public static void SetEditorSettings(ReorderableUnityEventSettings settings)
        {
            EditorPrefs.SetBool(OverrideEventDrawerKey, settings.eventDrawerEnabled);
            EditorPrefs.SetBool(ShowPrivateMembersKey, settings.privateMembersShown);
            EditorPrefs.SetBool(ShowInvokeFieldKey, settings.invokeButtonShown);
            EditorPrefs.SetBool(DisplayArgumentTypeKey, settings.argumentTypeDisplayed);
            EditorPrefs.SetBool(GroupSameComponentTypeKey, settings.sameComponentTypesGrouped);
            EditorPrefs.SetBool(UseHotkeys, settings.hotkeysEnabled);
        }
    }

#if UNITY_2018_3_OR_NEWER

// Use the new settings provider class instead so we don't need to add extra stuff to the Edit menu
// Using the IMGUI method
    /// <summary>
    /// <see cref="SettingsProvider"/> for reorderable <see cref="UnityEvent"/>s.
    /// Allows to configure <see cref="ReorderableUnityEventDrawer"/> project-wide through Project Settings window.
    /// </summary>
    public static class ReorderableUnityEventSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Zinnia/Reorderable Unity Events", SettingsScope.Project)
            {
                label = "Reorderable Unity Events",

                guiHandler = (searchContext) =>
                {
                    ReorderableUnityEventHandler.ReorderableUnityEventSettings settings = ReorderableUnityEventHandler.GetEditorSettings();

                    EditorGUI.BeginChangeCheck();
                    ReorderableUnityEventSettingsGUIContent.DrawSettingsButtons(settings);

                    if (EditorGUI.EndChangeCheck())
                    {
                        ReorderableUnityEventHandler.SetEditorSettings(settings);
                        ReorderableUnityEventHandler.ApplyEventPropertyDrawerPatch(true);
                    }
                },

                keywords = new HashSet<string>(new[] {"Zinnia", "Event", "UnityEvent", "Unity", "Reorderable"})
            };

            return provider;
        }
    }
    
    // TODO: everything inside following #else block below can be removed if this codee is not going to be used in versions earlier than 2018.3.
    //       As Zinnia supports only Unity 2018.3, removing it may make sense.
#else
public class ReorderableUnityEventSettings : EditorWindow
{
    [MenuItem("Edit/Easy Event Editor Settings")]
    static void Init()
    {
        ReorderableUnityEventSettings window = GetWindow<ReorderableUnityEventSettings>(false, "EEE Settings");
        window.minSize = new Vector2(350, 150);
        window.maxSize = new Vector2(350, 150);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Easy Event Editor Settings", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        ReorderableUnityEventHandler.EEESettings settings = ReorderableUnityEventHandler.GetEditorSettings();

        EditorGUI.BeginChangeCheck();
        SettingsGUIContent.DrawSettingsButtons(settings);

        if (EditorGUI.EndChangeCheck())
        {
            ReorderableUnityEventHandler.SetEditorSettings(settings);
            ReorderableUnityEventHandler.ApplyEventPropertyDrawerPatch(true);
        }
    }
}
#endif

    /// <summary>
    /// Static class with <see cref="GUIContent"/> for the reorderable events settings window.
    /// </summary>
    internal static class ReorderableUnityEventSettingsGUIContent
    {
        private static readonly GUIContent EnableToggleGuiContent = new GUIContent("Enable Reorderable Unity Events", "Replaces the default Unity event editing context with reorderable one");

        private static readonly GUIContent EnablePrivateMembersGuiContent =
            new GUIContent("Show private properties and methods", "Exposes private/internal/obsolete properties and methods to the function list on events");

        private static readonly GUIContent ShowInvokeFieldGuiContent =
            new GUIContent("Show invoke button on events", "Gives you a button on events that can be clicked to execute all functions on a given event");

        private static readonly GUIContent DisplayArgumentTypeContent = new GUIContent("Display argument type on function name", "Shows the argument that a function takes on the function header");

        private static readonly GUIContent GroupSameComponentTypeContent = new GUIContent("Do not group components of the same type",
            "If you have multiple components of the same type on one object, show all of them in listener function selection list. Unity hides duplicate components by default.");

        private static readonly GUIContent UseHotkeys = new GUIContent("Use hotkeys",
            "Adds common Unity hotkeys to event editor that operate on the currently selected event. The commands are Add (CTRL+A), Copy, Paste, Cut, Delete, and Duplicate");

        public static void DrawSettingsButtons(ReorderableUnityEventHandler.ReorderableUnityEventSettings settings)
        {
            EditorGUILayout.Separator();
            
            EditorGUI.indentLevel += 1;

            settings.eventDrawerEnabled = EditorGUILayout.ToggleLeft(EnableToggleGuiContent, settings.eventDrawerEnabled);
            EditorGUILayout.Separator();

            EditorGUI.BeginDisabledGroup(!settings.eventDrawerEnabled);

            settings.privateMembersShown = EditorGUILayout.ToggleLeft(EnablePrivateMembersGuiContent, settings.privateMembersShown);
            settings.invokeButtonShown = EditorGUILayout.ToggleLeft(ShowInvokeFieldGuiContent, settings.invokeButtonShown);
            settings.argumentTypeDisplayed = EditorGUILayout.ToggleLeft(DisplayArgumentTypeContent, settings.argumentTypeDisplayed);
            settings.sameComponentTypesGrouped = !EditorGUILayout.ToggleLeft(GroupSameComponentTypeContent, !settings.sameComponentTypesGrouped);
            settings.hotkeysEnabled = EditorGUILayout.ToggleLeft(UseHotkeys, settings.hotkeysEnabled);

            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel -= 1;
        }
    }

}