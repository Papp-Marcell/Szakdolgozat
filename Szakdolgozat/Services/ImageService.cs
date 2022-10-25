using System.Drawing;


namespace Szakdolgozat.Services
{
    public class ImageService
    {

        public void SaveBitmapToStream(Bitmap bitmap,Stream stream)
        {
            bitmap.Save(stream,System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public Bitmap StartBitmap()
        {
            return new Bitmap(200, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public Bitmap ResizeBitmap(Bitmap bitmap,int width, int height)
        {
            Bitmap result = new Bitmap(bitmap.Width+width, bitmap.Height+height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bitmap, 0, 0, bitmap.Width + width, bitmap.Height + height);
            }
            bitmap = result;
            return result;
        }
        public void DrawRectangleText(Bitmap bitmap,String text,bool jump)
        {
            Pen pen = Pens.Black;
            if (jump)
            {
                pen = Pens.Red;
            }
            Graphics graphics = Graphics.FromImage(bitmap);
            using (Font font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Point))
            {
                Rectangle rectangle = new Rectangle(bitmap.Width-160,bitmap.Height-80, 120, 60);
                graphics.DrawString(text,font,Brushes.Black,rectangle);
                graphics.DrawRectangle(pen, Rectangle.Round(rectangle));
            }
        }
        public void DrawStraightArrow(Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawLine(Pens.Black, bitmap.Width - 240, 100, bitmap.Width - 160, 100);
            graphics.DrawLine(Pens.Black, bitmap.Width - 180, 120, bitmap.Width - 160, 100);
            graphics.DrawLine(Pens.Black, bitmap.Width - 180, 80, bitmap.Width - 160, 100);
        }
        public void AddNextInstruction(Bitmap bitmap, String text, bool jump)
        {
            if (bitmap.Width > 220)
            {
                
                DrawRectangleText(bitmap, text, jump);
                DrawStraightArrow(bitmap);
                bitmap = ResizeBitmap(bitmap, 200, 100);
            }
            else
            {
                DrawRectangleText(bitmap, text, jump);
                bitmap = ResizeBitmap(bitmap, 200, 100);
            }
        }

        public void ForwardJump(Bitmap bitmap, int from, int to)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawLine(Pens.Green, (from * 200) - 100, 80, (from * 200) - 100, 95);
            graphics.DrawLine(Pens.Green, (to * 200) - 100, 80, (to * 200) - 100, 95);
            graphics.DrawLine(Pens.Green, (from * 200) - 100, 95, (to * 200) - 100, 95);
            graphics.DrawLine(Pens.Green, (to * 200) - 110, 90, (to * 200) - 100, 80);
            graphics.DrawLine(Pens.Green, (to * 200) - 90, 90, (to * 200) - 100, 80);
        }

        public void BackwardsJump(Bitmap bitmap, int from, int to)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawLine(Pens.Purple, (from * 200) - 100, 20, (from * 200) - 100, 5);
            graphics.DrawLine(Pens.Purple, (to * 200) - 100, 20, (to * 200) - 100, 5);
            graphics.DrawLine(Pens.Purple, (from * 200) - 100, 5, (to * 200) - 100, 5);
            graphics.DrawLine(Pens.Purple, (from * 200) - 110, 10, (from * 200) - 100, 20);
            graphics.DrawLine(Pens.Purple, (from * 200) - 90, 10, (from * 200) - 100, 20);
        }
    }
}
