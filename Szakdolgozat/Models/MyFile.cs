namespace Szakdolgozat.Models
{
    public class MyFile
    {

        public string? Name { get; set; }
        public string? MyStreamReader { get; set; }
        public List<MyLine> Lines { get; set; } = new List<MyLine>();

        public void RemoveEmptyLines()
        {
            Lines.RemoveAll(line => string.IsNullOrEmpty(line.line));
        }
    }
}
