using EFMVC.Web.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;

namespace EFMVC.Web.EFWebPDF
{
    public class GeneratePDF
    {
        private readonly CurrencyConversion _currencyConversion;

        public GeneratePDF(CurrencyConversion currencyConversion)
        {
            _currencyConversion = currencyConversion;
        }

        public Invoice Invoice { get; set; }
        public string CreatePDF(string path, string fromCurrencyCode, string toCurrencyCode)
        {
           

            string finalpath = path + "/Adtones_invoice_" + Invoice.InvoiceNumber + ".pdf";

            FileStream fileStream = new FileStream(finalpath,
                                                    FileMode.Create,
                                                    FileAccess.Write,
                                                    FileShare.None);

            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fileStream);
            PdfPTable saleTable = SaleTable(writer,doc, fromCurrencyCode, toCurrencyCode);
            Rectangle pageSize = writer.PageSize;
            doc.Open();

            //Image Singature
            // 21-06-2019
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Invoice.Imagepath);

            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Invoice.Imagepath);

            //// logo.SetAbsolutePosition((PageSize.A4.Width - logo.ScaledWidth) / 2, (PageSize.A4.Height - logo.ScaledHeight) / 2); // Add Code on 25-03-2019 For Logo Center
            ////logo.ScaleToFit(120f, 120f);
            //logo.ScaleToFit(130f, 130f);
            //logo.Alignment = Element.ALIGN_RIGHT;
            //doc.Add(logo);

            ////Title
            //Paragraph title = new Paragraph("Invoice: " + Invoice.InvoiceNumber);
            //title.Alignment = Element.ALIGN_LEFT;

            //Paragraph ponumber = new Paragraph("PO Number: " + Invoice.PONumber);
            //ponumber.Alignment = Element.ALIGN_LEFT;


            //Paragraph methodofpayment = new Paragraph("Payment method: " + Invoice.MethodOfPayment);
            //methodofpayment.Alignment = Element.ALIGN_LEFT;



            //Paragraph date = new Paragraph("Date: " + Invoice.Date.ToString("dd/MM/yyyy"));
            //date.Alignment = Element.ALIGN_LEFT;

            ////Code Commented on 25-03-2019
            ////Paragraph duedate = new Paragraph("Due Date: " + Invoice.SettledDate.ToString("dd/MM/yyyy"));



            //doc.Add(title);
            //doc.Add(ponumber);
            //if (Invoice.typeOfPayment != 1 && Invoice.typeOfPayment != null)
            //{
            //    doc.Add(methodofpayment);
            //}
            //doc.Add(date);

            //if (Invoice.SettledDate != null)
            //{
            //    Paragraph duedate = new Paragraph("Due Date: " + Invoice.SettledDate.Value.ToString("dd/MM/yyyy"));
            //    duedate.Alignment = Element.ALIGN_LEFT;
            //    doc.Add(duedate);
            //}
            PdfPTable tableLayout = new PdfPTable(3);
            float[] headers = {100,30,50 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage

            var logo = iTextSharp.text.Image.GetInstance(Invoice.Imagepath);
            logo.ScaleToFit(130f, 130f);

            iTextSharp.text.Font font = FontFactory.GetFont("Verdana", 12);
            //Add Title to the PDF file at the top
            tableLayout.AddCell(new PdfPCell(logo) { Colspan = 3, Border = 0, PaddingBottom = 1, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidthBottom = 0, PaddingRight = 105 });

            tableLayout.AddCell(new PdfPCell(new Paragraph("", font)) { Border = 0});
            tableLayout.AddCell(new PdfPCell(new Paragraph("Invoice:", font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Paragraph(Invoice.InvoiceNumber, font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Paragraph("", font)) { Border = 0 });
            tableLayout.AddCell(new PdfPCell(new Paragraph("PO Number:", font)) {  Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Paragraph(Invoice.PONumber, font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            if (Invoice.typeOfPayment != 1 && Invoice.typeOfPayment != null)
            {
                tableLayout.AddCell(new PdfPCell(new Paragraph("", font)) { Border = 0 });
                tableLayout.AddCell(new PdfPCell(new Paragraph("Payment method:", font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                tableLayout.AddCell(new PdfPCell(new Paragraph(Invoice.MethodOfPayment, font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            }
            tableLayout.AddCell(new PdfPCell(new Paragraph("", font)) { Border = 0 });
            tableLayout.AddCell(new PdfPCell(new Paragraph("Date:", font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Paragraph(Invoice.Date.ToString("dd/MM/yyyy"), font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });

            if (Invoice.SettledDate != null)
            {
                tableLayout.AddCell(new PdfPCell(new Paragraph("", font)) { Border = 0 });
                tableLayout.AddCell(new PdfPCell(new Paragraph("Due Date:", font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                tableLayout.AddCell(new PdfPCell(new Paragraph(Invoice.SettledDate.Value.ToString("dd/MM/yyyy"), font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            }
            doc.Add(tableLayout);
            //tableLayout.WriteSelectedRows(0, -1, 135, 150 , writer.DirectContent);
            //doc.Add(new Paragraph("\n"));
            /////// Laying out the document

            //Company Address
            // Chunk Addess = new Chunk("Address :", FontFactory.GetFont("dax-black"));
            // doc.Add(new Paragraph(Addess));
            if ((Invoice.typeOfPayment == 6 || Invoice.typeOfPayment == 1 )&& Invoice.CountryId == (int)CountryList.Kenya)
            {
                Paragraph companyName = new Paragraph("Adtones East Africa Ltd");
                doc.Add(companyName);
                doc.Add(new Paragraph("7th Floor, The Address,"));
                doc.Add(new Paragraph("Muthangari Drive,"));
                doc.Add(new Paragraph("PO Box 13924-00800"));
                doc.Add(new Paragraph("Nairobi"));
                doc.Add(new Paragraph("Phone:+254790727201"));
                doc.Add(new Paragraph("PIN: P051763249L"));
                // doc.Add(new Paragraph("Tax Number: xxxxxx"));
            }

            //ILW - 2020.09.17  the below code condition is a duplicate of the above code -  if there needs to be a condition to catch anything other than senegal or kenya then it shouldn't have kenya's details.

            //if (Invoice.typeOfPayment == 6 || (Invoice.typeOfPayment == 1 && Invoice.CountryId != (int)CountryList.Senegal))
            //{
            //    Paragraph companyName = new Paragraph("Adtones East Africa Ltd");
            //    doc.Add(companyName);
            //    doc.Add(new Paragraph("7th Floor, The Address,"));
            //    doc.Add(new Paragraph("Muthangari Drive,"));
            //    doc.Add(new Paragraph("PO Box 13924-00800"));
            //    doc.Add(new Paragraph("Nairobi"));
            //    doc.Add(new Paragraph("Phone:+254790727201"));
            //    doc.Add(new Paragraph("PIN: P051763249L"));

            //    //Paragraph companyName = new Paragraph("Adtones East Africa Ltd");
            //    //doc.Add(companyName);
            //    //doc.Add(new Paragraph("The Riverfront Building"));
            //    ////doc.Add(new Paragraph("Nairobi W1K 2XF"));
            //    //doc.Add(new Paragraph("Riverside Drive Road"));
            //    //doc.Add(new Paragraph("PO Box 13924-00 800"));
            //    //doc.Add(new Paragraph("Nairobi"));
            //    //doc.Add(new Paragraph("Phone:+254790727201"));
            //    //doc.Add(new Paragraph("PIN: P051763249L"));
            //    //// doc.Add(new Paragraph("Tax Number: xxxxxx"));
            //}
            else if (Invoice.typeOfPayment == 2 || Invoice.typeOfPayment == 3 || Invoice.typeOfPayment == 1 )
            { 
                Paragraph companyName = new Paragraph("Adtones Ltd");
                doc.Add(companyName);
                doc.Add(new Paragraph("35a South Street"));
                doc.Add(new Paragraph("London W1K 2XF"));
                doc.Add(new Paragraph("United Kingdom"));
                doc.Add(new Paragraph("Phone: +44 (0) 333 444 2044"));
                doc.Add(new Paragraph("Company Number: 09515191"));
                // doc.Add(new Paragraph("Tax Number: xxxxxx"));
            }
            doc.Add(new Paragraph("__________________________"));

            //Customer Details
            Chunk InvoiceTo = new Chunk("Invoice To:", FontFactory.GetFont("dax-black"));
            doc.Add(new Paragraph(InvoiceTo));
            doc.Add(new Paragraph(Invoice.Customer.FullName));
            doc.Add(new Paragraph(Invoice.Customer.AddressLine1));
            doc.Add(new Paragraph(Invoice.Customer.City));
            doc.Add(new Paragraph(Invoice.Customer.Postcode));
            doc.Add(new Paragraph(Invoice.Customer.Country));            
            doc.Add(new Paragraph("Phone: " + Invoice.Customer.PhoneNumber));
            doc.Add(new Paragraph("Email: " + Invoice.Customer.Email));

            Paragraph separator = new Paragraph("_____________________________________________________________________________      ");
            separator.SpacingAfter = 5.5f;
            doc.Add(separator);


            //Table and total
         //   doc.Add(saleTable);


            doc.Close();
            fileStream.Close();
            return finalpath;
        }

        private PdfPTable SaleTable(PdfWriter writer,Document doc, string fromCurrencyCode, string toCurrencyCode)
        {
            doc.Open();
            PdfContentByte pcb = writer.DirectContent;
            // Get Items from Invoice
         
            List<Item> itemList = Invoice.ItemList;
            //Item item = itemList[0];

            //produce table and set its props, widths are fractions
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 500f;
            float[] widths = new float[] { 4f, 1f, 3.5f, 3.4f, 2.5f };
            //if (Invoice.CountryId == 9)
            //{
            //    widths = new float[] { 4f, 1f, 3.5f, 3f, 2f };
            //}

            table.SetWidths(widths);
            //Font verdanaBold = FontFactory.GetFont("Verdana", 7f, FontRectangle.TOP_BORDER.BOLD);
            //Add Headers
            //table.AddCell(HeaderCell("Item"));
            //Chunk Description = new Chunk("Description", verdanaBold);
            table.AddCell(new PdfPCell(new Phrase("Description", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER , HorizontalAlignment = Element.ALIGN_LEFT });
            table.AddCell(new PdfPCell(new Phrase("Qty", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER});
            table.AddCell(new PdfPCell(new Phrase("Advertiser", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER });
            if (Invoice.CurrencySymbol.ToString().Substring(0, 1) == "(")
            {
                table.AddCell(new PdfPCell(new Phrase("Price " + Invoice.CurrencySymbol.ToString(), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER });
            }else
            {
                table.AddCell(new PdfPCell(new Phrase("Price " + "(" + Invoice.CurrencySymbol.ToString() + ")", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER });
            }

            if (Invoice.CurrencySymbol.ToString().Substring(0,1) == "(")
            {                
                table.AddCell(new PdfPCell(new Phrase("Total " + Invoice.CurrencySymbol.ToString(), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER });
            }else
            {
                table.AddCell(new PdfPCell(new Phrase("Total " + "(" + Invoice.CurrencySymbol.ToString() + ")", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.BOLD))) { Border = Rectangle.NO_BORDER });
            }

            //table.AddCell(HeaderCell("Description"));
            //table.AddCell(HeaderCell("Qty"));
            //table.AddCell(HeaderCell("Price"));           
            //table.AddCell(HeaderCell("Total"));

            //Add the data from individual items
            CurrencyModel currencyModel = new CurrencyModel();
            decimal currencyRate = 0.00M;
            currencyModel = _currencyConversion.ForeignCurrencyConversion("1", toCurrencyCode, fromCurrencyCode);
            currencyRate = currencyModel.Amount;
            foreach (var item in itemList)
            {
                // table.AddCell(QuantityCell((itemList.IndexOf(item) + 1).ToString() + "."));
                table.AddCell(QuantityCellNew(item.Description));
                table.AddCell(QuantityCell(item.Quantity.ToString()));
                //table.AddCell(PriceCell(string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", item.Price)));
                table.AddCell(QuantityCell(item.Organisation.ToString()));
                //if (Invoice.CountryId == 9)
                //{
                //    table.AddCell(new PdfPCell(new Phrase(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.Price.ToString("F")))) { Border = Rectangle.NO_BORDER });
                //    // table.AddCell(PriceCell(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.Price.ToString("F"))));
                //}
                //else
                //{
                //    table.AddCell(new PdfPCell(new Phrase(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.Price.ToString("F")))) { Border = Rectangle.NO_BORDER });
                //    // table.AddCell(PriceCell(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.Price.ToString("F"))));
                //}
                table.AddCell(new PdfPCell(new Phrase(string.Format(Math.Round(Convert.ToDouble(Convert.ToDecimal(item.Price) * currencyRate),2).ToString("F")))) { Border = Rectangle.NO_BORDER });
                //table.AddCell(PriceCell(string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", item.ItemTotal())));
                //if (Invoice.CountryId == 9)
                //{
                //    table.AddCell(new PdfPCell(new Phrase(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.ItemTotal().ToString("F")))) { Border = Rectangle.NO_BORDER });
                //    // table.AddCell(PriceCell(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.ItemTotal().ToString("F"))));
                //}
                //else
                //{
                //    table.AddCell(new PdfPCell(new Phrase(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.ItemTotal().ToString("F")))) { Border = Rectangle.NO_BORDER });
                //    // table.AddCell(PriceCell(string.Format(Invoice.CurrencySymbol.ToString() + " " + item.ItemTotal().ToString("F"))));
                //}
                table.AddCell(new PdfPCell(new Phrase(string.Format(Math.Round(Convert.ToDouble(Convert.ToDecimal(item.ItemTotal()) * currencyRate),2).ToString("F")))) { Border = Rectangle.NO_BORDER });
            }


            // Grand Total Cell/s
            //PdfPCell subtotal = new PdfPCell(new Phrase(string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", Invoice.GetSubtotal())));
            //subtotal.Border = Rectangle.TOP_BORDER;
            //subtotal.HorizontalAlignment = 2;

            PdfPCell stText = new PdfPCell(new Phrase("Subtotal:"));
            stText.HorizontalAlignment = Element.ALIGN_LEFT;
            stText.Colspan = 3;
            stText.PaddingTop = 5;
            stText.PaddingLeft = 160;
            stText.Border = Rectangle.NO_BORDER;

            table.AddCell(BlankCell());
            table.AddCell(stText);
            //table.AddCell(subtotal);
            //if (Invoice.CountryId == 9)
            //{
            //    table.AddCell(SubTotalCell(string.Format(Invoice.CurrencySymbol.ToString() + " " + Invoice.GetSubtotal().ToString("F"))));
            //}
            //else
            //{
            //    table.AddCell(SubTotalCell(string.Format(Invoice.CurrencySymbol.ToString() + " " + Invoice.GetSubtotal().ToString("F"))));
            //}
            //table.AddCell(SubTotalCell(string.Format(Invoice.GetSubtotal().ToString("F"))));
            table.AddCell(new PdfPCell(new Phrase(Math.Round(Convert.ToDouble(Convert.ToDecimal(Invoice.GetSubtotal()) * currencyRate),2).ToString())) { Border = Rectangle.NO_BORDER,PaddingTop = 5 });

            //PdfPCell vatCell = new PdfPCell(new Phrase(string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", Invoice.GetVATAmount())));
            //vatCell.Border = Rectangle.TOP_BORDER;
            //vatCell.HorizontalAlignment = 2;

            //Code Commented on 25-03-2019
            // PdfPCell vatText = new PdfPCell(new Phrase("Tax ("+Invoice.InvoiceTax+"% for the "+Invoice.InvoiceCountry+"):"));
            PdfPCell vatText = new PdfPCell(new Phrase("VAT @ " + Invoice.InvoiceTax + "%"));
            vatText.HorizontalAlignment = Element.ALIGN_LEFT;
            vatText.Colspan = 3;
            vatText.PaddingTop = 5;
            vatText.PaddingLeft = 160;
            vatText.Border = Rectangle.NO_BORDER;

            table.AddCell(BlankCell());
            table.AddCell(vatText);
            //table.AddCell(vatCell);
            //if (Invoice.CountryId == 9)
            //{
            //    table.AddCell(VATAmount(string.Format(Invoice.CurrencySymbol.ToString() + " " + Invoice.GetVATAmount().ToString("F"))));
            //}
            //else
            //{
            //    table.AddCell(VATAmount(string.Format(Invoice.CurrencySymbol.ToString() + " " + Invoice.GetVATAmount().ToString("F"))));
            //}
            //table.AddCell(VATAmount(string.Format(Invoice.GetVATAmount().ToString("F"))));
            table.AddCell(new PdfPCell(new Phrase(Math.Round(Convert.ToDouble(Convert.ToDecimal(Invoice.GetVATAmount() * currencyRate)),2).ToString())) { Border = Rectangle.NO_BORDER, PaddingTop = 5 });
            //PdfPCell grandTotal = new PdfPCell(new Phrase(string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", Invoice.GetTotal())));
            //grandTotal.Border = Rectangle.TOP_BORDER;
            //grandTotal.HorizontalAlignment = 2;



            if (Invoice.CurrencySymbol.ToString().Substring(0, 1) == "(")
            {
                PdfPCell gtText = new PdfPCell(new Phrase("Grand Total " + Invoice.CurrencySymbol.ToString() + ":"));
                gtText.HorizontalAlignment = Element.ALIGN_LEFT;
                gtText.Colspan = 3;
                gtText.PaddingTop = 5;
                gtText.Border = Rectangle.NO_BORDER;
                gtText.PaddingLeft = 160;
                table.AddCell(BlankCell());
                table.AddCell(gtText);
            }
            else
            {
                PdfPCell gtText = new PdfPCell(new Phrase("Grand Total (" + Invoice.CurrencySymbol.ToString()+") :"));
                gtText.HorizontalAlignment = Element.ALIGN_LEFT;
                gtText.Colspan = 3;
                gtText.PaddingTop = 5;
                gtText.Border = Rectangle.NO_BORDER;
                gtText.PaddingLeft = 160;
                table.AddCell(BlankCell());
                table.AddCell(gtText);
            }




            //table.AddCell(grandTotal);
            //if (Invoice.CountryId == 9)
            //{
            //    table.AddCell(GrandTotal(string.Format(Invoice.CurrencySymbol.ToString() + " " + Invoice.GetTotal().ToString("F"))));
            //}
            //else
            //{
            //    table.AddCell(GrandTotal(string.Format(Invoice.CurrencySymbol.ToString() + " " + Invoice.GetTotal().ToString("F"))));
            //}
            //table.AddCell(GrandTotal(string.Format(Invoice.GetTotal().ToString("F"))));

            //if (currencyModel.Code == "OK")
            //{
               
            //    _model.Fundamount = Convert.ToDecimal(Invoice.GetTotal().ToString());
            //    var taxpercantage = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage;
            //    var totaltaxamount = (_model.Fundamount) * (taxpercantage / 100);
            //    var final_amount = Convert.ToDecimal(FundAmount.ToString()) + totaltaxamount;
            //    _model.TaxPercantage = taxpercantage;
            //    _model.TotalAmount = final_amount;
            //}

            table.AddCell(new PdfPCell(new Phrase(Math.Round(Convert.ToDouble(Convert.ToDecimal(Invoice.GetTotal() * currencyRate)), 2).ToString("F"))) { Border = Rectangle.NO_BORDER, PaddingTop = 5 });

            var remainingPageSpace = writer.GetVerticalPosition(false) - doc.BottomMargin;

            table.WriteSelectedRows(0, -1, 35, remainingPageSpace - 440, writer.DirectContent);
            // table.WriteSelectedRows(0, -1, 500, 200, pcb);

            return table;
        }

        private PdfPHeaderCell HeaderCell(string CellContent)
        {
            PdfPHeaderCell cell = new PdfPHeaderCell();
            cell.BackgroundColor = CMYKColor.LIGHT_GRAY;
            cell.HorizontalAlignment = 1;
            cell.Phrase = new Phrase(CellContent);
            return cell;
        }

        private PdfPCell PriceCell(string price)
        {
            PdfPCell cell = new PdfPCell();
            //cell.HorizontalAlignment = 2;
           // cell.Border = Rectangle.RIGHT_BORDER;
            cell.Border = Rectangle.NO_BORDER;           
            cell.Phrase = new Phrase(price);
            return cell;
        }

        private PdfPCell SubTotalCell(string subtotal)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            //cell.HorizontalAlignment = 2;
            cell.Phrase = new Phrase(subtotal);

            return cell;
        }

        private PdfPCell VATAmount(string vatamount)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
            //  cell.HorizontalAlignment = 2;
            cell.Phrase = new Phrase(vatamount);

            return cell;
        }

        private PdfPCell GrandTotal(string grandtotal)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.NO_BORDER;
           // cell.HorizontalAlignment = 2;
            cell.Phrase = new Phrase(grandtotal);

            return cell;
        }

        private PdfPCell QuantityCell(string quantity)
        {
            PdfPCell cell = new PdfPCell();
           // cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            cell.Phrase = new Phrase(quantity);

            return cell;
        }

        private PdfPCell QuantityCellNew(string quantity)
        {
            PdfPCell cell = new PdfPCell();
            // cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            cell.Phrase = new Phrase(quantity + " Campaign");

            return cell;
        }

        private PdfPCell BlankCell()
        {
            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Border = Rectangle.NO_BORDER;

            return cell;
        }

    }
}