using AzureStorageLibrary;
using AzureStorageLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MVCWebApp.Models;
using System.Reflection.Metadata;
using System.Text;

namespace MVCWebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage _blobStorage;
        private readonly IHubContext<ImageHub> _hubContext;

        public BlobsController(IBlobStorage blobStorage, IHubContext<ImageHub> hubContext)
        {
            _blobStorage = blobStorage;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            AzQueue queue = new AzQueue("imagepaths");
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            ViewBag.blobs = names.Select(x => new FileBlob { Name = x, Url = $"{blobUrl}/{x}" }).ToList();

            foreach (var name in names) {

                await queue.SendMessageAsync($"{blobUrl}/{name}");
            }
            await _hubContext.Clients.All.SendAsync("AllImages");

            var pdfNames = _blobStorage.GetNames(EContainerName.pdf);

            string pdfUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pdf}";


            ViewBag.pdfs = pdfNames.Select(x => new FileBlob { Name = x, Url = $"{pdfUrl}/{x}" }).ToList();

            ViewBag.logs=await _blobStorage.GetLogsAsync("controller.txt");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile picture)
        {
            AzQueue queue = new AzQueue("imagepaths");
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures}";
            try
            {
                var newFilename = Guid.NewGuid().ToString()+Path.GetExtension(picture.FileName);
                await _blobStorage.UploadAsync(picture.OpenReadStream(), newFilename, EContainerName.pictures, "application/octet-stream");
                await queue.SendMessageAsync($"{blobUrl}/{newFilename}");
                await _hubContext.Clients.All.SendAsync("ImageUploaded");
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
                await _blobStorage.UploadAsync(pdf.OpenReadStream(), newFilename, EContainerName.pdf,"application/pdf");
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
