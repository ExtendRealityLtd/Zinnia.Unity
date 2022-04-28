namespace Zinnia.Tracking.Collision
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Zinnia.Extension;
    using Zinnia.Rule;

    /// <summary>
    /// Tracks collisions on the <see cref="GameObject"/> this component is on.
    /// </summary>
    public class CollisionTracker : CollisionNotifier
    {
        #region Tracker Settings
        [Header("Tracker Settings")]
        [Tooltip("Causes collisions to stop if the GameObject on either side of the collision is disabled.")]
        [SerializeField]
        private bool stopCollisionsOnDisable = true;
        /// <summary>
        /// Causes collisions to stop if the <see cref="GameObject"/> on either side of the collision is disabled.
        /// </summary>
        public bool StopCollisionsOnDisable
        {
            get
            {
                return stopCollisionsOnDisable;
            }
            set
            {
                stopCollisionsOnDisable = value;
            }
        }
        [Tooltip("Allows to optionally determine which colliders to allow collisions against.")]
        [SerializeField]
        private RuleContainer colliderValidity;
        /// <summary>
        /// Allows to optionally determine which colliders to allow collisions against.
        /// </summary>
        public RuleContainer ColliderValidity
        {
            get
            {
                return colliderValidity;
            }
            set
            {
                colliderValidity = value;
            }
        }
        [Tooltip("Allows to optionally determine which collider containing transforms to allow collisions against.")]
        [SerializeField]
        private RuleContainer containingTransformValidity;
        /// <summary>
        /// Allows to optionally determine which collider containing transforms to allow collisions against.
        /// </summary>
        public RuleContainer ContainingTransformValidity
        {
            get
            {
                return containingTransformValidity;
            }
            set
            {
                containingTransformValidity = value;
            }
        }
        [Tooltip("The delay interval in seconds defining how long to pause between processing the `Stay` method of the collision process. Negative values will be clamped to zero.")]
        [SerializeField]
        private float stayDelayInterval;
        /// <summary>
        /// The delay interval in seconds defining how long to pause between processing the `Stay` method of the collision process. Negative values will be clamped to zero.
        /// </summary>
        public float StayDelayInterval
        {
            get
            {
                return stayDelayInterval;
            }
            set
            {
                stayDelayInterval = value;
                if (this.IsMemberChangeAllowed())
                {
                    OnAfterStayDelayIntervalChange();
                }
            }
        }
        /// <summary>
        /// When to process the `Stay` method the next time. Updated automatically based on <see cref="StayDelayInterval"/> after a `Stay` method has been called.
        /// </summary>
        public float NextStayProcessTime { get; protected set; }
        #endregion

        /// <summary>
        /// Determines whether to apply the fix for the PhysX 4.11 issue where when a <see cref="Rigidbody"/> kinematic state is changed it force calls a <see cref="OnTriggerExit(Collider)"/> and a subsequent <see cref="OnTriggerExit(Collider)"/> for the collision even though nothing has changed with the collision.
        /// </summary>
        /// <remarks>
        /// This is set to <see langword="true"/> in Unity 2019.3 and above when the PhysX version was updated to 4.11.
        /// </remarks>
        public bool ApplyKinematicChangeTriggerEventFix { get; set; }

        /// <summary>
        /// A collection of current existing collisions.
        /// </summary>
        protected List<Collider> trackedCollisions = new List<Collider>();
        /// <summary>
        /// A collection to track the kinematic state changes in for the PhysX 4.11 trigger exit/enter issue.
        /// </summary>
        protected HashSet<Rigidbody> trackedStateChangers = new HashSet<Rigidbody>();
        /// <summary>
        /// A collection to track which colliders have caused the Trigger Exit within the same frame count to avoid duplicate kinematic change event fixes.
        /// </summary>
        Dictionary<Collider, int> exitColliderTimeStamps = new Dictionary<Collider, int>();
        /// <summary>
        /// A collection to track which collider containers have caused the Trigger Exit within the same frame count to avoid duplicate kinematic change event fixes.
        /// </summary>
        Dictionary<Transform, int> exitColliderContainerTimeStamps = new Dictionary<Transform, int>();
        /// <summary>
        /// An instruction to wait for the next FixedUpdate process in the life-cycle.
        /// </summary>
        protected WaitForFixedUpdate waitForFixedUpdateInstruction = new WaitForFixedUpdate();
        /// <summary>
        /// The coroutine for deferring the <see cref="OnTriggerExit(Collider)"/> call to the next FixedUpdate process in the life-cycle.
        /// </summary>
        protected Coroutine deferredTriggerExit;
        /// <summary>
        /// The <see cref="Transform"/> containing the current checked collider.
        /// </summary>
        protected Transform colliderContainingTransform;

        /// <summary>
        /// Clears <see cref="ColliderValidity"/>.
        /// </summary>
        public virtual void ClearColliderValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ColliderValidity = default;
        }

        /// <summary>
        /// Clears <see cref="ContainingTransformValidity"/>.
        /// </summary>
        public virtual void ClearContainingTransformValidity()
        {
            if (!this.IsValidState())
            {
                return;
            }

            ContainingTransformValidity = default;
        }

        /// <summary>
        /// Prepares the collision states for a kinematic state change on the given <see cref="Rigidbody"/>.
        /// </summary>
        /// <remarks>
        /// This is a requirement for Unity 2019.3 and above and the PhysX 4.11 handling of kinematic changes on a <see cref="Rigidbody"/>.
        /// </remarks>
        /// <param name="aboutToChange">The <see cref="Rigidbody"/> that is to have the kinematic state changed on imminently.</param>
        public virtual void PrepareKinematicStateChange(Rigidbody aboutToChange)
        {
            if (!ApplyKinematicChangeTriggerEventFix || aboutToChange == null)
            {
                return;
            }

            trackedStateChangers.Add(aboutToChange);
        }

        /// <summary>
        /// Stops the collision between this <see cref="CollisionTracker"/> and the given <see cref="Collider"/>. If there is still a physical intersection, then the collision will start again in the next physics frame.
        /// </summary>
        /// <param name="collider">The <see cref="Collider"/> to stop the collision with.</param>
        public virtual void StopCollision(Collider collider)
        {
            RemoveDisabledObserver(collider);

            if (collider == null || (StatesToProcess & CollisionStates.Exit) == 0)
            {
                return;
            }

            OnCollisionStopped(eventData.Set(this, collider.isTrigger, null, collider));
        }

        protected virtual void Awake()
        {
#if UNITY_2019_3_OR_NEWER
            ApplyKinematicChangeTriggerEventFix = true;
#endif
            NextStayProcessTime = Time.time;
        }

        protected virtual void OnEnable()
        {
            OnAfterStayDelayIntervalChange();
        }

        protected virtual void OnDisable()
        {
            if (!StopCollisionsOnDisable)
            {
                return;
            }

            foreach (Collider collider in trackedCollisions.ToArray())
            {
                StopCollision(collider);
            }

            StopDeferredTriggerExitRoutine();
            trackedStateChangers.Clear();
            exitColliderTimeStamps.Clear();
            exitColliderContainerTimeStamps.Clear();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            AddDisabledObserver(collision.collider);

            if ((StatesToProcess & CollisionStates.Enter) == 0)
            {
                return;
            }

            OnCollisionStarted(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            if (ShouldIgnoreStay())
            {
                return;
            }

            OnCollisionChanged(eventData.Set(this, false, collision, collision.collider));
            NextStayProcessTime = Time.time + StayDelayInterval;
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            RemoveDisabledObserver(collision.collider);

            if ((StatesToProcess & CollisionStates.Exit) == 0)
            {
                return;
            }

            OnCollisionStopped(eventData.Set(this, false, collision, collision.collider));
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (HasKinematicStateChanged(collider, true))
            {
                exitColliderContainerTimeStamps.Remove(collider.GetContainingTransform());
                if (exitColliderTimeStamps.TryGetValue(collider, out int colliderFrame) && colliderFrame == Time.frameCount)
                {
                    StopDeferredTriggerExitRoutine();
                    exitColliderTimeStamps.Remove(collider);
                    return;
                }
            }

            AddDisabledObserver(collider);

            if ((StatesToProcess & CollisionStates.Enter) == 0)
            {
                return;
            }

            OnCollisionStarted(eventData.Set(this, true, null, collider));
        }

        protected virtual void OnTriggerStay(Collider collider)
        {
            if (ShouldIgnoreStay())
            {
                return;
            }

            OnCollisionChanged(eventData.Set(this, true, null, collider));
            NextStayProcessTime = Time.time + StayDelayInterval;
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            colliderContainingTransform = collider.GetContainingTransform();
            if (HasKinematicStateChanged(collider, false) && (!exitColliderContainerTimeStamps.TryGetValue(colliderContainingTransform, out int colliderFrame) || colliderFrame != Time.frameCount))
            {
                exitColliderTimeStamps[collider] = Time.frameCount;
                exitColliderContainerTimeStamps[colliderContainingTransform] = Time.frameCount;
                StopDeferredTriggerExitRoutine();
                deferredTriggerExit = StartCoroutine(RunTriggerExitAfterNextFixedUpdate(collider));
                return;
            }

            RemoveDisabledObserver(collider);

            if ((StatesToProcess & CollisionStates.Exit) == 0)
            {
                return;
            }

            OnCollisionStopped(eventData.Set(this, true, null, collider));
        }

        /// <inheritdoc />
        protected override bool CanEmit(EventData data)
        {
            return base.CanEmit(data)
                && (data.ColliderData == null
                    ||
                    (ColliderValidity.Accepts(data.ColliderData.gameObject)
                    && ContainingTransformValidity.Accepts(data.ColliderData.GetContainingTransform().gameObject)));
        }

        /// <summary>
        /// Determines whether to ignore the collision/trigger stay state.
        /// </summary>
        /// <returns>Whether to ignore the state.</returns>
        protected virtual bool ShouldIgnoreStay()
        {
            return (StatesToProcess & CollisionStates.Stay) == 0 || NextStayProcessTime > Time.time;
        }

        /// <summary>
        /// Determines whether the kinematic state of the given <see cref="Collider.attachedRigidbody"/> has been flagged as it has just changed.
        /// </summary>
        /// <param name="collider">The <see cref="Collider"/> to get the <see cref="Rigidbody"/> kinematic state from.</param>
        /// <param name="remove">Whether to remove the <see cref="Rigidbody"/> from the <see cref="trackedStateChangers"/> collection.</param>
        /// <returns>Whether the kinematic state has changed on the given <see cref="Collider.attachedRigidbody"/>.</returns>
        protected virtual bool HasKinematicStateChanged(Collider collider, bool remove)
        {
            if (!ApplyKinematicChangeTriggerEventFix || collider.attachedRigidbody == null || !trackedStateChangers.Contains(collider.attachedRigidbody))
            {
                return false;
            }

            if (remove)
            {
                trackedStateChangers.Remove(collider.attachedRigidbody);
            }

            return true;
        }

        /// <summary>
        /// Executes the <see cref="OnTriggerExit(Collider)"/> after the next FixedUpdate process in the life-cycle.
        /// </summary>
        /// <param name="collider">The <see cref="Collider"/> to run with.</param>
        /// <returns>An Enumerator to manage the running of the Coroutine.</returns>
        protected IEnumerator RunTriggerExitAfterNextFixedUpdate(Collider collider)
        {
            yield return waitForFixedUpdateInstruction;
            trackedStateChangers.Remove(collider.attachedRigidbody);
            OnTriggerExit(collider);
            deferredTriggerExit = null;
        }

        /// <summary>
        /// Stops the <see cref="deferredTriggerExit"/> coroutine from running.
        /// </summary>
        protected virtual void StopDeferredTriggerExitRoutine()
        {
            if (deferredTriggerExit != null)
            {
                StopCoroutine(deferredTriggerExit);
            }
        }

        /// <summary>
        /// Adds a <see cref="CollisionTrackerDisabledObserver"/> to the <see cref="GameObject"/> that contains the <see cref="Collider"/> causing the collision.
        /// </summary>
        /// <param name="target">The target to add the <see cref="CollisionTrackerDisabledObserver"/> component to.</param>
        protected virtual void AddDisabledObserver(Collider target)
        {
            if (target == null || !StopCollisionsOnDisable || ContainsDisabledObserver(target))
            {
                return;
            }

            trackedCollisions.Add(target);
            CollisionTrackerDisabledObserver observer = target.gameObject.AddComponent<CollisionTrackerDisabledObserver>();
            observer.Source = this;
            observer.Target = target;
        }

        /// <summary>
        /// Removes the <see cref="CollisionTrackerDisabledObserver"/> from the <see cref="GameObject"/> that contains the <see cref="Collider"/> ceasing the collision.
        /// </summary>
        /// <param name="target">The target to remove the <see cref="CollisionTrackerDisabledObserver"/> component from.</param>
        protected virtual void RemoveDisabledObserver(Collider target)
        {
            if (target == null || !StopCollisionsOnDisable)
            {
                return;
            }

            foreach (CollisionTrackerDisabledObserver observer in target.gameObject.GetComponents<CollisionTrackerDisabledObserver>())
            {
                if (observer.Source == this && observer.Target == target)
                {
                    observer.Destroy();
                    trackedCollisions.Remove(target);
                }
            }
        }

        /// <summary>
        /// Checks to see if the target already contains a <see cref="CollisionTrackerDisabledObserver"/> for this source.
        /// </summary>
        /// <param name="target">The target to check the existence of a <see cref="CollisionTrackerDisabledObserver"/> on.</param>
        /// <returns>Whether the target already contains a <see cref="CollisionTrackerDisabledObserver"/>.</returns>
        protected virtual bool ContainsDisabledObserver(Collider target)
        {
            foreach (CollisionTrackerDisabledObserver observer in target.gameObject.GetComponents<CollisionTrackerDisabledObserver>())
            {
                if (observer.Source == this && observer.Target == target)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Called after <see cref="StayDelayInterval"/> has been changed.
        /// </summary>
        protected virtual void OnAfterStayDelayIntervalChange()
        {
            stayDelayInterval = Mathf.Max(0f, StayDelayInterval);
            NextStayProcessTime = Time.time;
        }
    }

    /// <summary>
    /// Observes the disabled state of any <see cref="GameObject"/> that the <see cref="CollisionTracker"/> is currently colliding with.
    /// </summary>
    public class CollisionTrackerDisabledObserver : MonoBehaviour
    {
        [Tooltip("The CollisionTracker that is causing the collision.")]
        [SerializeField]
        private CollisionTracker source;
        /// <summary>
        /// The <see cref="CollisionTracker"/> that is causing the collision.
        /// </summary>
        public CollisionTracker Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }
        [Tooltip("The Collider that is being collided with.")]
        [SerializeField]
        private Collider target;
        /// <summary>
        /// The <see cref="Collider"/> that is being collided with.
        /// </summary>
        public Collider Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        /// <summary>
        /// Whether <see cref="this"/> is being destroyed.
        /// </summary>
        protected bool isDestroyed;

        /// <summary>
        /// Clears <see cref="Source"/>.
        /// </summary>
        public virtual void ClearSource()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Source = default;
        }

        /// <summary>
        /// Clears <see cref="Target"/>.
        /// </summary>
        public virtual void ClearTarget()
        {
            if (!this.IsValidState())
            {
                return;
            }

            Target = default;
        }

        /// <summary>
        /// Destroys <see cref="this"/> from the scene.
        /// </summary>
        public virtual void Destroy()
        {
            isDestroyed = true;
            Destroy(this);
        }

        protected virtual void OnDisable()
        {
            if (Source != null && !isDestroyed)
            {
                Source.StopCollision(Target);
            }
        }
    }
}