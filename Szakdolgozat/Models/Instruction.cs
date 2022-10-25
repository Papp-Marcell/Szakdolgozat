namespace Szakdolgozat.Models
{
    public class Instruction
    {
        //A class that stores an instruction type and several variables, this is an intermediate representation of C code
        public Instruction(InstructionType instrucionType, string? var1, string? var2, string? var3, int? index, double? value1,double? value2,string? op=null,string? var4=null)
        {
            this.instrucionType = instrucionType;
            this.var1 = var1;
            this.var2 = var2;
            this.var3 = var3;
            this.var4 = var4;
            this.index = index;
            this.value1 = value1;
            this.value2 = value2;
            this.op = op;
        }

        public InstructionType instrucionType { get; set; }
        public string? var1 { get; set; }
        public string? var2 { get; set; }
        public string? var3 { get; set; }
        public string? var4 { get; set; }
        public int? index { get; set; }
        public double? value1 { get; set; }
        public double? value2 { get; set; }

        public string? op { get; set; }
    }
    
}
