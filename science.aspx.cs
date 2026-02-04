using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class science : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            string fontPath = Server.MapPath("~/font/NotoSansDevanagari2.ttf");
            string imagePath = Server.MapPath("~/assets/img/user.png");

            if (!File.Exists(fontPath))
                throw new FileNotFoundException("Font file not found.", fontPath);
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Image file not found.", imagePath);

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font hindiFont = new Font(baseFont, 10);
                Font regular = new Font(baseFont, 10, Font.NORMAL);
                Font bold = new Font(baseFont, 10, Font.BOLD);
                Font boldUnderline = new Font(baseFont, 10, Font.BOLD | Font.UNDERLINE);

                // Heading
                Paragraph p1 = new Paragraph("BIHAR SCHOOL EXAMINATION BOARD, PATNA", bold);
                p1.Alignment = Element.ALIGN_CENTER;
                doc.Add(p1);

                Paragraph p2 = new Paragraph("INTERMEDIATE EXAMINATION", boldUnderline);
                p2.Alignment = Element.ALIGN_CENTER;
                doc.Add(p2);

                doc.Add(new Paragraph("\n"));

                // Faculty Table
                PdfPTable facultyTable = new PdfPTable(2);
                facultyTable.WidthPercentage = 50;
                facultyTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                facultyTable.SetWidths(new float[] { 1, 2 });
                facultyTable.AddCell(new PdfPCell(new Phrase("Faculty -", hindiFont)) { Border = Rectangle.NO_BORDER });
                facultyTable.AddCell(new PdfPCell(new Phrase("SCIENCE", hindiFont)) { Border = Rectangle.BOX });
                doc.Add(facultyTable);

                doc.Add(new Paragraph("\n"));

                Paragraph subHead = new Paragraph("इंटरमीडिएट कक्षा में सत्र 2024-26 में मान्यता प्राप्त शिक्षण संस्थाओं में नामांकित विद्यार्थियों के लिए", hindiFont);
                subHead.Alignment = Element.ALIGN_CENTER;
                doc.Add(subHead);

                Paragraph regTitle = new Paragraph("REGISTRATION FORM- SESSION:2024-26", boldUnderline);
                regTitle.Alignment = Element.ALIGN_CENTER;
                doc.Add(regTitle);

                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("\n"));

                // Notes
                string notes = "नोट:- (i) OFSS प्रणाली से नामांकन हेतु आपके द्वारा दी गई सूचनाओं के आधार पर आपका नामांकन विवरण इस प्रपत्र के खंड 'A' (क्रमांक- 1 से 16) में अंकित है।\n\n" +
                           "(ii) खंड 'A' (क्रमांक- 1 से 16) के अंकित विवरणों में विद्यार्थी द्वारा किसी भी प्रकार का कोई छेड़-छाड़/परिवर्तन नहीं किया जाएगा। अर्थात् क्रमांक- 1 से 16 तक में विद्यार्थी द्वारा कुछ भी नहीं लिखा जाएगा।\n\n" +
                           "(iii) विद्यार्थी द्वारा इस आवेदन प्रपत्र में मात्र खंड 'B' के बिन्दुओं को ही भरा जाएगा।";
                PdfPCell noteCell = new PdfPCell(new Phrase(notes, regular));
                noteCell.Colspan = 5;
                noteCell.Padding = 5;
                noteCell.Border = Rectangle.BOX;
                PdfPTable noteTable = new PdfPTable(1);
                noteTable.WidthPercentage = 100;
                noteTable.AddCell(noteCell);
                doc.Add(noteTable);

                doc.Add(new Paragraph("\n"));

                // Title: खंड 'A'
                PdfPTable titleTable = new PdfPTable(1);
                titleTable.WidthPercentage = 100;
                titleTable.AddCell(new PdfPCell(new Phrase("खंड - 'A'", bold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Border = Rectangle.BOX
                }); doc.Add(titleTable);

                // Start Main Table with 5 columns
               
                // Footer
               

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 80; // Set table width to 80% of the page
                float[] widths = new float[] { 50f, 50f }; // 50%/50% column widths
                table.SetWidths(widths);

                // Add rows to the table
                table.AddCell(new PdfPCell(new Phrase("1. OFSS CAF No", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("24JG968019", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("2. Category", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("Regular", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("3. College/+2 School Code", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("11031", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("4. College/+2 School Name", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("PATNA COLLEGIATE SCHOOL, PATNA", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("5. District Name", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("PATNA", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("6. Student's Name", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("AASHIKA PRAJAPATI", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("7. Mother's Name", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("RACHANASHIL KUMARI", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("8. Father's Name", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("KUNDAN KUMAR", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("9. Date of Birth", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("10. Matric/Class X Passing Board’", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });


                table.AddCell(new PdfPCell(new Phrase("11. Matric/Class X Board’s Roll Code", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("Roll Number", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("Passing Yea", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("Gender", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });
             
                table.AddCell(new PdfPCell(new Phrase("Caste Category", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("Differently abled", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("Nationality", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                table.AddCell(new PdfPCell(new Phrase("Religion", FontFactory.GetFont("Arial", 10, Font.BOLD))) { BorderWidthRight = 0 });
                table.AddCell(new PdfPCell(new Phrase("07/01/2009", FontFactory.GetFont("Arial", 10))) { BorderWidthLeft = 0 });

                // Create an outer table to hold the main table and the image
                PdfPTable outerTable = new PdfPTable(2);
                outerTable.WidthPercentage = 100;
                outerTable.SetWidths(new float[] { 70f, 30f });

                // Add the main table to the left
                PdfPCell tableCell = new PdfPCell(table);
                tableCell.BorderWidthRight = 0;
                outerTable.AddCell(tableCell);

                // Add the image and date to the right
                PdfPCell imageCell = new PdfPCell();
                imageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                // Load the image (replace "path/to/image.jpg" with the actual path)
                // Map the virtual path to a physical path
                
                // Load the image using Server.MapPath
                string imagePath1 = HttpContext.Current.Server.MapPath("~/assets/img/user.png"); // Ensure the image exists
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath1);
                img.ScaleToFit(100f, 100f); // Adjust size as needed
                imageCell.AddElement(img);

                // Add the date below the image
               

                outerTable.AddCell(imageCell);
                // Add the outer table to the document
                doc.Add(outerTable);

                doc.Close();

                // Return PDF to browser
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=BSEB_Registration_Form.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            Response.Clear();
            Response.Write("Error generating PDF: " + ex.Message);
            Response.Flush();
        }
    }

    private void AddRow(PdfPTable table, string c1, string v1, string c2, string v2, Font font, Font bold)
    {
        table.AddCell(new PdfPCell(new Phrase(c1, font)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(v1, bold)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(c2, font)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(v2, bold)) { Padding = 4 });
    }

}
