using System;
using System.Collections.Generic;
using System.Drawing;
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
            
            //const string folder = @"~";
            Image img1 = Image.FromFile(Path.Combine(folder, backgroundImg));//相框图片 
            Image img2 = Image.FromFile(Path.Combine(folder, photo)); //照片图片               
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics       
            Graphics g = Graphics.FromImage(img1);
            g.DrawImage(img1, 0, 0, 1280, 716);// g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);
            //g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            //(20,20,370，1070)：上下左右可允许展示照片的位置，照片从右到左展示
            //照片宽>高 1、如果横向拉满纵向不溢出，那就这样展示 2、如果横向拉满，纵向溢出，那么按照纵向拉满比例展示
            //照片宽<高 1、纵向拉满展示,横向不溢出，纵向展示 2、纵向拉满横向溢出，按照横向比例展示
            float imgWith = Convert.ToSingle(imageWith);//照片实际宽度
            float imgHeight = Convert.ToSingle(imageHeight);//照片实际高度
            int bakMaxHeight = 716 - 80;//照片放入相册最大高度
            int bakMaxWidth = 1070 - 370;//照片放入相册最小高度
            float realWidth = 0F;//照片放入相册实际宽度
            float realHeight = 0F;//照片放入相册实际高度
            float leftSpan = 0F;//照片放入相册距离左侧间距
            float topSpan = 0f;

            realHeight = bakMaxHeight;
            realWidth = imgWith * ((Convert.ToSingle(bakMaxHeight) / imgHeight));
            if (realWidth <= bakMaxWidth)
            {
                topSpan = 40;
                leftSpan = 1070 - realWidth;
            }
            else
            {
                realWidth = 1070 - 370;
                realHeight = imgHeight * ((Convert.ToSingle(bakMaxWidth) / imgWith));
                leftSpan = 370;
                topSpan = (bakMaxHeight - realHeight) / 2;
            }
            Image realImage = ZoomPicture(img2, imgWith, imgHeight, realWidth, realHeight);
            g.DrawImage(realImage, leftSpan, topSpan, realWidth, realHeight);
            GC.Collect();
            string newPictureName = Common.GuId().Substring(0,16) + ".png";
            img1.Save(Path.Combine(folder, newPictureName));
            img1.Dispose();
            //给图片写字
            float leftPosition = 160;
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
            Brush whiteBrush = new SolidBrush(System.Drawing.Color.DodgerBlue);
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
    }
}
