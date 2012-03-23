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
using System.Text.RegularExpressions;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Core.Sourcecontrol;

namespace CruiseControl.Net.Plugin.JediVCS
{
    /// <summary>
    /// Source Control Plugin class for Cruise Control .Net that interact with JediVCS
    /// </summary>
    [ReflectorType("jedivcs")]
    public class JediVCS : ProcessSourceControl
    {
        #region Constants

        #region PrivateConstants

        private const string action_syncproject = "syncproject";
        private const string action_syncprojectgroup = "syncprojectgroup";
        private const string action_getmodule = "getmodule";
        private const string action_label = "labelproject";
        private const string action_listlocks = "listlocks";
        private const string action_listproject = "listproject";
        private const string MethodGetSourceSynchronize = "synchronise";
        private const string MethodGetModule = "getmodule";

        #endregion PrivateConstants
        
        #region PublicConstants

        /// <summary>
        /// 
        /// </summary>
        public const string JediVCSProjectType = "project";
        /// <summary>
        /// 
        /// </summary>
        public const string JediVCSProjectGroupeType = "groupe";
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultJediVCSExecutable = "jvcs.exe";
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultJediVCSPath = "C:\\Program Files\\Jedi\\JVCS";
        /// <summary>
        /// 
        /// </summary>
        public const short DefaultJediVCSServerPort = 2106;
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultMethodGetSource = "synchronise";
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultJediVCSProjectType = JediVCSProjectType;

        #endregion PublicConsts

        #endregion Constants

        #region Variables

        #region PrivateVariables

        /// <summary>
        /// Process executor for JVCS.EXE
        /// </summary>
        private ProcessExecutor Executor;
        /// <summary>
        /// Stdout
        /// </summary>
        private JediVCSHistoryParser HistoryParser;
        
        #endregion PrivateVariables

        #endregion Variables

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public JediVCS() : this(new JediVCSHistoryParser(), new ProcessExecutor())
        { }

        /// <summary>
        /// 
        /// </summary>
        public JediVCS(JediVCSHistoryParser historyParser, ProcessExecutor executor) : base(historyParser, executor)
        {
            HistoryParser = historyParser;
            Executor = executor;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The Name of JediVCS console/command line client program, by default JediVCS.exe
        /// </summary>
        [ReflectorProperty("executable", Required = false)]
        public string Executable = DefaultJediVCSExecutable;

        /// <summary>
        /// The JediVCS Path for executable, by default "C:\Program Files\Jedi\JVCS"
        /// </summary>
        [ReflectorProperty("jedivcspath", Required = false)]
        public string JediVCSPath = DefaultJediVCSPath;

        /// <summary>
        /// The JediVCS server Name or ip adress
        /// </summary>
        [ReflectorProperty("server")]
        public string Server = string.Empty;

        /// <summary>
        /// The JediVCS server port, by default is 2106
        /// </summary>
        [ReflectorProperty("serverPort", Required = false)]
        public short ServerPort = DefaultJediVCSServerPort;

        /// <summary>
        /// The JediVCS user Name
        /// </summary>
        [ReflectorProperty("userName")]
        public string UserName = String.Empty;

        /// <summary>
        /// The JediVCS password for the user
        /// </summary>
        [ReflectorProperty("password")]
        public string Password = string.Empty;

        /// <summary>
        /// The project Name
        /// </summary>
        [ReflectorProperty("projectName")]
        public string ProjectName = string.Empty;

        /// <summary>
        /// The project Id
        /// </summary>
        [ReflectorProperty("projectId", Required = false)]
        public string ProjectID = string.Empty;

        /// <summary>
        /// the project type
        /// </summary>
        [ReflectorProperty("projectType", Required = false)]
        public string ProjectType = DefaultJediVCSProjectType;

        /// <summary>
        /// The working directory
        /// </summary>
        [ReflectorProperty("workingDirectory", Required = false)]
        public string WorkingDirectory = string.Empty;

        /// <summary>
        /// Label on success
        /// </summary>
        [ReflectorProperty("labelOnSuccess", Required = false)]
        public bool LabelOnSuccess = false;

        /// <summary>
        /// branch
        /// </summary>
        [ReflectorProperty("branch", Required = false)]
        public string Branch = string.Empty;

        /// <summary>
        /// Method for (synchronize or getmodule) for obtain source
        /// </summary>
        [ReflectorProperty("methodGetSource", Required = false)]
        public string MethodGetSource = DefaultMethodGetSource;

		/// <summary>
		/// Whether to delete the working copy before updating the source. 
		/// </summary>
		/// <version>1.0</version>
		/// <default>false</default>
		[ReflectorProperty("cleanCopy", Required = false)]
		public bool CleanCopy = false;
 
        /// <summary>
        /// Folder to exclude when deleting working copy, relative to working directory
        /// </summary>
        [ReflectorProperty("cleanCopyAware", Required = false)]
        public string CleanCopyAware = string.Empty;

		#endregion Properties

        #region Methods

        // public method's
        #region PublicMethds

        #region InheritedPublicMethods

        /// <summary>
        /// Provided for getting files modified
        /// </summary>
        /// <param Name="from">the date and time of start modified (not really used)</param>
        /// <param Name="to">the date and time of end modified (not really used)</param>
        /// <returns>This method return array of Modification class</returns>
        public override Modification[] GetModifications(IIntegrationResult from, IIntegrationResult to)
        {
            Log.Debug("JediVCS plugin: Start getting modifications !");            
            Modification[] modifications = null;
            modifications = HistoryParser.GetModifications(ListDiff(from), from.StartTime, to.StartTime);
            Log.Debug("JediVCS plugin: End of getting modifications !");
            return modifications;
        }

        /// <summary>
        /// Provided for update the source :
        /// Jedi VCS support two methods :
        /// . Synchronize ;
        /// . Get module.
        /// The default is synchronize.
        /// </summary>
        /// <param Name="result">Integration parameters</param>
        public override void GetSource(IIntegrationResult result)
        {
            Log.Debug("JediVCS plugin: Start getting source!");
            ProcessResult ProcessCallResult;
            if (MethodGetSource == MethodGetSourceSynchronize)
            {
            	if (CleanCopy)
                {
		            Log.Info("Cleanup working directory: " + result.WorkingDirectory);
            		if (!String.IsNullOrEmpty(result.WorkingDirectory))
                    {
                        DeleteSource(result.WorkingDirectory);
            		}
            		else
            		{
			            Log.Error("Path to working directory not set!");
            		}
                }

                ProcessCallResult = Synchronize(result);
            }
            else
                if (MethodGetSource == MethodGetModule)
                    ProcessCallResult = GetModule(result);
                else
                {
                    Log.Debug("Unknown method Name for getting source !");
                    throw new JediVCSException("Unknown method Name for getting source !");
                }
            Log.Debug("JediVCS plugin: End of getting source !");
        }

        private void DeleteSource(string workingDirectory)
        {
           	Log.Debug("Deleting sources in folder: " + workingDirectory);
            if (System.IO.Directory.Exists(workingDirectory))
            {
				string[] files = Directory.GetFiles(workingDirectory);
				foreach (string file in files)
					new IoService().DeleteIncludingReadOnlyObjects(file);
				
				string[] Folders = Directory.GetDirectories(workingDirectory,"*", System.IO.SearchOption.TopDirectoryOnly);

				if (!String.IsNullOrEmpty(CleanCopyAware))
            	{
					string[] ExcludeFolders = CleanCopyAware.Split(';');
					bool ExcludeCurrent;
					foreach (string Folder in Folders) 
					{
						ExcludeCurrent = false;
			           	Log.Debug("Checking folder: " + Folder);
						if (Folder != "." && Folder != "..")
						{
							foreach (string ExcludeFolder in ExcludeFolders)
							{
								string afolder = IncludeTrailingPathDelimiter(workingDirectory);
								afolder = afolder + ExcludeFolder.Trim();
								afolder = afolder.ToLower();
								if (afolder.Equals(Folder.ToLower()))
								{
						           	Log.Debug("Excluding folder: " + Folder);
									ExcludeCurrent = true;
									break;
								}
							}

							if (!ExcludeCurrent)
								new IoService().DeleteIncludingReadOnlyObjects(Folder);
						}
					}
            	}
            	else
            	{
					foreach (string Folder in Folders) 
					{
						if (Folder != "." && Folder != "..")
							new IoService().DeleteIncludingReadOnlyObjects(Folder);
					}
            	}
            }
            Log.Debug("Finished deleting sources");
        }
        
        
		private string IncludeTrailingPathDelimiter(string path)
		{
			if (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
			{
				return path;
			}
			else
			{
				return (path + System.IO.Path.DirectorySeparatorChar);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param Name="project">Integration parameters</param>
        public override void Initialize(IProject project)
        {
            // not implemented for the moment
        }

        /// <summary>
        /// This method is provided for labelling projet in the source control
        /// </summary>
        /// <param name="result">Integration parameters</param>
        public override void LabelSourceControl(IIntegrationResult result)
        {
        	Log.Debug("JediVCS plugin: LabelOnSuccess=" + LabelOnSuccess.ToString());
            if (LabelOnSuccess && result.Succeeded)
            {
    	        Log.Debug("JediVCS plugin: Start labelling project !");
                Execute(NewLabelProcessInfo(result));
	            Log.Debug("JediVCS plugin: End labelling project !");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param Name="project"></param>
        public override void Purge(IProject project)
        {
        }

        #endregion InheritedPublicMethods

        #endregion PublicMethods

        // private method's

        #region PrivateMethods

        /// <summary>
        /// This method is provided for building list files command line
        /// </summary>
        /// <param Name="result">Integration parameters</param>
        /// <returns>Process result (standard output)</returns>
        private ProcessResult List(IIntegrationResult result)
        {
            return Execute(NewGetListProcessInfo(result));
        }

        /// <summary>
        /// This method is for list difference of files
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private ProcessResult ListDiff(IIntegrationResult result)
        {
            return Execute(NewGetListDiffProcessInfo(result));
        }

        /// <summary>
        /// Synchronize the source of project
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>The result of call (stdout)</returns>
        private ProcessResult Synchronize(IIntegrationResult result)
        {
            return Execute(NewSynchronizeProcessInfo(result));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>The result of call (stdout)</returns>
        private ProcessResult GetModule(IIntegrationResult result)
        {
            return Execute(NewGetModuleProcessInfo(result));
        }

        /// <summary>
        /// This fonction is provided define and return the name or the id of project
        /// </summary>
        /// <returns>The name or id of project in config file project</returns>
        private string GetProjectNameOrId()
        {
        	if (!String.IsNullOrEmpty(ProjectID))
                return ProjectID;
            else
                if (!String.IsNullOrEmpty(ProjectName))
                    return ProjectName;
                else
                    throw new JediVCSException("JediVCS plugin : Blank projet Name !");
        }

        /// <summary>
        /// This method is provided for building list of files command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewGetListProcessInfo(IIntegrationResult result)
        {
            Log.Debug("NewGetListProcessInfo");
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_listproject, GetProjectNameOrId());
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is provided for building list of modified files command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewGetListDiffProcessInfo(IIntegrationResult result)
        {
            Log.Debug("NewGetListDiffProcessInfo");
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_listproject, GetProjectNameOrId());
            builder.Append("diffonly");
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is provided for building list of new files command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewGetListNewProcessInfo(IIntegrationResult result)
        {
            Log.Debug("NewGetListNewProcessInfo");
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_listproject, GetProjectNameOrId());
            builder.Append("newonly");
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is provided for building list of old files command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewGetListOldProcessInfo(IIntegrationResult result)
        {
            Log.Debug("NewGetListOldProcessInfo");
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_listproject, GetProjectNameOrId());
            builder.Append("oldonly");
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is provided for building get module command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewGetSourceProcessInfo(IIntegrationResult result, string dir)
        {
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_syncprojectgroup, ProjectName);
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is provided for building synchronise command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewSynchronizeProcessInfo(IIntegrationResult result)
        {
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_syncproject, GetProjectNameOrId());
            builder.Append("noprompt");
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is provided for building get module command line
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>Process result (standard output)</returns>
        private ProcessInfo NewGetModuleProcessInfo(IIntegrationResult result)
        {
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AppendArgument(action_getmodule, GetProjectNameOrId());
            builder.Append("noprompt");
            return NewProcessInfoWithArgs(result, builder);
        }

        /// <summary>
        /// This method is the provided for build labelling command line arguments
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <returns>The command line to execute for labelling</returns>
        private ProcessInfo NewLabelProcessInfo(IIntegrationResult result)
        {
        	string QuotedLabel;
        	if (result.Label.Contains(" ")) {
        		QuotedLabel = "\"" + result.Label + "\"";
        	}
        	else {
        		QuotedLabel = result.Label;
        	}        		
        		
            JediVCSProcessArgumentBuilder buffer = new JediVCSProcessArgumentBuilder();
            buffer.AppendArgument(action_label, GetProjectNameOrId());
            buffer.Append(QuotedLabel);
            return NewProcessInfoWithArgs(result, buffer);
        }

        /// <summary>
        /// This method is the base to build full command line to execute Jedi VCS
        /// </summary>
        /// <params name="result">Integration parameters</params>
        /// <params name="args">The specific command to execute with parameters</params>
        /// <returns>The default command line to execute jvcs.exe with options</returns>
        private ProcessInfo NewProcessInfoWithArgs(IIntegrationResult result, JediVCSProcessArgumentBuilder args)
        {
            if (String.IsNullOrEmpty(Server))
                throw new JediVCSException("JediVCS plugin error : No Server Name or ip adress ");
            if (String.IsNullOrEmpty(UserName))
                throw new JediVCSException("JediVCS plugin error : blank user Name or password !");
            if (String.IsNullOrEmpty(Password))
                throw new JediVCSException("JediVCS plugin error : blank user Name or password !");
            JediVCSProcessArgumentBuilder builder = new JediVCSProcessArgumentBuilder();
            builder.AddArgument(Server);
            if (ServerPort != DefaultJediVCSServerPort)
            {
                builder.AppendArgument(":");
                builder.AppendArgument(ServerPort);
            }
            builder.AddArgument("user", UserName);
            builder.AddArgument("password", Password);
            builder.AddArguments(args);
            if (JediVCSPath.Equals(string.Empty))
                return new ProcessInfo(Executable, 
                                       builder.ToString(), 
                                       result.BaseFromWorkingDirectory(WorkingDirectory));
            else
                return new ProcessInfo(JediVCSPath + "\\" + Executable, 
                                       builder.ToString(), 
                                       result.BaseFromWorkingDirectory(WorkingDirectory));
        }

        #endregion PrivateMethods

        #endregion Methods
    }

}

