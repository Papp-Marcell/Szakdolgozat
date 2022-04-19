﻿using Szakdolgozat.Models;
using Szakdolgozat.Services;

namespace Szakdolgozat.Services
{
    public class Analysis
    {
        MyFile myFile;
        HardwareService hardwareService=new HardwareService();
        public List<MyElement> myElements= new List<MyElement>();
        public int CUs { get; set; }

        public void Analyze(MyFile file)
        {
            myFile = file;
  
            if (Search("CL_DEVICE_TYPE_GPU"))
            {
                CUs = 4;
            }
            else
            {
                CUs = (int)(uint)hardwareService.CPU["NumberOfLogicalProcessors"];
            }
            SearchAdd("cl_mem", "OpenCL Memory Allocation", 0,"clSet");
            SearchAdd("clGetPlatformIDs", "Platform binding", 0);
            SearchAdd("clGetDeviceIDs", "Finding Cl devices", 0);
            SearchAdd("clCreateContext", "Creating Context on a device", 0);
            SearchAdd("clCreateCommandQueue", "Createing Command Queue", 0);
            SearchAdd("clCreateProgram", "Createing OpenCL Program", 0);
            SearchAdd("clBuildProgram", "Building OpenCL Program", 0);
            SearchAdd("clCreateKernel", "Creating the kernel", 0);
            SearchAdd("clCreateBuffer", "Input/Output buffers", 0);
            SearchAdd("clEnqueueWriteBuffer", "Adding Inputs to the Queue", 0);
            SearchAdd("clSetKernelArg", "Setting kernel Parameters", 0);
            SearchAdd("clEnqueueNDRangeKernel", "OpenCL program execution", -1);
            SearchAdd("clFinish", "Waiting for program end", 0);
            SearchAdd("clEnqueueReadBuffer", "Reading results", 0);
            SearchAdd("clRelease", "Releasing Resources", 0);
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
        private void SearchAdd(string search, string name, int computeUnit,string filter)
        {
            foreach (var item in myFile.Lines)
            {
                if (item.line.Contains(search) & !item.line.Contains(filter))
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