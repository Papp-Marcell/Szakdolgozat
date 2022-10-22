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
        private Type? debug;
        private object? debugObject;


        
        public void InitializeDebug(List<Instruction> instructions)
        {
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    assemblyName,
                    AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "debugArrays",
                TypeAttributes.Public);
            
            foreach (Instruction instruction in instructions){
                instructionHandler.ExecuteDeclaration(instruction, typeBuilder);
            }
            debug = typeBuilder.CreateType();
            debugObject = Activator.CreateInstance(debug);
        }

        public void Simulate(List<Instruction> instructions)
        {
            for(int i=0; i<instructions.Count;)
            {
                instructionHandler.ExecuteInstruction(instructions[i], debugObject,ref i);
            }

        }
    }
}
