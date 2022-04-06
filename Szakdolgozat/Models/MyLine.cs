namespace Szakdolgozat.Models
{
    public class MyLine
    {
        public MyLine(string line)
        {
            this.line = line;
        }

        public string line { get; set; }
        public string color { get; set; } = "white";

        private void ChangeColor()
        {

        }
    }
}
