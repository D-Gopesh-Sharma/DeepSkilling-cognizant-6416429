using System;
using System.Collections.Generic;

namespace FactoryMethodPatternExample
{
    public enum DocumentType
    {
        Word,
        PDF,
        Excel
    }

    public abstract class Document
    {
        public string Name { get; protected set; }
        public string Extension { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public long Size { get; protected set; }

        protected Document(string name)
        {
            Name = name;
            Extension = string.Empty;
            CreatedDate = DateTime.Now;
        }
        public abstract void Open();
        public abstract void Save();
        public abstract void Close();
        public abstract void Print();
        public abstract string GetDocumentInfo();
    }

    public class WordDocument : Document
    {
        public int WordCount { get; private set; }
        public int PageCount { get; private set; }

        public WordDocument(string name) : base(name)
        {
            Extension = ".docx";
            Size = 1024 * 50;
            WordCount = 1000;
            PageCount = 3;
        }

        public override void Open()
        {
            Console.WriteLine($"Opening Word document: {Name}{Extension}");
            Console.WriteLine("Microsoft Word is launching...");
        }

        public override void Save()
        {
            Console.WriteLine($"Saving Word document: {Name}{Extension}");
            Console.WriteLine("Document saved in Word format");
        }

        public override void Close()
        {
            Console.WriteLine($"Closing Word document: {Name}{Extension}");
            Console.WriteLine("Microsoft Word document closed");
        }

        public override void Print()
        {
            Console.WriteLine($"Printing Word document: {Name}{Extension}");
            Console.WriteLine($"Printing {PageCount} pages...");
        }

        public override string GetDocumentInfo()
        {
            return $"Word Document - Name: {Name}{Extension}, Size: {Size} bytes, " +
                   $"Words: {WordCount}, Pages: {PageCount}, Created: {CreatedDate:yyyy-MM-dd HH:mm:ss}";
        }

        public void CheckSpelling()
        {
            Console.WriteLine("Running spell check on Word document...");
        }
    }

    public class PdfDocument : Document
    {
        public int PageCount { get; private set; }
        public bool IsPasswordProtected { get; private set; }

        public PdfDocument(string name) : base(name)
        {
            Extension = ".pdf";
            Size = 1024 * 200;
            PageCount = 10;
            IsPasswordProtected = false;
        }

        public override void Open()
        {
            Console.WriteLine($"Opening PDF document: {Name}{Extension}");
            Console.WriteLine("PDF viewer is launching...");
        }

        public override void Save()
        {
            Console.WriteLine($"Saving PDF document: {Name}{Extension}");
            Console.WriteLine("Document saved in PDF format");
        }

        public override void Close()
        {
            Console.WriteLine($"Closing PDF document: {Name}{Extension}");
            Console.WriteLine("PDF viewer closed");
        }

        public override void Print()
        {
            Console.WriteLine($"Printing PDF document: {Name}{Extension}");
            Console.WriteLine($"Printing {PageCount} pages in high quality...");
        }

        public override string GetDocumentInfo()
        {
            return $"PDF Document - Name: {Name}{Extension}, Size: {Size} bytes, " +
                   $"Pages: {PageCount}, Password Protected: {IsPasswordProtected}, " +
                   $"Created: {CreatedDate:yyyy-MM-dd HH:mm:ss}";
        }

        public void SetPassword(string password)
        {
            IsPasswordProtected = !string.IsNullOrEmpty(password);
            Console.WriteLine($"PDF password protection: {(IsPasswordProtected ? "Enabled" : "Disabled")}");
        }
    }

    public class ExcelDocument : Document
    {
        public int WorksheetCount { get; private set; }
        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public ExcelDocument(string name) : base(name)
        {
            Extension = ".xlsx";
            Size = 1024 * 75;
            WorksheetCount = 3;
            RowCount = 1000;
            ColumnCount = 26;
        }

        public override void Open()
        {
            Console.WriteLine($"Opening Excel document: {Name}{Extension}");
            Console.WriteLine("Microsoft Excel is launching...");
        }

        public override void Save()
        {
            Console.WriteLine($"Saving Excel document: {Name}{Extension}");
            Console.WriteLine("Document saved in Excel format");
        }

        public override void Close()
        {
            Console.WriteLine($"Closing Excel document: {Name}{Extension}");
            Console.WriteLine("Microsoft Excel document closed");
        }

        public override void Print()
        {
            Console.WriteLine($"Printing Excel document: {Name}{Extension}");
            Console.WriteLine($"Printing {WorksheetCount} worksheets...");
        }

        public override string GetDocumentInfo()
        {
            return $"Excel Document - Name: {Name}{Extension}, Size: {Size} bytes, " +
                   $"Worksheets: {WorksheetCount}, Rows: {RowCount}, Columns: {ColumnCount}, " +
                   $"Created: {CreatedDate:yyyy-MM-dd HH:mm:ss}";
        }

        public void CalculateFormulas()
        {
            Console.WriteLine("Calculating Excel formulas...");
        }
    }

    public abstract class DocumentFactory
    {
        public abstract Document CreateDocument(string name);

        public Document ProcessDocument(string name)
        {
            Console.WriteLine($"Processing document creation for: {name}");
            
            Document document = CreateDocument(name);
            
            Console.WriteLine($"Document created: {document.GetDocumentInfo()}");
            
            return document;
        }
    }

    public class WordDocumentFactory : DocumentFactory
    {
        public override Document CreateDocument(string name)
        {
            Console.WriteLine("Creating Word document using WordDocumentFactory");
            return new WordDocument(name);
        }
    }

    public class PdfDocumentFactory : DocumentFactory
    {
        public override Document CreateDocument(string name)
        {
            Console.WriteLine("Creating PDF document using PdfDocumentFactory");
            return new PdfDocument(name);
        }
    }

    public class ExcelDocumentFactory : DocumentFactory
    {
        public override Document CreateDocument(string name)
        {
            Console.WriteLine("Creating Excel document using ExcelDocumentFactory");
            return new ExcelDocument(name);
        }
    }

    public class DocumentManager
    {
        private readonly Dictionary<DocumentType, DocumentFactory> _factories;

        public DocumentManager()
        {
            _factories = new Dictionary<DocumentType, DocumentFactory>
            {
                { DocumentType.Word, new WordDocumentFactory() },
                { DocumentType.PDF, new PdfDocumentFactory() },
                { DocumentType.Excel, new ExcelDocumentFactory() }
            };
        }

        public Document CreateDocument(DocumentType type, string name)
        {
            if (_factories.TryGetValue(type, out var factory))
            {
                return factory.ProcessDocument(name);
            }
            
            throw new ArgumentException($"Unsupported document type: {type}");
        }

        public void DisplaySupportedFormats()
        {
            Console.WriteLine("Supported document formats:");
            foreach (var format in _factories.Keys)
            {
                Console.WriteLine($"- {format}");
            }
        }
    }

    public class FactoryMethodTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== Factory Method Pattern Test ===\n");

            Console.WriteLine("Test 1: Direct Factory Usage");
            TestDirectFactoryUsage();

            Console.WriteLine("\n" + new string('=', 50) + "\n");

            Console.WriteLine("Test 2: Document Manager Usage");
            TestDocumentManager();

            Console.WriteLine("\n" + new string('=', 50) + "\n");

            Console.WriteLine("Test 3: Document Operations");
            TestDocumentOperations();
        }

        private static void TestDirectFactoryUsage()
        {
            DocumentFactory wordFactory = new WordDocumentFactory();
            DocumentFactory pdfFactory = new PdfDocumentFactory();
            DocumentFactory excelFactory = new ExcelDocumentFactory();

            Document wordDoc = wordFactory.CreateDocument("BusinessPlan");
            Document pdfDoc = pdfFactory.CreateDocument("UserManual");
            Document excelDoc = excelFactory.CreateDocument("FinancialReport");

            Console.WriteLine($"\n{wordDoc.GetDocumentInfo()}");
            Console.WriteLine($"{pdfDoc.GetDocumentInfo()}");
            Console.WriteLine($"{excelDoc.GetDocumentInfo()}");
        }

        private static void TestDocumentManager()
        {
            DocumentManager manager = new DocumentManager();
            
            manager.DisplaySupportedFormats();
            Console.WriteLine();

            Document[] documents = {
                manager.CreateDocument(DocumentType.Word, "ProjectProposal"),
                manager.CreateDocument(DocumentType.PDF, "TechnicalSpecs"),
                manager.CreateDocument(DocumentType.Excel, "BudgetAnalysis")
            };

            Console.WriteLine("\nCreated documents:");
            foreach (var doc in documents)
            {
                Console.WriteLine($"- {doc.Name}{doc.Extension}");
            }
        }

        private static void TestDocumentOperations()
        {
            DocumentManager manager = new DocumentManager();

            Document wordDoc = manager.CreateDocument(DocumentType.Word, "TestDocument");
            Console.WriteLine("\nWord Document Operations:");
            wordDoc.Open();
            if (wordDoc is WordDocument wd)
            {
                wd.CheckSpelling();
            }
            wordDoc.Save();
            wordDoc.Print();
            wordDoc.Close();

            Console.WriteLine("\n" + new string('-', 30) + "\n");

            Document pdfDoc = manager.CreateDocument(DocumentType.PDF, "SecureDoc");
            Console.WriteLine("PDF Document Operations:");
            pdfDoc.Open();
            if (pdfDoc is PdfDocument pd)
            {
                pd.SetPassword("secret123");
            }
            pdfDoc.Save();
            pdfDoc.Print();
            pdfDoc.Close();

            Console.WriteLine("\n" + new string('-', 30) + "\n");

            Document excelDoc = manager.CreateDocument(DocumentType.Excel, "DataSheet");
            Console.WriteLine("Excel Document Operations:");
            excelDoc.Open();
            if (excelDoc is ExcelDocument ed)
            {
                ed.CalculateFormulas();
            }
            excelDoc.Save();
            excelDoc.Print();
            excelDoc.Close();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Factory Method Pattern Example - Document Management System");
            Console.WriteLine("============================================================\n");

            try
            {
                FactoryMethodTest.RunTests();

                Console.WriteLine("\n=== Interactive Demo ===");
                InteractiveDemo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void InteractiveDemo()
        {
            DocumentManager manager = new DocumentManager();
            
            Console.WriteLine("\nChoose a document type to create:");
            Console.WriteLine("1. Word Document");
            Console.WriteLine("2. PDF Document");
            Console.WriteLine("3. Excel Document");
            Console.Write("Enter your choice (1-3): ");

            string input = Console.ReadLine() ?? string.Empty;
            
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 3)
            {
                DocumentType type = (DocumentType)(choice - 1);
                
                Console.Write("Enter document name: ");
                string name = Console.ReadLine() ?? string.Empty;
                
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Document doc = manager.CreateDocument(type, name);
                    Console.WriteLine($"\nSuccessfully created: {doc.GetDocumentInfo()}");
                    
                    Console.WriteLine("\nDemonstrating document operations:");
                    doc.Open();
                    doc.Save();
                    doc.Close();
                }
                else
                {
                    Console.WriteLine("Invalid document name provided.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
            }
        }
    }
}