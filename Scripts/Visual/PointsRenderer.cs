namespace VRTK.Core.Visual
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VRTK.Core.Extension;

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
            [Tooltip("Represents the start, i.e. the first rendered point.")]
            public GameObject start;
            /// <summary>
            /// Represents the segments between <see cref="start"/> and <see cref="end"/>. This will get cloned to create all the segments.
            /// </summary>
            [Tooltip("Represents the segments between start and end. This will get cloned to create all the segments.")]
            public GameObject repeatedSegment;
            /// <summary>
            /// Represents the end, i.e. the last rendered point.
            /// </summary>
            [Tooltip("Represents the end, i.e. the last rendered point.")]
            public GameObject end;
            /// <summary>
            /// The points along the the most recent cast.
            /// </summary>
            public IReadOnlyList<Vector3> points;
        }

        /// <summary>
        /// The direction to scale the segment <see cref="GameObject"/>s in. Set axes to 0 to disable scaling on that axis.
        /// </summary>
        [Tooltip("The direction to scale the segment GameObjects in. Set axes to 0 to disable scaling on that axis.")]
        public Vector3 segmentScaleDirection = Vector3.forward;
        /// <summary>
        /// Represents the start, i.e. the first rendered point.
        /// </summary>
        [Tooltip("Represents the start, i.e. the first rendered point.")]
        public GameObject start;
        /// <summary>
        /// Represents the segments between <see cref="start"/> and <see cref="end"/>. This will get cloned to create all the segments.
        /// </summary>
        [Tooltip("Represents the segments between start and end. This will get cloned to create all the segments.")]
        public GameObject repeatedSegment;
        /// <summary>
        /// Represents the end, i.e. the last rendered point.
        /// </summary>
        [Tooltip("Represents the end, i.e. the last rendered point.")]
        public GameObject end;

        protected readonly List<GameObject> segmentClones = new List<GameObject>();

        /// <summary>
        /// Renders the given points.
        /// </summary>
        /// <param name="data">The data to render.</param>
        public virtual void RenderData(PointsData data)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            start = data.start;
            end = data.end;

            if (repeatedSegment != data.repeatedSegment)
            {
                segmentClones.ForEach(Destroy);
                segmentClones.Clear();

                repeatedSegment = data.repeatedSegment;
            }

            UpdateNumberOfClones(data.points);

            UpdateElement(data.points, 0, false, start);
            UpdateElement(data.points, data.points.Count - 1, false, end);

            for (int index = 0; index < segmentClones.Count; index++)
            {
                UpdateElement(data.points, index, true, segmentClones[index]);
            }
        }

        protected virtual void OnDisable()
        {
            segmentClones.ForEach(Destroy);
            segmentClones.Clear();

            foreach (GameObject @object in new[]
            {
                start,
                repeatedSegment,
                end
            }.Where(@object => @object != null))
            {
                @object.SetActive(false);
            }
        }

        /// <summary>
        /// Ensures the number of cloned elements matches the provided number of points.
        /// </summary>
        /// <param name="points">The points to create cloned elements for.</param>
        protected virtual void UpdateNumberOfClones(IReadOnlyList<Vector3> points)
        {
            if (repeatedSegment == null)
            {
                return;
            }

            int targetCount = (points.Count > 0 ? points.Count - 1 : 0);
            for (int index = segmentClones.Count - 1; index >= targetCount; index--)
            {
                Destroy(segmentClones[index]);
                segmentClones.RemoveAt(index);
            }

            for (int index = segmentClones.Count; index < targetCount; index++)
            {
                segmentClones.Add(Instantiate(repeatedSegment, repeatedSegment.transform.parent));
            }
        }

        /// <summary>
        /// Updates the element for a specific point.
        /// </summary>
        /// <param name="points">All points to render.</param>
        /// <param name="pointsIndex">The index of the point that is represented by the element.</param>
        /// <param name="isPartOfLine">Whether the element is part of the line or alternatively it's representing a point.</param>
        /// <param name="object">The <see cref="GameObject"/> to use for rendering.</param>
        protected virtual void UpdateElement(IReadOnlyList<Vector3> points, int pointsIndex, bool isPartOfLine, GameObject @object)
        {
            if (@object == null || 0 > pointsIndex || pointsIndex >= points.Count)
            {
                return;
            }

            Vector3 targetPoint = points[pointsIndex];
            Vector3 otherPoint = (pointsIndex + 1 < points.Count ? points[pointsIndex + 1] : points[pointsIndex - 1]);
            Vector3 forward = otherPoint - targetPoint;
            Vector3 position = isPartOfLine ? targetPoint + 0.5f * forward : targetPoint;
            float scaleTarget = Mathf.Abs(Vector3.Distance(targetPoint, otherPoint));

            @object.transform.position = position;

            if (!isPartOfLine)
            {
                return;
            }

            @object.transform.forward = forward;

            Vector3 scale = @object.transform.lossyScale;

            for (int index = 0; index < 3; index++)
            {
                if (Math.Abs(segmentScaleDirection[index]) >= float.Epsilon)
                {
                    scale[index] = segmentScaleDirection[index] * scaleTarget;
                }
            }

            @object.transform.SetGlobalScale(scale);
        }
    }
}