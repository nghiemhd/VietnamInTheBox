
using System.IO;

namespace Nop.Services.ExportImport
{
    /// <summary>
    /// Import manager interface
    /// </summary>
    public partial interface IImportManager
    {
        /// <summary>
        /// Import products from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        void ImportProductsFromXlsx(Stream stream);
        void Aero87ImportProductsFromXlsx(Stream stream, bool editProduct, bool editInvoice, bool isPublished, int storeId);
    }
}
