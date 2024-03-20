using System;
using System.Drawing;

namespace PixelArt.Tools
{
    #region From Dithering Mischa
    public abstract class DitheringBase<T>
    {
        /// <summary>
        /// Width of bitmap
        /// </summary>
        protected int width;

        /// <summary>
        /// Height of bitmap
        /// </summary>
        protected int height;

        /// <summary>
        /// Long name of the dither method
        /// </summary>
        private readonly string methodLongName = "";

        /// <summary>
        /// Filename addition
        /// </summary>
        private readonly string fileNameAddition = "";

        /// <summary>
        /// Color reduction function/method
        /// </summary>
        protected ColorFunction colorFunction = null;

        /// <summary>
        /// Current bitmap
        /// </summary>
        private IImageFormat<T> currentBitmap;

        /// <summary>
        /// Color function for color reduction
        /// </summary>
        /// <param name="inputColors">Input colors</param>
        /// <param name="outputColors">Output colors</param>
        public delegate void ColorFunction(in T[] inputColors, ref T[] outputColors, ColorApproximater colorApproximater = null);

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="colorfunc">Color reduction function/method</param>
        /// <param name="longName">Long name of dither method</param>
        /// <param name="fileNameAdd">Filename addition</param>
        public DitheringBase(ColorFunction colorfunc, string longName, string fileNameAdd)
        {
            this.colorFunction = colorfunc;
            this.methodLongName = longName;
            this.fileNameAddition = fileNameAdd;
        }

        /// <summary>
        /// Do dithering for chosen image with chosen color reduction method. Work horse, call this when you want to dither something
        /// </summary>
        /// <param name="input">Input image</param>
        /// <returns>Dithered image</returns>
        public IImageFormat<T> DoDithering(IImageFormat<T> input)
        {

            this.width = input.GetWidth();
            this.height = input.GetHeight();
            int channelsPerPixel = input.GetChannelsPerPixel();
            this.currentBitmap = input;

            T[] originalPixel = new T[channelsPerPixel];
            T[] newPixel = new T[channelsPerPixel];
            this.tempBuffer = new T[channelsPerPixel];
            double[] quantError = new double[channelsPerPixel];

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    input.GetPixelChannels(x, y, ref originalPixel);
                    this.colorFunction(in originalPixel, ref newPixel);

                    input.SetPixelChannels(x, y, newPixel);

                    input.GetQuantErrorsPerChannel(in originalPixel, in newPixel, ref quantError);

                    this.PushError(x, y, quantError);
                }
            }

            return input;
        }

        /// <summary>
        /// Get dither method name
        /// </summary>
        /// <returns>String method name</returns>
        public string GetMethodName()
        {
            return this.methodLongName;
        }

        /// <summary>
        /// Get filename addition
        /// </summary>
        /// <returns></returns>
        public string GetFilenameAddition()
        {
            return this.fileNameAddition;
        }

        /// <summary>
        /// Check if image coordinate is valid
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if valid; False otherwise</returns>
        protected bool IsValidCoordinate(int x, int y)
        {
            return (0 <= x && x < this.width && 0 <= y && y < this.height);
        }

        /// <summary>
        /// How error cumulation should be handled. Implement this for every dithering method
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="quantError">Quantization error</param>
        protected abstract void PushError(int x, int y, double[] quantError);

        private T[] tempBuffer = null;

        /// <summary>
        /// Modify image with error and multiplier
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="quantError">Quantization error</param>
        /// <param name="multiplier">Multiplier</param>
        public void ModifyImageWithErrorAndMultiplier(int x, int y, double[] quantError, double multiplier)
        {
            this.currentBitmap.GetPixelChannels(x, y, ref this.tempBuffer);

            // We limit the color here because we don't want the value go over min or max
            this.currentBitmap.ModifyPixelChannelsWithQuantError(ref this.tempBuffer, quantError, multiplier);

            this.currentBitmap.SetPixelChannels(x, y, this.tempBuffer);
        }
    }
    public sealed class AtkinsonDitheringRGBByte : DitheringBase<byte>
    {
        /// <summary>
        /// Constructor for Atkinson dithering
        /// </summary>
        /// <param name="colorfunc">Color function</param>
        public AtkinsonDitheringRGBByte(ColorFunction colorfunc) : base(colorfunc, "Atkinson", "_ATK")
        {

        }

        /// <summary>
        /// Push error method for Atkinson dithering
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="quantError">Quantization error</param>
        override protected void PushError(int x, int y, double[] quantError)
        {
            // Push error
            //        X    1/8   1/8 
            // 1/8   1/8   1/8
            //       1/8

            int xMinusOne = x - 1;
            int xPlusOne = x + 1;
            int xPlusTwo = x + 2;
            int yPlusOne = y + 1;
            int yPlusTwo = y + 2;

            double multiplier = 1.0 / 8.0; // Atkinson Dithering has same multiplier for every item

            // Current row
            int currentRow = y;
            if (this.IsValidCoordinate(xPlusOne, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, multiplier);
            }

            if (this.IsValidCoordinate(xPlusTwo, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, multiplier);
            }

            // Next row
            currentRow = yPlusOne;
            if (this.IsValidCoordinate(xMinusOne, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, multiplier);
            }

            if (this.IsValidCoordinate(x, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, multiplier);
            }

            if (this.IsValidCoordinate(xPlusOne, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, multiplier);
            }

            // Next row
            currentRow = yPlusTwo;
            if (this.IsValidCoordinate(x, currentRow))
            {
                this.ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, multiplier);
            }
        }
    }
    public interface IImageFormat<T>
    {
        /// <summary>
        /// Get width
        /// </summary>
        /// <returns>Width of image</returns>
        int GetWidth();

        /// <summary>
        /// Get height
        /// </summary>
        /// <returns>Height of image</returns>
        int GetHeight();

        /// <summary>
        /// Get channels per pixel
        /// </summary>
        /// <returns>Channels per pixel</returns>
        int GetChannelsPerPixel();

        /// <summary>
        /// Get raw content as array
        /// </summary>
        /// <returns>Array</returns>
        T[] GetRawContent();

        /// <summary>
        /// Set pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="newValues">New values</param>
        void SetPixelChannels(int x, int y, T[] newValues);

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Values as array</returns>
        T[] GetPixelChannels(int x, int y);

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="pixelStorage">Array where pixel channels values will be written</param>
        void GetPixelChannels(int x, int y, ref T[] pixelStorage);

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <returns>Error values as double array</returns>
        double[] GetQuantErrorsPerChannel(T[] originalPixel, T[] newPixel);

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <param name="errorValues">Error values as double array</param>
        void GetQuantErrorsPerChannel(in T[] originalPixel, in T[] newPixel, ref double[] errorValues);

        /// <summary>
        /// Modify existing values with quantization errors
        /// </summary>
        /// <param name="modifyValues">Values to modify</param>
        /// <param name="quantErrors">Quantization errors</param>
        /// <param name="multiplier">Multiplier</param>
        void ModifyPixelChannelsWithQuantError(ref T[] modifyValues, double[] quantErrors, double multiplier);
    }
    public sealed class TempByteImageFormat : IImageFormat<byte>
    {
        /// <summary>
        /// Width of bitmap
        /// </summary>
        public readonly int width;

        /// <summary>
        /// Height of bitmap
        /// </summary>
        public readonly int height;

        private readonly byte[,,] content3d;

        private readonly byte[] content1d;

        /// <summary>
        /// How many color channels per pixel
        /// </summary>
        public readonly int channelsPerPixel;

        /// <summary>
        /// Constructor for temp byte image format
        /// </summary>
        /// <param name="input">Input bitmap as three dimensional (widht, height, channels per pixel) byte array</param>
        /// <param name="createCopy">True if you want to create copy of data</param>
        public TempByteImageFormat(byte[,,] input, bool createCopy = false)
        {
            if (createCopy)
            {
                this.content3d = (byte[,,])input.Clone();
            }
            else
            {
                this.content3d = input;
            }

            this.content1d = null;
            this.width = input.GetLength(0);
            this.height = input.GetLength(1);
            this.channelsPerPixel = input.GetLength(2);
        }

        /// <summary>
        /// Constructor for temp byte image format
        /// </summary>
        /// <param name="input">Input byte array</param>
        /// <param name="imageWidth">Width</param>
        /// <param name="imageHeight">Height</param>
        /// <param name="imageChannelsPerPixel">Image channels per pixel</param>
        /// <param name="createCopy">True if you want to create copy of data</param>
        public TempByteImageFormat(byte[] input, int imageWidth, int imageHeight, int imageChannelsPerPixel, bool createCopy = false)
        {
            this.content3d = null;
            if (createCopy)
            {
                this.content1d = new byte[input.Length];
                Buffer.BlockCopy(input, 0, this.content1d, 0, input.Length);
            }
            else
            {
                this.content1d = input;
            }
            this.width = imageWidth;
            this.height = imageHeight;
            this.channelsPerPixel = imageChannelsPerPixel;
        }

        /// <summary>
        /// Constructor for temp byte image format
        /// </summary>
        /// <param name="input">Existing TempByteImageFormat</param>
        public TempByteImageFormat(TempByteImageFormat input)
        {
            if (input.content1d != null)
            {
                this.content1d = input.content1d;
                this.content3d = null;
            }
            else
            {
                this.content3d = input.content3d;
                this.content1d = null;
            }

            this.width = input.width;
            this.height = input.height;
            this.channelsPerPixel = input.channelsPerPixel;
        }

        /// <summary>
        /// Get width of bitmap
        /// </summary>
        /// <returns>Width in pixels</returns>
        public int GetWidth()
        {
            return this.width;
        }

        /// <summary>
        /// Get height of bitmap
        /// </summary>
        /// <returns>Height in pixels</returns>
        public int GetHeight()
        {
            return this.height;
        }

        /// <summary>
        /// Get channels per pixel
        /// </summary>
        /// <returns>Channels per pixel</returns>
        public int GetChannelsPerPixel()
        {
            return this.channelsPerPixel;
        }

        /// <summary>
        /// Get raw content as byte array
        /// </summary>
        /// <returns>Byte array</returns>
        public byte[] GetRawContent()
        {
            if (this.content1d != null)
            {
                return this.content1d;
            }
            else
            {
                byte[] returnArray = new byte[this.width * this.height * this.channelsPerPixel];
                int currentIndex = 0;
                for (int y = 0; y < this.height; y++)
                {
                    for (int x = 0; x < this.width; x++)
                    {
                        for (int i = 0; i < this.channelsPerPixel; i++)
                        {
                            returnArray[currentIndex] = this.content3d[x, y, i];
                            currentIndex++;
                        }
                    }
                }

                return returnArray;
            }
        }

        /// <summary>
        /// Set pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="newValues">New values as object array</param>
        public void SetPixelChannels(int x, int y, byte[] newValues)
        {
            if (this.content1d != null)
            {
                int indexBase = y * this.width * this.channelsPerPixel + x * this.channelsPerPixel;
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    this.content1d[indexBase + i] = newValues[i];
                }
            }
            else
            {
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    this.content3d[x, y, i] = newValues[i];
                }
            }
        }

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Values as byte array</returns>
        public byte[] GetPixelChannels(int x, int y)
        {
            byte[] returnArray = new byte[this.channelsPerPixel];

            if (this.content1d != null)
            {
                int indexBase = y * this.width * this.channelsPerPixel + x * this.channelsPerPixel;
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    returnArray[i] = this.content1d[indexBase + i];
                }
            }
            else
            {
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    returnArray[i] = this.content3d[x, y, i];
                }
            }

            return returnArray;
        }

        /// <summary>
        /// Get pixel channels of certain coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="pixelStorage">Array where pixel channels values will be written</param>
        public void GetPixelChannels(int x, int y, ref byte[] pixelStorage)
        {
            if (this.content1d != null)
            {
                int indexBase = y * this.width * this.channelsPerPixel + x * this.channelsPerPixel;
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    pixelStorage[i] = this.content1d[indexBase + i];
                }
            }
            else
            {
                for (int i = 0; i < this.channelsPerPixel; i++)
                {
                    pixelStorage[i] = this.content3d[x, y, i];
                }
            }
        }

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <returns>Error values as object array</returns>
        public double[] GetQuantErrorsPerChannel(byte[] originalPixel, byte[] newPixel)
        {
            double[] returnValue = new double[this.channelsPerPixel];

            for (int i = 0; i < this.channelsPerPixel; i++)
            {
                returnValue[i] = originalPixel[i] - newPixel[i];
            }

            return returnValue;
        }

        /// <summary>
        /// Get quantization errors per channel
        /// </summary>
        /// <param name="originalPixel">Original pixels</param>
        /// <param name="newPixel">New pixels</param>
        /// <param name="errorValues">Error values as double array</param>
        public void GetQuantErrorsPerChannel(in byte[] originalPixel, in byte[] newPixel, ref double[] errorValues)
        {
            for (int i = 0; i < this.channelsPerPixel; i++)
            {
                errorValues[i] = originalPixel[i] - newPixel[i];
            }
        }

        /// <summary>
        /// Modify existing values with quantization errors
        /// </summary>
        /// <param name="modifyValues">Values to modify</param>
        /// <param name="quantErrors">Quantization errors</param>
        /// <param name="multiplier">Multiplier</param>
        public void ModifyPixelChannelsWithQuantError(ref byte[] modifyValues, double[] quantErrors, double multiplier)
        {
            for (int i = 0; i < this.channelsPerPixel; i++)
            {
                modifyValues[i] = GetLimitedValue((byte)modifyValues[i], quantErrors[i] * multiplier);
            }
        }

        private static byte GetLimitedValue(byte original, double error)
        {
            double newValue = original + error;
            return Clamp(newValue, byte.MinValue, byte.MaxValue);
        }

        // C# doesn't have a Clamp method so we need this
        private static byte Clamp(double value, double min, double max)
        {
            return (value < min) ? (byte)min : (value > max) ? (byte)max : (byte)value;
        }
    }
    #endregion

    public class AtkinsonDithering
    {
        private static ColorApproximater _approximater;
        private static int ColorFunctionMode = 1;
        private static void ColorFunction(in byte[] input, ref byte[] output, ColorApproximater approximater)
        {

            switch (ColorFunctionMode)
            {
                case 0:
                    TrueColorBytesToWebSafeColorBytes(input, ref output);
                    break;
                default:
                    TrueColorBytesToPalette(input, ref output);
                    break;
            }
        }
        private static void TrueColorBytesToWebSafeColorBytes(in byte[] input, ref byte[] output)
        {
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (byte)(Math.Round(input[i] / 51.0) * 51);
            }
        }
        private static void TrueColorBytesToPalette(in byte[] input, ref byte[] output)
        {
            output = _approximater.Convert((input[0], input[1], input[2]));
            //output = new byte[] { i.R, i.G, i.B};
        }




        public static Bitmap Do(Bitmap image, ColorApproximater approximater)
        {
            _approximater = approximater;
            //approximater = new ColorApproximater(new Color[] { Color.White, Color.Black, Color.AliceBlue });
            AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(ColorFunction);
            byte[,,] bytes = ReadBitmapToColorBytes(image);

            TempByteImageFormat temp = new TempByteImageFormat(bytes);
            atkinson.DoDithering(temp);

            WriteToBitmap(image, temp.GetPixelChannels);

            return image;
        }
        /*
        public static Bitmap Do(Mat img, ColorApproximater approximater)
        {
            Bitmap image = img.ToBitmap();
            _approximater = approximater;
            //approximater = new ColorApproximater(new Color[] { Color.White, Color.Black, Color.AliceBlue, Color.Red, Color.Yellow });
            AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(ColorFunction);

            byte[,,] bytes = ReadBitmapToColorBytes(image);

            TempByteImageFormat temp = new TempByteImageFormat(bytes);
            atkinson.DoDithering(temp);

            WriteToBitmap(image, temp.GetPixelChannels);

            return image;
        }
        */ // openCV

        private static byte[,,] ReadBitmapToColorBytes(Bitmap bitmap)
        {
            byte[,,] returnValue = new byte[bitmap.Width, bitmap.Height, 3];
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    returnValue[x, y, 0] = color.R;
                    returnValue[x, y, 1] = color.G;
                    returnValue[x, y, 2] = color.B;
                }
            }
            return returnValue;
        }

        private static void WriteToBitmap(Bitmap bitmap, Func<int, int, byte[]> reader)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte[] read = reader(x, y);
                    Color color = Color.FromArgb(read[0], read[1], read[2]);
                    bitmap.SetPixel(x, y, color);
                }
            }
        }
    }
}

