using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Player
{
    public static class StringExtension
    {
        public static string ThreeDots(this string str, int lastLetterIndex)
        {
            var newStr = string.Empty;

            for(var i = 0; i < lastLetterIndex; i++)
            {
                newStr += str[i];
                
                if (i == str.Length - 1)
                {
                    return newStr;
                }  
            }

            newStr += "...";

            return newStr;
        }
    }
}
