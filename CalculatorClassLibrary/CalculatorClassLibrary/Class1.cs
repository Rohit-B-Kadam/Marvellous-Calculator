using System;
using System.Collections.Generic;

/// <summary>
/// EvaluationString: Using Shunting-yard algorithm(postfix) to solve the expression inside string
/// </summary>
public class EvaluateString
{

    /// <summary>
    /// Evaluate the expression
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>return the evaluated ans as a string</returns>
    public static string Evaluate(string expression)
    {
        string[] tokens = expression.Split(' ');
        
        // Operator support 
        string oprSupport = " + - * / | & ^ ! % >> << ( )";

        // Stack for numbers: values 
        Stack<long> values = new Stack<long>();

        // Stack for Operators: ops 
        Stack<string> ops = new Stack<string>();



        foreach (var token in tokens)
        {
            if (token == "")
                continue;

            // Current token is a number, push it to stack for numbers
            if (isNumber(token))
            {
                values.Push(Convert.ToInt64(token));
            }
            else if (token == "(")
            {
                ops.Push(token);
            }

            // solve the expression till "(" 
            else if (token == ")")
            {
                while (ops.Peek() != "(")
                    values.Push(DoOperation(ops.Pop(), values.Pop(), values.Pop()));
                ops.Pop(); // removing "("
            }

            // Current token is an operator. 
            else if (oprSupport.Contains(token))
            {
                while ((ops.Count != 0) && HasPrecedence(token, ops.Peek()))
                {
                    values.Push(DoOperation(ops.Pop(), values.Pop(), values.Pop()));
                }
                ops.Push(token);
            }
        }

        while (ops.Count != 0)
        {
            values.Push(DoOperation(ops.Pop(), values.Pop(), values.Pop()));
        }

        return values.Pop().ToString();
    }

    public static bool isNumber(string numString)
    {
        long number = 0;
        bool isNum = long.TryParse(numString, out number);

        if ( isNum)
            return true;
        else
            return false;
    }

    // Returns true if 'op2' has higher or same precedence as 'op1', 
    // otherwise returns false. 
    public static bool HasPrecedence(string op1, string op2)
    {
        int value1 = FindPrecedence(op1);
        int value2 = FindPrecedence(op2);

        if (value2 >= value1)
            return true;

        return false;
    }

    /* Given precedence according to:
     * https://introcs.cs.princeton.edu/java/11precedence/
     **/
    public static  int FindPrecedence(string opr)
    {
        int value = 0;

        switch (opr)
        {
            case "*":
            case "/":
            case "%":
                value = 12;
                break;
            case "+":
            case "-":
                value = 11;
                break;
            case ">":
            case "<":  //left sift and right shift
                value = 10;
                break;
            case "!":
                value = 14;
                break;
            case "&":
                value = 7;
                break;
            case "^":
                value = 6;
                break;
            case "|":
                value = 5;
                break;
        }
        return value;
    }

    // A utility method to apply an operator 'op' on operands 'a'  
    // and 'b'. Return the result. 
    public static  long DoOperation(string op, long b, long a)
    {
        
       checked
        {
            switch (op)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                case "%":
                    return a % b;
                case "&":
                    return a & b;
                case "^":
                    return a ^ b;
                case "|":
                    return a | b;
                    
                case ">>":
                   return Convert.ToInt64(Bitwise.Rsh(a.ToString(), b.ToString()));
                case "<<":
                   return Convert.ToInt64(Bitwise.Lsh(a.ToString(), b.ToString()));

            }
        }
        return 0;
    }

}

public class Number
{
    // Function list to display in calculator
    public static List<string> FunctionList = new List<string>()
    {
        "IsPrime", "IsPerfect", "IsArmstrong", "IsStrong" ,
        "Factorial", "Factors", "SumFactors", "CountDigit"
    };

    /* typing /// visual studio automatically create <summary>  */
    
    /// <summary>
    ///  IsPrime: To Check whether Number is Prime or Not
    /// </summary>
    /// <param name="iNum"></param>
    public static bool IsPrime(string sNum)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);
            long iHalf = lNum / 2;       // Divisor range of any number [ 1 , number/2 ]

            // Checking if there is any divisble of that number. if there is we return false 
            for (int i = 2; i <= iHalf; i++)
            {
                if ((lNum % i) == 0)
                    return false;
            }
            return true;
        }

    }
    
    /// <summary>
    /// IsPrefect: To check whether Number is Prefect or Not
    /// </summary>
    /// <param name="iNum"></param>
    public static bool IsPerfect(string sNum)
    {
       checked
        {
            long lNum = Convert.ToInt64(sNum);
            long iRet = 0;
            iRet = Convert.ToInt64(SumFactors(lNum.ToString()));

            if (iRet != lNum)
            {
                return false;
            }
            return true;
        }
    }


    /// <summary>
    /// IsArmstrong: Sum of digit raise to power of 371 eg 3**3 + 7**3 + 1**3 = 371
    /// </summary>
    /// <param name="iNum"></param>
    /// <returns></returns>
    public static bool IsArmstrong(string sNum)
    {
       checked
        {
            long lNum = Convert.ToInt64(sNum);
            long iValue = lNum;
            long iSum = 0;
            while (iValue != 0)
            {
                iSum += Convert.ToInt64(Math.Pow((iValue % 10), 3));
                iValue /= 10;
            }

            if (iSum != lNum)
                return false;
            return true;
        }    
    }


    /// <summary>
    /// IsStrong: Sum of factorial of digit 145 eg 1! + 4! + 5! = 145
    /// </summary>
    /// <param name="iNum"></param>
    /// <returns></returns>
    public static bool IsStrong( string sNum)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);
            long iValue = lNum;
            long iSum = 0;
            int iDigit = 0;
            long iMult = 1;
            while (iValue != 0)
            {
                iDigit = (int)iValue % 10;
                iMult = Convert.ToInt64(Factorial(iDigit.ToString()));
                iSum += iMult;
                iValue /= 10;
            }

            if (iSum != lNum)
                return false;
            return true;
        }
    }



    /// <summary>
    /// Factorial: To perform Factorial of given input( n! )
    /// </summary>
    /// <param name="iNum"></param>
    /// <returns></returns>
    public static string Factorial( string sNum)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);
            long lResult = 1;
            while (lNum != 0)
            {
                lResult *= lNum;
                lNum--;
            }
            return lResult.ToString();
        }
    }

   
    /// <summary>
    /// Factors: To Find Factors of Given Number
    /// </summary>
    /// <param name="iNum"></param>
    /// <returns>A string contain all the factors</returns>
    public static string Factors(string sNum )
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);
            string str = "{ ";
            for (int iCnt = 1; iCnt <= lNum / 2; iCnt++)
            {
                if ((lNum % iCnt) == 0)
                {
                    str += $" {iCnt},";
                }
            }
            str += " }";
            return str;
        }
    }

    /// <summary>
    /// SumFactor: Sum of all factors of number
    /// </summary>
    /// <param name="iNum"></param>
    /// <returns></returns>
    public static string SumFactors(string sNum)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);
            long lSum = 0;
            for (int iCnt = 1; iCnt <= lNum / 2; iCnt++)
            {
                if ((lNum % iCnt) == 0)
                {
                    lSum += iCnt;
                }
            }

            return lSum.ToString();
        }
    }


    /// <summary>
    /// Count Number of digit present in given Number
    /// </summary>
    /// <param name="iNum"></param>
    /// <returns></returns>
    public static string CountDigit(string sNum)
    {
       checked
        {
            long lNum = Convert.ToInt64(sNum);
            int iCnt = 0;
            while (lNum != 0)
            {
                iCnt++;
                lNum /= 10;
            }
            return iCnt.ToString();
        }
    }
  
}


public class Bitwise
{
   
   //not use
    public int CountOnBit( int iNum)
    {
        int iCnt = 0;
        int temp = iNum;

        while (temp != 0)
        {
            if ((temp % 2) == 1)
                iCnt++;
            temp /= 2;
        }
        return iCnt;
    }

    //not use
    public bool CheckBit(int iNum, int pos)
    {
        int mark = 0;
        mark = Convert.ToInt32(Math.Pow(2, pos));

        if ((iNum & mark) == 0)
            return false;
        return true;
    }


    public static string OnBit(string sNum, int iPos)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);

            long mark = 0;
            mark = Convert.ToInt64(Math.Pow(2, iPos));

            // I have make that bit 1
            lNum = lNum | mark;
            return lNum.ToString();
        }
    }

    public static string OffBit(string sNum, int iPos)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);

            long mark = 0;
            mark = Convert.ToInt64(Math.Pow(2, iPos));

            // I have make that bit 1
            lNum = lNum | mark;
            // Then I XOR that bit to make it 0
            lNum = lNum ^ mark;

            return lNum.ToString();
        }
    }
     
    public static string ToggleBit(string sNum, int iPos)
    {
        checked
        {
            long lNum = Convert.ToInt64(sNum);

            long mark = 0;
            mark = Convert.ToInt64(Math.Pow(2, iPos));
            lNum = lNum ^ mark;

            return lNum.ToString();
        }
    }

    
    public static string ToBinary(string sNum)
    {
        checked
        {
            return Convert.ToString(Convert.ToInt64(sNum), 2);
        }
    }
    
    public static string Not(string sNum1)
    {
        checked
        {
            string sNum2 = "";
            sNum1 = ToBinary(sNum1);

            int i;
            for (i = sNum1.Length - 1; i >= 0; i--)
            {
                if (sNum1[i] == '1')
                {
                    sNum2 += '0';
                }
                else
                    sNum2 += '1';
            }
            i = 63 - sNum1.Length;
            while (i >= 0)
            {
                sNum2 += '1';
                i--;
            }

            string rev = "";
            for (i = sNum2.Length - 1; i >= 0; i--)
            {
                rev += sNum2[i];
            }

            return Convert.ToInt64(rev, 2).ToString();
        }
    }

    public static string Rsh(string sNum1, string sNo)
    {
        checked
        {
            string sNum2;
            int no = Convert.ToInt32(sNo);
            sNum1 = ToBinary(sNum1);
            int len = sNum1.Length - no;

            sNum2 = sNum1.Substring(0, len);

            return Convert.ToInt64(sNum2, 2).ToString();
        }
    }

    public static string Lsh(string sNum1, string sNo)
    {
        checked
        {
            sNum1 = ToBinary(sNum1);
            int no = Convert.ToInt32(sNo);
            for (int i = 0; i < no; i++)
            {
                sNum1 += '0';
            }

            if (sNum1.Length >= 32)
            {
                sNum1 = sNum1.Substring(no - 1);
            }

            return Convert.ToInt64(sNum1, 2).ToString();
        }
    }

}
