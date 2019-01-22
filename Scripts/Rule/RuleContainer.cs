namespace Zinnia.Rule
{
    using System;
    using Zinnia.Utility;

    /// <summary>
    /// A proxy class used to make a <see cref="IRule"/> interface appear in the Unity Inspector.
    /// </summary>
    [Serializable]
    public sealed class RuleContainer : InterfaceContainer<IRule>
    {
    }
}