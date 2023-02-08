namespace Test.Zinnia.Utility.Helper
{
    using UnityEngine;

    public static class RaycastHitHelper
    {
        /// <summary>
        /// Creates a blocker <see cref="GameObject"/> at a default position.
        /// </summary>
        /// <returns>The blocker.</returns>
        public static GameObject CreateBlocker(string name)
        {
            GameObject blocker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blocker.name = name;
            blocker.transform.position = Vector3.forward * 2f;
            return blocker;
        }

        /// <summary>
        /// Generates dummy <see cref="RaycastHit"/> data.
        /// </summary>
        /// <param name="blocker">The <see cref="GameObject"/> to block the ray.</param>
        /// <param name="cleanUpBlocker">Whether to destroy the blocker after the raycast simulation.</param>
        /// <param name="rayOrigin">The origin point for the ray.</param>
        /// <param name="rayDirection">The direction for the ray to cast.</param>
        /// <returns>The valid hit data.</returns>
        public static RaycastHit GetRaycastHit(GameObject blocker = null, bool cleanUpBlocker = true, Vector3? rayOrigin = null, Vector3? rayDirection = null)
        {
            if (blocker == null)
            {
                blocker = CreateBlocker("RaycastHitHelper");
            }

            if (rayDirection == null)
            {
                rayDirection = Vector3.forward;
            }

#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.Script;
#else
            Physics.autoSimulation = false;
#endif
            Physics.Simulate(Time.fixedDeltaTime);
            Physics.Raycast(rayOrigin.GetValueOrDefault(), rayDirection.GetValueOrDefault(), out RaycastHit hitData);
#if UNITY_2022_2_OR_NEWER
            Physics.simulationMode = SimulationMode.FixedUpdate;
#else
            Physics.autoSimulation = true;
#endif
            if (cleanUpBlocker)
            {
                Object.Destroy(blocker);
            }

            return hitData;
        }
    }
}