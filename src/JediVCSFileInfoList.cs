/*
 * Cruise Control .Net JediVCS plugin
 * 
 * Cruise Control .Net web site : http://ccnet.sourceforge.net
 * JediVCS web site : http://jedivcs.sourceforge.net
 * 
 */

using System;
using System.Collections;
using System.Text;

namespace CruiseControl.Net.Plugin.JediVCS
{
    
    /// <summary>
    /// this class is provided for collection of JediVCSFileInfo
    /// </summary>
    public class JediVCSFileInfoList : CollectionBase
    {
        #region Properties

        /// <summary>
        /// Property to get one of the items
        /// </summary>
        /// <param Name="index">Items index to get</param>
        /// <returns>The selected item</returns>
        public JediVCSFileInfo this [int index]
        {
            get
            {
                return (JediVCSFileInfo)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }

        #endregion Properties

        #region Methods

        #region PublicMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, JediVCSFileInfo item)
        {
            this.List.Insert(index, item);
        }
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(JediVCSFileInfo item)
        {
            this.List.Add(item);
        }

        #endregion PublicMethods

        #endregion Methods

    }
}
