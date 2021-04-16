namespace AlbedoTeam.Sdk.FilterLanguage.Languages.ExpressionLanguage
{
    using System.Reflection;
    using Core.Utility;

    internal static class StringMembers
    {
        public static MethodInfo StartsWith = Type<string>.Method(x => x.StartsWith(default(string)));

        public static MethodInfo EndsWith = Type<string>.Method(x => x.EndsWith(default(string)));

        public static MethodInfo Contains = Type<string>.Method(x => x.Contains(default(string)));

        public static MethodInfo ToLower = Type<string>.Method(x => x.ToLower());

        public static MethodInfo ToUpper = Type<string>.Method(x => x.ToUpper());
    }
}