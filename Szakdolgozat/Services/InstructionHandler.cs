﻿using Szakdolgozat.Models;
using System.Reflection;
using System;
using System.Reflection.Emit;

namespace Szakdolgozat.Services
{
    public class InstructionHandler
    {

        public void ExecuteDeclaration(Instruction instruction, TypeBuilder typeBuilder,ref int counter)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArrayType(instruction.var1, instruction.var2, typeBuilder);
                    counter++;
                    break;
                case InstructionType.DECLARE:
                    CreateType(instruction.var1, instruction.var2,typeBuilder);
                    counter++;
                    break;
            }
        }

        public void ExecuteInstruction(Instruction instruction,object _object,ref int i,ref int counter,List<string> resultList,ref int memory)
        {
            string resultString = null;
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArray(instruction.var1, instruction.var2,instruction.var3, _object,ref memory);
                    counter++;
                    break;
                case InstructionType.DECLARE:
                    resultString = AssignValue(instruction.var1, instruction.var2,instruction.var3,instruction.value1, _object, ref memory);
                    counter++;
                    if (resultString != null)
                    {
                        resultList.Add($"At step {counter} : {resultString}");
                    }
                    break;
                case InstructionType.JUMP:
                    i += instruction.index.Value;
                    break;
                case InstructionType.COPY:
                    resultString = Copy(instruction.var1,instruction.var2,instruction.var3,instruction.value1, _object);
                    counter++;
                    resultList.Add($"At step {counter} : {resultString}");
                    break;
                case InstructionType.UOP:
                    Uop(instruction.var1,instruction.var2,instruction.value1,instruction.op,_object);
                    counter++;
                    break;
                case InstructionType.J_IF_EQUAL:
                    JumpIfEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i);
                    counter++;
                    break;
                case InstructionType.J_IF_GREATER:
                    JumpIfGreater(instruction.var1, instruction.var2, instruction.index, _object, ref i);
                    counter++;
                    break;
                case InstructionType.J_IF_GREATER_EQUAL:
                    JumpIfGreaterEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i);
                    counter++;
                    break;
                case InstructionType.J_IF_LESS:
                    JumpIfLess(instruction.var1, instruction.var2, instruction.index, _object, ref i);
                    counter++;
                    break;
                case InstructionType.J_IF_LESS_EQUAL:
                    JumpIfLessEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i);
                    counter++;
                    break;
                case InstructionType.COPY_TO_ARRAY:
                    CopyToArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, _object);
                    counter++;
                    break;
                case InstructionType.UOP_TO_ARRAY:
                    UopToArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, instruction.op, _object);
                    counter++;
                    break;
                case InstructionType.COPY_ARRAY_ARRAY:
                    CopyToArrayFromArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, _object);
                    counter++;
                    break;
                case InstructionType.UOP_ARRAY_ARRAY:
                    UopToArrayFromArray(instruction.var1,instruction.var2,instruction.var3,instruction.index, instruction.op,_object);
                    counter++;
                    break;
                case InstructionType.UOP_FROM_ARRAY:
                    UopFromArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, instruction.op, _object);
                    counter++;
                    break;
                case InstructionType.VAR_PRINT:
                    resultList.Add($"At step {counter} : {VarPrint(instruction.var1, _object)}");
                    break;
                case InstructionType.ARRAY_PRINT:
                    resultList.Add($"At step {counter} : {ArrayPrint(instruction.var1, _object)}");
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
        private void CreateArray(string name, string type, string size,object _object,ref int memory)
        {
            int sizeInt;
            if(!Int32.TryParse(size,out sizeInt))
            {
                sizeInt=Convert.ToInt32(_object.GetType().GetField(size).GetValue(_object));
            }
            FieldInfo fieldInfo;
            switch (type)
            {
                case "string":
                    fieldInfo = _object.GetType().GetField(name);
                    fieldInfo.SetValue(_object, new string[sizeInt]);
                    memory += (sizeInt * (20 * 8));
                    break;
                default:
                    fieldInfo = _object.GetType().GetField(name);
                    fieldInfo.SetValue(_object, new double[sizeInt]);
                    memory += (sizeInt * 16);
                    break;
            }
        }
        private string AssignValue(string name, string type, string value,double? doubleValue, object _object,ref int memory)
        {
            
            switch (type)
            {
                case "string":
                    if (value != null)
                    {
                        _object.GetType().GetField(name).SetValue(_object, value);
                        memory += 20;
                        return $"Value of {name} : {value}";
                    }
                    break;
                default:
                    if (doubleValue != null)
                    {
                        _object.GetType().GetField(name).SetValue(_object, doubleValue.Value);
                        memory += 16;
                        return $"Value of {name} : {doubleValue.Value}";
                    }
                    break;
            }
            return null;
        }
        private string Copy(string name,string var,string value,double? doubleValue,object _object)
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
            return $"Value of {name} : {_object.GetType().GetField(name).GetValue(_object)}";
        }
        private void Uop(string name,string value,double? doubleValue, string op,object _object) 
        {
            double opValue;
            if (doubleValue.HasValue)
            {
                opValue = doubleValue.Value;
            }
            else
            {
                opValue = (double)_object.GetType().GetField(value).GetValue(_object);
            }
            switch(op)
            {
                case "+":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) + opValue);
                    break;
                case "-":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) - opValue);
                    break;
                case "*":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) * opValue);
                    break;
                case "/":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) / opValue);
                    break;
                case "sin":
                    _object.GetType().GetField(name).SetValue(_object, Math.Sin(opValue));
                    break;
                case "cos":
                    _object.GetType().GetField(name).SetValue(_object, Math.Cos(opValue));
                    break;
            }
        }
        private void JumpIfEqual(string name,string name2,int? index,object _object,ref int i)
        {
            if((double)_object.GetType().GetField(name).GetValue(_object) == (double)_object.GetType().GetField(name2).GetValue(_object))
            {
                i += (int)index.Value;
            }
        }
        private void JumpIfGreater(string name, string name2, int? index, object _object, ref int i)
        {
            if ((double)_object.GetType().GetField(name).GetValue(_object) > (double)_object.GetType().GetField(name2).GetValue(_object))
            {
                i += (int)index.Value;
            }
        }
        private void JumpIfGreaterEqual(string name, string name2, int? index, object _object, ref int i)
        {
            if ((double)_object.GetType().GetField(name).GetValue(_object) >= (double)_object.GetType().GetField(name2).GetValue(_object))
            {
                i += (int)index.Value;
            }
        }
        private void JumpIfLess(string name, string name2, int? index, object _object, ref int i)
        {
            if ((double)_object.GetType().GetField(name).GetValue(_object) < (double)_object.GetType().GetField(name2).GetValue(_object))
            {
                i += (int)index.Value;
            }
        }
        private void JumpIfLessEqual(string name, string name2, int? index, object _object, ref int i)
        {
            if ((double)_object.GetType().GetField(name).GetValue(_object) <= (double)_object.GetType().GetField(name2).GetValue(_object))
            {
                i += (int)index.Value;
            }
        }
        private void CopyToArray(string arrayName, string name,string indexVar,int? index, object _object)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else { 
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object)); 
            }
            object value = _object.GetType().GetField(name).GetValue(_object);
            if (value.GetType() == typeof(string))
            {
                ((IList<string>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] = (string)value;
            }
            else
            {
                ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] = (double)value;
            }
        }
        private void UopToArray(string arrayName, string name, string indexVar, int? index, string op, object _object)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else
            {
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object));
            }
            object value = _object.GetType().GetField(name).GetValue(_object);
           
            switch (op)
            {
                case "+":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] += (double)value;
                    break;
                case "-":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] -= (double)value;
                    break;
                case "*":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] *= (double)value;
                    break;
                case "/":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] /= (double)value;
                    break;
            }
        }
        private void CopyToArrayFromArray(string arrayTo, string arrayFrom, string indexVar, int? index, object _object)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else
            {
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object));
            }
            object _arrayFrom = _object.GetType().GetField(arrayFrom).GetValue(_object);
            if (_arrayFrom.GetType() == typeof(string[]))
            {
                ((IList<string>)_object.GetType().GetField(arrayTo).GetValue(_object))[arrayIndex] = ((IList<string>)_arrayFrom)[arrayIndex];
            }
            else
            {
                ((IList<double>)_object.GetType().GetField(arrayTo).GetValue(_object))[arrayIndex] = ((IList<double>)_arrayFrom)[arrayIndex];
            }
        }
        private void UopToArrayFromArray(string arrayTo, string arrayFrom, string indexVar, int? index, string op, object _object)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else
            {
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object));
            }
            object _arrayFrom = _object.GetType().GetField(arrayFrom).GetValue(_object);

            switch (op)
            {
                case "+":
                    ((IList<double>)_object.GetType().GetField(arrayTo).GetValue(_object))[arrayIndex] += ((IList<Double>)_arrayFrom)[arrayIndex];
                    break;
                case "-":
                    ((IList<double>)_object.GetType().GetField(arrayTo).GetValue(_object))[arrayIndex] -= ((IList<Double>)_arrayFrom)[arrayIndex];
                    break;
                case "*":
                    ((IList<double>)_object.GetType().GetField(arrayTo).GetValue(_object))[arrayIndex] *= ((IList<Double>)_arrayFrom)[arrayIndex];
                    break;
                case "/":
                    ((IList<double>)_object.GetType().GetField(arrayTo).GetValue(_object))[arrayIndex] /= ((IList<Double>)_arrayFrom)[arrayIndex];
                    break;
            }
        }
        private void UopFromArray(string name,string arrayFrom,string indexVar, int? index,string op, object _object)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else
            {
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object));
            }
            object value = ((IList<double>)_object.GetType().GetField(arrayFrom).GetValue(_object))[arrayIndex];

            switch (op)
            {
                case "+":
                    _object.GetType().GetField(name).SetValue(_object,(double)_object.GetType().GetField(name).GetValue(_object) + (double)value);
                    break;
                case "-":
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) - (double)value);
                    break;                           
                case "*":                            
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) * (double)value);
                    break;                           
                case "/":                            
                    _object.GetType().GetField(name).SetValue(_object, (double)_object.GetType().GetField(name).GetValue(_object) / (double)value);
                    break;
            }
        }
        private string VarPrint(string name,object _object)
        {
            return $"Value of {name} : {_object.GetType().GetField(name).GetValue(_object)}";
        }
        private string ArrayPrint(string name, object _object)
        {
            string result = "";
            object array = _object.GetType().GetField(name).GetValue(_object);

            if (array.GetType() == typeof(string[]))
            {
                for (int i = 0; i < 5 && i< ((IList<String>)array).Count; i++){
                    result += $" {((IList<String>)array)[i]} ,";
                }
            }
            else
            {
                for (int i = 0; i < 5 && i < ((IList<double>)array).Count; i++)
                {
                    result += $" {((IList<double>)array)[i]} ,";
                }
            }

            return $"Values of array {name} : {result}";
        }
    }
}
