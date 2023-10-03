using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    class Codeing
    {
        public void Encode()
        {
            Bitmap image = new Bitmap(@"C:\Users\qoo09\OneDrive\program\C#\RGBTransform\RGBTransform\Example.jpg"); // 加載圖像

            Random rand = new Random(); // 初始化隨機數生成器

            StringBuilder storage = new StringBuilder(); // 儲存需要的資訊

            // 鎖定圖像的部分，進行讀寫操作
            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes); // 複製圖像像素值到 byte 陣列

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int position = (y * bmpData.Stride) + (x * 3);
                    byte blue = rgbValues[position];
                    byte green = rgbValues[position + 1];
                    byte red = rgbValues[position + 2];

                    // 檢查像素值是否小於或等於240
                    if (red <= 240 || green <= 240 || blue <= 240)
                    {
                        storage.Append("#").Append(x).Append("#").Append(y); // 如果是，則儲存該像素的座標
                    }

                    // 更新像素值為隨機色彩
                    rgbValues[position] = (byte)rand.Next(0, 256);
                    rgbValues[position + 1] = (byte)rand.Next(0, 256);
                    rgbValues[position + 2] = (byte)rand.Next(0, 256);
                }
            }

            Marshal.Copy(rgbValues, 0, ptr, bytes); // 將更新後的 byte 陣列複製回 Bitmap
            image.UnlockBits(bmpData); // 解鎖圖像資料

            image.Save(@"C:\Users\qoo09\OneDrive\program\C#\RGBTransform\RGBTransform\obj\output.jpg"); // 儲存新圖像
            File.WriteAllText(@"C:\Users\qoo09\OneDrive\program\C#\RGBTransform\RGBTransform\obj\Key.text", storage.ToString()); // 儲存關鍵資訊
        }

        public void Uncode()
        {
            string content = File.ReadAllText(@"C:\Users\qoo09\OneDrive\program\C#\RGBTransform\RGBTransform\obj\Key.text"); // 讀取儲存的關鍵資訊
            Bitmap image = new Bitmap(@"C:\Users\qoo09\OneDrive\program\C#\RGBTransform\RGBTransform\obj\output.jpg"); // 加載圖像

            BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            string sum = "";
            int count = 0;
            int x = 0, y = 0;

            for (int i = 1; i < content.Length; i++)
            {
                if (content[i] != '#')
                {
                    sum += content[i]; // 如果當前字符不是'#'，將其添加到 sum
                }
                else
                {
                    count++;

                    if (count == 1)
                        x = int.Parse(sum); // 解析 X 座標
                    else if (count == 2)
                    {
                        y = int.Parse(sum); // 解析 Y 座標
                        count = 0;

                        int position = (y * bmpData.Stride) + (x * 3);
                        rgbValues[position] = 0;
                        rgbValues[position + 1] = 0;
                        rgbValues[position + 2] = 0; // 將相應的像素設置為黑色
                    }

                    sum = "";
                }
            }

            Marshal.Copy(rgbValues, 0, ptr, bytes);
            image.UnlockBits(bmpData);

            image.Save(@"C:\Users\qoo09\OneDrive\program\C#\RGBTransform\RGBTransform\output.jpg"); // 儲存解碼後的圖像
        }
    }

    static void Main()
    {
        Codeing codeing = new Codeing();
        string choose;

        do
        {
            Console.Write("輸入1加密，輸入2解碼:");
            choose = Console.ReadLine(); // 獲取用戶選擇
        } while (choose != "1" && choose != "2");

        Console.WriteLine(choose);

        if (choose == "1")
            codeing.Encode(); // 根據選擇執行加密或解碼
        else if (choose == "2")
            codeing.Uncode();
    }
}
