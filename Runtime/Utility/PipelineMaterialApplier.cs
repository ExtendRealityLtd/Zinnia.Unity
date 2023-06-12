namespace Zinnia.Utility
{
    using System;
    using System.Text.RegularExpressions;
    using UnityEngine;
#if UNITY_2019_1_OR_NEWER
    using UnityEngine.Rendering;
#endif

    [ExecuteInEditMode]
    /// <summary>
    /// Applies the relevant <see cref="Material"/> collection to the specified <see cref="Renderer"/> based on the render pipeline being used by the project.
    /// </summary>
    /// <remarks>
    /// This is automatically run in the Unity editor when the script is awoken in the editor window. It does not run automatically during play mode.
    /// </remarks>
    public class PipelineMaterialApplier : MonoBehaviour
    {
        [Serializable]
        public class PipelineMaterials
        {
            [Tooltip("The name of the pipeline to match the Material collection to.")]
            [SerializeField]
            private string pipelineName = "";
            /// <summary>
            /// The name of the pipeline to match the <see cref="Material"/> collection to.
            /// </summary>
            public string PipelineName
            {
                get
                {
                    return pipelineName;
                }
                set
                {
                    pipelineName = value;
                }
            }
            [Tooltip("The name of the pipeline shader to match the Material collection to.")]
            [SerializeField]
            private string pipelineShaderName = "";
            /// <summary>
            /// The name of the pipeline shader to match the <see cref="Material"/> collection to.
            /// </summary>
            public string PipelineShaderName
            {
                get
                {
                    return pipelineShaderName;
                }
                set
                {
                    pipelineShaderName = value;
                }
            }

            [Tooltip("The Material collection to set the Renderer materials to if using this render pipeline.")]
            [SerializeField]
            private Material[] materials = new Material[0];
            /// <summary>
            /// The <see cref="Material"/> collection to set the <see cref="Renderer"/> materials to if using this render pipeline.
            /// </summary>
            public Material[] Materials
            {
                get
                {
                    return materials;
                }
                set
                {
                    materials = value;
                }
            }
        }

        [Tooltip("The Renderer to update the materials on.")]
        [SerializeField]
        private Renderer target;
        /// <summary>
        /// The <see cref="Renderer"/> to update the materials on.
        /// </summary>
        public Renderer Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        [Tooltip("The collection of PipelineMaterials to apply if the current element Pipeline Name matches the currently used render pipeline.")]
        [SerializeField]
        private PipelineMaterials[] pipelines = new PipelineMaterials[0];
        /// <summary>
        /// The collection of <see cref="PipelineMaterials"/> to apply if the current element Pipeline Name matches the currently used render pipeline.
        /// </summary>
        public PipelineMaterials[] Pipelines
        {
            get
            {
                return pipelines;
            }
            set
            {
                pipelines = value;
            }
        }

        /// <summary>
        /// Applies the relevant pipeline <see cref="Material"/> collection to the <see cref="Target"/>.
        /// </summary>
        public virtual void ApplyMaterialsToRenderer()
        {
            if (Target == null)
            {
                return;
            }

#if UNITY_2019_1_OR_NEWER && !ZINNIA_IGNORE_PIPELINE_MATERIALS
            foreach (PipelineMaterials pipeline in Pipelines)
            {
                string nameMatch = "default";
                string shaderMatch = "default";

                if (GraphicsSettings.currentRenderPipeline != null)
                {
                    nameMatch = GraphicsSettings.currentRenderPipeline.name;
                    shaderMatch = GraphicsSettings.currentRenderPipeline.defaultShader.ToString();
                }

                bool nameCheck = !string.IsNullOrEmpty(pipeline.PipelineName) && Regex.IsMatch(nameMatch, pipeline.PipelineName);
                bool shaderCheck = !string.IsNullOrEmpty(pipeline.PipelineShaderName) && Regex.IsMatch(shaderMatch, pipeline.PipelineShaderName);

                if (nameCheck || shaderCheck)
                {
                    Target.sharedMaterials = pipeline.Materials;
                    break;
                }
            }
#endif
        }

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            ApplyMaterialsToRenderer();
#endif
        }
    }
}