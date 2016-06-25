using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CertUtilities
{
    public static class CertUtils 
    {
        
        public static byte[] GetCert(String ThePath, Boolean Isx64) 
        {

            byte[] byteBuffer = new byte[4];
            FileStream fs = new FileStream(ThePath, FileMode.Open);
            fs.Seek(0x3c, SeekOrigin.Begin);
            fs.Read(byteBuffer, 0, 2);
            int peStart = BitConverter.ToInt16(byteBuffer, 0);
            if (Isx64 == true)
            {
                fs.Seek(peStart + 0xA8, SeekOrigin.Begin);
                fs.Read(byteBuffer, 0, 4);
                int CertPointer = BitConverter.ToInt32(byteBuffer, 0);
                fs.Read(byteBuffer, 0, 4);
                int CertSize = BitConverter.ToInt32(byteBuffer, 0);
                byte[] TheCert = new byte[CertSize];
                fs.Seek(CertPointer, SeekOrigin.Begin);
                fs.Read(TheCert, 0, CertSize);
                fs.Close();
                return TheCert;
            }
            else
            {
                fs.Seek(peStart + 0x98, SeekOrigin.Begin);
                fs.Read(byteBuffer, 0, 4);
                int CertPointer = BitConverter.ToInt32(byteBuffer, 0);
                fs.Read(byteBuffer, 0, 4);
                int CertSize = BitConverter.ToInt32(byteBuffer, 0);
                byte[] TheCert = new byte[CertSize];
                fs.Seek(CertPointer, SeekOrigin.Begin);
                fs.Read(TheCert, 0, CertSize);
                fs.Close();
                return TheCert;
            }

                    }
        
        private static void FixPointerAndSize(string TheFile, int CertSize, int Pointer, Boolean Isx64)
        {
             
            byte[] byteBuffer = new byte[4];

            FileStream fs = new FileStream(TheFile, FileMode.Open);
            if (Isx64 == true)
            {
                fs.Seek(0x3c, SeekOrigin.Begin);
                fs.Read(byteBuffer, 0, 2);
                int PeStart = BitConverter.ToInt16(byteBuffer, 0);
                fs.Seek(PeStart + 0xA0, SeekOrigin.Begin); 
            }

            else
            {
                fs.Seek(0x3c, SeekOrigin.Begin);
                fs.Read(byteBuffer, 0, 2);
                int PeStart = BitConverter.ToInt16(byteBuffer, 0);
                fs.Seek(PeStart + 0x98, SeekOrigin.Begin); 
            }
            
           

            fs.Write(BitConverter.GetBytes(Pointer), 0, 4);
            fs.Write(BitConverter.GetBytes(CertSize), 0, 4);

            fs.Flush();
            fs.Close();
        }
       
        public static void SignFile(string TheFileToSign, string TheCert, Boolean Isx64)
        {
            File.Copy(TheFileToSign, TheFileToSign + "_signed.exe");

            FileStream fs1 = File.OpenRead(TheCert);
            byte[] CertByt = new byte[fs1.Length];
            fs1.Read(CertByt, 0, Convert.ToInt32(fs1.Length));
            fs1.Close();


            FileStream fs = File.OpenRead(TheFileToSign);
            byte[] FileByte = new byte[fs.Length];
            fs.Read(FileByte, 0, Convert.ToInt32(fs.Length));
            fs1.Close();

            int newSize = FileByte.Length + CertByt.Length;
            var ms = new MemoryStream(new byte[newSize], 0, newSize, true, true);
            ms.Write(FileByte, 0, FileByte.Length);
            ms.Write(CertByt, 0, CertByt.Length);
            byte[] merged = ms.GetBuffer();

            File.WriteAllBytes(TheFileToSign + "_signed.exe", merged);
            FixPointerAndSize(TheFileToSign + "_signed.exe", CertByt.Length, FileByte.Length, Isx64);


        }

    }
}
