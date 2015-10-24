using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace aesPliki
{
    class Program
    {
        //wczytanie pliku do tablicy bit.
        public static byte[] getBytesFromFile(string fullFilePath)
        {
            
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        //zapisz tablicę bit. do pliku
        public static void backToFile(byte[] bytes, string path)
        {
            using (Stream file = File.OpenWrite(path))
            {
                file.Write(bytes, 0, bytes.Length);
            }
        }

        public static byte[] keyGenerator()
        {
            byte[] key;
            using(AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                key = aes.Key;

                return key;
            }
            
        }

        static void Main(string[] args)
        {
           // przykładowe ścieżki
            string path1 = @"C:\test.txt";
            string pathBack = @"C:\test1.txt";
            string path2 = @"C:\Users\Laura\Desktop\SystemModules.pdf";
            string pathBack2 = @"C:\Users\Laura\Desktop\SystemModules2.pdf";

            //plik do szyfrowania
            byte[] original = getBytesFromFile(path1);

            //klucz
           byte[] key1 = keyGenerator();

            try
            {

                //byte[] original = { 1, 2, 3, 45, 234, 6, 9, 3, 4, 5, 5, 5 };

                
                //nowa instancja AesCryptoServiceProvider; generuje klucz i wektor inicjalizujący (IV)
                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {

                    
                    byte[] encrypted = encrypt(original, key1, myAes.IV);

                    
                   // byte[] decrypted = decrypt(encrypted, key1, myAes.IV);

                  
                    

                    /*foreach (byte b in original)
                    {
                        Console.Write(b);
                    }
                    Console.WriteLine("\n\n\n\n\n");
                    foreach (byte b in encrypted)
                    {
                        Console.Write(b);
                    }
                    Console.WriteLine("\n\n\n\n\n");
                    foreach (byte b in roundtrip)
                    {
                        Console.Write(b);
                    }*/

                    //zapisz do pliku
                    backToFile(encrypted, pathBack);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            //Console.WriteLine("End");
            Console.ReadLine();

        }



        public static byte[] encrypt(byte[] plain, byte[] Key, byte[] IV)
        {
            byte[] encrypted; ;
            using (MemoryStream mstream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream,
                        aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plain, 0, plain.Length);
                    }
                    encrypted = mstream.ToArray();
                }
            }
            return encrypted;
        }

        public static byte[] decrypt(byte[] encrypted, byte[] Key, byte[] IV)
        {
            byte[] plain;
            int count;
            using (MemoryStream mStream = new MemoryStream(encrypted))
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    aesProvider.Mode = CipherMode.CBC;
                    using (CryptoStream cryptoStream = new CryptoStream(mStream,
                     aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                    {
                        plain = new byte[encrypted.Length];
                        count = cryptoStream.Read(plain, 0, plain.Length);
                    }
                }
            }

         
            byte[] returnValue = new byte[count];
            Array.Copy(plain, returnValue, count);
            return returnValue;
        }
    }
}
