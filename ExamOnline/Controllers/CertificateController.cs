using Application.Core.Domain.Interface;
using Application.Core.DTO.CertificateDto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExamOnline.Controllers
{
    [Route("Certificate")]
    public class CertificateController : Controller
    {
        private readonly ICertificate _Certificate;
        public CertificateController(ICertificate certificate)
        { 
          _Certificate = certificate;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetCertificate/{Id}")]
        public async Task<IActionResult> GetCertificate(Guid Id)
        {
            CertificateDetailsDTO? Result = await _Certificate.GetCertificate(Id);
            if (Result == null) return NotFound();
            return View(Result);
        }
    }
}
