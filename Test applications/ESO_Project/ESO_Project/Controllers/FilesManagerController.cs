using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ESO_Project.Models;
using ESO_Project.Entities;
using Microsoft.AspNet.Identity;
using System.Web.Hosting;
using ESO_Project.Cryptography;
using ESO_Project.Logs;
using System.Threading.Tasks;

namespace ESO_Project.Controllers
{
    [Authorize]
    public class FilesManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Upload()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var folders = db.Folders.Where(f => f.UserId == userId);
                var full = new List<ESO_Project.Entities.File>();
                foreach (var folder in folders)
                {
                    var files = db.Files.Where(c => c.FolderId == folder.Id);

                    foreach (var file in files)
                    {
                        full.Add(new ESO_Project.Entities.File()
                        {
                            Id = file.Id,
                            SyncTime = file.SyncTime,
                            Name = file.Name,
                            IsRoot = file.IsRoot,
                            Type = file.Type,
                            Size = file.Size,
                            Shared = file.Shared,
                            FolderId = file.FolderId
                        });
                    }
                }
                FilesManagerModel fileM = new FilesManagerModel();
                fileM.files = full;
                return PartialView(fileM);
            }
            catch(Exception e)
            {
                Logger.log(e.StackTrace);
                return RedirectToAction("DatabaseError", "Errors");
            }
        }
        // POST: FilesManager/Upload
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Upload(FilesManagerModel fileModel)
        {
            try
            {
                if(User.IsInRole("User"))
                {
                    if(fileModel.File.ContentLength > 100000000)
                    {
                        return RedirectToAction("Upload");
                    }
                }
                DirectoryInfo directorys = new DirectoryInfo(Server.MapPath(@"..\UserFiles\" + User.Identity.Name));
            
                //Na razie wszystko trafia do roota
                Boolean isRoot = true;
                if (!directorys.Exists)
                {
                    directorys.Create();
                    Folder root = new Folder();
                    root.UserId = User.Identity.GetUserId();
                    root.Name = User.Identity.Name;
                    root.Shared = 0;
                    db.Folders.Add(root);
                    await db.SaveChangesAsync();
                    isRoot = true;
                }

                var fileName = Path.GetFileName(fileModel.File.FileName);
                if (ModelState.IsValid && !db.Files.Any(fi => fi.Name == fileName))
                {
                    if (fileModel != null && fileModel.File != null && fileModel.File.ContentLength > 0)
                    {
                        ESO_Project.Entities.File file = new ESO_Project.Entities.File();
                        

                        using (var fileStream = System.IO.File.Create(Path.Combine(directorys.FullName, fileName)))
                        {
                            fileModel.File.InputStream.Seek(0, SeekOrigin.Begin);
                            fileModel.File.InputStream.CopyTo(fileStream);
                            byte[] fileInBytes = FileEncryption.getBytesFromFile(fileStream);
                        
                            byte[] keyToEncrypt = FileEncryption.getUserKey(User.Identity.GetUserId());
                            byte[] initializationVector = FileEncryption.getInitVector(User.Identity.GetUserId());
                            //byte[] initializationVector = FileEncryption.getInitializationVector();
                            //byte[] keyToEncrypt = FileEncryption.keyGenerator()

                            byte[] encryptedFile = FileEncryption.encrypt(fileInBytes, keyToEncrypt, initializationVector);
                            //keyToEncrypt = FileEncryption.getUserKey(User.Identity.GetUserId());
                            //byte[] decryptedFile = FileEncryption.decrypt(encryptedFile, keyToEncrypt, initializationVector);
                            fileStream.Seek(0,SeekOrigin.Begin);
                            fileStream.Write(encryptedFile, 0, encryptedFile.Length);
                        }
                    
                        //fileModel.File.SaveAs(Path.Combine(directorys.FullName, fileName));
                        if(isRoot)
                        {
                            if(db.Folders.Any(f => f.Name == User.Identity.Name))
                            {
                                    file.FolderId = (from f in db.Folders where f.Name == User.Identity.Name select f.Id).Single();
                                    file.Name = fileName;
                                    file.IsRoot = 0;
                                    file.Type = "." + fileName.Split('.')[1];
                                    file.Size = fileModel.File.ContentLength;
                                    file.Shared = 0;
                                    file.SyncTime = DateTime.Now;
                                    db.Files.Add(file);
                                    await db.SaveChangesAsync();
                                    Logger.log("Dodano plik.");
                            }
                        }
                    
                    }
                    
                }
                return RedirectToAction("Upload");
            }
            catch(Exception e)
            {
                Logger.log(e.StackTrace);
                return View();
            }
        }

        // GET: FilesManager Files
        [HttpGet]
        public ActionResult Download(int file)
        {
            try
            {
                ESO_Project.Entities.File fileNew = (from f in db.Files where f.Id == file select f).Single();
                using (var fileStream = System.IO.File.OpenRead(HostingEnvironment.ApplicationPhysicalPath + @".\UserFiles\" + User.Identity.Name + @"\" + fileNew.Name))
                {
                    byte[] fileInBytes = FileEncryption.getBytesFromFile(fileStream);
                    byte[] initializationVector = FileEncryption.getInitVector(User.Identity.GetUserId());
                    byte[] keyToEncrypt = FileEncryption.getUserKey(User.Identity.GetUserId());

                    byte[] decryptedFile = FileEncryption.decrypt(fileInBytes, keyToEncrypt, initializationVector);
                    Logger.log("Pobrano plik: " + fileNew.Name);
                    return File(decryptedFile, "application/force-download", fileNew.Name);
                }
            }
            catch(Exception e)
            {
                Logger.log(e.StackTrace);
                return RedirectToAction("DatabaseError", "Errors");
            }
            //ESO_Project.Entities.File fileNew = (from f in db.Files where f.Id == file select f).Single();
            //byte[] keyToDecrypt = FileEncryption.getUserKey(User.Identity.GetUserId());
            //byte[] initializationVector = FileEncryption.getInitVector(User.Identity.GetUserId());
            //byte[] decryptedFile = FileEncryption.decrypt(System.IO.File.ReadAllBytes(HostingEnvironment.ApplicationPhysicalPath + @".\UserFiles\" + User.Identity.Name + @"\" + fileNew.Name), keyToDecrypt, initializationVector);
            //return File(decryptedFile, "application/force-download", fileNew.Name);
            
            //return File(Server.MapPath(@"..\UserFiles\" + User.Identity.Name + @"\" + fileNew.Name), "application/force-download", fileNew.Name);
            
            //return new FileStreamResult(new FileStream(Server.MapPath(@"..\UserFiles\" + User.Identity.Name + @"\" + fileNew.Name), FileMode.Open) , fileNew.Type);
        }

        // GET: FilesManager
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("DatabaseError", "Errors");
            }
        }

        // GET: FilesManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FilesManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FilesManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FilesManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FilesManager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FilesManager/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                ESO_Project.Entities.File file = new ESO_Project.Entities.File();
                file = await db.Files.FindAsync(id);
                if (file == null)
                {
                    return RedirectToAction("DatabaseError", "Errors");
                }
                return View(file);
            }
            catch(Exception e)
            {
                Logger.log(e.StackTrace);
                return RedirectToAction("DatabaseError", "Errors");
            }
        }

        // POST: FilesManager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
            DirectoryInfo directorys = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath + "\\UserFiles\\" + User.Identity.Name);
                // TODO: Add delete logic here
                ESO_Project.Entities.File file = await db.Files.FindAsync(id);
                string fileName = file.Name;
                if (directorys.Exists)
                {
                    FileInfo fileInfo = new FileInfo(Path.Combine(directorys.ToString(), file.Name));
                    db.Files.Remove(file);
                    fileInfo.Delete();
                }
                else
                {
                    return RedirectToAction("DatabaseError", "Errors");
                }
                await db.SaveChangesAsync();
                Logger.log("Usunięto plik: " + fileName);
                return RedirectToAction("Upload");
            }
            catch(Exception e)
            {
                 Logger.log(e.StackTrace);
                 return RedirectToAction("DatabaseError", "Errors");
            }
        }



        [HttpGet]
        public ActionResult Show(int id)
        {
            try
            {
                ESO_Project.Entities.File fileNew = (from f in db.Files where f.Id == id select f).Single();
                using (var fileStream = System.IO.File.OpenRead(HostingEnvironment.ApplicationPhysicalPath + @".\UserFiles\" + User.Identity.Name + @"\" + fileNew.Name))
                {
                    byte[] fileInBytes = FileEncryption.getBytesFromFile(fileStream);
                    byte[] initializationVector = FileEncryption.getInitVector(User.Identity.GetUserId());
                    byte[] keyToEncrypt = FileEncryption.getUserKey(User.Identity.GetUserId());

                    byte[] decryptedFile = FileEncryption.decrypt(fileInBytes, keyToEncrypt, initializationVector);
                    return base.File(decryptedFile, "image/jpeg");
                }
            }
            catch (Exception e)
            {
                Logger.log(e.StackTrace);
                return RedirectToAction("DatabaseError", "Errors");
            }

            //var dir = Server.MapPath("/UserFiles/");
            //var path = Path.Combine(dir, username, id);

            //return base.File(path, "image/" + id.Split('.')[1]);
        }
    }
}
