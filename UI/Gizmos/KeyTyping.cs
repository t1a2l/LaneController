using System;
using System.Linq;
using UnityEngine;

namespace LaneController.UI.Gizmos
{
    public class KeyTyping
    {
        private Vector2 screenPos = Vector2.zero;

        public string registeredString = "";

        public float registeredFloat;

        public void Register()
        {
            if (registeredString.Length == 0)
            {
                if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    registeredString = "-";
                    screenPos = GUIUtils.MousePos;
                }
            }
            else
            {
                if (screenPos != Vector2.zero && screenPos != GUIUtils.MousePos)
                {
                    registeredString = "";
                    registeredFloat = 0f;
                    screenPos = Vector2.zero;
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    registeredString = registeredString.Remove(registeredString.Length - 1);
                    ParseRegisteredFloat();
                }
            }
            if (!registeredString.Contains('.') && registeredString.Length >= ((!registeredString.Contains('-')) ? 1 : 2) && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.KeypadPeriod)))
            {
                registeredString += ".";
            }
            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(i.ToString()) || Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), "Keypad" + i)))
                {
                    if (i == 0 && (registeredString == "0" || registeredString == "-0"))
                    {
                        break;
                    }
                    if (registeredString == "")
                    {
                        screenPos = GUIUtils.MousePos;
                    }
                    registeredString += i;
                    ParseRegisteredFloat();
                }
            }
        }

        private void ParseRegisteredFloat()
        {
            if (registeredString == "")
            {
                registeredFloat = 0f;
                screenPos = Vector2.zero;
                return;
            }
            registeredFloat = registeredString.Last() switch
            {
                '.' => float.Parse(registeredString.Remove(registeredString.Length - 1)),
                '-' => 0f,
                _ => float.Parse(registeredString),
            };
        }
    }
}
