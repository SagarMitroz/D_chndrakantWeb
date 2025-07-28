using D_Chandrakant.DataModels;
using Microsoft.EntityFrameworkCore;

namespace D_Chandrakant
{
    public class AutocompleteService
    {

        private readonly IConfiguration _configuration;
        private readonly AutocompleteService _autocompleteService;
        private readonly ILogger<AutocompleteService> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public readonly TailordbContext _tailordbContext;

        public AutocompleteService(ILogger<AutocompleteService> logger, IWebHostEnvironment hostingEnvironment, TailordbContext tailordbContext, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _tailordbContext = tailordbContext;
            _configuration = configuration;
         
         


        }
        public IEnumerable<Emp> GetEmpByPartialName(string partialName)
        {
            return _tailordbContext.Emps.Where(s => s.Name.Contains(partialName) && s.RecStatus == "A").ToList();
        }

       

    }
}
