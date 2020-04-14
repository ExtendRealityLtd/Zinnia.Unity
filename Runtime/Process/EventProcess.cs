namespace Zinnia.Process
{
    using Malimbe.XmlDocumentationAttribute;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Raises a <see cref="UnityEvent"/> when processed.
    /// </summary>
    public class EventProcess : MonoBehaviour, IProcessable
    {
        /// <summary>
        /// Emitted when processed.
        /// </summary>
        [DocumentedByXml]
        public UnityEvent Event = new UnityEvent();

        /// <summary>
        /// Emits <see cref="Event"/>.
        /// </summary>
        public void Process()
        {
            Event?.Invoke();
        }
    }
}
