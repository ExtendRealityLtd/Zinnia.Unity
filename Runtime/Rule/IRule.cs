namespace Zinnia.Rule
{
    /// <summary>
    /// Allows determining whether an object is accepted.
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// Determines whether an object is accepted.
        /// </summary>
        /// <param name="target">The object to check.</param>
        /// <returns><see langword="true"/> if <paramref name="target"/> is accepted, <see langword="false"/> otherwise.</returns>
        bool Accepts(object target);
    }
}