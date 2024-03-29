﻿using Szakdolgozat.Models;
using System.Reflection;
using System.Collections.Immutable;
using System.Drawing;
using System.Reflection.Emit;

namespace Szakdolgozat.Services
{
    //Handles and or simulates instrucions
    public class InstructionHandler
    {
        private ImageService imageService = new ImageService();
        private bool parallel = false;
        private ImmutableList<Instruction> parallelInstructions = ImmutableList<Instruction>.Empty;

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

        public void ExecuteInstruction(Instruction instruction,object _object,ref int i,ref int counter,List<string> resultList,ref int memory,int? parallelIndex=null)
        {
            if(instruction.instrucionType == InstructionType.PARALLEL_END)
            {
                parallel = false;
                ExecuteParallel(_object, ref counter, ref memory,resultList,GetParallelCount(instruction.var1,instruction.value1,_object));
                i++;
                return;
            }
            if (parallel)
            {
                parallelInstructions=parallelInstructions.Add(instruction);
                i++;
                return;
            }
            string resultString = null;
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    CreateArray(instruction.var1, instruction.var2,instruction.var3, _object,ref memory);
                    counter++;
                    break;
                case InstructionType.DECLARE:
                    resultString = AssignValue(instruction.var1, instruction.var2,instruction.var3,instruction.value1, _object, ref memory,i);
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
                    resultString = Copy(instruction.var1,instruction.var2,instruction.var3,instruction.value1, _object,i);
                    counter++;
                    //resultList.Add($"At step {counter} : {resultString}");
                    break;
                case InstructionType.UOP:
                    Uop(instruction.var1,instruction.var2,instruction.value1,instruction.op,_object);
                    counter++;
                    break;
                case InstructionType.J_IF_EQUAL:
                    JumpIfEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i, parallelIndex);
                    counter++;
                    break;
                case InstructionType.J_IF_NOT_EQUAL:
                    JumpIfNotEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i, parallelIndex);
                    counter++;
                    break;
                case InstructionType.J_IF_GREATER:
                    JumpIfGreater(instruction.var1, instruction.var2, instruction.index, _object, ref i, parallelIndex);
                    counter++;
                    break;
                case InstructionType.J_IF_GREATER_EQUAL:
                    JumpIfGreaterEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i, parallelIndex);
                    counter++;
                    break;
                case InstructionType.J_IF_LESS:
                    JumpIfLess(instruction.var1, instruction.var2, instruction.index, _object, ref i, parallelIndex);
                    counter++;
                    break;
                case InstructionType.J_IF_LESS_EQUAL:
                    JumpIfLessEqual(instruction.var1, instruction.var2, instruction.index, _object, ref i, parallelIndex);
                    counter++;
                    break;
                case InstructionType.COPY_TO_ARRAY:
                    CopyToArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, _object, parallelIndex);
                    counter++;
                    break;
                case InstructionType.COPY_FROM_ARRAY:
                    CopyFromArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, _object, parallelIndex);
                    counter++;
                    break;
                case InstructionType.UOP_TO_ARRAY:
                    UopToArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, instruction.op, _object, parallelIndex,instruction.value1);
                    counter++;
                    break;
                case InstructionType.COPY_ARRAY_ARRAY:
                    CopyToArrayFromArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, _object, parallelIndex);
                    counter++;
                    break;
                case InstructionType.UOP_ARRAY_ARRAY:
                    UopToArrayFromArray(instruction.var1,instruction.var2,instruction.var3,instruction.index, instruction.op,_object, parallelIndex);
                    counter++;
                    break;
                case InstructionType.UOP_FROM_ARRAY:
                    UopFromArray(instruction.var1, instruction.var2, instruction.var3, instruction.index, instruction.op, _object, parallelIndex);
                    counter++;
                    break;
                case InstructionType.VAR_PRINT:
                    resultList.Add($"At step {counter} : {VarPrint(instruction.var1, _object,i)}");
                    break;
                case InstructionType.ARRAY_PRINT:
                    resultList.Add($"At step {counter} : {ArrayPrint(instruction.var1, _object,i)}");
                    break;
                case InstructionType.PARALLEL_START:
                    ParallelStart();
                    break;
                case InstructionType.RANDOM:
                    SetRandom(instruction.var1, instruction.var2, instruction.index, instruction.value1, instruction.value2, _object,parallelIndex);
                    break;
            }
            i++;
        }

        public void DrawInstruction(Instruction instruction,int i,ref Bitmap bitmap)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.DECLARE_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Create array {instruction.var1}", false);
                    break;
                case InstructionType.DECLARE:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Create variable {instruction.var1}", false);
                    break;
                case InstructionType.UOP:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} UOP on {instruction.var1}", false);
                    break;
                case InstructionType.COPY_TO_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Copy to element of {instruction.var1} array", false);
                    break;
                case InstructionType.COPY_FROM_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Copy to {instruction.var1} from array", false);
                    break;
                case InstructionType.UOP_TO_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} UOP on element of {instruction.var1} array", false);
                    break;
                case InstructionType.ARRAY_PRINT:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Prints Values of first 5 elemtents of {instruction.var1} array", false);
                    break;
                case InstructionType.VAR_PRINT:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Print Value of {instruction.var1}", false);
                    break;
                case InstructionType.COPY_ARRAY_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Copy to {instruction.var1} array from {instruction.var2} array", false);
                    break;
                case InstructionType.UOP_ARRAY_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} UOP on element of {instruction.var1} array with element of {instruction.var2} array", false);
                    break;
                case InstructionType.UOP_FROM_ARRAY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} UOP on {instruction.var1} with element of {instruction.var2} array", false);
                    break;
                case InstructionType.JUMP:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Jump {instruction.index.Value} instructions", true);
                    break;
                case InstructionType.COPY:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Copy to {instruction.var1}", false);
                    break;
                case InstructionType.PARALLEL_START:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Start Parallel", true);
                    break;
                case InstructionType.PARALLEL_END:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} End Parallel", true);
                    break;
                case InstructionType.RANDOM:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Set {instruction.var1} to a random number", false);
                    break;
                case InstructionType.BARRIER:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Barrier", true);
                    break;
                case InstructionType.J_IF_EQUAL:
                case InstructionType.J_IF_NOT_EQUAL:
                case InstructionType.J_IF_GREATER:
                case InstructionType.J_IF_GREATER_EQUAL:
                case InstructionType.J_IF_LESS:
                case InstructionType.J_IF_LESS_EQUAL:
                    imageService.AddNextInstruction(ref bitmap, $"#{i} Jump {instruction.index.Value} instructions based on condition", true);
                    break;


            }

        }

        public void DrawJumps(Instruction instruction, int i, Bitmap bitmap, ref int j)
        {
            switch (instruction.instrucionType)
            {
                case InstructionType.JUMP:
                case InstructionType.J_IF_EQUAL:
                case InstructionType.J_IF_NOT_EQUAL:
                case InstructionType.J_IF_GREATER:
                case InstructionType.J_IF_GREATER_EQUAL:
                case InstructionType.J_IF_LESS:
                case InstructionType.J_IF_LESS_EQUAL:
                    if (instruction.index.Value > 0)
                    {
                        imageService.ForwardJump(bitmap, i, i + instruction.index.Value,j);
                        j += 2;
                    }
                    else
                    {
                        imageService.BackwardsJump(bitmap, i, i + instruction.index.Value,j);
                        j += 2;
                    }
                    break;
            }

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
        private string AssignValue(string name, string type, string value,double? doubleValue, object _object,ref int memory,int i)
        {
            
            switch (type)
            {
                case "string":
                    if (value != null)
                    {
                        _object.GetType().GetField(name).SetValue(_object, value);
                        memory += 20;
                        return $"#{i} Value of {name} : {value}";
                    }
                    break;
                default:
                    if (doubleValue != null)
                    {
                        _object.GetType().GetField(name).SetValue(_object, doubleValue.Value);
                        memory += 16;
                        return $"#{i} Value of {name} : {doubleValue.Value}";
                    }
                    break;
            }
            return null;
        }
        private string Copy(string name,string var,string value,double? doubleValue,object _object,int i)
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
            return $"#{i} Value of {name} : {_object.GetType().GetField(name).GetValue(_object)}";
        }
        private void Uop(string name,string value,double? doubleValue, string op,object _object) 
        {
            double opValue = 0;
            if (doubleValue.HasValue)
            {
                opValue = doubleValue.Value;
            }
            else if(value!=null)
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
                case "ceil":
                    _object.GetType().GetField(name).SetValue(_object, Math.Ceiling((double)_object.GetType().GetField(name).GetValue(_object)));
                    break;
                case "floor":
                    _object.GetType().GetField(name).SetValue(_object, Math.Floor((double)_object.GetType().GetField(name).GetValue(_object)));
                    break;
            }
        }
        private void JumpIfEqual(string name,string name2,int? index,object _object,ref int i, int? parallelIndex)
        {
            if (parallelIndex == null)
            {
                if ((double)_object.GetType().GetField(name).GetValue(_object) == (double)_object.GetType().GetField(name2).GetValue(_object))
                {
                    i += (int)index.Value;
                }
            }
            else
            {
                if (((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] == ((IList<double>)_object.GetType().GetField(name2).GetValue(_object))[parallelIndex.Value])
                {
                    i += (int)index.Value;
                }
            }
            
        }
        private void JumpIfNotEqual(string name, string name2, int? index, object _object, ref int i, int? parallelIndex)
        {
            if (parallelIndex == null)
            {
                if ((double)_object.GetType().GetField(name).GetValue(_object) != (double)_object.GetType().GetField(name2).GetValue(_object))
                {
                    i += (int)index.Value;
                }
            }
            else
            {
                if (((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] != ((IList<double>)_object.GetType().GetField(name2).GetValue(_object))[parallelIndex.Value])
                {
                    i += (int)index.Value;
                }
            }

        }
        private void JumpIfGreater(string name, string name2, int? index, object _object, ref int i, int? parallelIndex)
        {
            if (parallelIndex == null)
            {
                if ((double)_object.GetType().GetField(name).GetValue(_object) > (double)_object.GetType().GetField(name2).GetValue(_object))
                {
                    i += (int)index.Value;
                }
            }
            else
            {
                if (((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] > ((IList<double>)_object.GetType().GetField(name2).GetValue(_object))[parallelIndex.Value])
                {
                    i += (int)index.Value;
                }
            }
        }
        private void JumpIfGreaterEqual(string name, string name2, int? index, object _object, ref int i, int? parallelIndex)
        {
            if (parallelIndex == null)
            {
                if ((double)_object.GetType().GetField(name).GetValue(_object) >= (double)_object.GetType().GetField(name2).GetValue(_object))
                {
                    i += (int)index.Value;
                }
            }
            else
            {
                if (((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] >= ((IList<double>)_object.GetType().GetField(name2).GetValue(_object))[parallelIndex.Value])
                {
                    i += (int)index.Value;
                }
            }
        }
        private void JumpIfLess(string name, string name2, int? index, object _object, ref int i, int? parallelIndex)
        {
            if (parallelIndex == null)
            {
                if ((double)_object.GetType().GetField(name).GetValue(_object) < (double)_object.GetType().GetField(name2).GetValue(_object))
                {
                    i += (int)index.Value;
                }
            }
            else
            {
                if (((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] < ((IList<double>)_object.GetType().GetField(name2).GetValue(_object))[parallelIndex.Value])
                {
                    i += (int)index.Value;
                }
            }
        }
        private void JumpIfLessEqual(string name, string name2, int? index, object _object, ref int i, int? parallelIndex)
        {
            if (parallelIndex == null)
            {
                if ((double)_object.GetType().GetField(name).GetValue(_object) <= (double)_object.GetType().GetField(name2).GetValue(_object))
                {
                    i += (int)index.Value;
                }
            }
            else
            {
                if (((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] <= ((IList<double>)_object.GetType().GetField(name2).GetValue(_object))[parallelIndex.Value])
                {
                    i += (int)index.Value;
                }
            }
        }
        private void CopyToArray(string arrayName, string name,string indexVar,int? index, object _object,int? parallelIndex)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else if (parallelIndex.HasValue)
            {
                arrayIndex = (int)parallelIndex.Value;
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
        private void CopyFromArray(string name, string arrayName, string indexVar, int? index, object _object, int? parallelIndex)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else if (parallelIndex.HasValue)
            {
                arrayIndex = (int)parallelIndex.Value;
            }
            else
            {
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object));
            }



            object _arrayFrom = _object.GetType().GetField(arrayName).GetValue(_object);
            if (_arrayFrom.GetType() == typeof(string[]))
            {
                _object.GetType().GetField(name).SetValue(_object, ((IList<string>)_arrayFrom)[arrayIndex]);
                
            }
            else
            {
                _object.GetType().GetField(name).SetValue(_object, ((IList<double>)_arrayFrom)[arrayIndex]);
            }
        }
        private void UopToArray(string arrayName, string name, string indexVar, int? index, string op, object _object,int? parallelIndex,double? value1)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else if (parallelIndex.HasValue)
            {
                arrayIndex = (int)parallelIndex.Value;
            }
            else
            {
                arrayIndex = Convert.ToInt32(_object.GetType().GetField(indexVar).GetValue(_object));
            }

            
            double value=0;
            if (value1.HasValue)
            {
                value = value1.Value;
            }
            else if(name!=null)
            {
                value = (double)_object.GetType().GetField(name).GetValue(_object);
            }
            switch (op)
            {
                case "+":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] += value;
                    break;
                case "-":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] -= value;
                    break;
                case "*":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] *= value;
                    break;
                case "/":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] /= value;
                    break;
                case "sqrt":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] = Math.Sqrt(((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex]);
                    break;
                case "floor":
                    ((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex] = Math.Floor(((IList<double>)_object.GetType().GetField(arrayName).GetValue(_object))[arrayIndex]);
                    break;
            }
        }
        private void CopyToArrayFromArray(string arrayTo, string arrayFrom, string indexVar, int? index, object _object,int? parallelIndex)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else if (parallelIndex.HasValue)
            {
                arrayIndex = (int)parallelIndex.Value;
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
        private void UopToArrayFromArray(string arrayTo, string arrayFrom, string indexVar, int? index, string op, object _object,int? parallelIndex)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else if (parallelIndex.HasValue)
            {
                arrayIndex = (int)parallelIndex.Value;
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
        private void UopFromArray(string name,string arrayFrom,string indexVar, int? index,string op, object _object, int? parallelIndex)
        {
            int arrayIndex;
            if (index.HasValue)
            {
                arrayIndex = (int)index.Value;
            }
            else if (parallelIndex.HasValue)
            {
                arrayIndex = (int)parallelIndex.Value;
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
        private string VarPrint(string name,object _object,int i)
        {
            return $" #{i} Value of {name} : {_object.GetType().GetField(name).GetValue(_object)}";
        }
        private string ArrayPrint(string name, object _object,int k)
        {
            string result = $"#{k} ";
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
        private void ParallelStart()
        {
            parallelInstructions=parallelInstructions.Clear();
            parallel = true;
        }
        private int GetParallelCount(string name, double? value,object _object)
        {
            if (value.HasValue)
            {
                return Convert.ToInt32(value.Value);
            }
            else
            {
                return Convert.ToInt32(_object.GetType().GetField(name).GetValue(_object));
            }
        }
        private void ExecuteParallel(object _object,ref int counter, ref int memory, List<string> resultList,int parallelCount)
        {
            int parallelLoop = 0;
            int parallelCounter = 0;
            int parallelMemory = 0;
            int[] start=new int[parallelCount];

            while (parallelLoop!=parallelCount)
            {
                Parallel.For(0, parallelCount, (i, state) =>
                {
                    if (start[i] >= parallelInstructions.Count)
                    {
                        Interlocked.Increment(ref parallelLoop);
                        return;
                    } 
                    for (int j = start[i]; j < parallelInstructions.Count;)
                    {
                        if (parallelInstructions[j].instrucionType == InstructionType.BARRIER)
                        {
                            j++;
                            start[i] = j;
                            break;
                        }
                        ExecuteInstruction(parallelInstructions[j], _object, ref j, ref parallelCounter, resultList, ref parallelMemory, i);
                        start[i] = j;
                    }
                });
            }
            counter+=parallelCounter;
            memory+=parallelMemory;
        }
        private void SetRandom(string name, string stringIndex, int? index,double? min,double? max, object _object, int? parallelIndex)
        {
            var rand = new Random();
            if (parallelIndex.HasValue)
            {
                ((IList<double>)_object.GetType().GetField(name).GetValue(_object))[parallelIndex.Value] = min.Value + (rand.NextDouble() * max.Value);
                return;
            }
            if (index.HasValue)
            {
                ((IList<double>)_object.GetType().GetField(name).GetValue(_object))[index.Value] = min.Value + (rand.NextDouble() * max.Value);
                return;
            }
            if (stringIndex!=null)
            {
                ((IList<double>)_object.GetType().GetField(name).GetValue(_object))[Int32.Parse(stringIndex)] = min.Value + (rand.NextDouble() * max.Value);
                return;
            }
            _object.GetType().GetField(name).SetValue(_object, min.Value + (rand.NextDouble() * max.Value));
            

        }

    }
}
