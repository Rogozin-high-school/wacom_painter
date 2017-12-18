using System;
using System.Drawing;
using System.Net;
using System.IO;

class Utilities
{
    public static string ToBase64(Bitmap b)
    {
        //bitmap to byte array
        ImageConverter converter = new ImageConverter();
        byte[] byteArray = (byte[])converter.ConvertTo(b, typeof(byte[]));
        string base64String = Convert.ToBase64String(byteArray);
        return base64String;
    }

    public static Bitmap GetBitmapFromHttp(string url)
    {
        WebClient wc = new WebClient();
        Stream s = wc.OpenRead(url);
        Bitmap bmp = new Bitmap(s);
        return bmp;
    }
	public static List<string> FilesInDirectory(string path)
        {
            List<string> l = new List<string>();
            char[] s = new char[1];
            s[0] = '.';
            char[] s1 = new char[1];
            s1[0] = '\\';
            // get all files in directory
            string[] filenames = Directory.GetFiles(path);
            for(int i =0; i < filenames.Length && i < 3; i++)
            {
                string[] n = filenames[i].Split(s1);
                l.Add(n[n.Length-1].Split(s)[0]);
            }
            return l;
        }
}