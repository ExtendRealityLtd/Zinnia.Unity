namespace Zinnia.Extension
{
    using Zinnia.Rule;

    /// <summary>
    /// Extension methods for <see cref="RuleContainer"/>.
    /// </summary>
    public static class RuleContainerExtensions
    {
        /// <summary>
        /// Determines whether an object is accepted.
        /// </summary>
        /// <param name="container">The container of the <see cref="IRule"/> to check against.</param>
        /// <param name="target">The object to check.</param>
        /// <returns><see langword="true"/> if <paramref name="target"/> is accepted, <see langword="false"/> otherwise.</returns>
        public static bool Accepts(this RuleContainer container, object target)
        {
            return container?.Interface?.Accepts(target) != false;
        }
    }
}