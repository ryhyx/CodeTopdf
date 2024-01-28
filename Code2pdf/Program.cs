using System;
using System.IO;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

class Program
{
    static void Main()
    {
        // Ask the user for the source directory
        Console.Write("Enter the path of the source code directory: ");
        string sourceDirectory = Console.ReadLine();

        // Ask the user for the destination directory
        Console.Write("Enter the path of the destination directory for PDFs: ");
        string outputDirectory = Console.ReadLine();

        // Validate source directory
        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine("Source directory does not exist. Exiting.");
            return;
        }

        // Validate and create the output directory
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // Process files in the source directory
        string[] filepaths = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
        int fileCount = 1;

        foreach (var filepath in filepaths)
        {
            ConvertToPdf(filepath, outputDirectory, fileCount++);
        }

        Console.WriteLine("Conversion complete. Press any key to exit.");
        Console.ReadKey();
    }

    static void ConvertToPdf(string filepath, string outputDirectory, int fileNumber)
    {
        if (filepath.EndsWith(".cs") || filepath.EndsWith(".cshtml") || filepath.EndsWith(".js"))
        {
            string outputFileName = $"{fileNumber:D4}_{Path.GetFileName(filepath)}.pdf";
            string outputFilePath = Path.Combine(outputDirectory, outputFileName);

            using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
            {
                PdfWriter writer = new PdfWriter(fs);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Set A4 landscape size
                pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());

                // Add file address
                document.Add(new Paragraph($"File: {filepath}").SetBold().SetFontSize(10).SetItalic());

                // Read your file content and format it for PDF
                string[] fileLines = File.ReadAllLines(filepath);
                for (int lineNumber = 1; lineNumber <= fileLines.Length; lineNumber++)
                {
                    string line = $"{lineNumber,4}: {fileLines[lineNumber - 1]}";
                    document.Add(new Paragraph(line));
                }

                // Close the document
                document.Close();
            }
        }
    }
}
