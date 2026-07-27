using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public static class InputBinder
    {
        public static bool TryBindDigitKeys(int bindingsCount, Key start, out List<Key> bindings)
        {
            bindings = new List<Key>();
            
            if (start is < Key.Digit1 or > Key.Digit0) return false;
            
            if (bindingsCount <= 0) return false;

            int realBindingsCount = Mathf.Min(bindingsCount, Key.Digit0 - start + 1);
            
            for (int i = 0; i < realBindingsCount; i++)
            {
                bindings.Add(start + i);
            }
            
            return true;
        }
    }
}