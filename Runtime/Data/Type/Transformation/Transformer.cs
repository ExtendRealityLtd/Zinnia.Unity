namespace Zinnia.Data.Type.Transformation
{
    using UnityEngine;
    using UnityEngine.Events;
    using Malimbe.BehaviourStateRequirementMethod;
    using Malimbe.XmlDocumentationAttribute;

    /// <summary>
    /// The basis for all type transformations.
    /// </summary>
    /// <typeparam name="TInput">The variable type that will be input into transformation.</typeparam>
    /// <typeparam name="TOutput">The variable type that will be output from the result of the transformation.</typeparam>
    /// <typeparam name="TEvent">The <see cref="UnityEvent"/> type the transformation will emit.</typeparam>
    public abstract class Transformer<TInput, TOutput, TEvent> : MonoBehaviour where TEvent : UnityEvent<TOutput>, new()
    {
        /// <summary>
        /// The result of the transformation.
        /// </summary>
        public TOutput Result { get; protected set; }

        /// <summary>
        /// Emitted when the transformation is complete.
        /// </summary>
        [DocumentedByXml]
        public TEvent Transformed = new TEvent();

        /// <summary>
        /// Transforms the given input into the relevant output.
        /// </summary>
        /// <param name="input">The input to transform.</param>
        /// <returns>The transformed input or the default of <see cref="TOutput"/> if the current component is not <see cref="Behaviour.isActiveAndEnabled"/>.</returns>
        [RequiresBehaviourState]
        public virtual TOutput Transform(TInput input)
        {
            return ProcessResult(input);
        }

        /// <summary>
        /// Transforms the given input into the relevant output.
        /// </summary>
        /// <param name="input">The input to transform.</param>
        public virtual void DoTransform(TInput input)
        {
            Transform(input);
        }

        /// <summary>
        /// The process that applies the transformation.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected abstract TOutput Process(TInput input);

        /// <summary>
        /// Processes the given input into the output result.
        /// </summary>
        /// <param name="input">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        protected virtual TOutput ProcessResult(TInput input)
        {
            Result = Process(input);
            EmitTransformed(Result);
            return Result;
        }

        /// <summary>
        /// Emits the output result.
        /// </summary>
        /// <param name="output">The result of the transformation to emit.</param>
        protected virtual void EmitTransformed(TOutput output)
        {
            Transformed?.Invoke(output);
        }
    }
}