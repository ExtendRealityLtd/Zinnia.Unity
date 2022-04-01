namespace Zinnia.Visual
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Zinnia.Data.Type;
    using Zinnia.Extension;

    /// <summary>
    /// Renders points using <see cref="GameObject"/>s.
    /// </summary>
    public class PointsRenderer : MonoBehaviour
    {
        /// <summary>
        /// Contains all the data needed for a <see cref="PointsRenderer"/> to render.
        /// </summary>
        [Serializable]
        public class PointsData
        {
            [Tooltip("Represents the start, i.e. the first rendered point.")]
            [SerializeField]
            private GameObject startPoint;
            /// <summary>
            /// Represents the start, i.e. the first rendered point.
            /// </summary>
            public GameObject StartPoint
            {
                get
                {
                    return startPoint;
                }
                set
                {
                    startPoint = value;
                }
            }
            [Tooltip("Whether the first point should be visible.")]
            [SerializeField]
            private bool isStartPointVisible;
            /// <summary>
            /// Whether the first point should be visible.
            /// </summary>
            public bool IsStartPointVisible
            {
                get
                {
                    return isStartPointVisible;
                }
                set
                {
                    isStartPointVisible = value;
                }
            }
            [Tooltip("Represents the segments between Start and End. This will get cloned to create all the segments.")]
            [SerializeField]
            private GameObject repeatedSegmentPoint;
            /// <summary>
            /// Represents the segments between <see cref="Start"/> and <see cref="End"/>. This will get cloned to create all the segments.
            /// </summary>
            public GameObject RepeatedSegmentPoint
            {
                get
                {
                    return repeatedSegmentPoint;
                }
                set
                {
                    repeatedSegmentPoint = value;
                }
            }
            [Tooltip("Whether the repeated segment point(s) should be visible.")]
            [SerializeField]
            private bool isRepeatedSegmentPointVisible;
            /// <summary>
            /// Whether the repeated segment point(s) should be visible.
            /// </summary>
            public bool IsRepeatedSegmentPointVisible
            {
                get
                {
                    return isRepeatedSegmentPointVisible;
                }
                set
                {
                    isRepeatedSegmentPointVisible = value;
                }
            }
            [Tooltip("Represents the end, i.e. the last rendered point.")]
            [SerializeField]
            private GameObject endPoint;
            /// <summary>
            /// Represents the end, i.e. the last rendered point.
            /// </summary>
            public GameObject EndPoint
            {
                get
                {
                    return endPoint;
                }
                set
                {
                    endPoint = value;
                }
            }
            [Tooltip("Whether the end point should be visible.")]
            [SerializeField]
            private bool isEndPointVisible;
            /// <summary>
            /// Whether the end point should be visible.
            /// </summary>
            public bool IsEndPointVisible
            {
                get
                {
                    return isEndPointVisible;
                }
                set
                {
                    isEndPointVisible = value;
                }
            }
            [Tooltip("The points along the most recent cast.")]
            [SerializeField]
            private HeapAllocationFreeReadOnlyList<Vector3> points;
            /// <summary>
            /// The points along the most recent cast.
            /// </summary>
            public HeapAllocationFreeReadOnlyList<Vector3> Points
            {
                get
                {
                    return points;
                }
                set
                {
                    points = value;
                }
            }

            /// <inheritdoc />
            public override string ToString()
            {
                string[] titles = new string[]
                {
                "StartPoint",
                "IsStartPointVisible",
                "RepeatedSegmentPoint",
                "IsRepeatedSegmentPointVisible",
                "EndPoint",
                "IsEndPointVisible"
                };

                object[] values = new object[]
                {
                StartPoint,
                IsStartPointVisible,
                RepeatedSegmentPoint,
                IsRepeatedSegmentPointVisible,
                EndPoint,
                IsEndPointVisible
                };

                return StringExtensions.FormatForToString(titles, values);
            }
        }

        [Tooltip("The direction to scale the segment GameObjects in. Set axes to 0 to disable scaling on that axis.")]
        [SerializeField]
        private Vector3 segmentScaleDirection = Vector3.forward;
        /// <summary>
        /// The direction to scale the segment <see cref="GameObject"/>s in. Set axes to 0 to disable scaling on that axis.
        /// </summary>
        public Vector3 SegmentScaleDirection
        {
            get
            {
                return segmentScaleDirection;
            }
            set
            {
                segmentScaleDirection = value;
            }
        }
        [Tooltip("Represents the start, i.e. the first rendered point.")]
        [SerializeField]
        private GameObject start;
        /// <summary>
        /// Represents the start, i.e. the first rendered point.
        /// </summary>
        public GameObject Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }
        [Tooltip("Represents the segments between Start and End. This will get cloned to create all the segments.")]
        [SerializeField]
        private GameObject repeatedSegment;
        /// <summary>
        /// Represents the segments between <see cref="Start"/> and <see cref="End"/>. This will get cloned to create all the segments.
        /// </summary>
        public GameObject RepeatedSegment
        {
            get
            {
                return repeatedSegment;
            }
            set
            {
                repeatedSegment = value;
            }
        }
        [Tooltip("Represents the end, i.e. the last rendered point.")]
        [SerializeField]
        private GameObject end;
        /// <summary>
        /// Represents the end, i.e. the last rendered point.
        /// </summary>
        public GameObject End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        /// <summary>
        /// A collection of segment clones.
        /// </summary>
        protected readonly List<GameObject> segmentClones = new List<GameObject>();

        /// <summary>
        /// Sets the <see cref="SegmentScaleDirection"/> x value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSegmentScaleDirectionX(float value)
        {
            SegmentScaleDirection = new Vector3(value, SegmentScaleDirection.y, SegmentScaleDirection.z);
        }

        /// <summary>
        /// Sets the <see cref="SegmentScaleDirection"/> y value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSegmentScaleDirectionY(float value)
        {
            SegmentScaleDirection = new Vector3(SegmentScaleDirection.x, value, SegmentScaleDirection.z);
        }

        /// <summary>
        /// Sets the <see cref="SegmentScaleDirection"/> z value.
        /// </summary>
        /// <param name="value">The value to set to.</param>
        public virtual void SetSegmentScaleDirectionZ(float value)
        {
            SegmentScaleDirection = new Vector3(SegmentScaleDirection.x, SegmentScaleDirection.y, value);
        }

        /// <summary>
        /// Renders the given points.
        /// </summary>
        /// <param name="data">The data to render.</param>
        public virtual void RenderData(PointsData data)
        {
            if (!this.IsValidState())
            {
                return;
            }

            Start = data.StartPoint;
            End = data.EndPoint;

            if (RepeatedSegment != data.RepeatedSegmentPoint)
            {
                foreach (GameObject segmentClone in segmentClones)
                {
                    Destroy(segmentClone);
                }
                segmentClones.Clear();

                RepeatedSegment = data.RepeatedSegmentPoint;
            }

            UpdateNumberOfClones(data.Points, data.IsRepeatedSegmentPointVisible);

            UpdateElement(data.Points, 0, false, Start);
            UpdateElement(data.Points, data.Points.Count - 1, false, End);
            UpdateElement(data.Points, 0, true, RepeatedSegment);

            for (int index = 1; index <= segmentClones.Count; index++)
            {
                UpdateElement(data.Points, index, true, segmentClones[index - 1]);
            }
        }

        protected virtual void OnDisable()
        {
            foreach (GameObject segmentClone in segmentClones)
            {
                Destroy(segmentClone);
            }
            segmentClones.Clear();

            if (Start != null)
            {
                Start.SetActive(false);
            }

            if (RepeatedSegment != null)
            {
                RepeatedSegment.SetActive(false);
            }

            if (End != null)
            {
                End.SetActive(false);
            }
        }

        /// <summary>
        /// Ensures the number of cloned elements matches the provided number of points.
        /// </summary>
        /// <param name="points">The points to create cloned elements for.</param>
        /// <param name="isVisible" >Whether the points should be visible.</param>
        protected virtual void UpdateNumberOfClones(IReadOnlyList<Vector3> points, bool isVisible)
        {
            if (RepeatedSegment == null)
            {
                return;
            }

            int targetCount = points.Count > 0 ? points.Count - 1 : 0;
            for (int index = segmentClones.Count - 1; index >= targetCount; index--)
            {
                Destroy(segmentClones[index]);
                segmentClones.RemoveAt(index);
            }

            if (!isVisible)
            {
                return;
            }

            for (int index = segmentClones.Count; index < targetCount - 1; index++)
            {
                segmentClones.Add(Instantiate(RepeatedSegment, RepeatedSegment.transform.parent));
            }
        }

        /// <summary>
        /// Updates the element for a specific point.
        /// </summary>
        /// <param name="points">All points to render.</param>
        /// <param name="pointsIndex">The index of the point that is represented by the element.</param>
        /// <param name="isPartOfLine">Whether the element is part of the line or alternatively it's representing a point.</param>
        /// <param name="renderElement">The <see cref="GameObject"/> to use for rendering.</param>
        protected virtual void UpdateElement(IReadOnlyList<Vector3> points, int pointsIndex, bool isPartOfLine, GameObject renderElement)
        {
            if (renderElement == null || 0 > pointsIndex || pointsIndex >= points.Count)
            {
                return;
            }

            Vector3 targetPoint = points[pointsIndex];
            Vector3 otherPoint = pointsIndex + 1 < points.Count ? points[pointsIndex + 1] : points[pointsIndex - 1];
            Vector3 forward = otherPoint - targetPoint;
            Vector3 position = isPartOfLine ? targetPoint + 0.5f * forward : targetPoint;
            float scaleTarget = Mathf.Abs(Vector3.Distance(targetPoint, otherPoint));

            renderElement.transform.position = position;

            if (!isPartOfLine)
            {
                return;
            }

            renderElement.transform.forward = forward;

            Vector3 scale = renderElement.transform.lossyScale;

            for (int index = 0; index < 3; index++)
            {
                if (Math.Abs(SegmentScaleDirection[index]) >= float.Epsilon)
                {
                    scale[index] = SegmentScaleDirection[index] * scaleTarget;
                }
            }

            renderElement.transform.SetGlobalScale(scale);
        }
    }
}