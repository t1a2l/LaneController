using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ColossalFramework.UI;
using UnityEngine;

namespace LaneController.KianCommons.Utils
{
    internal static class Log
    {
        internal static class FlushTread
        {
            private static bool isRunning_;

            private static Thread flushThraad_;

            internal static void Init()
            {
                try
                {
                    if (!isRunning_)
                    {
                        Info("Initializing Log.FlushTread");
                        flushThraad_ = new Thread(FlushThread)
                        {
                            Name = "FlushThread",
                            IsBackground = true
                        };
                        isRunning_ = true;
                        flushThraad_.Start();
                    }
                }
                catch (Exception ex)
                {
                    ex.Exception();
                }
            }

            internal static void Terminate()
            {
                try
                {
                    Info("FlushTread.Terminate() called");
                    isRunning_ = false;
                    flushThraad_.Join();
                    flushThraad_ = null;
                    Flush();
                }
                catch (Exception ex)
                {
                    ex.Exception();
                }
            }

            private static void FlushThread()
            {
                try
                {
                    while (isRunning_)
                    {
                        Thread.Sleep(FlushInterval);
                        Flush();
                    }
                    Info("Flush Thread Exiting...");
                }
                catch (Exception ex)
                {
                    ex.Exception();
                }
            }
        }

        private enum LogLevel
        {
            Debug,
            Info,
            Error,
            Warning,
            Exception
        }

        private static readonly bool ShowLevel;

        private static readonly bool ShowTimestamp;

        private static readonly string assemblyName_;

        private static readonly string LogFileName;

        private static readonly string LogFilePath;

        private static readonly Stopwatch Timer;

        private static StreamWriter filerWrier_;

        private static readonly object LogLock;

        internal static bool ShowGap;

        private static long prev_ms_;

        internal static int FlushInterval;

        public static HashSet<object> logged_ids_;

        public const int MAX_WAIT_ID = 1000;

        private static readonly DateTime[] times_;

        private static readonly string nl;

        internal static bool VERBOSE { get; set; }

        internal static bool Buffered
        {
            get
            {
                return filerWrier_ != null;
            }
            set
            {
                if (value == Buffered)
                {
                    return;
                }
                if (value)
                {
                    try
                    {
                        filerWrier_ = new StreamWriter(LogFilePath, append: true);
                    }
                    catch (Exception ex)
                    {
                        ex.Exception("failed to setup log buffer");
                    }
                    FlushTread.Init();
                }
                else
                {
                    filerWrier_.Flush();
                    filerWrier_.Dispose();
                    filerWrier_ = null;
                    FlushTread.Terminate();
                }
            }
        }

        internal static void Flush()
        {
            if (filerWrier_ != null)
            {
                lock (LogLock)
                {
                    filerWrier_?.Flush();
                }
            }
        }

        public static Stopwatch GetSharedTimer()
        {
            return (AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((_asm) => _asm.GetName().Name == "LoadOrderIPatch")?.GetType("LoadOrderIPatch.Patches.UnityLoggerPatch", throwOnError: false))?.GetField("m_Timer")?.GetValue(null) as Stopwatch;
        }

        static Log()
        {
            ShowLevel = true;
            ShowTimestamp = true;
            assemblyName_ = Assembly.GetExecutingAssembly().GetName().Name;
            LogFileName = assemblyName_ + ".log";
            LogLock = new object();
            ShowGap = false;
            VERBOSE = false;
            FlushInterval = 500;
            logged_ids_ = [];
            times_ = new DateTime[1000];
            nl = "\n";
            try
            {
                string text = Path.Combine(Application.dataPath, "Logs");
                LogFilePath = Path.Combine(text, LogFileName);
                string path = Path.Combine(Application.dataPath, LogFileName);
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                if (File.Exists(LogFilePath))
                {
                    File.Delete(LogFilePath);
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (ShowTimestamp)
                {
                    Timer = GetSharedTimer() ?? Stopwatch.StartNew();
                }
                AssemblyName name = typeof(Log).Assembly.GetName();
                Info($"Log file at {LogFilePath} now={DateTime.Now}", copyToGameLog: true);
                Info($"{name.Name} Version:{name.Version} " + "Commit:12cb450 CommitDate:2022-11-26T01:05:45+02:00", copyToGameLog: true);
                string path2 = Path.Combine(Application.dataPath, LogFileName);
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
            }
            catch (Exception ex)
            {
                LogUnityException(ex);
            }
        }

        public static void ReadCommandLineArgs()
        {
            VERBOSE = Environment.GetCommandLineArgs().Contains("-VERBOSE");
            Buffered = !Environment.GetCommandLineArgs().Contains("-UNBUF");
        }

        [Conditional("DEBUG")]
        public static void DebugOnce(string message, object id = null, bool copyToGameLog = true)
        {
            id ??= Environment.StackTrace + message;
            if (!logged_ids_.Contains(id))
            {
                logged_ids_.Add(id);
            }
        }

        [Conditional("DEBUG")]
        public static void DebugWait(string message, int id, float seconds = 0.5f, bool copyToGameLog = true)
        {
            float num = seconds + 1f;
            if (id < 0)
            {
                id = -id;
            }
            id = Mathf.Abs(id % 1000);
            _ = ref times_[id];
            num = (DateTime.Now - times_[id]).Seconds;
            if (num >= seconds)
            {
                times_[id] = DateTime.Now;
            }
        }

        [Conditional("DEBUG")]
        public static void DebugWait(string message, object id = null, float seconds = 0.5f, bool copyToGameLog = true)
        {
            _ = id ?? Environment.StackTrace + message;
        }

        [Conditional("DEBUG")]
        public static void Debug(string message, bool copyToGameLog = true)
        {
            LogImpl(message, LogLevel.Debug, copyToGameLog);
        }

        public static void Info(string message, bool copyToGameLog = false)
        {
            LogImpl(message, LogLevel.Info, copyToGameLog);
        }

        public static void Error(string message, bool copyToGameLog = true)
        {
            LogImpl(message, LogLevel.Error, copyToGameLog);
        }

        public static void Warning(string message, bool copyToGameLog = true)
        {
            LogImpl(message, LogLevel.Warning, copyToGameLog);
        }

        private static string ExceptionData(Exception ex)
        {
            ICollection collection = ex.Data?.Keys;
            if (collection != null)
            {
                List<string> list = [];
                foreach (object item in collection)
                {
                    list.Add($"'{item}' : '{ex.Data[item]}'");
                }
                if (list.Any())
                {
                    return "Data: " + string.Join(" | ", [.. list]);
                }
            }
            return null;
        }

        internal static void Exception(this Exception ex, string m = "", bool showInPanel = true)
        {
            if (ex == null)
            {
                Error("null argument e was passed to Log.Exception()");
            }
            try
            {
                string text = ex.ToString() + "\n\t-- end of exception --";
                if (!string.IsNullOrEmpty(m))
                {
                    text = m + " -> \n" + text;
                }
                string text2 = ExceptionData(ex);
                if (!string.IsNullOrEmpty(text2))
                {
                    text = text2 + "\n" + text;
                }
                LogImpl(text, LogLevel.Exception, copyToGameLog: true);
                if (showInPanel)
                {
                    UIView.ForwardException(ex);
                }
            }
            catch (Exception ex2)
            {
                LogUnityException(ex2);
            }
        }

        internal static void LogUnityException(Exception ex, bool showInPanel = true)
        {
            UnityEngine.Debug.LogException(ex);
            if (showInPanel)
            {
                UIView.ForwardException(ex);
            }
        }

        internal static void ShowModalException(string title, string message, bool error = false)
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(title, message, error);
            string message2 = title + " : " + message;
            if (error)
            {
                Error(message2);
            }
            else
            {
                Info(message2, copyToGameLog: true);
            }
        }

        private static void LogImpl(string message, LogLevel level, bool copyToGameLog)
        {
            try
            {
                long elapsedTicks = Timer.ElapsedTicks;
                string text = "";
                if (ShowLevel)
                {
                    int num = Enum.GetNames(typeof(LogLevel)).Max((str) => str.Length);
                    text += string.Format($"{{0, -{num}}}", $"[{level}] ");
                }
                long elapsedMilliseconds = Timer.ElapsedMilliseconds;
                long num2 = elapsedMilliseconds - prev_ms_;
                prev_ms_ = elapsedMilliseconds;
                if (ShowTimestamp)
                {
                    text += $"{elapsedMilliseconds:#,0}ms | ";
                    if (ShowGap)
                    {
                        text += $"gap={num2:#,0}ms | ";
                    }
                }
                text += message;
                if (level == LogLevel.Error || level == LogLevel.Exception)
                {
                    text = text + nl + GetStackTrace();
                    text = nl + text + nl;
                }
                try
                {
                    lock (LogLock)
                    {
                        if (filerWrier_ != null)
                        {
                            filerWrier_.WriteLine(text);
                        }
                        else
                        {
                            using StreamWriter streamWriter = File.AppendText(LogFilePath);
                            streamWriter.WriteLine(text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUnityException(ex, showInPanel: false);
                }
                if (copyToGameLog)
                {
                    Flush();
                    text = assemblyName_ + " | " + text;
                    text = RemoveExtraNewLine(text);
                    switch (level)
                    {
                        case LogLevel.Error:
                        case LogLevel.Exception:
                            UnityEngine.Debug.LogError(text);
                            break;
                        case LogLevel.Warning:
                            UnityEngine.Debug.LogWarning(text);
                            break;
                        default:
                            UnityEngine.Debug.Log(text);
                            break;
                    }
                }
                if (num2 > FlushInterval)
                {
                    Flush();
                }
            }
            catch (Exception ex2)
            {
                LogUnityException(ex2);
            }
        }

        private static string GetStackTrace()
        {
            StackTrace stackTrace = new();
            int i;
            for (i = 0; i < stackTrace.FrameCount; i++)
            {
                Type declaringType = stackTrace.GetFrame(i).GetMethod().DeclaringType;
                if (declaringType != typeof(Assertion) && declaringType != typeof(Log))
                {
                    break;
                }
            }
            return new StackTrace(i - 1, fNeedFileInfo: true).ToString();
        }

        public static string RemoveExtraNewLine(string str)
        {
            return str.Replace("\r\n", "\n");
        }

        internal static void LogToFileSimple(string file, string message)
        {
            using StreamWriter streamWriter = File.AppendText(file);
            streamWriter.WriteLine(message);
            streamWriter.WriteLine(new StackTrace().ToString());
            streamWriter.WriteLine();
        }

        internal static void Called(params object[] args)
        {
            Info(ReflectionHelpers.CurrentMethod(2, args) + " called.");
        }

        internal static void Succeeded(string m = null)
        {
            Info(ReflectionHelpers.CurrentMethod(2) + " succeeded! " + m);
        }
    }
}
