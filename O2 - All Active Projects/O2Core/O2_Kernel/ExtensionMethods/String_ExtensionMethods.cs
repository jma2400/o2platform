﻿
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace O2.Kernel.ExtensionMethods
{
    public static class String_ExtensionMethods
    {

        public static string str(this object _object)
        {
            return _object.ToString();
        }
        
        public static string str(this bool value, string trueValue, string falseValue)
        {
            return value ? trueValue : falseValue;
        }

        public static bool validString(this object _object)
        {
            return _object.str().valid();
        }

        public static bool eq(this string string1, string string2)
        {
            return (string1 == string2);
        }

        public static void eq(this string string1, string stringToFind, MethodInvoker onMatch)
        {
            string1.eq(new [] {stringToFind}, onMatch);
        }
        public static void eq(this string string1, List<string> stringsToFind, MethodInvoker onMatch)
        {
            string1.eq(stringsToFind.ToArray(), onMatch);
        }

        public static void eq(this string string1, string[] stringsToFind, MethodInvoker onMatch) 
        {
            foreach(var stringToFind in stringsToFind)
                if (string1 == stringToFind)
                {
                    onMatch();
                    return;
                }
        }        

        public static bool neq(this string string1, string string2)
        {
            return (string1 != string2);
        }
        
        public static bool contains(this string targetString, string stringToFind)
        {
            return (stringToFind != null)
                        ? targetString.Contains(stringToFind)
                        : false;
        }

        public static bool contains(this string targetString, List<string> stringsToFind)
        {
            if (stringsToFind != null)
                foreach (var stringToFind in stringsToFind)
                    if (targetString.contains(stringToFind))
                        return true;
            return false;
        }
   
        public static bool starts(this string textToSearch, List<string> stringsToFind)
        {
            foreach(var stringToFind in stringsToFind)
                if (textToSearch.starts(stringToFind))
                    return true;
            return false;
        }

        public static bool starts(this string stringToSearch, string stringToFind)
        {
            return stringToSearch.StartsWith(stringToFind);
        }

        public static void starts(this string stringToSearch, string[] stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }

        public static void starts(this string stringToSearch, List<string> stringsToFind, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind, true, onMatch);
        }

        public static void starts(this string stringToSearch, List<string> stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            stringToSearch.starts(stringsToFind.ToArray(), invokeOnMatchIfEqual, onMatch);
        }

        public static void starts(this string stringToSearch, string[] stringsToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            foreach(var stringToFind in stringsToFind)
                if (stringToSearch.starts(stringToFind, invokeOnMatchIfEqual, onMatch))
                    return;
        }

        public static void starts(this string stringToSearch, string textToFind, Action<string> onMatch)
        {
            stringToSearch.starts(textToFind, true, onMatch);
        }

        public static bool starts(this string stringToSearch, string textToFind, bool invokeOnMatchIfEqual, Action<string> onMatch)
        {
            if (stringToSearch.starts(textToFind))
            {
                var textForCallback = stringToSearch.remove(textToFind);
                if (invokeOnMatchIfEqual || textForCallback.valid())
                {
                    onMatch(textForCallback);
                    return true;
                }
            }
            return false;
        }

        public static bool nstarts(this string stringToSearch, string stringToFind)
        {
            return ! starts(stringToSearch, stringToFind);
        }

        public static bool ends(this string string1, string string2)
        {
            return string1.EndsWith(string2);
        }

        public static bool inValid(this string _string)
        {
            return !_string.valid();
        }
        public static bool valid(this string _string)
        {
            if (false == string.IsNullOrEmpty(_string))
                if (_string.Trim() != "")
                    return true;
            return false;
        }

        public static bool empty(this string _string)
        {
            return !(_string.valid());
        }

        public static string format(this string format, params object[] parameters)
        {
            return string.Format(format, parameters);
        }

        public static string remove(this string _string, params string[] stringsToRemove)
        {
            return _string.replaceAllWith("", stringsToRemove);
        }

        public static string toSpace(this string _string, params string[] stringsToChange)
        {
            return _string.replaceAllWith(" ", stringsToChange);
        }

        public static string replace(this string targetString, string stringToFind, string stringToReplaceWith)
        {
            targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            targetString = targetString.Replace(stringToFind.lower(), stringToReplaceWith);
            targetString = targetString.Replace(stringToFind.upper(), stringToReplaceWith);
            return targetString;
        }

        public static string replaceAllWith(this string targetString, string stringToReplaceWith, params string[] stringsToFind)
        {
            foreach (var stringToFind in stringsToFind)
                targetString = targetString.Replace(stringToFind, stringToReplaceWith);
            return targetString;
        }

        public static int size(this string _string)
        {
            return _string.Length;
        }

        public static string line(this string firstString, string secondString)
        {
            return firstString.line() + secondString;
        }

        public static string line(this string firstString)
        {
            return firstString + Environment.NewLine;
        }

        public static int toInt(this string _string)
        {
            Int32 value;
            Int32.TryParse(_string, out value);
            return value;
        }

        public static string hex(this byte value)
        {
            return Convert.ToString(value, 16).caps();
        }

        public static string hex(this int value)
        {
            return Convert.ToString(value, 16).caps();
        }

        public static string caps(this string value)
        {
            return value.ToUpper();
        }

        public static string ascii(this byte value)
        {
            return Encoding.ASCII.GetString(new[] { value });
        }

        public static string ascii(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static string unicode(this byte value)
        {
            return Encoding.Unicode.GetString(new[] { value });
        }

        public static string unicode(this byte[] bytes)
        {
            return Encoding.Unicode.GetString(bytes);
        }

        //this method is only really good to find ASCII binary strings
        public static List<string> strings(this byte[] bytes, bool ignoreCharZeroAfterValidChar, int minimumStringSize)
        {
            var extractedStrings = new List<string>();
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                var value = bytes[i];
                if (value > 31 && value < 127) // see http://www.asciitable.com/
                {
                    var str = value.ascii();
                    stringBuilder.Append(str);
                    if (ignoreCharZeroAfterValidChar)
                        if (bytes[i + 1] == 0)
                            i++;
                }
                else
                {
                    if (stringBuilder.Length > 0)
                    {
                        if (minimumStringSize == -1 || stringBuilder.Length > minimumStringSize)
                            extractedStrings.Add(stringBuilder.ToString());
                        stringBuilder = new StringBuilder();
                    }
                }
            }            
            return extractedStrings;
        }

        public static void removeLastChar(this StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
        }

        public static string removeLastChar(this string _string)
        {
            if (_string.Length > 0)
                return _string.Remove(_string.Length - 1, 1);
            return _string;
        }

        public static string appendGuid(this string _string)
        {
            return "{0} {1}".format(_string, Guid.NewGuid());
        }

        public static string lower(this string _string)
        {
            return _string.ToLower();
        }

        public static string upper(this string _string)
        {
            return _string.ToUpper();
        }

        public static string trim(this string _string)
        {           
            return _string.Trim();
        }

        public static int index(this string targetString, string stringToFind)
        {
            return targetString.IndexOf(stringToFind);
        }

        public static int indexLast(this string targetString, string stringToFind)
        {
            return targetString.LastIndexOf(stringToFind);
        }

        public static string pad(this string targetString, int totalWidth)
        {
            return targetString.PadLeft(totalWidth);
        }

        public static string add(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }

        public static string insertAfter(this string targetString, string stringToAdd)
        {
            return targetString + stringToAdd;
        }

        public static string insertBefore(this string targetString, string stringToAdd)
        {
            return stringToAdd + targetString;
        }
    }
}