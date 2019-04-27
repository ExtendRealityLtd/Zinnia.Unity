namespace Zinnia.Visual
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
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
            /// <summary>
            /// Represents the start, i.e. the first rendered point.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public GameObject StartPoint { get; set; }
            /// <summary>
            /// Whether the first point should be visible.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public bool IsStartPointVisible { get; set; }
            /// <summary>
            /// Represents the segments between <see cref="Start"/> and <see cref="End"/>. This will get cloned to create all the segments.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public GameObject RepeatedSegmentPoint { get; set; }
            /// <summary>
            /// Whether the repeated segment point(s) should be visible.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public bool IsRepeatedSegmentPointVisible { get; set; }
            /// <summary>
            /// Represents the end, i.e. the last rendered point.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public GameObject EndPoint { get; set; }
            /// <summary>
            /// Whether the end point should be visible.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public bool IsEndPointVisible { get; set; }
            /// <summary>
            /// The points along the the most recent cast.
            /// </summary>
            [Serialized]
            [field: DocumentedByXml]
            public HeapAllocationFreeReadOnlyList<Vector3> Points { get; set; }
        }

        /// <summary>
        /// The direction to scale the segment <see cref="GameObject"/>s in. Set axes to 0 to disable scaling on that axis.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public Vector3 SegmentScaleDirection { get; set; } = Vector3.forward;
        /// <summary>
        /// Represents the start, i.e. the first rendered point.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObject Start { get; set; }
        /// <summary>
        /// Represents the segments between <see cref="Start"/> and <see cref="End"/>. This will get cloned to create all the segments.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObject RepeatedSegment { get; set; }
        /// <summary>
        /// Represents the end, i.e. the last rendered point.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public GameObject End { get; set; }

        /// <summary>
        /// A collection of segment clones.
        /// </summary>
        protected readonly List<GameObject> segmentClones = new List<GameObject>();

        /// <summary>
        /// Renders the given points.
        /// </summary>
        /// <param name="data">The data to render.</param>
        [RequiresBehaviourState]
        public virtual void RenderData(PointsData data)
        {
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