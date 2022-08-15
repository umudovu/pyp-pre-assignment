using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using pyp_pre_assignment.Data;
using pyp_pre_assignment.Entities;

namespace pyp_pre_assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            using(var stream  = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package  = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowcount; row++)
                    {
                        Commerce commerce = new();

                        commerce.Segment = worksheet.Cells[row, 1].Value.ToString()?.Trim();
                        commerce.Country = worksheet.Cells[row,2].Value.ToString()?.Trim();
                        commerce.Product = worksheet.Cells[row,3].Value.ToString()?.Trim();
                        commerce.DiscountBand = worksheet.Cells[row, 4].Value.ToString().Trim();
                        commerce.UnitsSold = double.Parse(worksheet.Cells[row, 5].Value.ToString().Trim());
                        commerce.ManufacturingPrice = double.Parse(worksheet.Cells[row, 6].Value.ToString().Trim());
                        commerce.SalePrice = double.Parse(worksheet.Cells[row, 7].Value.ToString().Trim());
                        commerce.GrossSales = double.Parse(worksheet.Cells[row, 8].Value.ToString().Trim());
                        commerce.Discounts = double.Parse(worksheet.Cells[row, 9].Value.ToString().Trim());
                        commerce.Sales = double.Parse(worksheet.Cells[row, 10].Value.ToString().Trim());
                        commerce.COGS = double.Parse(worksheet.Cells[row, 11].Value.ToString().Trim());
                        commerce.Profit = double.Parse(worksheet.Cells[row, 12].Value.ToString().Trim());
                        commerce.Date = DateTime.Parse(worksheet.Cells[row, 13].Value.ToString().Trim());


                        await _context.Commerces.AddAsync(commerce);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
