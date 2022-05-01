namespace Zinnia.Utility
{
#if ZINNIA_IGNORE_CUSTOM_INSPECTOR_EDITOR
#else
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    public class ZinniaInspector : Editor
    {
        protected static readonly string UndoRedoWarningSessionStateKey = typeof(ZinniaInspector).FullName + nameof(UndoRedoWarningPropertyPath);
        protected static string UndoRedoWarningPropertyPath
        {
            get => SessionState.GetString(UndoRedoWarningSessionStateKey, null);
            set => SessionState.SetString(UndoRedoWarningSessionStateKey, value);
        }
        protected static readonly string ScriptProperty = "m_Script";
        protected static readonly string UndoRedoWarningMessage = "Undo/redo is unsupported for this field at runtime: The change won't be noticed by components depending on it.";
        protected static readonly string recordObjectMessage = "Before change handlers";
        protected static readonly string beforeMethodPrefix = "OnBefore";
        protected static readonly string afterMethodPrefix = "OnAfter";
        protected static readonly string methodSuffix = "Change";

        protected SerializedProperty serializedProperty;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            serializedProperty = GetObjectProperties();
            if (serializedProperty == null)
            {
                return;
            }

            try
            {
                ProcessObjectProperties(serializedProperty);
            }
            catch (Exception exception)
            {
                if (exception.GetType() != typeof(ExitGUIException) && exception.GetType() != typeof(ArgumentException))
                {
                    Debug.LogError(exception);
                }
            }
        }

        protected virtual void DrawProperty(SerializedProperty property) => EditorGUILayout.PropertyField(property, true);

        protected virtual void ApplyModifiedProperty(SerializedProperty property, bool hasChangeHandlers)
        {
            if (hasChangeHandlers)
            {
                UndoRedoWarningPropertyPath = property.propertyPath;
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        protected virtual SerializedProperty GetObjectProperties()
        {
            SerializedProperty property = serializedObject.GetIterator();
            if (!property.NextVisible(true))
            {
                return null;
            }

            return property;
        }

        protected virtual void ProcessObjectProperties(SerializedProperty property)
        {
            string undoRedoWarningPropertyPath = UndoRedoWarningPropertyPath;
            do
            {
                ProcessObjectProperty(property, undoRedoWarningPropertyPath);
            }
            while (property.NextVisible(false));
        }

        protected virtual void ProcessObjectProperty(SerializedProperty property, string undoRedoWarningPropertyPath)
        {
            string propertyPath = property.propertyPath;
            Object targetObject = property.serializedObject.targetObject;

            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
            using (new EditorGUI.DisabledGroupScope(propertyPath == ScriptProperty))
            {
                DrawPropertyWithWarningMessage(property, propertyPath, undoRedoWarningPropertyPath);

                // No change has been done, nothing to do.
                if (!changeCheckScope.changed)
                {
                    return;
                }

                TryApplyChangeHandlersToProperty(targetObject, property);
            }
        }

        protected virtual bool CanApplyChangeHandlers(Object targetObject)
        {
            return Application.isPlaying && targetObject is Behaviour behaviour && behaviour.isActiveAndEnabled;
        }

        protected virtual bool TryApplyChangeHandlersToProperty(Object targetObject, SerializedProperty property)
        {
            // At design time we need to still allow Unity to persist the change and enable undo.
            if (!CanApplyChangeHandlers(targetObject))
            {
                ApplyModifiedProperty(property, false);
                return false;
            }

            ApplyChangeHandlersToProperty(targetObject, property);
            return true;
        }

        protected virtual void BeforeChange(MethodInfo methodInfo, SerializedProperty property)
        {
            InvokeMethod(methodInfo, property);
        }

        protected virtual void AfterChange(MethodInfo methodInfo, SerializedProperty property)
        {
            InvokeMethod(methodInfo, property);
        }

        protected virtual void ApplyChangeHandlersToProperty(Object targetObject, SerializedProperty property)
        {
            Type scriptType = property.serializedObject.targetObject.GetType();
            MethodInfo beforeChanged = GetChangeHandler(scriptType, GetBeforeChangeMethodName(property));
            MethodInfo afterChanged = GetChangeHandler(scriptType, GetAfterChangeMethodName(property));

            if (beforeChanged == null && afterChanged == null)
            {
                BeforeChange(beforeChanged, property);
                ApplyModifiedProperty(property, false);
                AfterChange(afterChanged, property);
            }
            else
            {
                Undo.RecordObject(targetObject, recordObjectMessage);
                BeforeChange(beforeChanged, property);
                Undo.FlushUndoRecordObjects();

                using (SerializedObject serializedObjectCopy =
                        new SerializedObject(property.serializedObject.targetObject))
                {
                    SerializedProperty propertyCopy = serializedObjectCopy.GetIterator();
                    if (propertyCopy.Next(true))
                    {
                        do
                        {
                            if (propertyCopy.propertyPath != property.propertyPath)
                            {
                                property.serializedObject.CopyFromSerializedProperty(propertyCopy);
                            }
                        }
                        while (propertyCopy.Next(false));
                    }
                }

                ApplyModifiedProperty(property, true);
                AfterChange(afterChanged, property);
            }
        }

        protected virtual void InvokeMethod(MethodInfo methodIndo, SerializedProperty property)
        {
            if (methodIndo == null)
            {
                return;
            }

            methodIndo.Invoke(property.serializedObject.targetObject, null);
        }

        protected virtual MethodInfo GetChangeHandler(Type type, string handlerName)
        {
            return type.GetMethod(handlerName, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        protected virtual string GetPropertyName(SerializedProperty property)
        {
            return property.displayName.Replace(" ", "");
        }

        protected virtual string GetBeforeChangeMethodName(SerializedProperty property)
        {
            return beforeMethodPrefix + GetPropertyName(property) + methodSuffix;
        }

        protected virtual string GetAfterChangeMethodName(SerializedProperty property)
        {
            return afterMethodPrefix + GetPropertyName(property) + methodSuffix;
        }

        protected virtual void DrawPropertyWithWarningMessage(SerializedProperty property, string propertyPath, string undoRedoWarningPropertyPath)
        {
            bool showUndoRedoWarning = CanShowUndoRedoWarning(propertyPath, undoRedoWarningPropertyPath);
            BeginDrawWarningMessage(showUndoRedoWarning);
            DrawProperty(property);
            EndDrawWarningMessage(showUndoRedoWarning);
        }

        protected virtual bool CanShowUndoRedoWarning(string propertyPath, string undoRedoWarningPropertyPath)
        {
            return Application.isPlaying && propertyPath == undoRedoWarningPropertyPath;
        }

        protected virtual void BeginDrawWarningMessage(bool show)
        {
            if (show)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.HelpBox(UndoRedoWarningMessage, MessageType.Warning);
                EditorGUI.indentLevel++;
            }
        }

        protected virtual void EndDrawWarningMessage(bool show)
        {
            if (show)
            {
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
        }
    }
#endif
}