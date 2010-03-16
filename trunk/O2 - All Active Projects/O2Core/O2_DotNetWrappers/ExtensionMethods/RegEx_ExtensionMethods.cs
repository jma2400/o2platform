﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.ExtensionMethods
{
    public static class RegEx_ExtensionMethods
    {
        public static bool nregEx(this string targetString, string regEx)
        {
            return !targetString.regEx(regEx);
        }

        public static bool regEx(this string targetString, string regEx)
        {
            return RegEx.findStringInString(targetString, regEx);
        }

        public static bool regEx(this List<string> targetStrings, string regEx)
        {
            foreach (var targetString in targetStrings)
                if (RegEx.findStringInString(targetString, regEx))
                    return true;
            return false;
        }

        public static bool regExOk(this string _string)
        {
            return (RegEx.createRegEx(_string) != null);
        }
    }
}