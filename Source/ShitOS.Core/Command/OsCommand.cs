namespace ShitOS.Core.Command
{
    public class OsCommand {

        public OsCommand(OsCommandOptions options)
        {
            TotalTics = 0;
            IsComplited = false;
            Options = options;
        }
        
        public OsCommandType Type => Options.CommandType;
        public OsCommandOptions Options { get; }
        public int TotalTics { get; private set; }
        public bool IsComplited { get; private set; }
        
        /// <param name="tics">Доступное количество тиков</param>
        /// <returns>Оставшееся количество тиков</returns>
        public int Process(int tics)
        {
            TotalTics += tics;
            if (Options.RequiredTics <= TotalTics)
            {
                TotalTics = Options.RequiredTics;
                IsComplited = true;
                
                return tics - TotalTics;
            }
            
            return 0;
        }
    }
}
