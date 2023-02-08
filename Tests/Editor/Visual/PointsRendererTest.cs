using Zinnia.Visual;

namespace Test.Zinnia.Visual
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

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
            containingObject = new GameObject("containingObject");
            subject = containingObject.AddComponent<PointsRenderer>();

            start = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, StartName);
            segment = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, SegmentName);
            end = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, EndName);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
            Object.DestroyImmediate(start);
            Object.DestroyImmediate(segment);
            Object.DestroyImmediate(end);
        }

        [Test]
        public void RenderData()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

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
                new Vector3(0.01f, 0.01f, 3.61f),
                new Vector3(0.01f, 0.01f, 1.41f),
                new Vector3(0.01f, 0.01f, 1.41f),
                new Vector3(0.01f, 0.01f, 1f),
                new Vector3(0.01f, 0.01f, 0.01f)
            };

            subject.RenderData(data);

            Assert.That(GameObject.Find(StartName).transform.position, Is.EqualTo(points[0]).Using(comparer));
            Assert.That(GameObject.Find(StartName).transform.localScale, Is.EqualTo(Vector3.one * 0.01f).Using(comparer));

            int segmentIndex = 0;
            foreach (GameObject currentSegment in FindObjectsContainingName(SegmentName))
            {
                Assert.That(currentSegment.transform.position, Is.EqualTo(expectedSegmentPositions[segmentIndex]).Using(comparer), "Position - Segment Index " + segmentIndex);
                Assert.That(currentSegment.transform.localScale, Is.EqualTo(expectedSegmentScale[segmentIndex]).Using(comparer), "Scale - Segment Index " + segmentIndex);
                segmentIndex++;
            }
            Assert.That(GameObject.Find(EndName).transform.position, Is.EqualTo(points[4]).Using(comparer));
            Assert.That(GameObject.Find(EndName).transform.localScale, Is.EqualTo(Vector3.one * 0.01f).Using(comparer));
        }

        [Test]
        public void NoRenderDataOnDisabledComponent()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);

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
                Vector3.one * 0.01f,
                Vector3.one * 0.01f,
                Vector3.one * 0.01f,
                Vector3.one * 0.01f,
                Vector3.one * 0.01f
            };

            subject.enabled = false;
            subject.RenderData(data);

            Assert.That(GameObject.Find(StartName).transform.position, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(GameObject.Find(StartName).transform.localScale, Is.EqualTo(Vector3.one * 0.01f).Using(comparer));

            int segmentIndex = 0;
            foreach (GameObject currentSegment in FindObjectsContainingName(SegmentName))
            {
                Assert.That(currentSegment.transform.position, Is.EqualTo(expectedSegmentPositions[segmentIndex]).Using(comparer), "Position - Segment Index " + segmentIndex);
                Assert.That(currentSegment.transform.localScale, Is.EqualTo(expectedSegmentScale[segmentIndex]).Using(comparer), "Scale - Segment Index " + segmentIndex);
                segmentIndex++;
            }

            Assert.That(GameObject.Find(EndName).transform.position, Is.EqualTo(Vector3.zero).Using(comparer));
            Assert.That(GameObject.Find(EndName).transform.localScale, Is.EqualTo(Vector3.one * 0.01f).Using(comparer));
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