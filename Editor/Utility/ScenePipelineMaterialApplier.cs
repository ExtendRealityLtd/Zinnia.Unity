namespace Zinnia.Utility
{
#if UNITY_2020_1_OR_NEWER
    using UnityEditor;
#endif
    using UnityEngine;

    public class ScenePipelineMaterialApplier : ScriptableObject
    {
#if UNITY_2020_1_OR_NEWER
        [MenuItem("Window/Zinnia/Apply Pipeline Materials", false, 51)]
        private static void AddSpatialTargetsDispatcher()
        {
            foreach (PipelineMaterialApplier current in FindObjectsOfType<PipelineMaterialApplier>(true))
            {
                current.ApplyMaterialsToRenderer();
            }
        }
#endif
    }
}