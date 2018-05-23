namespace VRTK.Core.Visual
{
    using UnityEngine;
    using NUnit.Framework;
    using System.Linq;

    public class PointsRendererTest
    {
        private GameObject containingObject;
        private PointsRenderer subject;

        private GameObject start;
        private GameObject segment;
        private GameObject end;
        private string startName = "TestStart";
        private string segmentName = "TestSegment";
        private string endName = "TestEnd";

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<PointsRenderer>();

            start = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, startName);
            segment = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, segmentName);
            end = CreatePrimitive(PrimitiveType.Cube, Vector3.one * 0.01f, endName);
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
            Vector3[] points = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 2f),
                new Vector3(0f, 2f, 3f),
                new Vector3(0f, 5f, 5f)
            };

            PointsRendererData data = new PointsRendererData()
            {
                start = start,
                repeatedSegment = segment,
                end = end,
                points = points
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

            Assert.AreEqual(points[0], GameObject.Find(startName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(startName).transform.localScale);

            int segmentIndex = 0;
            foreach (GameObject currentSegment in FindObjectsContainingName(segmentName))
            {
                Assert.AreEqual(expectedSegmentPositions[segmentIndex].ToString(), currentSegment.transform.position.ToString(), "Position - Segment Index " + segmentIndex);
                Assert.AreEqual(expectedSegmentScale[segmentIndex].ToString(), currentSegment.transform.localScale.ToString(), "Scale - Segment Index " + segmentIndex);
                segmentIndex++;
            }
            Assert.AreEqual(points[4], GameObject.Find(endName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(endName).transform.localScale);
        }

        [Test]
        public void NoRenderDataOnDisabledComponent()
        {
            Vector3[] points = new Vector3[]
           {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, 1f),
                new Vector3(0f, 1f, 2f),
                new Vector3(0f, 2f, 3f),
                new Vector3(0f, 5f, 5f)
           };

            PointsRendererData data = new PointsRendererData()
            {
                start = start,
                repeatedSegment = segment,
                end = end,
                points = points
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

            Assert.AreEqual(Vector3.zero, GameObject.Find(startName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(startName).transform.localScale);

            int segmentIndex = 0;
            foreach (GameObject currentSegment in FindObjectsContainingName(segmentName))
            {
                Assert.AreEqual(expectedSegmentPositions[segmentIndex].ToString(), currentSegment.transform.position.ToString(), "Position - Segment Index " + segmentIndex);
                Assert.AreEqual(expectedSegmentScale[segmentIndex].ToString(), currentSegment.transform.localScale.ToString(), "Scale - Segment Index " + segmentIndex);
                segmentIndex++;
            }
            Assert.AreEqual(Vector3.zero, GameObject.Find(endName).transform.position);
            Assert.AreEqual(Vector3.one * 0.01f, GameObject.Find(endName).transform.localScale);
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

        protected virtual GameObject[] FindObjectsContainingName(string name)
        {
            return Object.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains(name)).ToArray<GameObject>();
        }
    }
}