namespace Zinnia.Process.Moment
{
    using Malimbe.PropertySerializationAttribute;
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using Zinnia.Process.Moment.Collection;

    /// <summary>
    /// Processes a list of <see cref="MomentProcess"/> as if it's a single <see cref="IProcessable"/> process.
    /// </summary>
    public class CompositeProcess : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// A collection of <see cref="MomentProcess"/> to process.
        /// </summary>
        [Serialized]
        [field: DocumentedByXml]
        public MomentProcessObservableList Processes { get; set; }

        /// <summary>
        /// Iterates through <see cref="Processes"/> and calls <see cref="MomentProcess.Process"/> on each one.
        /// </summary>
        public void Process()
        {
            if (Processes == null)
            {
                return;
            }

            foreach (MomentProcess currentProcess in Processes.NonSubscribableElements)
            {
                currentProcess.Process();
            }
        }
    }
}
