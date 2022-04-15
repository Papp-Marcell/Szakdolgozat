namespace Szakdolgozat.Models
{
    public class MyElement
    {
        public MyElement(string name, int index, int computeUnit)
        {
            Name = name;
            Index = index;
            ComputeUnit = computeUnit;
        }

        public string Name { get; set; }
        public int Index { get; set; }
        public int ComputeUnit { get; set; }
    }
}
