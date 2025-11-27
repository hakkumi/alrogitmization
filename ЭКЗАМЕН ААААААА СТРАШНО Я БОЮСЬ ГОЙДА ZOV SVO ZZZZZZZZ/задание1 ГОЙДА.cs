using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    abstract class Document
    {
        public abstract void Open();
        public abstract void Save();
        public abstract void Export(string format);
        public abstract void Print();
        public abstract bool Search(string text);
        public abstract string GetMetadata();
    }

    class TextDocument : Document
    {
        private string content;

        public TextDocument(string content)
        {
            this.content = content;
        }

        public override void Open()
        {
            Console.WriteLine($"[Text] Opened: {content}");
        }

        public override void Save()
        {
            Console.WriteLine("[Text] Saved");
        }

        public override void Export(string format)
        {
            Console.WriteLine($"[Text] Exported to {format}");
        }

        public override void Print()
        {
            Console.WriteLine($"[Text] Printing: {content}");
        }

        public override bool Search(string text)
        {
            return content.Contains(text);
        }

        public override string GetMetadata()
        {
            return $"TextDocument (Length={content.Length})";
        }
    }

    class SpreadsheetDocument : Document
    {
        private string[,] cells;

        public SpreadsheetDocument(int rows, int cols)
        {
            cells = new string[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    cells[i, j] = "val";
        }

        public override void Open()
        {
            Console.WriteLine("[Spreadsheet] Opened");
        }

        public override void Save()
        {
            Console.WriteLine("[Spreadsheet] Saved");
        }

        public override void Export(string format)
        {
            Console.WriteLine($"[Spreadsheet] Exported to {format}");
        }

        public override void Print()
        {
            Console.WriteLine($"[Spreadsheet] Printing ({cells.GetLength(0)}x{cells.GetLength(1)})");
        }

        public override bool Search(string text)
        {
            foreach (var cell in cells)
                if (cell == text)
                    return true;

            return false;
        }

        public override string GetMetadata()
        {
            return $"SpreadsheetDocument (Rows={cells.GetLength(0)}, Cols={cells.GetLength(1)})";
        }
    }

    class PresentationDocument : Document
    {
        private int slides;

        public PresentationDocument(int slides)
        {
            this.slides = slides;
        }

        public override void Open()
        {
            Console.WriteLine($"[Presentation] Opened ({slides} slides)");
        }

        public override void Save()
        {
            Console.WriteLine("[Presentation] Saved");
        }

        public override void Export(string format)
        {
            Console.WriteLine($"[Presentation] Exported to {format}");
        }

        public override void Print()
        {
            Console.WriteLine($"[Presentation] Printing {slides} slides");
        }

        public override bool Search(string text)
        {
            return text == "slide";
        }

        public override string GetMetadata()
        {
            return $"PresentationDocument (Slides={slides})";
        }
    }

    class PDFDocument : Document
    {
        public override void Open()
        {
            Console.WriteLine("[PDF] Opened");
        }

        public override void Save()
        {
            Console.WriteLine("[PDF] Saved");
        }

        public override void Export(string format)
        {
            Console.WriteLine($"[PDF] Exported to {format}");
        }

        public override void Print()
        {
            Console.WriteLine("[PDF] Printing");
        }

        public override bool Search(string text)
        {
            return text == "pdf";
        }

        public override string GetMetadata()
        {
            return "PDFDocument";
        }
    }

    class DocumentManager
    {
        private List<Document> documents = new List<Document>();

        public void Add(Document doc)
        {
            documents.Add(doc);
        }

        public void OpenAll()
        {
            foreach (var doc in documents)
                doc.Open();
        }

        public void PrintAll()
        {
            foreach (var doc in documents)
                doc.Print();
        }

        public void SearchAll(string text)
        {
            foreach (var doc in documents)
            {
                Console.WriteLine($"{doc.GetMetadata()} => " +
                                  (doc.Search(text) ? "FOUND" : "NOT FOUND"));
            }
        }
    }

    class Program
    {
        static void Main()
        {
            DocumentManager manager = new DocumentManager();

            // Создаем 20+ документов
            for (int i = 0; i < 10; i++)
                manager.Add(new TextDocument($"Sample text {i}"));

            for (int i = 0; i < 5; i++)
                manager.Add(new SpreadsheetDocument(5, 5));

            for (int i = 0; i < 3; i++)
                manager.Add(new PresentationDocument(10 + i));

            for (int i = 0; i < 5; i++)
                manager.Add(new PDFDocument());

            // Операции
            Console.WriteLine("=== OPEN ALL ===");
            manager.OpenAll();

            Console.WriteLine("\n=== PRINT ALL ===");
            manager.PrintAll();

            Console.WriteLine("\n=== SEARCH 'text' ===");
            manager.SearchAll("text");

            Console.WriteLine("\n=== SEARCH 'pdf' ===");
            manager.SearchAll("pdf");
        }
    }
}
