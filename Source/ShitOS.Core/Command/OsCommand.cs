namespace ShitOS.Core.Command
{
    public class OsCommand {
        private int _totalTics;

        public OsCommand(OsCommandOptions options)
        {
            _totalTics = 0;
            IsComplited = false;
            Options = options;
        }
        public OsCommandOptions Options { get; }
        public bool IsComplited { get; private set; }
        public OsCommandType Type => Options.CommandType;
        
        /// <param name="tics">Доступное количество тиков</param>
        /// <returns>Оставшееся количество тиков</returns>
        public int Process(int tics)
        {
            _totalTics += tics;
            if (Options.RequiredTics <= _totalTics)
            {
                _totalTics = Options.RequiredTics;
                IsComplited = true;
                
                return tics - _totalTics;
            }
            
            return 0;
        }
    }
}
