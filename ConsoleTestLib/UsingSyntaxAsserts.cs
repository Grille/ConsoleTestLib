using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.ConsoleTestLib.Asserts;

namespace Grille.ConsoleTestLib;

/// <summary>Same methods as <see cref="Assert"/> intended for use with <c>using static</c>.</summary>
public static class UsingSyntaxAsserts
{
    public static void AssertThrows<T>(Action action) where T : Exception => Assert.Throws<T>(action);

    public static T AssertThrowsAndReturn<T>(Action action) where T : Exception => Assert.ThrowsAndReturn<T>(action);


    public static void AssertIsTrue(bool value, string message) => Assert.IsTrue(value, message);


    public static void AssertIsNotEqual<T>(T expected, T actual) => Assert.IsNotEqual(expected, actual);

    public static void AssertIsNotEqual<T>(T expected, T actual, string? message) => Assert.IsNotEqual(expected, actual, message);

    public static void AssertIsNotEqual<T>(T expected, T actual, Func<T, string>? toString) => Assert.IsNotEqual(expected, actual, null, toString);

    public static void AssertIsNotEqual<T>(T expected, T actual, string? message, Func<T, string>? toString) => Assert.IsNotEqual(expected, actual, message, toString);


    public static void AssertIsEqual<T>(T expected, T actual) => Assert.IsEqual(expected, actual);

    public static void AssertIsEqual<T>(T expected, T actual, string? message) => Assert.IsEqual(expected, actual, message);

    public static void AssertIsEqual<T>(T expected, T actual, Func<T, string>? toString) => Assert.IsEqual(expected, actual, null, toString);

    public static void AssertIsEqual<T>(T expected, T actual, string? message, Func<T, string>? toString) => Assert.IsEqual(expected, actual, message, toString);


    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual) => Assert.IListIsEqual(expected, actual);

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual, string message) => Assert.IListIsEqual(expected, actual, message);

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual, Func<IList<T>, string> toString) => Assert.IListIsEqual(expected, actual, null, toString);

    public static void AssertIListIsEqual<T>(IList<T> expected, IList<T> actual, string? message, Func<IList<T>, string> toString) => Assert.IListIsEqual(expected, actual, message, toString);

    public static void AssertIEnumerableIsEqual(IEnumerable expected, IEnumerable actual) => Assert.IEnumerableIsEqual(expected, actual);
}
