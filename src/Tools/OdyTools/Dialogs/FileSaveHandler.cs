using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource;
using FileResource = BioWare.Extract.FileResource;
using OdyTools.Utils;

namespace OdyTools.Dialogs
{
    public class FileSaveHandler
    {
        private List<FileResource> _resources;

        public FileSaveHandler(List<FileResource> resources)
        {
            _resources = resources ?? new List<FileResource>();
        }

        public Dictionary<FileResource, string> SaveFiles(Dictionary<FileResource, string> pathsToWrite = null, Window parentForErrors = null)
        {
            var successfullySavedPaths = new Dictionary<FileResource, string>();
            var failedExtractions = new Dictionary<string, Exception>();

            if (pathsToWrite == null)
            {
                pathsToWrite = BuildPathsToWrite();
            }

            foreach (var kvp in pathsToWrite)
            {
                try
                {
                    byte[] data = kvp.Key.GetData();
                    AtomicFileWriter.WriteAtomic(kvp.Value, data, new AtomicWriteOptions());
                    successfullySavedPaths[kvp.Key] = kvp.Value;
                }
                catch (Exception ex)
                {
                    failedExtractions[kvp.Value] = ex;
                }
            }

            if (failedExtractions.Count > 0)
            {
                HandleFailedExtractions(failedExtractions, parentForErrors);
            }

            return successfullySavedPaths;
        }

        /// <summary>
        /// Prompts for save path(s) via file/folder picker, then writes files. Shows error message box on failures when parent is provided.
        /// </summary>
        public async Task<Dictionary<FileResource, string>> SaveFilesWithPromptAsync(Window parent)
        {
            if (parent?.StorageProvider == null || _resources.Count == 0)
            {
                return new Dictionary<FileResource, string>();
            }

            Dictionary<FileResource, string> pathsToWrite = await BuildPathsToWriteAsync(parent);
            if (pathsToWrite == null || pathsToWrite.Count == 0)
            {
                return new Dictionary<FileResource, string>();
            }

            return SaveFiles(pathsToWrite, parent);
        }

        // Returns default paths without UI. Use BuildPathsToWriteAsync(parent) to prompt the user.
        private Dictionary<FileResource, string> BuildPathsToWrite()
        {
            var pathsToWrite = new Dictionary<FileResource, string>();

            if (_resources.Count == 1)
            {
                var resource = _resources[0];
                string defaultPath = BuildResourceFileName(resource);
                pathsToWrite[resource] = defaultPath;
            }
            else if (_resources.Count > 1)
            {
                string folderPath = Path.GetTempPath();
                return BuildPathsForFolder(folderPath);
            }

            return pathsToWrite;
        }

        private async Task<Dictionary<FileResource, string>> BuildPathsToWriteAsync(Window parent)
        {
            var storage = parent.StorageProvider;
            if (storage == null) return null;

            if (_resources.Count == 1)
            {
                var resource = _resources[0];
                var options = new FilePickerSaveOptions
                {
                    Title = "Save resource",
                    SuggestedFileName = BuildResourceFileName(resource),
                    FileTypeChoices = new[] { new FilePickerFileType("Resource") { Patterns = new[] { "*.*" } }, new FilePickerFileType("All files") { Patterns = new[] { "*.*" } } }
                };
                var file = await storage.SaveFilePickerAsync(options);
                if (file == null) return null;
                string path = file.Path?.LocalPath ?? "";
                if (string.IsNullOrWhiteSpace(path)) return null;
                return new Dictionary<FileResource, string> { { resource, path } };
            }

            var folder = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = "Select folder to save resources" });
            if (folder == null || folder.Count == 0) return null;
            string folderPath = folder[0].Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(folderPath)) return null;

            return BuildPathsForFolder(folderPath);
        }

        private void HandleFailedExtractions(Dictionary<string, Exception> failedExtractions, Window parentForErrors)
        {
            string message = BuildFailedExtractionsMessage(failedExtractions);
            if (parentForErrors != null)
            {
                ShowSaveFailedMessage(parentForErrors, message);
            }
            else
            {
                LogFailedExtractions(failedExtractions);
            }
        }

        private Dictionary<FileResource, string> BuildPathsForFolder(string folderPath)
        {
            var paths = new Dictionary<FileResource, string>();
            foreach (var resource in _resources)
            {
                paths[resource] = Path.Combine(folderPath, BuildResourceFileName(resource));
            }

            return paths;
        }

        private static string BuildResourceFileName(FileResource resource)
        {
            return $"{resource.ResName}.{resource.ResType.Extension}";
        }

        private static string BuildFailedExtractionsMessage(Dictionary<string, Exception> failedExtractions)
        {
            return string.Join(Environment.NewLine, failedExtractions.Select(kvp => $"{kvp.Key}: {kvp.Value.Message}"));
        }

        private static void LogFailedExtractions(Dictionary<string, Exception> failedExtractions)
        {
            foreach (var kvp in failedExtractions)
            {
                System.Console.WriteLine($"Failed to save {kvp.Key}: {kvp.Value}");
            }
        }

        private static void ShowSaveFailedMessage(Window parentForErrors, string message)
        {
            DialogHelper.ShowWindow(
                parentForErrors,
                "Save failed",
                "Failed to save one or more files:" + Environment.NewLine + message,
                MsBox.Avalonia.Enums.Icon.Error);
        }
    }
}
