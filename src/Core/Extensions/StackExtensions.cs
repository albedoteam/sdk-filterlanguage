namespace AlbedoTeam.Sdk.FilterLanguage.Core.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class StackExtensions
    {
        /// <summary>
        ///     Pops the stack while a predicate is found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack">The stack</param>
        /// <param name="predicate">The predicate for determining whether to pop an item</param>
        /// <returns>Enumerable over the popped items</returns>
        public static IEnumerable<T> PopWhile<T>(this Stack<T> stack, Func<T, bool> predicate)
        {
            while (stack.Count > 0 && predicate(stack.Peek()))
                yield return stack.Pop();
        }

        /// <summary>
        ///     Pops the stack while a predicate is found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack">The stack</param>
        /// <param name="predicate">The predicate for determining whether to pop an item</param>
        /// <returns>Enumerable over the popped items</returns>
        public static IEnumerable<T> PopWhile<T>(this Stack<T> stack, Func<T, int, bool> predicate)
        {
            var count = 0;
            while (stack.Count > 0 && predicate(stack.Peek(), count++))
                yield return stack.Pop();
        }
    }
}