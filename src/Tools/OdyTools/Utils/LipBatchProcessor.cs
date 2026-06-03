using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Resource.Formats.LIP;
using BioWare.Resource.Formats.WAV;

namespace OdyTools.Utils
{
    /// <summary>
    /// Batch WAV → LIP conversion matching Holocron BatchLIPProcessor.
    /// Reference: vendor/src/toolset/gui/editors/lip/batch_processor.py
    /// </summary>
    public static class LipBatchProcessor
    {
        private static readonly LIPShape[] DefaultShapes = new[]
        {
            LIPShape.MPB,
            LIPShape.AH,
            LIPShape.OH,
            LIPShape.MPB,
        };

        public sealed class LipBatchFileResult
        {
            public string InputPath { get; set; }
            public string OutputPath { get; set; }
            public string Error { get; set; }
            public bool Success { get { return string.IsNullOrEmpty(Error); } }
        }

        public sealed class LipBatchProcessResult
        {
            public List<LipBatchFileResult> Files { get; } = new List<LipBatchFileResult>();

            public int SuccessCount
            {
                get
                {
                    int count = 0;
                    foreach (LipBatchFileResult file in Files)
                    {
                        if (file.Success)
                        {
                            count++;
                        }
                    }

                    return count;
                }
            }
        }

        public static float GetWavDurationSeconds(string wavPath)
        {
            if (string.IsNullOrWhiteSpace(wavPath))
            {
                throw new ArgumentException("WAV path is required.", "wavPath");
            }

            if (!File.Exists(wavPath))
            {
                throw new FileNotFoundException("WAV file not found.", wavPath);
            }

            WAV wav = WAVAuto.ReadWav(wavPath);
            if (wav == null || wav.SampleRate <= 0)
            {
                throw new InvalidDataException("WAV file has invalid sample rate: " + wavPath);
            }

            int blockAlign = wav.BlockAlign;
            if (blockAlign <= 0)
            {
                blockAlign = wav.Channels * (wav.BitsPerSample / 8);
            }

            if (blockAlign <= 0)
            {
                throw new InvalidDataException("WAV file has invalid block alignment: " + wavPath);
            }

            int frameCount = wav.Data != null ? wav.Data.Length / blockAlign : 0;
            return frameCount / (float)wav.SampleRate;
        }

        public static LIP CreatePlaceholderLip(float duration)
        {
            LIP lip = new LIP();
            if (duration <= 0f)
            {
                return lip;
            }

            lip.Length = duration;
            float interval = duration / (DefaultShapes.Length + 1);
            for (int i = 0; i < DefaultShapes.Length; i++)
            {
                lip.Add(interval * (i + 1), DefaultShapes[i]);
            }

            return lip;
        }

        public static byte[] CreateLipBytesFromWav(string wavPath)
        {
            float duration = GetWavDurationSeconds(wavPath);
            LIP lip = CreatePlaceholderLip(duration);
            return LIPAuto.BytesLip(lip);
        }

        public static LipBatchProcessResult ProcessFiles(IEnumerable<string> audioFiles, string outputDirectory)
        {
            LipBatchProcessResult batchResult = new LipBatchProcessResult();
            if (audioFiles == null)
            {
                return batchResult;
            }

            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                throw new ArgumentException("Output directory is required.", "outputDirectory");
            }

            Directory.CreateDirectory(outputDirectory);

            foreach (string audioFile in audioFiles)
            {
                LipBatchFileResult fileResult = new LipBatchFileResult
                {
                    InputPath = audioFile,
                };

                try
                {
                    if (string.IsNullOrWhiteSpace(audioFile) || !File.Exists(audioFile))
                    {
                        throw new FileNotFoundException("Audio file not found.", audioFile);
                    }

                    string stem = Path.GetFileNameWithoutExtension(audioFile);
                    string outputPath = Path.Combine(outputDirectory, stem + ".lip");
                    byte[] lipBytes = CreateLipBytesFromWav(audioFile);
                    File.WriteAllBytes(outputPath, lipBytes);
                    fileResult.OutputPath = outputPath;
                }
                catch (Exception ex)
                {
                    fileResult.Error = ex.Message;
                }

                batchResult.Files.Add(fileResult);
            }

            return batchResult;
        }
    }
}
