using CleverCore.Application.ViewModels.Product;
using System.Collections.Generic;
using System.IO;

namespace CleverCore.WebApp.Services
{
    public interface IExcelService
    {
        void WriteExcel(FileInfo file, List<ProductViewModel> products);
    }
}
