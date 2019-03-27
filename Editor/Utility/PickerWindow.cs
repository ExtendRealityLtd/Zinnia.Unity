namespace Zinnia.Utility
{
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class PickerWindow<TElement, TSelf> : EditorWindow where TSelf : PickerWindow<TElement, TSelf>
    {
        protected static readonly string SearchTextSessionStateKey = typeof(PickerWindow<,>).FullName + nameof(SearchTextSessionStateKey);

        protected readonly List<TElement> elements = new List<TElement>();
        protected readonly List<TElement> searchResults = new List<TElement>();

        protected Action<TElement> elementSelector;
        protected Func<TElement, string> searchTermExtractor;
        protected Func<TElement, GUIContent> elementDrawer;

        protected SearchField searchField;
        protected GUIStyle searchLabelPrefixStyle;
        protected GUIContent searchLabelPrefixContent;

        protected GUIStyle elementStyle;
        protected float elementHeight;

        protected string searchText = string.Empty;
        protected Vector2 scrollPosition = Vector2.zero;

        protected int? visibleCount;
        protected bool scrollToSelectedIndex;
        protected int selectedIndex;

        public static TSelf Show(
            Rect sourceRect,
            IEnumerable<TElement> elements,
            Action<TElement> elementSelector,
            Func<TElement, string> searchTermExtractor,
            Func<TElement, GUIContent> elementDrawer)
        {
            TSelf window = CreateInstance<TSelf>();
            window.elementSelector = elementSelector;
            window.searchTermExtractor = searchTermExtractor;
            window.elementDrawer = elementDrawer;

            window.elements.AddRange(elements);
            window.FindElements(SessionState.GetString(SearchTextSessionStateKey, string.Empty));

            window.ShowAsDropDown(sourceRect, new Vector2(Mathf.Max(250f, sourceRect.width), 350f));

            return window;
        }

        protected virtual void Awake()
        {
            titleContent = new GUIContent("Picker");
            wantsMouseMove = true;

            searchField = new SearchField();
            searchLabelPrefixStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true
            };

            string color = EditorGUIUtility.isProSkin ? "4F80F8" : "0808FC";
            searchLabelPrefixContent = new GUIContent(
                $"Search (<color=#{color}>Regex</color>):",
                "This search supports regular expressions. Click to learn more about them.");

            elementStyle = new GUIStyle(EditorStyles.label);
            elementHeight = elementStyle.CalcSize(GUIContent.none).y;

            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        }

        protected virtual void OnBeforeAssemblyReload()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;

            try
            {
                Close();
            }
            catch
            {
                DestroyImmediate(this);
            }
        }

        protected virtual void OnGUI()
        {
            // The search field drawn below will use keyboard events, thus we need to check them before it's drawn.
            HandleKeyboard();

            using (new EditorGUILayout.VerticalScope("grey_border"))
            {
                DrawSearch();
                DrawList();
            }
        }

        protected virtual void HandleKeyboard()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type != EventType.KeyDown)
            {
                return;
            }

            switch (currentEvent.keyCode)
            {
                case KeyCode.Escape when string.IsNullOrEmpty(searchText):
                    Close();
                    GUIUtility.ExitGUI();
                    currentEvent.Use();
                    break;
                case KeyCode.DownArrow:
                    selectedIndex++;
                    scrollToSelectedIndex = true;
                    currentEvent.Use();
                    break;
                case KeyCode.UpArrow:
                    selectedIndex--;
                    scrollToSelectedIndex = true;
                    currentEvent.Use();
                    break;
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    elementSelector(searchResults[selectedIndex]);
                    Close();
                    GUIUtility.ExitGUI();
                    currentEvent.Use();
                    break;
            }
        }

        protected virtual void DrawSearch()
        {
            GUILayout.Space(5f);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(5f);

                Vector2 labelSize = searchLabelPrefixStyle.CalcSize(searchLabelPrefixContent);
                GUILayoutOption widthLayoutOption = GUILayout.Width(labelSize.x);

                using (new EditorGUILayout.VerticalScope(widthLayoutOption))
                {
                    GUILayout.Space(3f);
                    EditorGUILayout.LabelField(searchLabelPrefixContent, searchLabelPrefixStyle, widthLayoutOption);

                    Rect labelRect = GUILayoutUtility.GetLastRect();
                    EditorGUIUtility.AddCursorRect(labelRect, MouseCursor.Link);

                    Event currentEvent = Event.current;
                    if (currentEvent.type == EventType.MouseUp && labelRect.Contains(currentEvent.mousePosition))
                    {
                        Application.OpenURL("https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expressions");
                        currentEvent.Use();
                    }
                }

                searchField.SetFocus();
                string newSearchText = searchField.OnGUI(EditorGUILayout.GetControlRect(false), searchText);
                if (searchText != newSearchText)
                {
                    FindElements(newSearchText);
                }

                GUILayout.Space(3f);
            }
        }

        protected virtual void DrawList()
        {
            Event currentEvent = Event.current;
            Rect? selectedRect;

            using (EditorGUILayout.ScrollViewScope scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition, EditorStyles.helpBox))
            {
                scrollPosition = scrollViewScope.scrollPosition;
                selectedIndex = Mathf.Clamp(selectedIndex, 0, searchResults.Count - 1);

                int startIndex = visibleCount == null
                    ? 0
                    : Mathf.Clamp((int)(scrollPosition.y / elementHeight), 0, searchResults.Count - visibleCount.Value);
                int count = visibleCount == null
                    ? searchResults.Count
                    : Mathf.Min(startIndex + visibleCount.Value, searchResults.Count);

                GUILayout.Space(startIndex * elementHeight);
                selectedRect = DrawElements(startIndex, count);
                GUILayout.Space(Mathf.Max(0, (searchResults.Count - startIndex - (visibleCount ?? searchResults.Count)) * elementHeight));
            }

            Rect scrollViewRect = GUILayoutUtility.GetLastRect();

            if (scrollToSelectedIndex && currentEvent.type == EventType.Repaint)
            {
                if (selectedRect == null)
                {
                    selectedRect = new Rect(0f, selectedIndex * elementHeight, 0f, 0f);
                }

                scrollToSelectedIndex = false;

                Rect selectedRectValue = selectedRect.Value;
                if (selectedRectValue.yMax - scrollViewRect.height > scrollPosition.y)
                {
                    scrollPosition.y = selectedRectValue.yMax - scrollViewRect.height;
                    Repaint();
                }
                else if (selectedRectValue.y < scrollPosition.y)
                {
                    scrollPosition.y = selectedRectValue.y;
                    Repaint();
                }
            }

            if (currentEvent.type == EventType.Repaint)
            {
                visibleCount = Mathf.Clamp(Mathf.CeilToInt(scrollViewRect.height / elementHeight), 0, searchResults.Count);
            }
        }

        protected virtual Rect? DrawElements(int startIndex, int count)
        {
            Color previousBackgroundColor = GUI.backgroundColor;
            Texture2D previousNormalBackground = elementStyle.normal.background;
            Event currentEvent = Event.current;
            Rect? selectedRect = null;

            for (int index = startIndex; index < count; index++)
            {
                bool isSelected = selectedIndex == index;
                if (isSelected)
                {
                    GUI.backgroundColor = GUI.skin.settings.selectionColor;
                    elementStyle.normal.background = Texture2D.whiteTexture;
                }

                TElement element = searchResults[index];
                EditorGUILayout.LabelField(elementDrawer(element), elementStyle);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (isSelected)
                {
                    selectedRect = rect;

                    elementStyle.normal.background = previousNormalBackground;
                    GUI.backgroundColor = previousBackgroundColor;
                }

                bool isHoveredOver = rect.Contains(currentEvent.mousePosition);
                if ((currentEvent.type == EventType.MouseMove || currentEvent.type == EventType.MouseDrag)
                    && !isSelected
                    && isHoveredOver)
                {
                    selectedIndex = index;
                    currentEvent.Use();
                }

                if (currentEvent.type != EventType.MouseUp || !isHoveredOver)
                {
                    continue;
                }

                selectedIndex = index;
                elementSelector(searchResults[selectedIndex]);
                Close();
                GUIUtility.ExitGUI();
                currentEvent.Use();
            }

            return selectedRect;
        }

        protected virtual void FindElements(string searchText)
        {
            searchText = searchText ?? string.Empty;

            this.searchText = searchText;
            SessionState.SetString(SearchTextSessionStateKey, searchText);

            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            Regex regex;

            try
            {
                regex = new Regex(this.searchText, options);
            }
            catch
            {
                regex = new Regex(".*", options);
            }

            searchResults.Clear();
            searchResults.AddRange(elements.Where(element => regex.IsMatch(searchTermExtractor(element))));

            if (visibleCount != null)
            {
                visibleCount = Mathf.Clamp(visibleCount.Value, 0, searchResults.Count);
            }
        }
    }
}