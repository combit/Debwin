using Debwin.Core;
using Debwin.Core.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Debwin.UI.Panels
{
    public partial class LlJobAnalyzerPanel : DockContent
    {

        private IMainWindow _mainWindow;

        public LlJobAnalyzerPanel(IMainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {

            var originalLogView = _mainWindow.GetActiveLogView().LogController.GetLogViews().FirstOrDefault() as IQueryableLogView;
            if (originalLogView != null)
            {
                MemoryBasedLogView view = new MemoryBasedLogView(-1);
                originalLogView.CopyMessagesTo(view, null);


                List<JobEntry> jobEntries = new List<JobEntry>();
                Dictionary<string, Stack<JobEntry>> currentJobOfThread = new Dictionary<string, Stack<JobEntry>>();
                Stack<JobEntry> jobEntryStackForThread = null;

                for (int i = 0; i < view.MessageCount; i++)
                {
                    LogMessage msg = view.GetMessage(i);
                    currentJobOfThread.TryGetValue(msg.Thread, out jobEntryStackForThread);

                    bool waitingForJobOpen = (jobEntryStackForThread == null || jobEntryStackForThread?.Count == 0);
                    bool messageIsJobOpen = msg.Message.Contains("<LlJobOpen");


                    if (messageIsJobOpen || waitingForJobOpen)
                    {
                        JobEntry newEntry = null;
                        if (messageIsJobOpen)
                        {
                            newEntry = new JobEntry() { Title = "Job " + GetJobNumberFromLlJobOpen(msg.Message) + " [Thread " + FormatThread(msg.Thread) + "]" };
                        }
                        else
                        {
                            // 
                            string assumedJobEntry = GetAssumedJobNumberFromInconsistentLog(msg.Message);
                            if (assumedJobEntry == null)
                                continue;
                            waitingForJobOpen = false;
                            newEntry = new JobEntry() { Title = "Job " + assumedJobEntry + " [Thread " + FormatThread(msg.Thread) + "]" };
                        }

                        if (jobEntryStackForThread == null)
                        {
                            jobEntryStackForThread = new Stack<JobEntry>();
                            currentJobOfThread[msg.Thread] = jobEntryStackForThread;
                        }                        
                        if (jobEntryStackForThread.Count != 0)
                            jobEntryStackForThread.Peek().JobItems.Add(newEntry);
                        else
                            jobEntries.Add(newEntry);
                        jobEntryStackForThread.Push(newEntry);
                        newEntry.JobItems.Add(msg);
                    }

                    if (waitingForJobOpen) // wait for JobOpen to arrive, sanity expected...
                    {
                        continue;
                    }

                    if (msg.Message.Contains(">LlJobClose"))
                    {
                        jobEntryStackForThread.Peek().JobItems.Add(msg);
                    }
                    else if (msg.Message.Contains("<LlJobClose"))
                    {
                        jobEntryStackForThread.Pop();
                    }
                    else if (msg.Message.Contains("<LlPrintWithBoxStart") || msg.Message.Contains("<LlPrintStart"))
                    {
                        OpenBracket(jobEntryStackForThread, msg, "Printing");
                    }
                    else if (msg.Message.Contains(">LlPrint("))
                    {
                        OpenBracket(jobEntryStackForThread, msg, "LlPrint");
                    }
                    else if (msg.Message.Contains("<LlPrint("))
                    {
                        CloseBracket(jobEntryStackForThread, msg, GetReturnValue(msg.Message));
                    }
                    else if (msg.Message.Contains("<LlPrintEnd"))
                    {
                        CloseBracket(jobEntryStackForThread, msg);
                    }
                    else if (msg.Message.Contains(">LlDefineLayout"))
                    {
                        OpenBracket(jobEntryStackForThread, msg, "Designing");
                    }
                    else if (msg.Message.Contains("<LlDefineLayout"))
                    {
                        CloseBracket(jobEntryStackForThread, msg);
                    }
                    else if (msg.Message.Contains("---StartPage"))
                    {
                        jobEntryStackForThread.Peek().JobItems.Add(msg);
                    }
                    else if (msg.Message.Contains("---EndPage"))
                    {
                        jobEntryStackForThread.Peek().JobItems.Add(msg);
                    }
                    else if (msg.Message.Contains("---StartDoc"))
                    {
                        jobEntryStackForThread.Peek().JobItems.Add(msg);
                    }
                    else if (msg.Message.Contains("---EndDoc"))
                    {
                        jobEntryStackForThread.Peek().JobItems.Add(msg);
                    }
                    else if (msg.Level >= LogLevel.Warning)
                    {
                        jobEntryStackForThread.Peek().JobItems.Add(msg);
                    }
                }

                treeView.BeginUpdate();
                treeView.Nodes.Clear();
                foreach (var jobEntry in jobEntries)
                {                  
                    HandleJobEntry(jobEntry);
                }
                treeView.EndUpdate();
            }
        }

        private static void CloseBracket(Stack<JobEntry> jobEntryStackForThread, LogMessage msg, string returnValue = "")
        {
            jobEntryStackForThread.Peek().JobItems.Add(msg);
            jobEntryStackForThread.Peek().ReturnValue = returnValue;
            jobEntryStackForThread.Pop();
        }

        private static void OpenBracket(Stack<JobEntry> jobEntryStackForThread, LogMessage msg, string bracketName)
        {
            JobEntry newEntry = new JobEntry() { Title = bracketName };
            jobEntryStackForThread.Peek().JobItems.Add(newEntry);
            jobEntryStackForThread.Push(newEntry);
            newEntry.JobItems.Add(msg);
        }

        private void HandleJobEntry(JobEntry jobEntry, TreeNode parentNode = null)
        {
            if (jobEntry.JobItems.Count == 0)
                return;

            var newParentNode = parentNode != null ? parentNode.Nodes.Add(jobEntry.Title) : treeView.Nodes.Add(jobEntry.Title);
            foreach (var item in jobEntry.JobItems)
            {
                if (item is LogMessage msg)
                {
                    var messageNode = newParentNode.Nodes.Add(FormatThread(msg.Thread) + ": " + msg.Message.TrimStart());
                    messageNode.Tag = item;

                    if (msg.Level == LogLevel.Warning) { messageNode.BackColor = System.Drawing.Color.LightYellow; }
                    else if (msg.Level == LogLevel.Error) { messageNode.BackColor = System.Drawing.Color.MistyRose; }
                }
                else if (item is JobEntry entry)
                {
                    HandleJobEntry(entry, newParentNode);
                }
            }
        }

        private string FormatThread(string input)
        {
            return input.TrimStart(new char[] { '0' });
        }

        private string GetJobNumberFromLlJobOpen(string input)
        {
            Match match = Regex.Match(input, @"LlJobOpen(Copy)?\(\) -> ([0-9]+)");
            Group isCopy = match.Groups[1];
            Group jobNumber = match.Groups[2];
            if (jobNumber.Success)
            {
                return jobNumber.Value + (isCopy.Success ? " (copied job)" : String.Empty);
            }
            else
            {
                return "?";
            }
        }

        List<string> _joblessAPIs = new List<string>() { ">LlJobOpen", "LlSystemTimeTo", "LlSetDebug", "LlSetOption(-1" };
        private string GetAssumedJobNumberFromInconsistentLog(string input)
        {

            // not this one, we're actually waiting for it...

            foreach (string api in _joblessAPIs)
            {
                if (input.Contains(api))
                    return null;

            }

            Match match = Regex.Match(input, @"Ll[\w\d]+?\(([0-9]+).+?\)");
            Group jobNumber = match.Groups[1];
            if (jobNumber.Success)
            {
                return jobNumber.Value + " (assumed, no job open found)";
            }
            else
            {
                return null;
            }
        }

        private string GetReturnValue(string input)
        {
            Match match = Regex.Match(input, @"<Ll.+\(.*\)( -> .+)");
            Group returnValue = match.Groups[1];
            if (returnValue.Success)
            {
                return returnValue.Value;
            }
            else
            {
                return String.Empty;
            }
        }


        private class JobEntry
        {
            private string _title;
            public string Title
            {
                get
                {
                    return _title + " "+ReturnValue;
                }
                set
                {
                    _title = value;
                }
            }
            public List<object> JobItems { get; } = new List<object>();
            public string ReturnValue { get; set; }
        }

        private void treeView_NodeMouseDoubleClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            LogMessage msg = e.Node.Tag as LogMessage;
            if (msg != null)
            {
                _mainWindow.GetActiveLogView().JumpToLogLine(msg.LineNr);
            }
        }
    }
}
