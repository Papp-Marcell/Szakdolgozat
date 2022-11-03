namespace Szakdolgozat.Models

{
    //generating instructions from a txt file is too complicated, so we create a mock list of instructions
    public class MockInstructionList
    {
        public List<Instruction>? _instructions;

        public MockInstructionList()
        {
            _instructions = new List<Instruction>();
        }

        public void CreateMockInstructionList()
        {
            _instructions.Add(new Instruction(InstructionType.DECLARE, "n", "int", null, null,10000,null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "localSize", "int", null, null, 64, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "globalSize", "int", null, null, null, null));
           
            _instructions.Add(new Instruction(InstructionType.COPY, "globalSize", "n", null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP, "globalSize", "localSize", null, null, null, null,"/"));
            _instructions.Add(new Instruction(InstructionType.UOP, "globalSize", null, null, null, null, null, "ceil"));
            _instructions.Add(new Instruction(InstructionType.UOP, "globalSize", "localSize", null, null, null, null, "*"));
            _instructions.Add(new Instruction(InstructionType.DECLARE_ARRAY,"h_a","double","n",null,null,null));
            _instructions.Add(new Instruction(InstructionType.DECLARE_ARRAY, "h_b", "double", "n", null, null,null));
            _instructions.Add(new Instruction(InstructionType.DECLARE_ARRAY, "h_c", "double", "n", null, null, null));

            _instructions.Add(new Instruction(InstructionType.DECLARE, "i", "int", null, null,0, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "temp1", "double", null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "temp2", "double", null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.J_IF_EQUAL, "i", "n", null, 8, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP, "temp1", "i", null, null, null, null,"sin"));
            _instructions.Add(new Instruction(InstructionType.UOP, "temp2", "i",null, null, null, null,"cos"));
            _instructions.Add(new Instruction(InstructionType.COPY_TO_ARRAY,"h_a","temp1","i",null,null,null));
            _instructions.Add(new Instruction(InstructionType.UOP_TO_ARRAY, "h_a","temp1","i",null, null, null,"*"));
            _instructions.Add(new Instruction(InstructionType.COPY_TO_ARRAY, "h_b", "temp2", "i", null, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP_TO_ARRAY, "h_b", "temp2", "i", null, null, null, "*"));
            _instructions.Add(new Instruction(InstructionType.UOP, "i", null, null, null, 1, null, "+"));
            _instructions.Add(new Instruction(InstructionType.JUMP, null, null, null,-9, null, null));
            _instructions.Add(new Instruction(InstructionType.ARRAY_PRINT, "h_a", null, null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.ARRAY_PRINT, "h_b", null, null, null, null, null));

            //paralell
            //_instructions.Add(new Instruction(InstructionType.COPY, "i", null, null, null, 0, null));
            //_instructions.Add(new Instruction(InstructionType.J_IF_EQUAL, "i", "n", null, 4, null, null));
            //_instructions.Add(new Instruction(InstructionType.COPY_ARRAY_ARRAY, "h_c", "h_a", "i", null, null, null));
            //_instructions.Add(new Instruction(InstructionType.UOP_ARRAY_ARRAY, "h_c", "h_b", "i", null, null, null,"+"));
            //_instructions.Add(new Instruction(InstructionType.UOP, "i", null, null, null, 1, null, "+"));
            //_instructions.Add(new Instruction(InstructionType.JUMP, null, null, null, -5, null, null));

            _instructions.Add(new Instruction(InstructionType.PARALLEL_START, null, null, null, null, null, null));
            
            _instructions.Add(new Instruction(InstructionType.COPY_ARRAY_ARRAY, "h_c", "h_a", "i", null, null, null));
            _instructions.Add(new Instruction(InstructionType.BARRIER, null, null, null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP_ARRAY_ARRAY, "h_c", "h_b", "i", null, null, null, "+"));
            _instructions.Add(new Instruction(InstructionType.PARALLEL_END, "n", null, null, null, null, null));




            _instructions.Add(new Instruction(InstructionType.ARRAY_PRINT, "h_c", null, null, null, null, null));

            _instructions.Add(new Instruction(InstructionType.COPY, "i", null, null, null, 0, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "sum", "double", null, null, 0, null));
            
            _instructions.Add(new Instruction(InstructionType.J_IF_EQUAL, "i", "n", null, 3, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP_FROM_ARRAY, "sum", "h_c", "i", null, null, null, "+","i"));
            _instructions.Add(new Instruction(InstructionType.UOP, "i", null, null, null, 1, null, "+"));
            _instructions.Add(new Instruction(InstructionType.JUMP, null, null, null, -4, null, null));
            _instructions.Add(new Instruction(InstructionType.VAR_PRINT, "sum", null, null, null, null, null));

        }
    }
}
