using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrassrootsFloodCtrl.Logic.Common
{
    public class QRCodeHelper
    {
        /// <summary>  
        /// 生成二维码  
        /// </summary>  
        /// <param name="content">内容</param>
        /// <param name="moduleSize">二维码的大小</param>
        /// <returns>输出流</returns>  
        public static string GetQRCode(string content,string realname,string filename, int moduleSize = 9)
        {
            var r = "";
            try
            {
                var encoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = encoder.Encode(content);
                GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two), Brushes.Black, Brushes.White);

                MemoryStream memoryStream = new MemoryStream();
                render.WriteToStream(qrCode.Matrix, ImageFormat.Jpeg, memoryStream);

                //生成图片的代码
                DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
                Bitmap bitmap = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
               // var filename = Guid.NewGuid().ToString().Replace("-", "");
                var filesavepath = "Files/" + realname + "/QRPic";
                var _fileExits = System.Web.HttpContext.Current.Server.MapPath("~/" + filesavepath);
                if (!Directory.Exists(_fileExits))
                {
                    Directory.CreateDirectory(_fileExits);
                }
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/") + filesavepath + "/" + filename + ".jpg";

                Graphics g = Graphics.FromImage(bitmap);
                render.Draw(g, qrCode.Matrix);
                bitmap.Save(fileName, ImageFormat.Jpeg);
                r= filesavepath + "/" + filename + ".jpg"; 
            }
            catch(Exception ex)
            {
                var abc = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 生成带Logo二维码  
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="iconPath">logo路径</param>
        /// <param name="moduleSize">二维码的大小</param>
        /// <returns>输出流</returns>
        //public static MemoryStream GetQRCode(string content, string iconPath, int moduleSize = 9)
        //{
        //    QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
        //    QrCode qrCode = qrEncoder.Encode(content);

        //    GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(moduleSize, QuietZoneModules.Two), Brushes.Black, Brushes.White);

        //    DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
        //    Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
        //    Graphics g = Graphics.FromImage(map);
        //    render.Draw(g, qrCode.Matrix);

        //    //追加Logo图片 ,注意控制Logo图片大小和二维码大小的比例
        //    //PS:追加的图片过大超过二维码的容错率会导致信息丢失,无法被识别
        //    Image img = Image.FromFile(iconPath);

        //    Point imgPoint = new Point((map.Width - img.Width) / 2, (map.Height - img.Height) / 2);
        //    g.DrawImage(img, imgPoint.X, imgPoint.Y, img.Width, img.Height);

        //    MemoryStream memoryStream = new MemoryStream();
        //    map.Save(memoryStream, ImageFormat.Jpeg);

        //    return memoryStream;

        //    //生成图片的代码： map.Save(fileName, ImageFormat.Jpeg);//fileName为存放的图片路径
        //}

    }
}
