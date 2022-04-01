namespace Zinnia.Data.Type.Observer
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using Zinnia.Extension;

    /// <summary>
    /// The basis for all Observable Property types.
    /// </summary>
    public abstract class ObservableProperty : MonoBehaviour { }

    /// <summary>
    /// Allows observing changes to a property.
    /// </summary>
    /// <typeparam name="TType">The data type.
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type to use.</typeparam>
    public abstract class ObservableProperty<TType, TEvent> : ObservableProperty where TEvent : UnityEvent<TType>, new()
    {
        /// <summary>
        /// Emitted when the property value is set and modified from its previous value.
        /// </summary>
        [Header("Observable Events")]
        public TEvent Modified = new TEvent();
        /// <summary>
        /// Emitted when the property value is set and unmodified from its previous value.
        /// </summary>
        public TEvent Unmodified = new TEvent();
        /// <summary>
        /// Emitted when the property value is set and modified back to the data type's default value.
        /// </summary>
        public TEvent Defaulted = new TEvent();
        /// <summary>
        /// Emitted when the property value is set and modified from the data type's default value to a defined value.
        /// </summary>
        public TEvent Defined = new TEvent();

        [Header("Property Settings")]
        [Tooltip("The observed data.")]
        [SerializeField]
        private TType data;
        /// <summary>
        /// The observed data.
        /// </summary>
        public TType Data
        {
            get
            {
                return data;
            }
            set
            {
                if (this.IsMemberChangeAllowed())
                {
                    OnBeforeDataChange();
                }
                data = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterDataChange();
                }
            }
        }
        [Tooltip("Whether to observe data changes that were made when the component was disabled and subsequently re-enabled. Events are not raised when component is disabled.")]
        [SerializeField]
        private bool observeChangesFromDisabledState = true;
        /// <summary>
        /// Whether to observe data changes that were made when the component was disabled and subsequently re-enabled. Events are not raised when component is disabled.
        /// </summary>
        public bool ObserveChangesFromDisabledState
        {
            get
            {
                return observeChangesFromDisabledState;
            }
            set
            {
                observeChangesFromDisabledState = value;
            }
        }

        /// <summary>
        /// The previous value of the <see cref="TType"/>.
        /// </summary>
        protected TType previousDataValue;
        /// <summary>
        /// Whether to raise the unmodified event.
        /// </summary>
        protected bool shouldRaiseUnmodifiedEvent;

        /// <summary>
        /// Resets the <see cref="Data"/> back to its default value.
        /// </summary>
        public virtual void ResetToDefault()
        {
            Data = default;
        }

        protected virtual void Awake()
        {
            CacheExistingValue();
        }

        protected virtual void OnEnable()
        {
            shouldRaiseUnmodifiedEvent = false;
            if (ObserveChangesFromDisabledState)
            {
                CheckForChanges();
            }
            shouldRaiseUnmodifiedEvent = true;
        }

        protected virtual void OnDisable()
        {
            CacheExistingValue();
        }

        /// <summary>
        /// Determines whether the two given <see cref="TType"/> values are equal.
        /// </summary>
        /// <param name="a">The <see cref="TType"/> to compare against.</param>
        /// <param name="b">The <see cref="TType"/> to compare with.</param>
        /// <returns><see langword="true"/> if the two <see cref="TType"/> values are considered equal.</returns>
        protected virtual bool Equals(TType a, TType b)
        {
            return EqualityComparer<TType>.Default.Equals(a, b);
        }

        /// <summary>
        /// Caches the existing <see cref="Data"/> value.
        /// </summary>
        protected virtual void CacheExistingValue()
        {
            previousDataValue = Data;
        }

        /// <summary>
        /// Checks for changes from the current <see cref="Data"/> value to the previous value.
        /// </summary>
        protected virtual void CheckForChanges()
        {
            if (!this.IsValidState())
            {
                return;
            }

            if (Equals(Data, previousDataValue))
            {
                if (shouldRaiseUnmodifiedEvent)
                {
                    Unmodified?.Invoke(Data);
                }
            }
            else
            {
                if (Equals(previousDataValue, default) && !Equals(Data, default))
                {
                    Defined?.Invoke(Data);
                }

                Modified?.Invoke(Data);

                if (!Equals(previousDataValue, default) && Equals(Data, default))
                {
                    Defaulted?.Invoke(Data);
                }
            }
            CacheExistingValue();
        }

        /// <summary>
        /// Called before <see cref="Data"/> has been changed.
        /// </summary>
        protected virtual void OnBeforeDataChange()
        {
            CacheExistingValue();
        }

        /// <summary>
        /// Called after <see cref="Data"/> has been changed.
        /// </summary>
        protected virtual void OnAfterDataChange()
        {
            CheckForChanges();
        }
    }
}