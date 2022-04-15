using Szakdolgozat.Models;
using Szakdolgozat.Services;

namespace Szakdolgozat.Services
{
    public class Analysis
    {
        MyFile myFile;
        HardwareService hardwareService=new HardwareService();
        public List<MyElement> myElements= new List<MyElement>();

        public void Analyze(MyFile file)
        {
            myFile = file;
            int CUs;
            if (Search("CL_DEVICE_TYPE_GPU"))
            {
                CUs = 1;
            }
            else
            {
                CUs = (int)(uint)hardwareService.CPU["NumberOfLogicalProcessors"];
            }
            SearchAdd("cl_mem", "CL Memory Allocation", 0);
        }
        private void SearchAdd(string search,string name,int computeUnit)
        {
            foreach (var item in myFile.Lines)
            {
                if (item.line.Contains(search))
                {
                    myElements.Add(new MyElement(name, myFile.Lines.IndexOf(item), computeUnit));
                }
            }
        }
        private bool Search(string search)
        {
            foreach(var item in myFile.Lines)
            {
                if (item.line.Contains(search, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

    
}
