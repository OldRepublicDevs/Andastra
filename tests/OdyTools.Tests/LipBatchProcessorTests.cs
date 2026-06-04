using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Resource.Formats.LIP;
using BioWare.Resource.Formats.WAV;
using NUnit.Framework;
using OdyTools.Utils;

namespace OdyTools.Tests
{
    [TestFixture]
    public class LipBatchProcessorTests
    {
        private static string CreateTempWav(float durationSeconds, int sampleRate = 44100)
        {
            int blockAlign = 2;
            int frameCount = (int)(durationSeconds * sampleRate);
            int dataSize = frameCount * blockAlign;
            byte[] wavBytes;
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
                    bw.Write((uint)(4 + 8 + 16 + 8 + dataSize));
                    bw.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 });
                    bw.Write(new byte[] { 0x66, 0x6D, 0x74, 0x20 });
                    bw.Write((uint)16);
                    bw.Write((ushort)1);
                    bw.Write((ushort)1);
                    bw.Write((uint)sampleRate);
                    bw.Write((uint)(sampleRate * blockAlign));
                    bw.Write((ushort)blockAlign);
                    bw.Write((ushort)16);
                    bw.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 });
                    bw.Write((uint)dataSize);
                    bw.Write(new byte[dataSize]);
                }
                wavBytes = ms.ToArray();
            }

            string path = Path.Combine(Path.GetTempPath(), "lipbatch_" + Guid.NewGuid().ToString("N") + ".wav");
            File.WriteAllBytes(path, wavBytes);
            return path;
        }

        [Test]
        public void GetWavDurationSeconds_ReturnsExpectedDuration()
        {
            string wavPath = CreateTempWav(2.0f);
            try
            {
                float duration = LipBatchProcessor.GetWavDurationSeconds(wavPath);
                Assert.That(duration, Is.EqualTo(2.0f).Within(0.01f));
            }
            finally
            {
                if (File.Exists(wavPath))
                {
                    File.Delete(wavPath);
                }
            }
        }

        [Test]
        public void CreatePlaceholderLip_UsesHolocronShapeSequence()
        {
            LIP lip = LipBatchProcessor.CreatePlaceholderLip(4.0f);
            Assert.That(lip.Length, Is.EqualTo(4.0f).Within(0.001f));
            Assert.That(lip.Frames.Count, Is.EqualTo(4));
            Assert.That(lip.Frames[0].Shape, Is.EqualTo(LIPShape.MPB));
            Assert.That(lip.Frames[1].Shape, Is.EqualTo(LIPShape.AH));
            Assert.That(lip.Frames[2].Shape, Is.EqualTo(LIPShape.OH));
            Assert.That(lip.Frames[3].Shape, Is.EqualTo(LIPShape.MPB));

            float interval = 4.0f / 5.0f;
            Assert.That(lip.Frames[0].Time, Is.EqualTo(interval * 1).Within(0.001f));
            Assert.That(lip.Frames[1].Time, Is.EqualTo(interval * 2).Within(0.001f));
            Assert.That(lip.Frames[2].Time, Is.EqualTo(interval * 3).Within(0.001f));
            Assert.That(lip.Frames[3].Time, Is.EqualTo(interval * 4).Within(0.001f));
        }

        [Test]
        public void CreatePlaceholderLip_ZeroDuration_ReturnsEmptyLip()
        {
            LIP lip = LipBatchProcessor.CreatePlaceholderLip(0f);
            Assert.That(lip.Frames.Count, Is.EqualTo(0));
        }

        [Test]
        public void ProcessFiles_WritesLipFilesForEachWav()
        {
            string outputDir = Path.Combine(Path.GetTempPath(), "lipbatch_out_" + Guid.NewGuid().ToString("N"));
            string wavOne = CreateTempWav(1.0f);
            string wavTwo = CreateTempWav(2.0f);
            try
            {
                var result = LipBatchProcessor.ProcessFiles(
                    new List<string> { wavOne, wavTwo },
                    outputDir);

                Assert.That(result.SuccessCount, Is.EqualTo(2));
                Assert.That(result.Files.Count, Is.EqualTo(2));

                string lipOne = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(wavOne) + ".lip");
                string lipTwo = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(wavTwo) + ".lip");
                Assert.That(File.Exists(lipOne), Is.True);
                Assert.That(File.Exists(lipTwo), Is.True);

                LIP loadedOne = LIPAuto.ReadLip(File.ReadAllBytes(lipOne));
                LIP loadedTwo = LIPAuto.ReadLip(File.ReadAllBytes(lipTwo));
                Assert.That(loadedOne.Length, Is.EqualTo(1.0f).Within(0.01f));
                Assert.That(loadedTwo.Length, Is.EqualTo(2.0f).Within(0.01f));
                Assert.That(loadedOne.Frames.Count, Is.EqualTo(4));
                Assert.That(loadedTwo.Frames.Count, Is.EqualTo(4));
            }
            finally
            {
                if (File.Exists(wavOne))
                {
                    File.Delete(wavOne);
                }

                if (File.Exists(wavTwo))
                {
                    File.Delete(wavTwo);
                }

                if (Directory.Exists(outputDir))
                {
                    Directory.Delete(outputDir, true);
                }
            }
        }

        [Test]
        public void ProcessFiles_MissingFile_RecordsErrorWithoutThrowing()
        {
            string outputDir = Path.Combine(Path.GetTempPath(), "lipbatch_out_" + Guid.NewGuid().ToString("N"));
            try
            {
                var result = LipBatchProcessor.ProcessFiles(
                    new List<string> { Path.Combine(outputDir, "missing.wav") },
                    outputDir);

                Assert.That(result.SuccessCount, Is.EqualTo(0));
                Assert.That(result.Files.Count, Is.EqualTo(1));
                Assert.That(result.Files[0].Success, Is.False);
                Assert.That(result.Files[0].Error, Is.Not.Null.And.Not.Empty);
            }
            finally
            {
                if (Directory.Exists(outputDir))
                {
                    Directory.Delete(outputDir, true);
                }
            }
        }
    }
}
