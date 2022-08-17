using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using pyp_pre_assignment.Data;
using pyp_pre_assignment.Dtos;
using pyp_pre_assignment.Entities;
using pyp_pre_assignment.Extentions;
using pyp_pre_assignment.Helpers;

namespace pyp_pre_assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public HomeController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("download")]
        public IActionResult Download()
        {
            var path = @"C:\Users\umudo\Desktop\Asp.net\pyp-pre-assignment\pyp-pre-assignment\wwwroot\upload_template.xlsx";

            MemoryStream ms = new MemoryStream();
            var file = new FileStream(path, FileMode.Open, FileAccess.Read);
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
                file.Close();

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template.xlsx");

        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            if(!file.IsExcell()) return BadRequest("only excell");

            if(file.ExcelSize(5000)) return BadRequest("only 5 mb");

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


        [HttpGet("filter")]
        public IActionResult GetData([FromQuery] DataFilterDto dataFilter)
        {
            
            var IsEmail = dataFilter.AcceptorEmail.Split("@")[1] == "code.edu.az";

            if(!IsEmail) return BadRequest("Email only code.edu.az");

            //string type = Enum.GetName(typeof(Filter), filter);

            DateTime startDate = dataFilter.StartDate;

            DateTime endDate = dataFilter.EndDate;

            string email = dataFilter.AcceptorEmail;

            var query = _context.Commerces.Where(d => d.Date >= startDate && d.Date <= endDate);

            var datas = query.ToList();
            var mergedList = new List<DataReturnDto>();

            switch (dataFilter.Filter)
            {
                case Filter.Segment:
                    mergedList = datas.GroupBy(x => x.Segment)
                                         .Select(g => new DataReturnDto
                                         {
                                             FilterName = g.Key,
                                             Discount = g.Sum(x => x.Discounts),
                                             Profit = g.Sum(x => x.Profit),
                                             Sale = g.Sum(x => x.Sales),
                                             TotalCount = g.Count()
                                         })
                                         .ToList();

                    break;
                case Filter.Country:
                    mergedList = datas.GroupBy(x => x.Country)
                                       .Select(g => new DataReturnDto
                                       {
                                           FilterName = g.Key,
                                           Discount = g.Sum(x => x.Discounts),
                                           Profit = g.Sum(x => x.Profit),
                                           Sale = g.Sum(x => x.Sales),
                                           TotalCount = g.Count()
                                       })
                                         .ToList();

                    break;
                case Filter.Product:
                    mergedList = datas.GroupBy(x => x.Product)
                         .Select(g => new DataReturnDto
                         {
                             FilterName = g.Key,
                             Discount = g.Sum(x => x.Discounts),
                             Profit = g.Sum(x => x.Profit),
                             Sale = g.Sum(x => x.Sales),
                             TotalCount = g.Count()
                         })
                                         .ToList();
                    break;
                case Filter.Discount:
                    mergedList = datas.GroupBy(x => x.Product)
                                       .Select(g => new DataReturnDto
                                       {
                                           FilterName = g.Key,
                                           Discount = g.Sum(x => x.Discounts),
                                           Profit = g.Sum(x => x.Profit),
                                           Sale = g.Sum(x => x.Sales),
                                           TotalCount = g.Count()
                                       })
                                       .ToList();
                    break;
                default:
                    break;
            }

            string excelName = $"{dataFilter.Filter.ToString()}-report.xlsx";

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Commerces");
            

            worksheet.Cell(2, 1).Value = $"Report date :";
            worksheet.Cell(2, 2).Value = DateTime.Now.ToString("g");
            worksheet.Cell(2, 2).DataType = XLDataType.DateTime;

            worksheet.Cell(3, 1).Value = "Filter date";
            worksheet.Cell(3, 2).Value = dataFilter.StartDate.ToString("g");
            worksheet.Cell(3, 2).DataType = XLDataType.DateTime;

            worksheet.Cell(3, 3).Value = "-";
            worksheet.Cell(3, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(3, 4).Value = dataFilter.EndDate.ToString("g");
            worksheet.Cell(3, 4).DataType = XLDataType.DateTime;

            var currentRow = 7;

            worksheet.Row(currentRow).Height = 25.0;
            worksheet.Row(currentRow).Style.Font.Bold = true;
            worksheet.Row(currentRow).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            worksheet.Cell(currentRow, 1).Value = "FilterName";
            worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(currentRow, 2).Value = "Discount";
            worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(currentRow, 3).Value = "Profit";
            worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(currentRow, 4).Value = "Sale";
            worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(currentRow, 5).Value = "TotalCount";
            worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            foreach (var item in mergedList)
            {
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = item.FilterName;
                worksheet.Cell(currentRow, 2).Value = item.Discount;
                worksheet.Cell(currentRow, 3).Value = item.Profit;
                worksheet.Cell(currentRow, 4).Value = item.Sale;
                worksheet.Cell(currentRow, 5).Value = item.TotalCount;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            EmailService emailService = new EmailService(_config.GetSection("ConfirmationParams:Email").Value, _config.GetSection("ConfirmationParams:Password").Value);
            emailService.SendEmail(email,"excell","bax",excelName,content );

            Ok("getdi");
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


    }
}
