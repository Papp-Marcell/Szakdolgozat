using System.Management;
using System.Runtime.InteropServices;

namespace Szakdolgozat.Services
{
    public class HardwareService
    {
        public string CPUname { get; set; }
        public uint CPUthreads { get; set; }
        public UInt64 RamMemory { get; set; }
        public string GPUname { get; set; }
        public UInt32 GPUram { get; set; }
        

        public void Initialize()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                 ManagementObject CPU = new ManagementObjectSearcher("select * from Win32_Processor")
                    .Get()
                    .Cast<ManagementObject>()
                    .First();

                ManagementObject Memory = new ManagementObjectSearcher("select * from Win32_PhysicalMemory")
                    .Get()
                    .Cast<ManagementObject>()
                    .First();

                ManagementObject GPU = new ManagementObjectSearcher("select * from Win32_VideoController")
                    .Get()
                    .Cast<ManagementObject>()
                    .First();

                this.CPUname = (string)CPU["Name"];
                this.GPUname = (string)GPU["Name"];

                this.RamMemory = (UInt64)Memory["Capacity"];
                this.RamMemory = this.RamMemory/ 1000000000;

                this.GPUram = (UInt32)GPU["AdapterRam"];
                this.GPUram = this.GPUram / 1000000000;

                this.CPUthreads= (uint)CPU["NumberOfLogicalProcessors"];

            }
            else
            {
                this.CPUname = "Linux CPU";
                this.GPUname = "Linux GPU";
                this.RamMemory = 10;
                this.GPUram = 4;
                this.CPUthreads = 8;
            }
        }
    }
}
