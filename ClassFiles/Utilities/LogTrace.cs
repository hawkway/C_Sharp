using System;
using System.IO;
using System.Reflection;

namespace ChangeMe
{
    class LogTrace
    {
        private const string TAB = "\t";
        private string _sLogFileName = Assembly.GetEntryAssembly().GetName().Name + ".txt";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LogTrace()
        {
        } // constructor

        /// <summary>
        /// Constructor with specified filename
        /// </summary>
        /// <param name="sUserFileName"></param>
        public LogTrace(string sUserFileName)
        {
            _sLogFileName = sUserFileName;
        } // overloaded constructor

        public void Add(string s)
        {
            StreamWriter sw = null;
            try
            {
                sw = File.AppendText(_sLogFileName);

                // format timestamp
                string sLogFormat = string.Format("{0} {1} {2} {3}", DateTime.Now.ToString("h:mm:ss tt"), 
                        DateTime.Now.ToShortDateString(), TAB, s);

                // add timestamps for text only
                if (!string.IsNullOrWhiteSpace(s))
                    sw.WriteLine(sLogFormat);
                else
                    sw.WriteLine();

                sw.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to write to file: {0}\nError: {1}", _sLogFileName, ex.Message));
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        } // Add

    } // class LogTrace

} // namespace