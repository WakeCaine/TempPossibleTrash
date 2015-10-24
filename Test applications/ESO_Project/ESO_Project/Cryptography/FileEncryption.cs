using ESO_Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using ESO_Project.Entities;
using System.Text;
using ESO_Project.Logs;

namespace ESO_Project.Cryptography
{
    public static class FileEncryption
    {
        public static byte[] getInitializationVector()
        {
            AesCryptoServiceProvider myAes = new AesCryptoServiceProvider();
            myAes.GenerateIV();
            return myAes.IV;
        }

        //wczytanie pliku do tablicy bit.
        public static byte[] getBytesFromFile(string fullFilePath)
        {

            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            catch(Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
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

        public static byte[] getBytesFromFile(FileStream file)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    file.Seek(0, SeekOrigin.Begin);
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
        }

        //zapisz tablicę bit. do pliku
        public static void backToFile(byte[] bytes, string path)
        {
            try
            {
                using (Stream file = System.IO.File.OpenWrite(path))
                {
                    file.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }            
        }

        public static FileStream backToFile(byte[] bytes)
        {
            try
            {
                FileStream file = null;
                file.Write(bytes, 0, Convert.ToInt32(bytes.Length));
                return file;
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
        }

        public static byte[] keyGenerator()
        {
            try
            {
                byte[] key;
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    key = aes.Key;

                    return key;
                }
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
        }

        public static byte[] getUserKey(string id)
        {
            try
            {
                KeysVector key = new KeysVector();
                ApplicationDbContext db = new ApplicationDbContext();
                if (!db.KeyVectors.Any(f => f.userId == id))
                {
                    key.userId = id;
                    byte[] generatedkey = keyGenerator();
                    key.key = generatedkey;
                    key.vector = getInitializationVector();
                    key.SyncTime = DateTime.Now;
                    db.KeyVectors.Add(key);
                    db.SaveChanges();
                    return generatedkey;
                }
                else
                {
                    var keyvectors = db.KeyVectors.Where(c => c.userId == id);
                    foreach (var keyvector in keyvectors)
                    {
                        return keyvector.key;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
            

            //string path = HostingEnvironment.ApplicationPhysicalPath + @".\Cryptography\TempUserKey";
            //FileInfo keyTemp = new FileInfo(path);
            //if (!keyTemp.Exists)
            //{
            //    byte[] temp = keyGenerator();
            //    File.WriteAllBytes(path, temp);
            //    return temp;
            //}
            //else
            //{
            //    return File.ReadAllBytes(path);
            //}
        }

        public static byte[] getInitVector(string id)
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var keyvectors = db.KeyVectors.Where(c => c.userId == id);
                foreach (var keyvector in keyvectors)
                {
                    return keyvector.vector;
                }
                return null;
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
            
            //string path = HostingEnvironment.ApplicationPhysicalPath + @".\Cryptography\TempUserVector";
            //FileInfo keyTemp = new FileInfo(path);
            //if (!keyTemp.Exists)
            //{
            //    byte[] temp = getInitializationVector();
            //    File.WriteAllBytes(path, temp);
            //    return temp;
            //}
            //else
            //{
            //    return File.ReadAllBytes(path);
            //}
        }

        public static byte[] encrypt(byte[] plain, byte[] Key, byte[] IV)
        {
            try
            {
                byte[] encrypted;
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
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
            
        }

        public static byte[] decrypt(byte[] encrypted, byte[] Key, byte[] IV)
        {
            try
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
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                throw e;
            }
            
        }
    }
}