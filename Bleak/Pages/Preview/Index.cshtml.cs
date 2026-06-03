using Bleak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Drawing.IconLib;
using System.Drawing.Imaging;
using System.Text;

namespace Bleak.Pages.Preview
{
    public class IndexModel : PageModel
    {
        private readonly Actions _actions;
        public IndexModel(Actions actions)
        {
            _actions = actions;
        }

        private Bitmap DownSize(MemoryStream dataStream, double ratio = 1)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromStream(dataStream);
            bitmap = new Bitmap(bitmap, new Size((int)(300*ratio), (int)(300*ratio)));
            return bitmap;
        }

        private MemoryStream ReduceQuality(Bitmap bitmap, long quality=20L)
        {
            var ms = new MemoryStream();
            var encoder = ImageCodecInfo.GetImageEncoders()
                .First(x => x.CodecName == "Built-in JPEG Codec");
            bitmap.Save(ms, encoder, new EncoderParameters(1)
            {
                Param = [new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality)]
            });
            ms.Position = 0;
            return ms;
        }

        private IActionResult PdfPreview(Item item)
        {
            var ms = new MemoryStream();
            PDFtoImage.Conversion.SaveJpeg(ms, item.Data, 1);
            ms.Position = 0;
            var ms2 = ReduceQuality(DownSize(ms, 1.5), 90L);
            return new FileStreamResult(ms2, "image/jpg");
        }

        private string GetExtension(string filename)
        {
            var ext = filename.Split(".").Last();
            return ext.Trim();
        }

        private IActionResult DefaultImage()
        {
            var ms = new MemoryStream();
            Bitmap bmp = new(100, 100);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawLine(Pens.White, new Point(0, 1), new Point(100, 1));
            g.DrawLine(Pens.White, new Point(0, 99), new Point(100, 99));
            var s = "NO PREVIEW";
            var f = new Font("Arial", 11);
            var measurements = g.MeasureString(s, f);
            g.DrawString(s, f, Brushes.Red, new PointF(50-measurements.Width/2, 50-measurements.Height/2));
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    if (pixel.R < 10)
                        bmp.SetPixel(x, y, Color.FromArgb(0, 255, 255, 255));
                    else
                        bmp.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
                }
            bmp.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
            return new FileStreamResult(ReduceQuality(DownSize(ms)), "image/jpg");
        }

        private IActionResult ImagePreview(Item item)
        {
            var dataStream = new MemoryStream(item.Data ?? []);
            var bitmap = DownSize(dataStream);
            var ms = ReduceQuality(bitmap);
            return new FileStreamResult(ms, "image/jpg");
        }

        private IActionResult TextPreview(Item item)
        {
            var text = Encoding.Latin1.GetString(item.Data ?? []);
            text = string.Join("", text.Take(255-3)).Trim() + "...";
            var img = new Bitmap(620, 480);
            
            Graphics dr = Graphics.FromImage(img);
            var brush = new SolidBrush(Color.FromArgb(1, Color.White));
            dr.DrawString(text, new Font("Arial", 20), Brushes.White, new PointF(10, 10));
            
            var dataStream = new MemoryStream();
            img.Save(dataStream, ImageFormat.Jpeg);
            dataStream.Position = 0;
            var ms = ReduceQuality(DownSize(dataStream));
            return new FileStreamResult(ms, "image/jpg");
        }

        private IActionResult RawPreview(Item item)
        {

            if (GetExtension(item.Filename) != "exe")
                return DefaultImage();

            var multiIcon = new MultiIcon();
            var dataStream = new MemoryStream(item.Data ?? []);
            multiIcon.Load(dataStream);
            var icon = multiIcon[0];
            var ms = new MemoryStream();
            icon.Save(ms);
            ms.Position = 0;
            var ms2 = ReduceQuality(DownSize(ms));
            return new FileStreamResult(ms2, "image/jpg");
        }

        public IActionResult OnGet(string id)
        {
            var hasToLogin = _actions.GetConfig()?.AnonymousViewing ?? false;
            if (hasToLogin && !(User.Identity?.IsAuthenticated ?? false)) return Redirect("/account/login");

            var item = _actions.GetItem(id ?? "");
            if (item is null) return NotFound();

            return item.Type switch
            {
                0 => TextPreview(item),
                1 => PdfPreview(item),
                4 => ImagePreview(item),
                5 => RawPreview(item),
            };

        }
    }
}
