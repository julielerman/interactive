﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text.Encodings.Web;
using static Microsoft.DotNet.Interactive.Formatting.PocketViewTags;

namespace Microsoft.DotNet.Interactive.Formatting
{
    public static class HtmlFormatter
    {
        static HtmlFormatter()
        {
            Formatter.Clearing += (obj, sender) =>
            {
                MaxProperties = DefaultMaxProperties;
            };
        }

        /// <summary>
        ///   Indicates the maximum number of properties to show in the default HTML display of arbitrary objects.
        ///   If set to zero no properties are shown.
        /// </summary>
        public static int MaxProperties { get; set; } = DefaultMaxProperties;

        internal const int DefaultMaxProperties = 20;

        public static ITypeFormatter GetPreferredFormatterFor(Type type) =>
            Formatter.GetPreferredFormatterFor(type, MimeType);

        public static ITypeFormatter GetPreferredFormatterFor<T>() =>
            GetPreferredFormatterFor(typeof(T));

        public const string MimeType = "text/html";

        internal static ITypeFormatter GetDefaultFormatterForAnyObject(Type type, bool includeInternals = false) =>
            FormattersForAnyObject.GetFormatter(type, includeInternals);

        internal static ITypeFormatter GetDefaultFormatterForAnyEnumerable(Type type) =>
            FormattersForAnyEnumerable.GetFormatter(type, false);

        internal static void FormatObjectAsPlainText(FormatContext context, object value, TextWriter writer)
        {
            using var swriter = Formatter.CreateWriter();
            value.FormatTo(context, swriter, PlainTextFormatter.MimeType);
            var text = swriter.ToString();
            FormatStringAsPlainText(text, writer);
        }

        internal static void FormatStringAsPlainText(string text, TextWriter writer)
        {
            if (!string.IsNullOrEmpty(text))
            {
                PocketView tag = div(text);
                tag.HtmlAttributes["class"] = "dni-plaintext";
                tag.WriteTo(writer, HtmlEncoder.Default);
            }
        }

        internal class EmbeddedFormat
        {
            internal FormatContext Context { get; }
            internal object Object { get; }
            internal EmbeddedFormat(FormatContext context, object instance)
                { Object = instance;  Context = context;  }
        }

        internal static FormatterTable FormattersForAnyObject =
            new(typeof(HtmlFormatter<>), nameof(HtmlFormatter<object>.CreateForAnyObject));

        internal static FormatterTable FormattersForAnyEnumerable =
            new(typeof(HtmlFormatter<>), nameof(HtmlFormatter<object>.CreateForAnyEnumerable));

    }
}