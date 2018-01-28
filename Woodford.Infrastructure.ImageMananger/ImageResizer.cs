using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;

namespace Woodford.Infrastructure.ImageMananger {


    public class ImageResizer : IImageSizer {
        private readonly ISettingService _settings;
        private const string _acceptedImageTypesNotSet = "Accepted Image types not set";
        private const string _imageSaveQualityNotSet = "Resize Image save quality not set";
        public ImageResizer(ISettingService settings) {
            _settings = settings;
        }

        private enum imageResizeTypeEnum {
            MaxLength = 0,
            MaxWidth = 1,
            MaxHeight = 2
        }

        public byte[] ImageResize(byte[] fileContent, int? width, int? height, int? crop, int? upscale) {

            MemoryStream msImage = new MemoryStream(fileContent);
            Image image = Image.FromStream(msImage);

            bool isUpscaleImage = ((upscale ?? 0) == 1);
            bool isCropImage = ((crop ?? 0) == 1);
            int imageSaveQuality = 0;

            try {
                imageSaveQuality = _settings.GetValue<int>(Setting.Resize_Image_Save_Quality);
            }
            catch (Exception) {
                throw new Exception(_imageSaveQualityNotSet);
            }


            if (isCropImage && width.HasValue && height.HasValue) {
                double cropAspectRatio = (double)width.Value / (double)height.Value;
                int cropX = 0;
                int cropY = 0;
                int cropWidth = image.Width;
                int cropHeight = (int)Math.Round(cropWidth / cropAspectRatio, 0);

                if (cropHeight > image.Height) {
                    cropHeight = image.Height;
                    cropWidth = (int)Math.Round(cropHeight * cropAspectRatio, 0);
                }

                cropX = ((image.Width - cropWidth) / 2);
                cropY = ((image.Height - cropHeight) / 2);

                image = cropImage(image, cropX, cropY, cropWidth, cropHeight, imageSaveQuality);
            }

            image = resizeImage(image, width, height, isUpscaleImage, imageSaveQuality);

            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);

            return ms.ToArray();


        }

        public bool IsImage(string extension) {
            string imageAcceptedFileTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            if (string.IsNullOrEmpty(imageAcceptedFileTypes))
                throw new Exception(_acceptedImageTypesNotSet);
            return imageAcceptedFileTypes.Contains(extension.ToLower());
        }

        private Image cropImage(Image image, int cropX, int cropY, int cropWidth, int cropHeight, int imageSaveQuality) {

            Image res;
            MemoryStream imageMemoryStream = null;

            using (Bitmap bitmap = new Bitmap(cropWidth, cropHeight)) {
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (Graphics graphic = Graphics.FromImage(bitmap)) {

                    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphic.DrawImage(image, new Rectangle(0, 0, cropWidth, cropHeight), cropX, cropY, cropWidth, cropHeight, GraphicsUnit.Pixel);

                    string extension = ".jpg"; // Path.GetExtension(originalFilePath);

                    using (EncoderParameters encoderParameters = new EncoderParameters(1)) {
                        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt64(imageSaveQuality));
                        imageMemoryStream = new MemoryStream();
                        bitmap.Save(imageMemoryStream, getImageCodec(extension), encoderParameters);
                        res = Image.FromStream(imageMemoryStream);
                    }
                }
            }

            return res;

        }

        private Image resizeImage(Image image, int? newWidth, int? newHeight, bool upscaleImage, int imageSaveQuality) {

            Image res;
            MemoryStream imageMemoryStream = null;

            int currentWidth = image.Width;
            int currentHeight = image.Height;

            bool resizeImage = upscaleImage;
            if (newWidth.HasValue && currentWidth > newWidth.Value) {
                resizeImage = true;
            }

            if (newHeight.HasValue && currentHeight > newHeight.Value) {
                resizeImage = true;
            }


            if (resizeImage) {

                //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                //from displaying as the thumbnail image later, in other words, we want a clean
                //resize, not a grainy one.
                image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                ratio = (float)currentWidth / (float)currentHeight;

                imageResizeTypeEnum resizeType = imageResizeTypeEnum.MaxLength;
                if (newWidth.HasValue && !newHeight.HasValue) {
                    resizeType = imageResizeTypeEnum.MaxWidth;
                } else {
                    if (newHeight.HasValue && !newWidth.HasValue) {
                        resizeType = imageResizeTypeEnum.MaxHeight;
                    }
                }

                switch (resizeType) {
                    case imageResizeTypeEnum.MaxLength:
                        if (currentWidth > currentHeight) {
                            //ratio = (float)width / (float)height;
                            currentWidth = newWidth.Value;
                            currentHeight = Convert.ToInt32(Math.Round((float)currentWidth / ratio));
                            if (newHeight.HasValue && currentHeight > newHeight.Value) {
                                currentHeight = newHeight.Value;
                                currentWidth = Convert.ToInt32(Math.Round((float)currentHeight * ratio));
                            }
                        } else {
                            //ratio = (float)height / (float)width;
                            currentHeight = newHeight.Value;
                            currentWidth = Convert.ToInt32(Math.Round((float)currentHeight * ratio));
                            if (newWidth.HasValue && currentWidth > newWidth.Value) {
                                currentWidth = newWidth.Value;
                                currentHeight = Convert.ToInt32(Math.Round((float)currentWidth / ratio));
                            }
                        }
                        break;

                    case imageResizeTypeEnum.MaxWidth:
                        currentWidth = newWidth.Value;
                        currentHeight = Convert.ToInt32(Math.Round((float)currentWidth / ratio));
                        break;

                    case imageResizeTypeEnum.MaxHeight:
                        currentHeight = newHeight.Value;
                        currentWidth = Convert.ToInt32(Math.Round((float)currentHeight * ratio));
                        break;
                }


                using (Bitmap bitmap = new Bitmap(currentWidth, currentHeight)) {
                    bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                    using (Graphics graphic = Graphics.FromImage(bitmap)) {

                        graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphic.DrawImage(image, 0, 0, currentWidth, currentHeight);

                        string extension = ".jpg"; // Path.GetExtension(originalFilePath);

                        using (EncoderParameters encoderParameters = new EncoderParameters(1)) {
                            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt64(imageSaveQuality));
                            imageMemoryStream = new MemoryStream();
                            bitmap.Save(imageMemoryStream, getImageCodec(extension), encoderParameters);
                            res = Image.FromStream(imageMemoryStream);
                        }
                    }
                }

                return res;

                //return the resized image
                //return image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            } else {
                //return the original resized image
                return image;
            }

        }

        private Image rotateImage(Image image, int rotation) {

            if (rotation == 90) {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            } else {
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }

            return image;

        }

        private static ImageCodecInfo getImageCodec(string extension) {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.FilenameExtension.Contains(extension)) {
                    return codec;
                }
            }
            return codecs[1];
        }

    }
}
