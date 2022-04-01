namespace Zinnia.Pattern
{
    using UnityEngine;
    using Zinnia.Extension;

    /// <summary>
    /// Matches the name of the selected <see cref="Application"/> property.
    /// </summary>
    public class ApplicationPatternMatcher : PatternMatcher
    {
        /// <summary>
        /// The property source.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// The whether the application is in editor mode.
            /// </summary>
            IsEditor,
            /// <summary>
            /// The whether the application currently has user focus.
            /// </summary>
            IsFocused,
            /// <summary>
            /// The application platform.
            /// </summary>
            Platform,
            /// <summary>
            /// The language of the operating system the application is running on.
            /// </summary>
            SystemLanguage,
            /// <summary>
            /// The version of Unity the application is built in.
            /// </summary>
            UnityVersion
        }

        [Tooltip("The source property to match against.")]
        [SerializeField]
        private Source propertySource;
        /// <summary>
        /// The source property to match against.
        /// </summary>
        public Source PropertySource
        {
            get
            {
                return propertySource;
            }
            set
            {
                propertySource = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterPropertySourceChange();
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="PropertySource"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="Source"/>.</param>
        public virtual void SetPropertySource(int index)
        {
            PropertySource = EnumExtensions.GetByIndex<Source>(index);
        }

        /// <inheritdoc/>
        protected override string DefineSourceString()
        {
            switch (PropertySource)
            {
                case Source.IsEditor:
                    return Application.isEditor.ToString();
                case Source.IsFocused:
                    return Application.isFocused.ToString();
                case Source.Platform:
                    return Application.platform.ToString();
                case Source.SystemLanguage:
                    return Application.systemLanguage.ToString();
                case Source.UnityVersion:
                    return Application.unityVersion.ToString();
            }

            return null;
        }

        /// <summary>
        /// Called after <see cref="PropertySource"/> has been changed.
        /// </summary>
        protected virtual void OnAfterPropertySourceChange()
        {
            ProcessSourceString();
        }
    }
}