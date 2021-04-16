namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using Core;

    /// <summary>
    ///     Exception when a list delimiter is not enclosed in brackets
    /// </summary>
    internal class ListDelimeterNotWithinBrackets : ParseException
    {
        public readonly StringSegment DelimeterStringSegment;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ListDelimeterNotWithinBrackets" /> class.
        /// </summary>
        /// <param name="delimeterStringSegment">The location where the delimeter was found</param>
        public ListDelimeterNotWithinBrackets(StringSegment delimeterStringSegment)
            : base(delimeterStringSegment, $"List delimeter '{delimeterStringSegment.Value}' is not within brackets")
        {
            DelimeterStringSegment = delimeterStringSegment;
        }
    }
}