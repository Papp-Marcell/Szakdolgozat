using System.Management;

namespace Szakdolgozat.Services
{
    public class HardwareService
    {
        public ManagementObject CPU { get; set; } = new ManagementObjectSearcher("select * from Win32_Processor")
            .Get()
            .Cast<ManagementObject>()
            .First();

        public ManagementObject Memory { get; set; } = new ManagementObjectSearcher("select * from Win32_PhysicalMemory")
            .Get()
            .Cast<ManagementObject>()
            .First();

        public ManagementObject GPU { get; set; } = new ManagementObjectSearcher("select * from Win32_VideoController")
            .Get()
            .Cast<ManagementObject>()
            .First();
    }
}
