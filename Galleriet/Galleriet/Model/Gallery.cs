using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.IO;

namespace Galleriet.Model
{
    public class Gallery
    {
        // Kollar om filen har tillåten filändelse
        private static readonly Regex ApprovedExtensions;

        // Innehåller fysiska sökvägen
        private static string PhysicalUploadImagePath;

        // Kollar om filnamnet är okej
        private static readonly Regex SantizePath;

        // Konstruktor
        static Gallery()
        {
            ApprovedExtensions = new Regex("^.*.(gif|jpg|png)$", RegexOptions.IgnoreCase);

            PhysicalUploadImagePath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "Images");

            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SantizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        }

        //Returnerar en referens med bildernas filnamn i bokstavsordning                                                                             
        public IEnumerable<ThumbImage> GetImageNames()
        {
            var sortedFiles = new DirectoryInfo(Path.Combine(PhysicalUploadImagePath, "Thumbnails"));
            return (from fi in sortedFiles.GetFiles()
                    select new ThumbImage
                    {
                        Name = fi.Name,
                        ImgFileUrl = Path.Combine("/?img=", fi.Name),
                        ThumbImgUrl = Path.Combine("Images/Thumbnails/", fi.Name)
                    }).OrderBy(fi => fi.Name).ToList();
        }

        // Kollar om en bild redan finns
        public static bool ImageExists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadImagePath, name));
        }

        // Kollar om den uppladdade filen är rätt typ
        public static bool IsValidImage(Image image)
        {
            return image.RawFormat.Guid == ImageFormat.Gif.Guid ||
                image.RawFormat.Guid == ImageFormat.Jpeg.Guid ||
                image.RawFormat.Guid == ImageFormat.Png.Guid;
        }

        // kontrollerar/sparar bild och skapar/sparar en tumnagelbild
        public static string SaveImage(Stream stream, string fileName)
        {
            var image = System.Drawing.Image.FromStream(stream);

            if (!IsValidImage(image))
            {
                throw new ArgumentException("Bilden har fel MIME-typ");
            }

            fileName = SantizePath.Replace(fileName, "");

            if (ImageExists(fileName))//Om bilden finns lägger den på en (1)(2)... i slutet
            {
                int count = 0;
                string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);
                string path = Path.GetDirectoryName(fileName);

                while (ImageExists(fileName))
                {
                    fileName = string.Format("{0}({1}){2}", fileNameOnly, ++count, extension);
                }
            }

            // Skapar tumnagel
            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);

            // Sparar bild och tumnagel
            image.Save(Path.Combine(PhysicalUploadImagePath, fileName));
            thumbnail.Save(PhysicalUploadImagePath + "/Thumbnails/" + fileName);

            return fileName;
        }
    }
}