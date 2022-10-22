using Szakdolgozat.Models;
using System.Reflection;
using System;
using System.Reflection.Emit;

namespace Szakdolgozat.Services
{
    public class InstructionHandler
    {

        public void ExecuteDeclaration(Instruction instruction, TypeBuilder typeBuilder)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArrayType(instruction.var1, instruction.var2, typeBuilder);
                    break;
                case InstructionType.DECLARE:
                    CreateType(instruction.var1, instruction.var2,typeBuilder);
                    break;
            }
        }

        public void ExecuteInstruction(Instruction instruction,object _object,ref int i)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArray(instruction.var1, instruction.var2,instruction.var3, _object);
                    break;
                case InstructionType.DECLARE:
                    AssignValue(instruction.var1, instruction.var2,instruction.var3,instruction.value1, _object);
                    break;
                case InstructionType.JUMP:
                    i += instruction.index.Value;
                    break;
                case InstructionType.COPY:
                    Copy(instruction.var1,instruction.var2,instruction.var3,instruction.value1, _object);
                    break;
                case InstructionType.UOP:
                    Uop(instruction.var1,instruction.value1,instruction.op,_object);
                    break;
                case InstructionType.J_IF_EQUAL:
                    JumpIfEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i);
                    break;
            }
            i++;
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
        private void CreateType(string name, string type,TypeBuilder typeBuilder)
        {
            switch (type)
            {
                case "string":
                    typeBuilder.DefineField(name,typeof(string),FieldAttributes.Public);
                    break;
                default:
                    typeBuilder.DefineField(name, typeof(double), FieldAttributes.Public);
                    break;
            }
        }
        private void CreateArray(string name, string type, string size,object _object)
        {
            int sizeInt;
            if(!Int32.TryParse(size,out sizeInt))
            {
                sizeInt=(int)_object.GetType().GetField(size).GetValue(_object);
            }
            FieldInfo fieldInfo;
            switch (type)
            {
                case "string":
                    fieldInfo = _object.GetType().GetField(name);
                    fieldInfo.SetValue(_object, new string[sizeInt]);
                    break;
                default:
                    fieldInfo = _object.GetType().GetField(name);
                    fieldInfo.SetValue(_object, new double[sizeInt]);
                    break;
            }
        }
        private void AssignValue(string name, string type, string value,double? doubleValue, object _object)
        {
            
            switch (type)
            {
                case "string":
                    if (value != null)
                    {
                        _object.GetType().GetField(name).SetValue(_object, value);
                    }
                    break;
                default:
                    if (doubleValue != null)
                    {
                        _object.GetType().GetField(name).SetValue(_object, doubleValue.Value);
                    }
                    break;
            }
        }
        private void Copy(string name,string var,string value,double? doubleValue,object _object)
        {
            if (var != null)
            {
                _object.GetType().GetField(name).SetValue(_object, _object.GetType().GetField(var).GetValue(_object));
            }
            if (value != null)
            {
                _object.GetType().GetField(name).SetValue(_object, value);
            }
            if (doubleValue!= null)
            {
                _object.GetType().GetField(name).SetValue(_object, doubleValue.Value);
            }
        }
        private void Uop(string name,double? doubleValue, string op,object _object) 
        {
            switch(op)
            {
                case "+":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) + doubleValue.Value);
                    break;
                case "-":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) - doubleValue.Value);
                    break;
                case "*":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) * doubleValue.Value);
                    break;
                case "/":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) / doubleValue.Value);
                    break;
            }
        }
        private void JumpIfEqual(string name,string name2,double? index,object _object,ref int i)
        {
            if(_object.GetType().GetField(name).GetValue(_object) == _object.GetType().GetField(name2).GetValue(_object))
            {
                i += (int)index.Value;
            }
        }



    }
}
