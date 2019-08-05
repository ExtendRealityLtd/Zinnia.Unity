using Zinnia.Data.Type;
using Zinnia.Utility;

namespace Test.Zinnia.Utility
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class BezierCurveGeneratorTest
    {
        [Test]
        public void GeneratePoints()
        {
            const int curvePoints = 10;
            Vector3 origin = Vector3.zero;
            Vector3 forward = Vector3.forward * 4;
            Vector3 down = (Vector3.forward * 4) + (Vector3.down * 2);
            Vector3[] controlPoints = new Vector3[]
            {
                origin,
                forward,
                down,
                down
            };

            Vector3[] expectedResults = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, -0.1f, 1.2f),
                new Vector3(0f, -0.3f, 2.1f),
                new Vector3(0f, -0.5f, 2.8f),
                new Vector3(0f, -0.8f, 3.3f),
                new Vector3(0f, -1.2f, 3.6f),
                new Vector3(0f, -1.5f, 3.9f),
                new Vector3(0f, -1.7f, 4f),
                new Vector3(0f, -1.9f, 4f),
                new Vector3(0f, -2f, 4f),
            };

            HeapAllocationFreeReadOnlyList<Vector3> actualResults = BezierCurveGenerator.GeneratePoints(curvePoints, controlPoints);

            for (int index = 0; index < actualResults.Count; index++)
            {
                Assert.AreEqual(expectedResults[index].ToString(), actualResults[index].ToString(), "index " + index);
            }
        }
    }
}