using Zinnia.Visual;

namespace Test.Zinnia.Visual
{
    using System.Collections.Generic;
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class PointsRendererTest
    {
        private GameObject containingObject;
        private PointsRenderer subject;

        private GameObject start;
        private GameObject segment;
        private GameObject end;
        private const string StartName = "TestStart";
        private const string SegmentName = "TestSegment";
        private const string EndName = "TestEnd";

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PointsRenderer>();

            start = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, StartName);
            segment = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, SegmentName);
            end = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, EndName);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);

            Object.DestroyImmediate(start);
            Object.DestroyImmediate(segment);
            Object.DestroyImmediate(end);
        }

        [Test]
        public void RenderData()
        {
            List<Vector3> points = new List<Vector3>
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 2f),
                new Vector3(0f, 2f, 3f),
                new Vector3(0f, 5f, 5f)
            };

            PointsRenderer.PointsData data = new PointsRenderer.PointsData
            {
                StartPoint = start,
                IsStartPointVisible = true,
                RepeatedSegmentPoint = segment,
                IsRepeatedSegmentPointVisible = true,
                EndPoint = end,
                IsEndPointVisible = true,
                Points = points
            };

            Vector3[] expectedSegmentPositions = new Vector3[]
            {
                new Vector3(0f, 3.5f, 4f),
                new Vector3(0f, 1.5f, 2.5f),
                new Vector3(0f, 0.5f, 1.5f),
                new Vector3(0f, 0f, 0.5f),
                new Vector3(0f, 0f, 0f)
            };

            Vector3[] expectedSegmentScale = new Vector3[]
            {
                new Vector3(0f, 0f, 3.6f),
                new Vector3(0f, 0f, 1.4f),
                new Vector3(0f, 0f, 1.4f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 0f, 0f)
            };

            subject.RenderData(data);

            Assert.AreEqual(points[0], GameObject.Find(StartName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(StartName).transform.localScale);

            int segmentIndex = 0;
            foreach (GameObject currentSegment in FindObjectsContainingName(SegmentName))
            {
                Assert.AreEqual(expectedSegmentPositions[segmentIndex].ToString(), currentSegment.transform.position.ToString(), "Position - Segment Index " + segmentIndex);
                Assert.AreEqual(expectedSegmentScale[segmentIndex].ToString(), currentSegment.transform.localScale.ToString(), "Scale - Segment Index " + segmentIndex);
                segmentIndex++;
            }
            Assert.AreEqual(points[4], GameObject.Find(EndName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(EndName).transform.localScale);
        }

        [Test]
        public void NoRenderDataOnDisabledComponent()
        {
            List<Vector3> points = new List<Vector3>
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 2f),
                new Vector3(0f, 2f, 3f),
                new Vector3(0f, 5f, 5f)
            };

            PointsRenderer.PointsData data = new PointsRenderer.PointsData
            {
                StartPoint = start,
                RepeatedSegmentPoint = segment,
                EndPoint = end,
                Points = points
            };

            Vector3[] expectedSegmentPositions = new Vector3[]
            {
                Vector3.zero,
                Vector3.zero,
                Vector3.zero,
                Vector3.zero,
                Vector3.zero
            };

            Vector3[] expectedSegmentScale = new Vector3[]
            {
                Vector3.zero,
                Vector3.zero,
                Vector3.zero,
                Vector3.zero,
                Vector3.zero
            };

            subject.enabled = false;
            subject.RenderData(data);

            Assert.AreEqual(Vector3.zero, GameObject.Find(StartName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(StartName).transform.localScale);

            int segmentIndex = 0;
            foreach (GameObject currentSegment in FindObjectsContainingName(SegmentName))
            {
                Assert.AreEqual(expectedSegmentPositions[segmentIndex].ToString(), currentSegment.transform.position.ToString(), "Position - Segment Index " + segmentIndex);
                Assert.AreEqual(expectedSegmentScale[segmentIndex].ToString(), currentSegment.transform.localScale.ToString(), "Scale - Segment Index " + segmentIndex);
                segmentIndex++;
            }
            Assert.AreEqual(Vector3.zero, GameObject.Find(EndName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(EndName).transform.localScale);
        }

        protected virtual GameObject CreatePrimitive(PrimitiveType type, Vector3 scale, string name = "")
        {
            GameObject newObject = GameObject.CreatePrimitive(type);
            newObject.transform.localScale = scale;
            if (name != "")
            {
                newObject.name = name;
            }
            return newObject;
        }

        protected virtual IEnumerable<GameObject> FindObjectsContainingName(string name)
        {
            foreach (GameObject foundObject in Object.FindObjectsOfType<GameObject>())
            {
                if (foundObject.name.Contains(name))
                {
                    yield return foundObject;
                }
            }
        }
    }
}