namespace Zinnia.Extension
{
    using System;

    /// <summary>
    /// Extended methods for the <see cref="string"/> Type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the given data for a ToString method return type.
        /// </summary>
        /// <param name="titles">The titles of the data to return.</param>
        /// <param name="values">The values of the data to return.</param>
        /// <returns>The formatted data for use in a ToString method.</returns>
        public static string FormatForToString(string[] titles, object[] values, string baseData = null)
        {
            if (titles.Length != values.Length)
            {
                throw new ArgumentException(string.Format("titles length is {0} and does not match values length of {1}", titles.Length, values.Length));
            }

            int dataLength = baseData != null ? titles.Length + 1 : titles.Length;
            int startIndex = baseData != null ? 1 : 0;

            string[] data = new string[dataLength];

            if (baseData != null)
            {
                data[0] = baseData.Remove(baseData.Length - 2).Remove(0, 2);
            }

            for (int index = startIndex; index < dataLength; index++)
            {
                object value = values[index - startIndex];
                data[index] = string.Join(" = ", titles[index - startIndex], value != null ? value.ToString() : "[null]");
            }

            return "{ " + string.Join(" | ", data) + " }";
        }
    }
}