using System.Reflection;
using System;
using System.Diagnostics;
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
        public int stepCount;
        public List<String> resultList = new List<String>();
        public int memory = 0;
        private Stopwatch timer = new Stopwatch();
        public string? elapsedTime;



        public void InitializeDebug(List<Instruction> instructions)
        {
            stepCount = 0;
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    assemblyName,
                    AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "debugArrays",
                TypeAttributes.Public);
            timer.Start();

            foreach (Instruction instruction in instructions){
                instructionHandler.ExecuteDeclaration(instruction, typeBuilder,ref stepCount);
            }
            debug = typeBuilder.CreateType();
            debugObject = Activator.CreateInstance(debug);


            Simulate(instructions);
        }

        public void Simulate(List<Instruction> instructions)
        {
            
            for(int i=0; i<instructions.Count;)
            {
                instructionHandler.ExecuteInstruction(instructions[i], debugObject,ref i,ref stepCount,resultList,ref memory);
            }
            timer.Stop();
            TimeSpan ts = timer.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
        }
    }
}
