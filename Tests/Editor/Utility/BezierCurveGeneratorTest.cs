using VRTK.Core.Utility;

namespace Test.VRTK.Core.Utility
{
    using UnityEngine;
    using NUnit.Framework;

    public class BezierCurveGeneratorTest
    {
        [Test]
        public void GeneratePoints()
        {
            int curvePoints = 10;
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

            Vector3[] actualResults = BezierCurveGenerator.GeneratePoints(curvePoints, controlPoints);

            for (int i = 0; i < actualResults.Length; i++)
            {
                Assert.AreEqual(expectedResults[i].ToString(), actualResults[i].ToString(), "index " + i);
            }
        }
    }
}