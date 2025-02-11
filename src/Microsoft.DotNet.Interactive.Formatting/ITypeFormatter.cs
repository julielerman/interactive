﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.DotNet.Interactive.Formatting
{
    public interface ITypeFormatter
    {
        string MimeType { get; }

        Type Type { get; }

        bool Format(FormatContext context, object instance, TextWriter writer);
    }
}
