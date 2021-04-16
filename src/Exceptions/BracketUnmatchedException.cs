namespace AlbedoTeam.Sdk.FilterLanguage.Exceptions
{
    using Core;

    /// <summary>
    ///     Exception when a bracket does not have a match
    /// </summary>
    internal class BracketUnmatchedException : ParseException
    {
        public readonly StringSegment BracketStringSegment;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BracketUnmatchedException" /> class.
        /// </summary>
        /// <param name="bracketStringSegment">The string segment that contains the bracket that is unmatched</param>
        public BracketUnmatchedException(StringSegment bracketStringSegment)
            : base(bracketStringSegment, $"Bracket '{bracketStringSegment.Value}' is unmatched")
        {
            BracketStringSegment = bracketStringSegment;
        }
    }
}