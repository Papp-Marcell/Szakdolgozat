using System.Reflection;
using System;
using System.Reflection.Emit;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services
{
    public class DebugService
    {
        private InstructionHandler instructionHandler = new InstructionHandler();
        private AssemblyName assemblyName = new AssemblyName("assembly");
        private Type? debugType;
        private object? debugObject;
        public void InitializeDebug(List<Instruction> instructions)
        {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    assemblyName,
                    AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "debugModel",
                TypeAttributes.Public);
            //FieldBuilder fieldBuilder = typeBuilder.DefineField("xd", typeof(int).MakeArrayType(), FieldAttributes.Public);
            foreach (Instruction instruction in instructions){
                instructionHandler.ExecuteInstruction(instruction, typeBuilder);
            }
            debugType = typeBuilder.CreateType();
            debugObject = Activator.CreateInstance(debugType);
            foreach(Instruction instruction in instructions)
            {
                instructionHandler.ExecuteInstruction(instruction,debugObject);
            }
        }
    }
}
