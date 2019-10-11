using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kezhi.Code
{
    public class PictureUtil
    {
        private static string folder = AppDomain.CurrentDomain.BaseDirectory + "Files\\Photo\\";
        /// <summary>
        ///  IT员工风貌制作
        /// </summary>
        /// <param name="imageWith">照片宽</param>
        /// <param name="imageHeight">照片高</param>
        /// <param name="photo">照片名称</param>
        /// <param name="backgroundImg">背景图片</param>
        /// <returns></returns>
        public static string PhotoAssembleyIT(string[] strText, string imageWith, string imageHeight, string photo, string backgroundImg)
        {
            float imgWith = Convert.ToSingle(imageWith);//照片实际宽度
            float imgHeight = Convert.ToSingle(imageHeight);//照片实际高度
            int bakMaxHeight = 716 - 80;//照片放入相册最大高度
            int bakMaxWidth = 1070 - 370;//照片放入相册最小宽度
            float realWidth = 0F;//照片放入相册实际宽度
            float realHeight = 0F;//照片放入相册实际高度
            float leftSpan = 0F;//照片放入相册距离左侧间距
            float topSpan = 0f;
            string imagePath = Path.Combine(folder, photo); //传入的照片
            string realPhoto = imagePath;//截取的照片
            bool flag = false;
            //1、计算是否需要截取照片
            realHeight = bakMaxHeight;
            realWidth = imgWith * ((Convert.ToSingle(bakMaxHeight) / imgHeight));

            //2、宽比长大的照片，直接合并
            if (realWidth <= bakMaxWidth)
            {

                topSpan = 40;
                leftSpan = 1070 - realWidth;
                if (realWidth >= 480)
                {
                    leftSpan += 40;
                }
            }
            else
            {
                //4、如果宽高比例大于1.5，截取适合的比例
                float proportion = imgWith / imgHeight;
                if (proportion >= 1.5)
                {
                    int cutWidth = Convert.ToInt32(imgHeight * 1.5);
                    int cutHeight = Convert.ToInt32(imgHeight);
                    int cutSpan = Convert.ToInt32((imgWith - cutWidth) / 2);
                    realPhoto = Path.Combine(folder, Common.GuId().Substring(0, 16) + ".png");//截取的照片
                    CaptureImage(imagePath, cutSpan, 0, realPhoto,cutWidth,cutHeight);
                    realWidth = cutWidth * (636 / imgHeight);
                    realHeight = 636;
                    leftSpan = 1280 - 70 - realWidth;
                    flag = true;

                }
                else 
                {
                    realWidth = imgWith * (636 / imgHeight);
                    realHeight = 600;
                    leftSpan = 1280 - 100 - realWidth;
                }
                topSpan = (716 - realHeight) / 2;
            }
            Image img1 = Image.FromFile(Path.Combine(folder, backgroundImg));//相框图片 
            Image img2 = Image.FromFile(realPhoto); //照片图片               
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics       
            Graphics g = Graphics.FromImage(img1);
            g.DrawImage(img1, 0, 0, 1280, 716);// g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
            //g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            //(20,20,370，1070)：上下左右可允许展示照片的位置，照片从右到左展示
      
            Image realImage = ZoomPicture(img2, imgWith, imgHeight, realWidth, realHeight);
            g.DrawImage(realImage, leftSpan, topSpan, realWidth, realHeight);
            GC.Collect();
            string newPictureName = Common.GuId().Substring(0,16) + ".png";
            img1.Save(Path.Combine(folder, newPictureName));
            img1.Dispose();
            //给图片写字
            float leftPosition = 160;
            if (flag)
            {
                leftPosition = 120;
            }
            float topPosition = 40;
            if (strText.Length > 0)
            {
                for (var i = 0; i < strText.Length; i++)
                {
                    newPictureName = AddTextToImg(strText[i], folder + newPictureName, leftPosition, topPosition);
                    leftPosition += 50;
                }
            }
           
            return newPictureName;
        }

        // 按比例缩放图片
        public static Image ZoomPicture(Image SourceImage, float width, float height, float realWIdth,float realHeight)
        {

            //新的图片宽
            int IntWidth = Convert.ToInt32(realWIdth);
            //新的图片高
            int IntHeight = Convert.ToInt32(realHeight);
            //照片宽高
            int TargetWidth = Convert.ToInt32(width);
            int TargetHeight = Convert.ToInt32(height);
            try
            {
                System.Drawing.Imaging.ImageFormat format = SourceImage.RawFormat;
                System.Drawing.Bitmap SaveImage = new System.Drawing.Bitmap(IntWidth, IntHeight);
                Graphics g = Graphics.FromImage(SaveImage);
                g.Clear(Color.White);

                g.DrawImage(SourceImage, 0, 0, IntWidth, IntHeight);
                SourceImage.Dispose();

                return SaveImage;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        /// <summary>
        /// 指定图片添加指定文字
        /// </summary> 
        /// <param name="text">添加的文字</param>
        /// <param name="picname">生成文件名</param>
        private static string AddTextToImg(string text,string filePath,float rectX,float rectY)
        {
            //判断指定图片是否存在
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file don't exist!");
            }
            if (text == string.Empty)
            {
                return filePath;
            }
            Image image = Image.FromFile(filePath);
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //字体大小
            float fontSize = 30.0f;
            //文本的长度
            float textWidth = text.Length * fontSize;
            //下面定义一个矩形区域，以后在这个矩形里画上白底黑字
            //float rectX = 160;
            //float rectY = 40;
            float rectWidth = 80;
            float rectHeight = 600;
            //float rectWidth = 200;
            //float rectHeight = 500;
            //声明矩形域
            RectangleF textArea = new RectangleF(rectX, rectY, rectWidth, rectHeight);
            //定义字体
            System.Drawing.Font font = new System.Drawing.Font("微软雅黑", fontSize, System.Drawing.FontStyle.Bold);
            //font.Bold = true;
            //设置文字字体颜色
            Brush whiteBrush = new SolidBrush(System.Drawing.Color.Red);
            //黑笔刷，画背景用
            //Brush blackBrush = new SolidBrush(Color.Black);   
            //g.FillRectangle(blackBrush, rectX, rectY, rectWidth, rectHeight);
            //定义文字格式
            
            StringFormat famat = new StringFormat();
            famat.LineAlignment = StringAlignment.Far;
            famat.Alignment = StringAlignment.Center;
            famat.FormatFlags = StringFormatFlags.DirectionVertical;
            //famat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            //g.DrawString(text, font, whiteBrush, textArea);
            g.DrawString(text, font, whiteBrush, textArea, famat);
            //输出方法一：将文件生成并保存到C盘
            string newName = Common.GuId().Substring(0, 16) + ".png";
            string path = folder + newName;
            bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            bitmap.Dispose();
            image.Dispose();
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return newName;
            }
            else
            {
                return filePath;
            }
        }
        #region 从大图中截取一部分图片
        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="fromImagePath">来源图片地址</param>        
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="toImagePath">保存图片地址</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns></returns>
        public static void CaptureImage(string fromImagePath, int offsetX, int offsetY, string toImagePath, int width, int height)
        {
            //原图片文件
            Image fromImage = Image.FromFile(fromImagePath);
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图片
            saveImage.Save(toImagePath, ImageFormat.Png);
            //释放资源   
            saveImage.Dispose();
            graphic.Dispose();
            bitmap.Dispose();
        }
        #endregion
    }
}
