using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace LaneController.ModsCommon
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    public class CommonLocalize
    {
        private static ResourceManager resourceMan;

        private static CultureInfo resourceCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    ResourceManager resourceManager = new ResourceManager("ModsCommon.CommonLocalize", typeof(CommonLocalize).Assembly);
                    resourceMan = resourceManager;
                }
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        public static string Dependency_Conflict => ResourceManager.GetString("Dependency_Conflict", resourceCulture);

        public static string Dependency_Disable => ResourceManager.GetString("Dependency_Disable", resourceCulture);

        public static string Dependency_DisableMod => ResourceManager.GetString("Dependency_DisableMod", resourceCulture);

        public static string Dependency_Enable => ResourceManager.GetString("Dependency_Enable", resourceCulture);

        public static string Dependency_EnableMod => ResourceManager.GetString("Dependency_EnableMod", resourceCulture);

        public static string Dependency_Get => ResourceManager.GetString("Dependency_Get", resourceCulture);

        public static string Dependency_Missing => ResourceManager.GetString("Dependency_Missing", resourceCulture);

        public static string Dependency_MissingAndConflict => ResourceManager.GetString("Dependency_MissingAndConflict", resourceCulture);

        public static string Dependency_NeedFix => ResourceManager.GetString("Dependency_NeedFix", resourceCulture);

        public static string Dependency_NoIssues => ResourceManager.GetString("Dependency_NoIssues", resourceCulture);

        public static string Dependency_Remove => ResourceManager.GetString("Dependency_Remove", resourceCulture);

        public static string Dependency_Subscribe => ResourceManager.GetString("Dependency_Subscribe", resourceCulture);

        public static string Dependency_Unsubscribe => ResourceManager.GetString("Dependency_Unsubscribe", resourceCulture);

        public static string FieldPanel_ScrollWheel => ResourceManager.GetString("FieldPanel_ScrollWheel", resourceCulture);

        public static string Key_Alt => ResourceManager.GetString("Key_Alt", resourceCulture);

        public static string Key_Control => ResourceManager.GetString("Key_Control", resourceCulture);

        public static string Key_Enter => ResourceManager.GetString("Key_Enter", resourceCulture);

        public static string Key_Shift => ResourceManager.GetString("Key_Shift", resourceCulture);

        public static string Key_Tab => ResourceManager.GetString("Key_Tab", resourceCulture);

        public static string ListPanel_ScrollWheel => ResourceManager.GetString("ListPanel_ScrollWheel", resourceCulture);

        public static string MessageBox_Cancel => ResourceManager.GetString("MessageBox_Cancel", resourceCulture);

        public static string MessageBox_DontShowAgain => ResourceManager.GetString("MessageBox_DontShowAgain", resourceCulture);

        public static string MessageBox_MoreInfo => ResourceManager.GetString("MessageBox_MoreInfo", resourceCulture);

        public static string MessageBox_No => ResourceManager.GetString("MessageBox_No", resourceCulture);

        public static string MessageBox_OK => ResourceManager.GetString("MessageBox_OK", resourceCulture);

        public static string MessageBox_Yes => ResourceManager.GetString("MessageBox_Yes", resourceCulture);

        public static string Mod_BetaWarningAgree => ResourceManager.GetString("Mod_BetaWarningAgree", resourceCulture);

        public static string Mod_BetaWarningCaption => ResourceManager.GetString("Mod_BetaWarningCaption", resourceCulture);

        public static string Mod_BetaWarningGetStable => ResourceManager.GetString("Mod_BetaWarningGetStable", resourceCulture);

        public static string Mod_BetaWarningMessage => ResourceManager.GetString("Mod_BetaWarningMessage", resourceCulture);

        public static string Mod_DescriptionBeta => ResourceManager.GetString("Mod_DescriptionBeta", resourceCulture);

        public static string Mod_LinuxWarning => ResourceManager.GetString("Mod_LinuxWarning", resourceCulture);

        public static string Mod_LoadedWithErrors => ResourceManager.GetString("Mod_LoadedWithErrors", resourceCulture);

        public static string Mod_Locale_cs => ResourceManager.GetString("Mod_Locale_cs", resourceCulture);

        public static string Mod_Locale_da => ResourceManager.GetString("Mod_Locale_da", resourceCulture);

        public static string Mod_Locale_de => ResourceManager.GetString("Mod_Locale_de", resourceCulture);

        public static string Mod_Locale_en => ResourceManager.GetString("Mod_Locale_en", resourceCulture);

        public static string Mod_Locale_en_gb => ResourceManager.GetString("Mod_Locale_en-gb", resourceCulture);

        public static string Mod_Locale_es => ResourceManager.GetString("Mod_Locale_es", resourceCulture);

        public static string Mod_Locale_fi => ResourceManager.GetString("Mod_Locale_fi", resourceCulture);

        public static string Mod_Locale_fr => ResourceManager.GetString("Mod_Locale_fr", resourceCulture);

        public static string Mod_Locale_hu => ResourceManager.GetString("Mod_Locale_hu", resourceCulture);

        public static string Mod_Locale_id => ResourceManager.GetString("Mod_Locale_id", resourceCulture);

        public static string Mod_Locale_it => ResourceManager.GetString("Mod_Locale_it", resourceCulture);

        public static string Mod_Locale_ja => ResourceManager.GetString("Mod_Locale_ja", resourceCulture);

        public static string Mod_Locale_ko => ResourceManager.GetString("Mod_Locale_ko", resourceCulture);

        public static string Mod_Locale_mr => ResourceManager.GetString("Mod_Locale_mr", resourceCulture);

        public static string Mod_Locale_nl => ResourceManager.GetString("Mod_Locale_nl", resourceCulture);

        public static string Mod_Locale_pl => ResourceManager.GetString("Mod_Locale_pl", resourceCulture);

        public static string Mod_Locale_pt => ResourceManager.GetString("Mod_Locale_pt", resourceCulture);

        public static string Mod_Locale_ro => ResourceManager.GetString("Mod_Locale_ro", resourceCulture);

        public static string Mod_Locale_ru => ResourceManager.GetString("Mod_Locale_ru", resourceCulture);

        public static string Mod_Locale_tr => ResourceManager.GetString("Mod_Locale_tr", resourceCulture);

        public static string Mod_Locale_zh_cn => ResourceManager.GetString("Mod_Locale_zh-cn", resourceCulture);

        public static string Mod_Locale_zh_tw => ResourceManager.GetString("Mod_Locale_zh-tw", resourceCulture);

        public static string Mod_LocaleGame => ResourceManager.GetString("Mod_LocaleGame", resourceCulture);

        public static string Mod_Status => ResourceManager.GetString("Mod_Status", resourceCulture);

        public static string Mod_Status_GameOutOfDate => ResourceManager.GetString("Mod_Status_GameOutOfDate", resourceCulture);

        public static string Mod_Status_LoadingError => ResourceManager.GetString("Mod_Status_LoadingError", resourceCulture);

        public static string Mod_Status_ModOutOfDate => ResourceManager.GetString("Mod_Status_ModOutOfDate", resourceCulture);

        public static string Mod_Status_OperateNormally => ResourceManager.GetString("Mod_Status_OperateNormally", resourceCulture);

        public static string Mod_Status_Unknown => ResourceManager.GetString("Mod_Status_Unknown", resourceCulture);

        public static string Mod_Support => ResourceManager.GetString("Mod_Support", resourceCulture);

        public static string Mod_Version => ResourceManager.GetString("Mod_Version", resourceCulture);

        public static string Mod_VersionWarning_GameOutOfDate => ResourceManager.GetString("Mod_VersionWarning_GameOutOfDate", resourceCulture);

        public static string Mod_VersionWarning_ModOutOfDate => ResourceManager.GetString("Mod_VersionWarning_ModOutOfDate", resourceCulture);

        public static string Mod_WhatsNewCaption => ResourceManager.GetString("Mod_WhatsNewCaption", resourceCulture);

        public static string Mod_WhatsNewMessageBeta => ResourceManager.GetString("Mod_WhatsNewMessageBeta", resourceCulture);

        public static string Mod_WhatsNewVersion => ResourceManager.GetString("Mod_WhatsNewVersion", resourceCulture);

        public static string Panel_Additional => ResourceManager.GetString("Panel_Additional", resourceCulture);

        public static string Setting_Donate => ResourceManager.GetString("Setting_Donate", resourceCulture);

        public static string Settings_Cancel => ResourceManager.GetString("Settings_Cancel", resourceCulture);

        public static string Settings_ChangeLog => ResourceManager.GetString("Settings_ChangeLog", resourceCulture);

        public static string Settings_ForLinuxUsers => ResourceManager.GetString("Settings_ForLinuxUsers", resourceCulture);

        public static string Settings_General => ResourceManager.GetString("Settings_General", resourceCulture);

        public static string Settings_GeneralTab => ResourceManager.GetString("Settings_GeneralTab", resourceCulture);

        public static string Settings_Language => ResourceManager.GetString("Settings_Language", resourceCulture);

        public static string Settings_Notifications => ResourceManager.GetString("Settings_Notifications", resourceCulture);

        public static string Settings_PressAnyKey => ResourceManager.GetString("Settings_PressAnyKey", resourceCulture);

        public static string Settings_ShortcutActivateTool => ResourceManager.GetString("Settings_ShortcutActivateTool", resourceCulture);

        public static string Settings_Shortcuts => ResourceManager.GetString("Settings_Shortcuts", resourceCulture);

        public static string Settings_ShortcutSelectionStepOver => ResourceManager.GetString("Settings_ShortcutSelectionStepOver", resourceCulture);

        public static string Settings_ShowOnlyMajor => ResourceManager.GetString("Settings_ShowOnlyMajor", resourceCulture);

        public static string Settings_ShowTooltips => ResourceManager.GetString("Settings_ShowTooltips", resourceCulture);

        public static string Settings_ShowWhatsNew => ResourceManager.GetString("Settings_ShowWhatsNew", resourceCulture);

        public static string Settings_SolveCrashOnLinux => ResourceManager.GetString("Settings_SolveCrashOnLinux", resourceCulture);

        public static string Settings_SupportTab => ResourceManager.GetString("Settings_SupportTab", resourceCulture);

        public static string Settings_ToolButton => ResourceManager.GetString("Settings_ToolButton", resourceCulture);

        public static string Settings_ToolButtonBoth => ResourceManager.GetString("Settings_ToolButtonBoth", resourceCulture);

        public static string Settings_ToolButtonOnlyToolbar => ResourceManager.GetString("Settings_ToolButtonOnlyToolbar", resourceCulture);

        public static string Settings_ToolButtonOnlyUUI => ResourceManager.GetString("Settings_ToolButtonOnlyUUI", resourceCulture);

        public static string Settings_TranslationDescription => ResourceManager.GetString("Settings_TranslationDescription", resourceCulture);

        public static string Settings_TranslationImprove => ResourceManager.GetString("Settings_TranslationImprove", resourceCulture);

        public static string Settings_TranslationNew => ResourceManager.GetString("Settings_TranslationNew", resourceCulture);

        public static string Settings_Troubleshooting => ResourceManager.GetString("Settings_Troubleshooting", resourceCulture);

        public static string Tool_InfoSelectionStepOver => ResourceManager.GetString("Tool_InfoSelectionStepOver", resourceCulture);

        public static string WhatsNew_FIXED => ResourceManager.GetString("WhatsNew_FIXED", resourceCulture);

        public static string WhatsNew_NEW => ResourceManager.GetString("WhatsNew_NEW", resourceCulture);

        public static string WhatsNew_REMOVED => ResourceManager.GetString("WhatsNew_REMOVED", resourceCulture);

        public static string WhatsNew_REVERTED => ResourceManager.GetString("WhatsNew_REVERTED", resourceCulture);

        public static string WhatsNew_TRANSLATION => ResourceManager.GetString("WhatsNew_TRANSLATION", resourceCulture);

        public static string WhatsNew_UPDATED => ResourceManager.GetString("WhatsNew_UPDATED", resourceCulture);

        public static string WhatsNew_WARNING => ResourceManager.GetString("WhatsNew_WARNING", resourceCulture);

        internal CommonLocalize()
        {
        }
    }
}
