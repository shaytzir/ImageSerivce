namespace Infrastructure
{
    public class Settings
    {
        private string _OutputDir;
        public string OutputDir
        {
            get { return _OutputDir; }
            set { _OutputDir = value; }
        }

        private string _SourceName;
        public string SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; }
        }

        private string _LogName;
        public string LogName
        {
            get { return _LogName; }
            set { _LogName = value; }
        }

        private string _ThumbSize;
        public string ThumbSize
        {
            get { return _ThumbSize; }
            set { _ThumbSize = value; }
        }

        private string _Handlers;
        public string Handlers
        {
            get { return _Handlers; }
            set { _Handlers = value; }
        }
    }
}