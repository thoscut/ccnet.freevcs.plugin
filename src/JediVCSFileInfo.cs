/*
 * Cruise Control .Net JediVCS plugin
 * 
 * Cruise Control .Net web site : http://ccnet.sourceforge.net
 * JediVCS web site : http://jedivcs.sourceforge.net
 * 
 */

using System;
using System.Collections;
using ThoughtWorks.CruiseControl.Core;

namespace CruiseControl.Net.Plugin.JediVCS
{    
    /// <summary>
    /// this class is provided for storing file information from stdout call result
    /// </summary>
    public class JediVCSFileInfo
    {
        #region PrivateVariables

        private int _Id;
        private string _Path;
        private string _Name;
        private string _Version;
        private string _InOut;
        private string _Owner;
        private string _Hidden;
        private string _Stamp;
        private string _CRC;

        #endregion PrivateVariables

        #region PublicProperties

        /// <summary>
        /// the id of file
        /// </summary>
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

       /// <summary>
       /// the path
       /// </summary>
       public string Path
         {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

       /// <summary>
       /// the name
       /// </summary>
       public string Name
         {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

       /// <summary>
       /// the version of file
       /// </summary>
       public string Version
         {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
            }
        }

       /// <summary>
       /// the file is in or out
       /// </summary>
       public string InOut
        {
            get
            {
                return _InOut;
            }
            set
            {
                _InOut = value;
            }
        }

       	/// <summary>
       	/// the owner of file
       	/// </summary>
        public string Owner
        {
            get
            {
                return _Owner;
            }
            set
            {
                _Owner = value;
            }
        }
		
        /// <summary>
        /// The file is hidden or not
        /// </summary>
        public string Hidden
        {
            get
            {
                return _Hidden;
            }
            set
            {
                _Hidden = value;
            }
        }

        /// <summary>
        /// The stamp
        /// </summary>
        public string Stamp
        {
            get
            {
                return _Stamp;
            }
            set
            {
                _Stamp = value;
            }
        }

        /// <summary>
        /// The CRC
        /// </summary>
        public string CRC
        {
            get
            {
                return _CRC;
            }
            set
            {
                _CRC = value;
            }
        }

        #endregion PublicProperties

        /// <summary>
        /// 
        /// </summary>
        public bool IsModified()
        {
        	bool Result = true;
//        	Result = _CRC.Equals("*");
            return Result;
        }
    }
}
