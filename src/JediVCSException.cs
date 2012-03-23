/*
 * Cruise Control .Net JediVCS plugin
 * 
 * Cruise Control .Net web site : http://ccnet.sourceforge.net
 * JediVCS web site : http://jedivcs.sourceforge.net
 * 
 */

using System;
using System.Runtime.Serialization;

namespace CruiseControl.Net.Plugin.JediVCS
{
    /// <summary>
    /// this class provides exception generation and management
    /// on Jedi VCS command line call errors
    /// </summary>
	[Serializable]
    public class JediVCSException : ApplicationException
    {
        #region Methods

        #region PublicMethods

        /// <summary>
        /// 
        /// </summary>
        public JediVCSException() : base("") {}
        /// <summary>
        /// 
        /// </summary>
		public JediVCSException(string s) : base(s) {}
        /// <summary>
        /// 
        /// </summary>
		public JediVCSException(string s, Exception e) : base(s, e) {}
        /// <summary>
        /// 
        /// </summary>
        public JediVCSException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion PublicMethods

        #endregion Methods
    }
}
