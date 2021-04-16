namespace AlbedoTeam.Sdk.FilterLanguage.Languages.ExpressionLanguage
{
    using System;
    using System.Reflection;
    using Core.Utility;

    internal static class DateTimeMembers
    {
        public static MemberInfo Year = Type<DateTime>.Member(x => x.Year);

        public static MemberInfo Month = Type<DateTime>.Member(x => x.Month);

        public static MemberInfo Day = Type<DateTime>.Member(x => x.Day);

        public static MemberInfo Hour = Type<DateTime>.Member(x => x.Hour);

        public static MemberInfo Minute = Type<DateTime>.Member(x => x.Minute);

        public static MemberInfo Second = Type<DateTime>.Member(x => x.Second);
    }
}