using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class StringExtension
    {
        private static readonly Dictionary<char, int> AlphabeticDigits = new()
        {
            {'a', 10}, {'b', 11}, {'c', 12}, {'d', 13}, {'e', 14}, {'f', 15}
        };
        
        public static bool TryConvertToDec(this string number, int from, out int destination)
        {
            destination = 0;
            
            for (int i = 0; i < number.Length; i++)
            {
                char symbol = number[i];
                int decDigit = symbol - '0';
                
                if ((decDigit < 0 || decDigit > 9) && !AlphabeticDigits.ContainsKey(symbol))
                {
                    return false;
                }

                if (AlphabeticDigits.TryGetValue(symbol, out int digit))
                {
                    destination += digit * (int)Mathf.Pow(from, number.Length - 1 - i);
                }
                else
                {
                    destination += decDigit * (int) Mathf.Pow(from, number.Length - 1 - i);
                }
            }

            return true;
        }
    }
}