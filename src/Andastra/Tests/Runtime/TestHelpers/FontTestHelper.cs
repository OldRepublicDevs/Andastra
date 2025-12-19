using System;
using System.Collections.Generic;
using Andastra.Parsing.Formats.TPC;
using Andastra.Parsing.Formats.TXI;

namespace Andastra.Tests.Runtime.TestHelpers
{
    /// <summary>
    /// Helper class for creating test TPC and TXI data for font testing.
    /// </summary>
    public static class FontTestHelper
    {
        /// <summary>
        /// Creates a minimal test TPC texture for font testing.
        /// </summary>
        public static TPC CreateTestTPC(int width = 256, int height = 256, TPCTextureFormat format = TPCTextureFormat.RGBA)
        {
            var tpc = new TPC();
            tpc._format = format;
            
            var layer = new TPCLayer();
            int bytesPerPixel = format.BytesPerPixel();
            int dataSize = width * height * bytesPerPixel;
            byte[] textureData = new byte[dataSize];
            
            // Fill with a simple pattern (white texture)
            for (int i = 0; i < textureData.Length; i += bytesPerPixel)
            {
                if (format == TPCTextureFormat.RGBA || format == TPCTextureFormat.BGRA)
                {
                    textureData[i] = 255;     // R
                    textureData[i + 1] = 255; // G
                    textureData[i + 2] = 255; // B
                    textureData[i + 3] = 255; // A
                }
                else if (format == TPCTextureFormat.RGB || format == TPCTextureFormat.BGR)
                {
                    textureData[i] = 255;     // R
                    textureData[i + 1] = 255; // G
                    textureData[i + 2] = 255; // B
                }
                else if (format == TPCTextureFormat.Greyscale)
                {
                    textureData[i] = 255; // Grayscale
                }
            }
            
            var mipmap = new TPCMipmap(width, height, format, textureData);
            layer.Mipmaps.Add(mipmap);
            tpc.Layers.Add(layer);
            
            return tpc;
        }

        /// <summary>
        /// Creates a test TXI with font metrics.
        /// </summary>
        public static TXI CreateTestTXI(
            float fontHeight = 16.0f,
            float fontWidth = 8.0f,
            float baselineHeight = 12.0f,
            float spacingR = 1.0f,
            float spacingB = 2.0f,
            int cols = 16,
            int rows = 16,
            float textureWidth = 256.0f)
        {
            var txi = new TXI();
            txi.Features.Fontheight = fontHeight;
            txi.Features.Fontwidth = fontWidth;
            txi.Features.Baselineheight = baselineHeight;
            txi.Features.SpacingR = spacingR;
            txi.Features.SpacingB = spacingB;
            txi.Features.Cols = cols;
            txi.Features.Rows = rows;
            txi.Features.Texturewidth = textureWidth;
            
            // Create upperleftcoords and lowerrightcoords for a simple grid
            txi.Features.Upperleftcoords = new List<Tuple<float, float, int>>();
            txi.Features.Lowerrightcoords = new List<Tuple<float, float, int>>();
            
            int numChars = cols * rows;
            float cellWidth = textureWidth / cols;
            float cellHeight = (textureWidth / cols) * (rows > 0 ? (float)rows / cols : 1.0f);
            
            for (int i = 0; i < numChars; i++)
            {
                int col = i % cols;
                int row = i / cols;
                float x1 = col * cellWidth;
                float y1 = row * cellHeight;
                float x2 = x1 + cellWidth;
                float y2 = y1 + cellHeight;
                
                txi.Features.Upperleftcoords.Add(Tuple.Create(x1, y1, i));
                txi.Features.Lowerrightcoords.Add(Tuple.Create(x2, y2, i));
            }
            
            return txi;
        }

        /// <summary>
        /// Creates test TPC data as byte array (simplified TGA format).
        /// </summary>
        public static byte[] CreateTestTPCData(int width = 256, int height = 256)
        {
            // Create a minimal TGA header + data
            // TGA header: 18 bytes
            // For simplicity, we'll create RGBA data
            int dataSize = width * height * 4;
            byte[] tgaData = new byte[18 + dataSize];
            
            // TGA header (simplified)
            tgaData[0] = 0; // ID length
            tgaData[1] = 0; // Color map type
            tgaData[2] = 2; // Image type (uncompressed RGB)
            tgaData[12] = (byte)(width & 0xFF);
            tgaData[13] = (byte)((width >> 8) & 0xFF);
            tgaData[14] = (byte)(height & 0xFF);
            tgaData[15] = (byte)((height >> 8) & 0xFF);
            tgaData[16] = 32; // Bits per pixel (RGBA)
            tgaData[17] = 8; // Image descriptor
            
            // Fill with white texture data
            for (int i = 18; i < tgaData.Length; i += 4)
            {
                tgaData[i] = 255;     // B
                tgaData[i + 1] = 255; // G
                tgaData[i + 2] = 255; // R
                tgaData[i + 3] = 255; // A
            }
            
            return tgaData;
        }

        /// <summary>
        /// Creates test TXI data as string.
        /// </summary>
        public static string CreateTestTXIData(
            float fontHeight = 16.0f,
            float fontWidth = 8.0f,
            float baselineHeight = 12.0f,
            float spacingR = 1.0f,
            float spacingB = 2.0f,
            int cols = 16,
            int rows = 16,
            float textureWidth = 256.0f)
        {
            var lines = new List<string>
            {
                $"fontheight {fontHeight}",
                $"fontwidth {fontWidth}",
                $"baselineheight {baselineHeight}",
                $"spacingr {spacingR}",
                $"spacingb {spacingB}",
                $"cols {cols}",
                $"rows {rows}",
                $"texturewidth {textureWidth}",
                $"numchars {cols * rows}",
                "upperleftcoords",
            };
            
            int numChars = cols * rows;
            float cellWidth = textureWidth / cols;
            float cellHeight = (textureWidth / cols) * (rows > 0 ? (float)rows / cols : 1.0f);
            
            for (int i = 0; i < numChars; i++)
            {
                int col = i % cols;
                int row = i / cols;
                float x1 = col * cellWidth;
                float y1 = row * cellHeight;
                lines.Add($"{x1} {y1} {i}");
            }
            
            lines.Add("lowerrightcoords");
            for (int i = 0; i < numChars; i++)
            {
                int col = i % cols;
                int row = i / cols;
                float x1 = col * cellWidth;
                float y1 = row * cellHeight;
                float x2 = x1 + cellWidth;
                float y2 = y1 + cellHeight;
                lines.Add($"{x2} {y2} {i}");
            }
            
            return string.Join("\n", lines);
        }
    }
}

