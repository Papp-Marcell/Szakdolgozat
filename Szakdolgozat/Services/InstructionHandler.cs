using Szakdolgozat.Models;
using System.Reflection;
using System;
using System.Reflection.Emit;

namespace Szakdolgozat.Services
{
    public class InstructionHandler
    {
        


        
        public void ExecuteInstruction(Instruction instruction,TypeBuilder typeBuilder)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArrayType(instruction.var1, instruction.var2,typeBuilder);
                    break;





            }
        }

        public void ExecuteInstruction(Instruction instruction,Object _object)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArray(instruction.var1, instruction.var2, instruction.var3,_object);
                    break;





            }
        }

        private void CreateArrayType(string name,string type,TypeBuilder typeBuilder)
        {
            switch (type)
            {
                case "string":
                    typeBuilder.DefineField(name,typeof(string).MakeArrayType(),FieldAttributes.Public);
                    break;
                default:
                    typeBuilder.DefineField(name, typeof(double).MakeArrayType(), FieldAttributes.Public);
                    break;
            }
        }

        private void CreateArray(string name, string type, string size,object _object)
        {
            FieldInfo fieldInfo;
            switch (type)
            {
                case "string":
                    fieldInfo = _object.GetType().GetField(name);
                    fieldInfo.SetValue(_object, new string[Int32.Parse(size)]);
                    break;
                default:
                    fieldInfo = _object.GetType().GetField(name);
                    fieldInfo.SetValue(_object, new double[Int32.Parse(size)]);
                    break;
            }
        }
    }
}
