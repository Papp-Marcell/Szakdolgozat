namespace Szakdolgozat.Models
{
    //enum list for Instruction Types
    public enum InstructionType
    {
        COPY,
        DECLARE,
        DECLARE_ARRAY,
        DECLARE_LIST,
        COPY_FROM_ARRAY,
        COPY_TO_ARRAY,
        COPY_ARRAY_ARRAY,
        UOP,
        UOP_TO_ARRAY,
        UOP_FROM_ARRAY,
        UOP_ARRAY_ARRAY,
        BIOP,
        BIOP_TO_ARRAY,
        BIOP_FROM_ARRAY,
        BIOP_ARRAY_ARRAY,
        JUMP,
        J_IF_EQUAL,
        J_IF_GREATER,
        J_IF_GREATER_EQUAL,
        J_IF_LESS,
        J_IF_LESS_EQUAL,
        VAR_PRINT,
        ARRAY_PRINT,
        PARALLEL_START,
        PARALLEL_END,
        BARRIER,
    }
}
