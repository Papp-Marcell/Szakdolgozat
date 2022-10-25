namespace Szakdolgozat.Models
{
    //Stores information about text lines
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
