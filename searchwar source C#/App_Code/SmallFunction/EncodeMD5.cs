using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for EncodeMD5
/// </summary>
public static class EncodeMD5 {
    public static string EncodeToMd5(this string text) {
        //Declarations
        Byte[] originalBytes;
        Byte[] encodedBytes;
        MD5 md5;

        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
        md5 = new MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(text);
        encodedBytes = md5.ComputeHash(originalBytes);

        //Convert encoded bytes back to a 'readable' string
        return BitConverter.ToString(encodedBytes);
    }
}
