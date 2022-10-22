namespace Szakdolgozat.Models

{
    public class MockInstructionList
    {
        List<Instruction>? _instructions;

        public MockInstructionList()
        {
            _instructions = new List<Instruction>();
        }

        private void CreateMockInstructionList()
        {
            _instructions.Add(new Instruction(InstructionType.DECLARE, "n", "int", null, null,10000,null));
            _instructions.Add(new Instruction(InstructionType.DECLARE_ARRAY,"h_a","double","n",null,null,null));
            _instructions.Add(new Instruction(InstructionType.DECLARE_ARRAY, "h_b", "double", "n", null, null,null));
            _instructions.Add(new Instruction(InstructionType.DECLARE_ARRAY, "h_c", "double", "n", null, null, null));

            _instructions.Add(new Instruction(InstructionType.DECLARE, "i", "int", null, null,0, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "temp1", "double", null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "temp2", "double", null, null, null, null));
            _instructions.Add(new Instruction(InstructionType.J_IF_EQUAL, "i", "n", null, 6, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP, "temp1", "i", null, null, null, null,"sin"));
            _instructions.Add(new Instruction(InstructionType.UOP, "temp2", "i",null, null, null, null,"cos"));
            _instructions.Add(new Instruction(InstructionType.BIOP_TO_ARRAY, "h_a","temp1","temp1",null, null, null,"*","i"));
            _instructions.Add(new Instruction(InstructionType.BIOP_TO_ARRAY, "h_b", "temp2", "temp2", null, null, null, "*", "i"));
            _instructions.Add(new Instruction(InstructionType.UOP, "i", null, null, null, 1, null, "+"));
            _instructions.Add(new Instruction(InstructionType.JUMP, null, null, null,-7, null, null));
            
            //paralell
            _instructions.Add(new Instruction(InstructionType.COPY, "i", null, null, null, 0, null));
            _instructions.Add(new Instruction(InstructionType.J_IF_EQUAL, "i", "n", null, 3, null, null));
            _instructions.Add(new Instruction(InstructionType.BIOP_ARRAY_ARRAY, "h_c", "h_a", "h_b", null, null, null,"+","i"));
            _instructions.Add(new Instruction(InstructionType.UOP, "i", null, null, null, 1, null, "+"));
            _instructions.Add(new Instruction(InstructionType.JUMP, null, null, null, -4, null, null));

            _instructions.Add(new Instruction(InstructionType.COPY, "i", null, null, null, 0, null));
            _instructions.Add(new Instruction(InstructionType.DECLARE, "sum", "double", null, null, 0, null));
            _instructions.Add(new Instruction(InstructionType.COPY, "i", null, null, null,0, null));
            _instructions.Add(new Instruction(InstructionType.J_IF_EQUAL, "i", "n", null, 3, null, null));
            _instructions.Add(new Instruction(InstructionType.UOP_FROM_ARRAY, "sum", "h_c", null, null, null, null, "+","i"));
            _instructions.Add(new Instruction(InstructionType.UOP, "i", null, null, null, 1, null, "+"));
            _instructions.Add(new Instruction(InstructionType.JUMP, null, null, null, -4, null, null));

        }
    }
}
