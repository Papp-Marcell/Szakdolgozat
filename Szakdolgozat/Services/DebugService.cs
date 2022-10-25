using System.Reflection;
using System.Drawing;
using System.Diagnostics;
using System.Reflection.Emit;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services
{

    //Handles everything related to the mock Instruction List including simulation and Creating a Syntax Tree
    public class DebugService
    {
        private InstructionHandler instructionHandler = new InstructionHandler();
        private AssemblyName assemblyName = new AssemblyName("assembly");
        private ImageService ImageService = new ImageService();
        private Type? debug;
        private object? debugObject;
        public int stepCount;
        public List<String> resultList = new List<String>();
        public int memory = 0;
        private Stopwatch timer = new Stopwatch();
        public string? elapsedTime;
        public Bitmap? AST;


        //Creates a new object to simply store variables and arrays related to the simulation
        public void InitializeDebug(List<Instruction> instructions)
        {
            timer.Reset();
            memory = 0;
            stepCount = 0;
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    assemblyName,
                    AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "debugArrays",
                TypeAttributes.Public);
            timer.Start();

            //Initializes the types
            foreach (Instruction instruction in instructions){
                instructionHandler.ExecuteDeclaration(instruction, typeBuilder,ref stepCount);
            }
            debug = typeBuilder.CreateType();
            debugObject = Activator.CreateInstance(debug);

            
            Simulate(instructions);
            GetAST(instructions);
        }

        //Loops through the Instruction List, creates actual variables, simulates operations, 
        //measures step count, minimum memory required, and runtime.
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


        //Creates a Syntax Tree
        public void GetAST(List<Instruction> instructions)
        {
            AST = ImageService.StartBitmap();
            for (int i = 0; i < instructions.Count;i++)
            {
                //AST needs to be explicitly reference or resize wont work
                instructionHandler.DrawInstruction(instructions[i],i,ref AST);
            }
            for (int i = 0; i < instructions.Count;i++)
            {
                instructionHandler.DrawJumps(instructions[i], i, AST);
            }

        }


    }
}
