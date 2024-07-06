using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using FinanceTracker.DAL.Dtos;
using FinanceTracker.DAL.Resources;
using FinanceTracker.DAL.Model;
using Microsoft.Extensions.Options;
namespace FinanceTracker.BLL.shared.Enum
{
    public class GeneratePdf
    {
        public static string GeneratePdfDocument(int month, int year, List<UserTransactionDto> userTransactions,string imagePath)
        {
            var formattedDate = new DateTime(year, month, 1).ToString(ResponseResources.DateFormat);
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    page.Header()
                        .ShowOnce()
                        .Column(column =>
                        {
                            column.Item()
                          .Row(row =>
                              {
                                  row.ConstantItem(120)
                                     .Image(imagePath)
                                     .FitArea();
                                  row.RelativeItem();
                              });
                            column.Item()
                                  .AlignCenter()
                                  .Padding(8)
                                  .Text(ResponseResources.TransactionReport)
                                  .SemiBold()
                                  .FontSize(20)
                                  .FontColor(Colors.Black);
                            column.Item()
                                  .AlignLeft()
                                  .PaddingBottom(6)
                                  .Text($"{ResponseResources.ContentPrefix}{formattedDate}")
                                  .SemiBold()
                                  .FontSize(12)
                                  .FontColor(Colors.Black);
                        });
                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);

                            });
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text(ResponseResources.Date);
                                header.Cell().Element(CellStyle).Text(ResponseResources.Category);
                                header.Cell().Element(CellStyle).Text(ResponseResources.Description);
                                header.Cell().Element(CellStyle).Text(ResponseResources.Income);
                                header.Cell().Element(CellStyle).Text(ResponseResources.Expense);

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.ExtraBold()).Border(1).BorderColor(Colors.Black).Background(Colors.Grey.Darken1).Padding(8).AlignCenter();
                                }
                            });

                            foreach (var transaction in userTransactions)
                            {
                                table.Cell().Element(CellStyle).Text(transaction.TransactionDate.ToString(ResponseResources.TransactionDateFormat));
                                table.Cell().Element(CellStyle).Text(transaction.Category);
                                table.Cell().Element(CellStyle).Text(transaction.Description);
                                if (transaction.CategoryTypeId == 1)
                                {
                                    table.Cell().Element(CellStyle).Text(transaction.Amount);
                                    table.Cell().Element(CellStyle).Text(ValidationResources.EmptyString);
                                }
                                else
                                {
                                    table.Cell().Element(CellStyle).Text(ValidationResources.EmptyString);
                                    table.Cell().Element(CellStyle).Text(transaction.Amount);
                                }
                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.Border(1).BorderColor(Colors.Black).Padding(5);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span(ResponseResources.FooterPage);
                            x.CurrentPageNumber();
                            x.Span(ResponseResources.FooterOf);
                            x.TotalPages();
                        });
                });
            });
            var pdfDataStream = new MemoryStream();
            document.GeneratePdf(pdfDataStream);
            pdfDataStream.Position = 0;
            var base64String = Convert.ToBase64String(pdfDataStream.ToArray());
            return base64String;
        }
    }
}