using System;
using System.Collections.Generic;
using System.IO;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Content.ResourceProviders;
using Andastra.Runtime.Core.Save;
using BioWare.Extract;

namespace Andastra.Game.Games.Odyssey.Save
{
    /// <summary>
    /// ISaveDataProvider implementation that uses OdysseySaveGameManager for KOTOR save format.
    /// Enables SaveSystem to produce full SaveGameData and persist via Odyssey ERF format.
    /// Reva: 0x004eb750 save, 0x00708990 load.
    /// </summary>
    public sealed class OdysseySaveDataProvider : ISaveDataProvider
    {
        private readonly IGameResourceProvider _resourceProvider;
        private readonly string _savesDirectory;
        private readonly OdysseySaveGameManager _manager;

        public OdysseySaveDataProvider(IGameResourceProvider resourceProvider, string savesDirectory)
        {
            _resourceProvider = resourceProvider ?? throw new ArgumentNullException(nameof(resourceProvider));
            _savesDirectory = savesDirectory ?? throw new ArgumentNullException(nameof(savesDirectory));
            _manager = new OdysseySaveGameManager(resourceProvider, savesDirectory);
        }

        public bool WriteSave(SaveGameData saveData)
        {
            if (saveData == null || string.IsNullOrEmpty(saveData.Name))
                return false;
            return _manager.SaveGameAsync(saveData, saveData.Name).GetAwaiter().GetResult();
        }

        public SaveGameData ReadSave(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
                return null;
            return _manager.LoadGameAsync(saveName).GetAwaiter().GetResult();
        }

        public IEnumerable<SaveGameInfo> EnumerateSaves()
        {
            return _manager.ListSaves();
        }

        public bool DeleteSave(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
                return false;
            try
            {
                string dirPath = Path.Combine(_savesDirectory, saveName);
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, recursive: true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OdysseySaveDataProvider] DeleteSave error: {ex.Message}");
            }
            return false;
        }

        public bool SaveExists(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
                return false;
            string saveFilePath = Path.Combine(_savesDirectory, saveName, "savegame.sav");
            return File.Exists(saveFilePath);
        }
    }
}
