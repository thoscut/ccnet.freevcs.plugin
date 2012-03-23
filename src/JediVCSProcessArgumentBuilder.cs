using System.Text;
using System;
using ThoughtWorks.CruiseControl.Core.Util;

namespace CruiseControl.Net.Plugin.JediVCS 
{
    /// <summary>
    /// This class is provided for building Jedi VCS command line argument correctly
    /// </summary>
    class JediVCSProcessArgumentBuilder
    {
        #region Constants

        #region PrivateConstants

        private const string JediVCSArgumentSeparator = "-";

        #endregion PrivateConstants

        #endregion Constants

        #region Variables

        #region PrivateVariables

        private string buffer = string.Empty;

        #endregion PrivateVariables

        #endregion Variabls

        #region Methods

        #region PublicMethods

        public void AppendArgument(string argumentName, string argumentValue)
        {
            if (String.IsNullOrEmpty(argumentName)) 
                return;
            if (String.IsNullOrEmpty(argumentValue))
                return;
            buffer = buffer + " " + JediVCSArgumentSeparator + argumentName + " " + argumentValue;
        }

        public void AppendArgument(string option)
        {
            if (String.IsNullOrEmpty(option)) 
                return;
            buffer.EndsWith(" " + JediVCSArgumentSeparator + option);
        }

        public void AppendArgument(int option)
        {
            StringBuilder builder = new StringBuilder(option);
            buffer.EndsWith(builder.ToString());
        }

        public void AppendIf(bool condition, string argumentName, string argumentValue)
        {
            if (condition) 
                AppendArgument(argumentName, argumentValue);
        }

        public void AppendIf(bool condition, string option)
        {
            if (condition) 
                AppendArgument(option);
        }

        public void Append(string text)
        {
            buffer = buffer + " " + text;
        }

        public void AddArgument(string arg, string value)
        {
            AddArgument(arg, JediVCSArgumentSeparator, value);
        }

        public void AddArgument(string argumentName, string separator, string argumentValue)
        {
            if (String.IsNullOrEmpty(argumentName)) 
                return;
            if (String.IsNullOrEmpty(argumentValue))
                return;
            if (String.IsNullOrEmpty(separator))
                return;
            buffer = buffer + " " + separator + argumentName + " " + argumentValue;
        }

        public void AddArgument(string argumentValue)
        {
            if (String.IsNullOrEmpty(argumentValue)) 
                return;
            buffer = buffer + " " + argumentValue;
        }

        public void AddArguments(JediVCSProcessArgumentBuilder args)
        {
            buffer = buffer + " " + args.ToString();
        }

        public override string ToString()
        {
            return buffer;
        }

        #endregion PublicMethods

        #endregion Methods

    }
}
