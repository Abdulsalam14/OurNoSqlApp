using AzureStorageLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using SecondMvcApp.Models;
using System.Diagnostics;
using System.Text;

namespace SecondMvcApp.Controllers
{
    public class SecondHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetLastImage()
        {
            var queue = new AzQueue("imagepaths");
            var result = await queue.RetrieveNextMessageAsync();

            if (result != null)
            {
                string message = result.MessageText;
                await queue.DeleteMessage(result.MessageId, result.PopReceipt);
                return Ok(message);
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> GetAllImages()
        {
            var queue = new AzQueue("imagepaths");
            var result = await queue.GetAllMessagesFromQueueAsync();

            if (result != null)
            {
                var messages = new List<string>();
                foreach (var item in result)
                {
                    string message = item.MessageText;
                    messages.Add(message);
                    await queue.DeleteMessage(item.MessageId, item.PopReceipt);
                }
                return Ok(messages);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
