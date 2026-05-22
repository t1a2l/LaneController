using System;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using LaneController.KianCommons.Utils;
using UnityEngine;

namespace LaneController.KianCommons.UI
{
    public static class SettingsUIHelperExtension
    {
        public static UICheckBox AddUpdatingCheckbox(this UIHelperBase helper, string label, Action<bool> SetValue, Func<bool> GetValue)
        {
            Log.Info("option " + label + " is " + GetValue());
            UICheckBox uICheckBox = helper.AddCheckbox(label, GetValue(), delegate (bool value)
            {
                try
                {
                    SetValue(value);
                    Log.Info("option '" + label + "' is set to " + value);
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }) as UICheckBox;
            uICheckBox.eventVisibilityChanged += delegate (UIComponent c, bool val)
            {
                (c as UICheckBox).isChecked = GetValue();
            };
            return uICheckBox;
        }

        public static UICheckBox AddSavedToggle(this UIHelperBase helper, string label, SavedBool savedBool, Action<bool> OnToggled)
        {
            Log.Info("option " + label + " is " + savedBool.value);
            return helper.AddCheckbox(label, savedBool, delegate (bool value)
            {
                try
                {
                    savedBool.value = value;
                    Log.Info("option '" + label + "' is set to " + value);
                    OnToggled(value);
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }) as UICheckBox;
        }

        public static UICheckBox AddSavedToggle(this UIHelperBase helper, string label, SavedBool savedBool, Action OnToggled)
        {
            return helper.AddSavedToggle(label, savedBool, (Action<bool>)delegate
            {
                OnToggled();
            });
        }

        public static UICheckBox AddSavedToggle(this UIHelperBase helper, string label, SavedBool savedBool)
        {
            return helper.AddSavedToggle(label, savedBool, (Action<bool>)delegate
            {
            });
        }

        public static UITextField AddSavedClampedIntTextfield(this UIHelperBase helper, string label, SavedInt savedInt, int min, int max, Action<int> OnSubmit)
        {
            UITextField field = null;
            field = helper.AddTextfield(label, savedInt.value.ToString(), delegate
            {
            }, delegate (string value)
            {
                if (int.TryParse(value, out var result))
                {
                    result = Mathf.Clamp(result, min, max);
                    if (result != savedInt.value)
                    {
                        savedInt.value = result;
                        OnSubmit(result);
                    }
                }
                else
                {
                    field?.text = savedInt.value.ToString();
                }
            }) as UITextField;
            field.numericalOnly = true;
            field.allowFloats = false;
            field.allowNegative = min < 0;
            return field;
        }

        public static UITextField AddSavedClampedIntTextfield(this UIHelperBase helper, string label, SavedInt savedInt, int min, int max, Action OnSubmit)
        {
            return helper.AddSavedClampedIntTextfield(label, savedInt, min, max, (Action<int>)delegate
            {
                OnSubmit();
            });
        }

        public static UITextField AddSavedClampedIntTextfield(this UIHelperBase helper, string label, SavedInt savedInt, int min, int max)
        {
            return helper.AddSavedClampedIntTextfield(label, savedInt, min, max, (Action<int>)delegate
            {
            });
        }

        public static UILabel AddLabel(this UIHelper helper, string text, string tooltip = null, Color32? textColor = null)
        {
            Assertion.NotNull(helper.self, "self");
            Assertion.Assert(helper.self is UIComponent, "self is " + helper.self.GetType().Name);
            UILabel uILabel = (helper.self as UIComponent).AddUIComponent<UILabel>();
            uILabel.text = text;
            uILabel.tooltip = tooltip;
            if (textColor.HasValue)
            {
                uILabel.textColor = textColor.Value;
            }
            return uILabel;
        }
    }
}
