/*
 * Cruise Control .Net JediVCS plugin
 * 
 * Cruise Control .Net web site : http://ccnet.sourceforge.net
 * JediVCS web site : http://jedivcs.sourceforge.net
 * 
 */

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Core.Sourcecontrol;

namespace CruiseControl.Net.Plugin.JediVCS
{
	/// <summary>
	/// This class is provided for parsing the output of cass of Jedi VCS command line
	/// </summary>
    public class JediVCSHistoryParser : IHistoryParser
    {
        #region Methods

        #region PublicMethods

        #region InheritedPublicMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param Name="history"></param>
        /// <param Name="from"></param>
        /// <param Name="to"></param>
        /// <returns></returns>
        public Modification[] Parse(TextReader history, DateTime from, DateTime to)
        {
            Modification[] Result = null;

            return Result;
        }

        #endregion InheritedPublicMethods

        /// <summary>
        /// Get modified files
        /// </summary>
        /// <param name="processResult">stdout process result</param>
        /// <param name="from">start date and time (not used)</param>
        /// <param name="to">end date and time (not used)</param>
        /// <returns></returns>
        public Modification[] GetModifications(ProcessResult processResult, DateTime from, DateTime to)
		{
			Modification[] Result = null;
            string StdOut = processResult.StandardOutput;
            StringReader reader = new StringReader(StdOut);            
            JediVCSFileInfoList Files = GetJediVCSFileInfoList(reader);
            Result = GetModifiedFiles(Files);
            return Result;
        }

        #endregion PublicMethods

        #region PrivateMethods

        /// <summary>
        /// Parse the reader for obtain the files list
        /// </summary>
        /// <param Name="reader">flow how contains the files list table</param>
        /// <returns></returns>
        private JediVCSFileInfoList GetJediVCSFileInfoList(TextReader reader)
		{
            const string RegExLineHead = "[ ]*ID[ ]*File Path[ ]*File Name[ ]*Version[ ]*Out[ ]*Owner[ ]*Hid[ ]*Stamp[ ]*CRC[ ]*";
            const string RegExLineSep = "^([= ]*)$";
            StringReader buffer = GetReaderWithNoBanner(reader);
            JediVCSFileInfoList Files = new JediVCSFileInfoList();

            Regex expHeader = new Regex(RegExLineHead, RegexOptions.IgnoreCase);
            Regex expLineSep = new Regex(RegExLineSep, RegexOptions.IgnoreCase);

            // Goto on the first file line
            string Line = string.Empty;
            Line = buffer.ReadLine();
            while (Line != null)
            {
	            Line = Line.Replace(System.Convert.ToChar(0x0).ToString(), " ");
                if (Line != string.Empty)
                {
                	if (!expHeader.IsMatch(Line))
                	{
                		if (!expLineSep.IsMatch(Line))
                		{
                        	Files.Add(GetJediVCSFileInfo(Line));
                		}
                	}
                }
                Line = buffer.ReadLine();
            }
            return Files;
		}
		
        /// <summary>
        /// Get the files modified
        /// </summary>
        /// <param name="files">List of files</param>
        /// <returns>Modification array</returns>
        private Modification[] GetModifiedFiles(JediVCSFileInfoList files)
        {
        	Log.Debug("Checking " + files.Count.ToString() +  " files for modifications");
            Modification[] Result = new Modification[0];
            for (int iFile = 0; iFile < files.Count; iFile++)
            {
                JediVCSFileInfo FileInfo = files[iFile];
                if (FileInfo.IsModified())
                {
		        	Log.Debug("Modified file: " + FileInfo.Name);
                	Array.Resize(ref Result, Result.Length + 1);
                    Result[Result.Length - 1] = Convert(FileInfo);
                }
            }
        	Log.Debug("Found " + Result.Length.ToString() +  " modified files");
            return Result;
        }
		
        /// <summary>
        /// Parse the buffer to obtain the data at the start 
        /// </summary>
        /// <param name="buffer">file informations</param>
        /// <returns>just data</returns>
        private String GetJediVCSFileInfoElement(ref String buffer)
        {
            String Result = String.Empty;
            if (buffer != string.Empty)
            {
                int pos = buffer.IndexOf(" ");
                Result = buffer.Substring(0, pos);
                buffer = buffer.Substring(pos + 1);
                while (buffer.StartsWith(" "))
                    buffer = buffer.Substring(1);
            }
            return Result;
        }
		
        /// <summary>
        /// Parse the row
        /// </summary>
        /// <param name="line">buffer file line</param>
        /// <returns>The file infos</returns>
        private JediVCSFileInfo GetJediVCSFileInfo(string line)
        {
            JediVCSFileInfo Result = new JediVCSFileInfo();

            const string RegExLine = "([0-9 ]{8}) (.{50}) (.{30}) ([0-9. ]{7}) (.{3}) (.{20}) (.{3}) (.{5}) (.{5})";

            Regex expLine = new Regex(RegExLine, RegexOptions.IgnoreCase);
            
            Match expMatch = expLine.Match(line);
           
            if (expMatch.Success)
            {
            	Log.Debug("Regex match on line: " + line);

            	Result.Id 		= System.Convert.ToInt32(expMatch.Groups[1].Value);
	            Result.Path 	= expMatch.Groups[2].Value.Trim();
	            Result.Name 	= expMatch.Groups[3].Value.Trim();
	            Result.Version 	= expMatch.Groups[4].Value.Trim();
	            Result.InOut 	= expMatch.Groups[5].Value.Trim();
	            Result.Owner 	= expMatch.Groups[6].Value.Trim();
	            Result.Hidden 	= expMatch.Groups[7].Value.Trim();
	            Result.Stamp 	= expMatch.Groups[8].Value.Trim();
	            Result.CRC 		= expMatch.Groups[9].Value.Trim();
            }
            else
            	Log.Debug("No regex match on line: " + line);

            return Result;
        }
		
        /// <summary>
        /// Remove Jedi VCS stdout banner
        /// </summary>
        /// <param name="reader">the stdout with banner</param>
        /// <returns>the stdout with banner removed</returns>
        private StringReader GetReaderWithNoBanner(TextReader reader)
        {
            StringWriter buffer = new StringWriter();
            String Line = string.Empty;
            int iLine = 1;
            Line = reader.ReadLine();
            while (Line != null)
            {
                Line = reader.ReadLine();
                iLine++;
                if (iLine > 10)
                {
                    if (Line != null)
                    {
                    	while (Line.StartsWith("\0"))
                        //while (Line.Contains("\0"))
                        {
                            int pos = Line.IndexOf('\0');
                            String tmp = Line.Substring(0, pos);
                            Line = tmp + Line.Substring(pos + 2);
                        }
                        buffer.WriteLine(Line);
                    }
                }
            }
            String temp = buffer.ToString();
            StringReader Result = new StringReader(temp);
            buffer.Close();
            return Result;
        }

        /// <summary>
        /// Convert file informations to Modification instance
        /// </summary>
        /// <param name="fileInfo">file informations</param>
        /// <returns>Modification instance</returns>
        private Modification Convert(JediVCSFileInfo fileInfo)
        {
            Modification Result = new Modification();
            Result.FileName = fileInfo.Name;
            Result.FolderName = fileInfo.Path;
            Result.ChangeNumber = "1";
            Result.Comment = "";
            Result.EmailAddress = "";
            Result.ModifiedTime = DateTime.UtcNow;
            Result.Type = "";
            Result.Url = "";
            Result.UserName = "";
            Result.Version = "";
            return Result;
        }
        #endregion PrivateMethods

        #endregion Methods

    }
}
