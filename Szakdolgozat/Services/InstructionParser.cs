using Szakdolgozat.Models;
using System.Globalization;

namespace Szakdolgozat.Services
{
    public class InstructionParser
    {
        private Instruction ParseInstruction(string textInstruction)
        {
            
            string currentInstruction = textInstruction.Trim(new char[] { '(', ')',' ' });
            currentInstruction = currentInstruction.Replace(" ", "");
            string[] currentInstructionArray = currentInstruction.Split(',');
            string[] instructionType = currentInstructionArray[0].Split('.');
            string? instructionVar7 = null;
            string? instructionVar8 = null;
            if (currentInstructionArray.Length > 7)
            {
                instructionVar7 = nullCheck(currentInstructionArray[7]);
            }
            if (currentInstructionArray.Length > 8)
            {
                instructionVar8 = nullCheck(currentInstructionArray[8]);
            }
            return  new Instruction
                (
                    parseInstructionType(instructionType[1]),
                    nullCheck(currentInstructionArray[1]),
                    nullCheck(currentInstructionArray[2]),
                    nullCheck(currentInstructionArray[3]), 
                    nullCheckInt(currentInstructionArray[4]),
                    nullCheckDouble(currentInstructionArray[5]),
                    nullCheckDouble(currentInstructionArray[6]),
                    instructionVar7,
                    instructionVar8);
        }

        public MockInstructionList ParseInstructons(string textInstructions)
        {
            String[] textInstructionArray = textInstructions.Split(") (");
            MockInstructionList list = new MockInstructionList();
            
            foreach(String instruction in textInstructionArray)
            {
                
                list._instructions.Add(ParseInstruction(instruction));
            }

            return list;
        }

        private InstructionType parseInstructionType(string instructionType)
        {
            InstructionType _instructionType;
            Enum.TryParse(instructionType, out _instructionType);
            return _instructionType;
            
        }
        private string nullCheck(string text)
        {
            if (text == "null")
            {
                return null;
            }
            return text;
        }
        private int? nullCheckInt(string text)
        {
            if (text == "null")
            {
                return null;
            }
            return int.Parse(text);
        }
        private double? nullCheckDouble(string text)
        {
            if (text == "null")
            {
                return null;
            }
            return double.Parse(text,CultureInfo.InvariantCulture);
        }
    }
}
