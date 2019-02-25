namespace Zinnia.Utility
{
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class TypePickerWindow : EditorWindow
    {
        protected static string searchText = string.Empty;

        protected Type type;
        protected Type[] componentTypes;
        protected readonly List<Type> searchedComponentTypes = new List<Type>();

        protected Action<Type> selectAction;

        protected SearchField searchField;
        protected GUIStyle labelStyle;

        protected Vector2 scrollPosition = Vector2.zero;
        protected int selectionIndex;

        public static void Show(Rect sourceRect, Type type, Action<Type> selectAction)
        {
            TypePickerWindow window = CreateInstance<TypePickerWindow>();
            window.type = type;
            window.componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(
                    possibleComponentType =>
                    {
                        AddComponentMenu addComponentMenuAttribute = possibleComponentType.GetCustomAttributes<AddComponentMenu>(true).FirstOrDefault();
                        return type.IsAssignableFrom(possibleComponentType)
                            && !possibleComponentType.IsAbstract
                            && !possibleComponentType.IsNestedPrivate
                            && (addComponentMenuAttribute == null
                                || !string.IsNullOrWhiteSpace(addComponentMenuAttribute.componentMenu));
                    })
                .OrderBy(componentType => componentType.Name)
                .ToArray();
            window.selectAction = selectAction;
            window.ShowAsDropDown(sourceRect, new Vector2(Mathf.Max(250f, sourceRect.width), 250f));
        }

        protected virtual void Awake()
        {
            titleContent = new GUIContent("Type Picker");
            wantsMouseMove = true;
            searchField = new SearchField();
            labelStyle = new GUIStyle(EditorStyles.label);
        }

        protected virtual void OnDisable()
        {
            searchText = null;
        }

        protected virtual void OnGUI()
        {
            Event currentEvent = Event.current;
            if (type == null
                || currentEvent.type == EventType.KeyDown
                && currentEvent.keyCode == KeyCode.Escape
                && string.IsNullOrEmpty(searchText))
            {
                Close();
                return;
            }

            if (currentEvent.type == EventType.KeyDown)
            {
                if (currentEvent.keyCode == KeyCode.DownArrow)
                {
                    selectionIndex++;
                    Repaint();
                    return;
                }

                if (currentEvent.keyCode == KeyCode.UpArrow)
                {
                    selectionIndex--;
                    Repaint();
                    return;
                }
            }

            using (new EditorGUILayout.VerticalScope("grey_border"))
            {
                const float searchRectPadding = 8f;
                Rect searchRect = GUILayoutUtility.GetRect(36f, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                searchRect.x += searchRectPadding;
                searchRect.y = 7f;
                searchRect.width -= searchRectPadding * 2f;
                searchRect.height = 30f;

                searchField.SetFocus();
                string newSearchText = searchField.OnGUI(searchRect, searchText);
                if (searchText != newSearchText || searchedComponentTypes.Count == 0 && string.IsNullOrEmpty(newSearchText))
                {
                    searchText = newSearchText;
                    searchedComponentTypes.Clear();
                    searchedComponentTypes.AddRange(componentTypes.Where(type1 => Regex.IsMatch(type1.Name, $".*{searchText}.*", RegexOptions.IgnoreCase)));
                }

                selectionIndex = Math.Min(searchedComponentTypes.Count - 1, Math.Max(0, selectionIndex));

                EditorGUILayout.Separator();

                using (EditorGUILayout.ScrollViewScope scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition, EditorStyles.helpBox))
                {
                    scrollPosition = scrollViewScope.scrollPosition;

                    if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 || currentEvent.type == EventType.KeyDown && (currentEvent.keyCode == KeyCode.Return || currentEvent.keyCode == KeyCode.KeypadEnter))
                    {
                        selectAction(searchedComponentTypes[selectionIndex]);
                        Close();
                        return;
                    }

                    for (int index = 0; index < searchedComponentTypes.Count; index++)
                    {
                        Color previousBackgroundColor = GUI.backgroundColor;
                        Texture2D previousNormalBackground = labelStyle.normal.background;
                        if (selectionIndex == index)
                        {
                            GUI.backgroundColor = GUI.skin.settings.selectionColor;
                            labelStyle.normal.background = Texture2D.whiteTexture;
                            EditorGUILayout.BeginHorizontal();
                        }

                        Type matchingComponentType = searchedComponentTypes[index];
                        EditorGUILayout.LabelField(
                            new GUIContent(
                                ObjectNames.NicifyVariableName(matchingComponentType.Name),
                                AssetPreview.GetMiniTypeThumbnail(matchingComponentType)),
                            labelStyle);

                        if (selectionIndex == index)
                        {
                            EditorGUILayout.EndHorizontal();
                            labelStyle.normal.background = previousNormalBackground;
                            GUI.backgroundColor = previousBackgroundColor;
                        }

                        if (currentEvent.type == EventType.MouseMove && GUILayoutUtility.GetLastRect().Contains(currentEvent.mousePosition))
                        {
                            selectionIndex = index;
                            Repaint();
                            return;
                        }
                    }
                }
            }
        }
    }
}