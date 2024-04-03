using AzureStorageLibrary;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;

namespace MVCWebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage _blobStorage;

        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<IActionResult> Index()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            ViewBag.blobs = names.Select(x => new FileBlob { Name = x, Url = $"{blobUrl}/{x}" }).ToList();


            var pdfNames = _blobStorage.GetNames(EContainerName.pdf);

            string pdfUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pdf}";

            ViewBag.pdfs = pdfNames.Select(x => new FileBlob { Name = x, Url = $"{pdfUrl}/{x}" }).ToList();

            ViewBag.logs=await _blobStorage.GetLogsAsync("controller.txt");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile picture)
        {
            try
            {

                var newFilename = Guid.NewGuid().ToString()+Path.GetExtension(picture.FileName);
                await _blobStorage.UploadAsync(picture.OpenReadStream(), newFilename, EContainerName.pictures);
                await _blobStorage.SetLogAsync("File Uploaded succesfully", "controller.txt");
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Download(string filename)
        {

            try
            {
                var stream = await _blobStorage.DownloadAsync(filename, EContainerName.pictures);
                await _blobStorage.SetLogAsync("File Downloaded succesfully", "controller.txt");
                return File(stream, "application/octet-stream", filename);
            }
            catch (Exception)
            {
                await _blobStorage.SetLogAsync("File Downloaded with error", "controller.txt");
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(string filename)
        {
            await _blobStorage.DeleteAsync(filename, EContainerName.pictures);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UploadPdf(IFormFile pdf)
        {
            try
            {

                var newFilename = Guid.NewGuid().ToString()+Path.GetExtension(pdf.FileName);
                await _blobStorage.UploadAsync(pdf.OpenReadStream(), newFilename, EContainerName.pdf);
                await _blobStorage.SetLogAsync("Pdf File Uploaded succesfully", "controller.txt");
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }

    }
}
