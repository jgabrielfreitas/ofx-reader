using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFX.Reader.Application.OFX.Commands.Create;
using OFX.Reader.Application.OFX.Models;
using OFX.Reader.Web.Models;

namespace OFX.Reader.Web.Controllers {
    
    public class HomeController : BaseController {
        
        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file) {
            
            if (file == null || file.Length == 0)  
                return this.Content("file not selected");
            
            string path = Path.Combine( @"..\..\ofx_files", file.FileName);  
  
            using (FileStream stream = new FileStream(path, FileMode.Create)) {  
                await file.CopyToAsync(stream);  
            }
            
            FinancialExchangeModel financialExchangeModel = await this.Mediator.Send<FinancialExchangeModel>(new CreateOFXCommand {
                OFXFileName = file.FileName
            });

            return this.View("Transactions", financialExchangeModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
