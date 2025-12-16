using System;
using System.IO;

namespace Andastra.Formats.Tests.Common
{

    internal static class TestFileHelper
    {
        public static string GetPath(string relativePath) =>
            Path.Combine(AppContext.BaseDirectory, "test_files", relativePath);
    }
}

