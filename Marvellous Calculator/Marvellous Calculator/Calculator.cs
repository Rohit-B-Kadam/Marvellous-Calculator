using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.IO;
using System.Text;

namespace Marvellous_Calculator
{

    class Calculator
    {
        // Basic Characteristic
        public string value;    // long
        public StringBuilder expression;
        public string screenData;
        public string displayType;                  //  In which format to display [Hex , Dec , Oct , Bin]
        public string answer;                       //  To store the data after equal to
        public int openBracketsCnt;
         
        // Adv characteristic
        Assembly DLL;
        Type NumberT;                   //  To store Address of Number class present inside DLL
        Type BitwiseT;                  //  To store Address of Bitwise class present inside DLL
        Type EvaluateStringT;
        
        //NumberFunctionList
        public List<string> NumberFunctName;

        //To store the data in file
        FileInfo PaperTape;

        
        /// <summary>
        /// Setting Up default setting from calculator
        /// </summary>
        public Calculator()
        {
            string fileName;
            string DLLPath;
            
            expression = new StringBuilder();
            displayType = "dec";

            // Clear all the data
            ClearAll();
            
            // Loaded DLL
            try
            {
                // Path of DLL
                DLLPath = @"C:\Copy_to_C_Drive\CalculatorClassLibrary.dll";
                DLL = Assembly.LoadFile(DLLPath);
                
                
                // Get Addr of Number Class
                NumberT = DLL.GetType("Number");
                
                // Get Addr of BitWise Class
                BitwiseT = DLL.GetType("Bitwise");
                
                // Get Addr of BitWise Class
                EvaluateStringT = DLL.GetType("EvaluateString");
                
                // Get static string[] characteristic form DLL
                FieldInfo field = NumberT.GetField("FunctionList");
                NumberFunctName = (List<string>)field.GetValue(null);


                //creating File
                fileName = "Log_calc_"; 
                fileName += DateTime.Now.ToString("dd-MM-yy-hh-mm-ss");
                fileName += ".txt";
                PaperTape = new FileInfo(fileName);
                
                using (StreamWriter sw = PaperTape.AppendText())
                {
                    sw.WriteLine("All Calculation Perform at  {0}", DateTime.Now.ToString("dd-MM-yy-hh-mm-ss"));
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception");
            }
            
        }

        /// <summary>
        /// Clearing the data
        /// </summary>
        public void ClearAll()
        {
            expression.Clear();
            value = "0";
            screenData = "0";
            openBracketsCnt = 0;
            answer = "";
        }

        /// <summary>
        /// Converting data from one format to another format
        /// </summary>
        /// <param name="data"> Actual Value </param>
        /// <param name="from"> Actual value format </param>
        /// <param name="to">   In which fromat data as to convert</param>
        /// <returns></returns>
        public string DataConversion(string data, string from, string to)
        {
            string decData;
            string ConvData;
            int mode = 10;
            try
            {
                checked
                {
                    if (to.Equals("Hex"))
                        mode = 16;
                    else if (to.Equals("Dec"))
                        mode = 10;
                    else if (to.Equals("Oct"))
                        mode = 8;
                    else
                        mode = 2;

                    // convert to decimal
                    if (from.Equals("Hex"))
                    {
                        decData = Convert.ToInt64(data, 16).ToString();
                    }
                    else if (from.Equals("Oct"))
                    {
                        decData = Convert.ToInt64(data, 8).ToString();
                    }
                    else if (from.Equals("Bin"))
                    {
                        decData = Convert.ToInt64(data, 2).ToString();
                    }
                    else
                    {
                        decData = data;
                    }
                }
                ConvData = Convert.ToString(Convert.ToInt64(decData), mode);
                return ConvData.ToUpper();
            }
            catch (OverflowException e)
            {
                MessageBox.Show(e.Message, "Marvellous Error");
                ClearAll();
                return "0";
            }
            // then decimal to specific 
            
        }

        /// <summary>
        /// Convert the any specfic value to Decmail
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertDataToDec( string value)
        {
            string data;
            try
            {
                checked
                {
                    if (displayType.Equals("Hex"))
                    {
                        // Hex to dec to string
                        data = Convert.ToInt64(value, 16).ToString();
                    }
                    else if (displayType.Equals("Dec"))
                    {
                        // Dec to dec to string
                        data = value.ToString();
                    }
                    else if (displayType.Equals("Oct"))
                    {
                        // Oct to dec to string
                        data = Convert.ToInt64(value, 8).ToString();
                    }
                    else
                    {
                        // Bin to dec to string
                        data = Convert.ToInt64(value.ToString(), 2).ToString();
                    }
                }
            }
            catch(OverflowException e)
            {
                MessageBox.Show(e.Message, "Marvellous Error");
                ClearAll();
                return "0";
            }
            return data;
        }

        /// <summary>
        /// Get two data (expression and value ). It can be used to display data on screen
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayData()
        {

            if (!answer.Equals(""))
                value = answer;
            
            string[] data =
            {
                DataConversion( value.ToString() , "dec" , displayType),
                expression.ToString()
            };

            return data;
        }

        /// <summary>
        /// Comvert the value to negative if the data is in positive and vise versa
        /// </summary>
        /// <returns></returns>
        public string[] PlusMinusFunc()
        {
            string[] data;
            long result;
            
            result = Convert.ToInt64(value) * (-1);
            value = result.ToString();

            data = GetDisplayData();
            data[1] = data[1] + value;
            return data;
        }

        /// <summary>
        /// Action to perform when digit is clicked
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string[] DigitClick(string str)
        {
            
            if ( value.Equals("0") && expression.Length != 0)
            {
                string exp = expression.ToString();
                char lastChar = exp[exp.Length - 1];
                if (lastChar == ')')
                {
                    expression.Append(" * ");
                }
            }

            screenData = DataConversion(value, "dec", displayType);
            // don't take input like this 00000000
            if ( !str.Equals("0") || !screenData.Equals("0"))
            {
                if (screenData.Equals("0"))
                {
                    screenData = string.Empty;
                    screenData += str;
                }
                else
                {
                    screenData += str;
                }
                value = ConvertDataToDec(screenData);
                
            }
            answer = "";

            string[] data =
            {
                DataConversion( value.ToString() , "dec" , displayType),
                expression.ToString() + value
            };

            return data;
           
        }

        /// <summary>
        /// Perform expression Evalution 
        /// </summary>
        public int PreformEvaluation()
        {
            if (openBracketsCnt != 0)
            {
                return 0;
            }
            // Binary Operator
            try
            {
                MethodInfo mi = EvaluateStringT.GetMethod("Evaluate");
                object[] param = new object[1];
                param[0] = expression.ToString();

                object iRet = mi.Invoke(null, param);
                value = iRet.ToString(); // set to evaluate answer
                return 0;
            }
            catch (TargetInvocationException ex)
            {
                // Exception throw by the DLL function
                Exception e = ex.InnerException;
                string msg = e.Message;

                if (e is InvalidOperationException)
                {
                    msg = "Syntax Error: " + e.Message;
                    MessageBox.Show($"{msg}", "Error");
                    return 1;   // Stack empty
                }
                else if (e is OverflowException)
                {
                    msg = "Maths Error: " + e.Message;
                    MessageBox.Show($"{msg}", "Error");
                    return 2;   // OverFlowException
                }
   
            }
            return -1;
        }

        /// <summary>
        /// Perform all Binary operator like ( + , - , | , & ..)
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        public string[] BinaryOperator(string opr)
        {
            string[] data = new string[2];
            int iRet;

            if (answer.Equals(""))
            {
                expression.Append(value);
            }
            else
            {
                expression.Append(answer);
                answer = "";
            }

            // evaluate the expression
            iRet = PreformEvaluation();

            // handling return value
            if (iRet == 0) //Everything is ok
            {
                expression.AppendFormat(" {0} ", opr);
            }
            else if (iRet == 1)    //Maths Error
            {
                //if (expression.Length >= 2)
                //{
                //    // Replacing last opertor with new operator
                //    int position = expression.Length - 2 - opr.Length;
                //    expression.Remove(position, expression.Length - position);
                //    expression.AppendFormat(" {0} ", opr);
                //}
            }
            else                    // OverFlow Exception
            { 
                ClearAll();
            }

            data = GetDisplayData();
            value = "0";
            return data;
            
        }

        /// <summary>
        /// Perform Unary opertion like Not(!)
        /// </summary>
        /// <param name="opr"></param>
        /// <returns></returns>
        public string[] UniaryOperator(string opr)
        {
            try
            {
                if (!answer.Equals(""))
                {
                    value = answer;
                    answer = "";
                }
                MethodInfo mi = BitwiseT.GetMethod("Not");
                object[] param = new object[1];
                param[0] = value;

                object oRet = mi.Invoke(null, param);
                value = oRet.ToString();
                
            }
            catch (TargetInvocationException ex)
            {
                Exception e = ex.InnerException;
                MessageBox.Show($"{e.Message}", "Marvellous Calculator");
                ClearAll();
            }
            return GetDisplayData();
        }

        /// <summary>
        /// Perform action when open bracket is clicked
        /// </summary>
        /// <returns></returns>
        public string[] OpenBracket()
        {
            string[] data = new string[2];
            openBracketsCnt++;
            
            if (value.Equals("0") && screenData.Length != 0)
            {
                // if user don't give the data
            }
            else if (answer.Equals(""))
            {
                expression.Append(value);
            }
            else
            {
                expression.Append(answer);
                answer = "";
            }


            if (expression.Length == 0)
            {
                expression.Append("( ");
            }
            else
            {
                string exp = expression.ToString();
                char lastChar = exp[exp.Length - 1];
                if (char.IsNumber(lastChar) || lastChar == ')')
                {
                    expression.Append(" * ( ");
                }
                else
                {
                    expression.Append("( ");
                }

            }

            data[0] = value.ToString();
            data[1] = expression.ToString();

            value = "0";
            return data;
        }


        //Perform the action when close bracket is clicked 
        public string[] CloseBracket()
        {
            int iRet;
            string[] data = new string[2];

            if (value.Equals("0") && screenData.Length != 0)
            {
                // if user don't give the data.. Do nothing
            }
            else
            {
                expression.Append(value);
            }
            openBracketsCnt--;
            expression.Append(" )");


            // evaluate the expression
            iRet = PreformEvaluation();

            if (iRet == 0) //Everything is ok
            {
            }
            else if (iRet == 1)    //Maths Error
            {

                int position = expression.Length - 2;
                expression.Remove(position, expression.Length - position);
                openBracketsCnt++;
            }
            else                    // OverFlow Exception
            {
                ClearAll();
            }

            data = GetDisplayData();
            value = "0";
            return data;
        }

        // When user click '='
        public string[] EqualOpeartor()
        {
            string[] data = new string[2];
            
            int iRet;
            
            if (answer.Equals(""))
            {
                if (value.Equals("0") && screenData.Length != 0)
                {
                    // if user don't give the data
                }
                else
                {
                    expression.Append(value);
                }
                // evaluate the expression
                iRet = PreformEvaluation();

                if (iRet == 0) //Everything is ok
                {
                }
                else if (iRet == 1)    //Maths Error
                {

                }
                else                    // OverFlow Exception
                {
                    ClearAll();
                }


                data = GetDisplayData();
                expression.Clear();
                answer = value;
                value = "0";

                using (StreamWriter sw = PaperTape.AppendText())
                {
                    sw.WriteLine();
                    sw.WriteLine("EQU: {0}", data[1]);
                    sw.WriteLine("    = {0} {1}", data[0], displayType);
                    sw.Flush();
                }
            }
            else
            {
                data = GetDisplayData();
            }
            return data;
        }


        // Bitwise opertion like (onBit , offBit, toggleBit
        public string[] BitwiseOperation( string opr , int pos)
        {
            string[] data;
            try
            {

                if (!answer.Equals(""))
                {
                    value = answer;
                    answer = "";
                }
                
                string valueStore = value;

                MethodInfo mi = BitwiseT.GetMethod(opr);
                object[] param = new object[2];
                param[0] = value;
                param[1] = pos;

                object oRet = mi.Invoke(null, param);
                value = oRet.ToString();

                using (StreamWriter sw = PaperTape.AppendText())
                {
                    sw.WriteLine();
                    sw.WriteLine("{2} operation poistion {1} on {0} is: {3} ", valueStore , pos , opr , value);
                    sw.Flush();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Marvellous Calculator");
            }

            data = GetDisplayData();
            data[1] += value;
            return data;
            //screenData = DataConversion(value, "Dec", displayType);
            
        }

        // To function like isPrime , isPrefect. Perpose to add this, is to use DLL concept
        public void NumberFunction(string functName)
        {
            try
            {
                if (!answer.Equals(""))
                {
                    value = answer;
                    answer = "";
                }

                MethodInfo mi = NumberT.GetMethod(functName);
                object[] param = new object[1];
                param[0] = value.ToString();

                object oRet = mi.Invoke(null, param);
                string temp = $"{value} {functName}: {oRet.ToString()}";
                MessageBox.Show($"{temp}", "Answer");

                using (StreamWriter sw = PaperTape.AppendText())
                {
                    sw.WriteLine();
                    sw.WriteLine(temp);
                    sw.Flush();
                }

            }
            catch (TargetInvocationException ex)
            {
                Exception e = ex.InnerException;
                MessageBox.Show($"{e.Message}", "Marvellous Calculator");
               
            }
        }
        
        /// <summary>
        /// Save or delete the file according to data
        /// </summary>
        /// <param name="result"></param>
        public void SaveFileOrNot(char result)
        {
            if(result == 'y')
            {
                MessageBox.Show($"File is save as {PaperTape.Name}");
            }
            else
            {
                PaperTape.Delete();
            }
        }

        ~Calculator()
        {
            //unload Dll
        }
    }
}
