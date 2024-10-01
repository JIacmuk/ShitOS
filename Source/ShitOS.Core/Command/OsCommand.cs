namespace ShitOS.Core.Command
{
    public class OsCommand(
        OsCommandOptions options
    ){
        private int _totalTics = 0;
        public bool IsComplited { get; private set; } = false;
        public OsCommandOptions Options { get; } = options;
        
        
        /// <param name="tics">Доступное количество тиков</param>
        /// <returns>Затраченное количество тиков</returns>
        public int Process(int tics)
        {
            _totalTics += tics;
            if (Options.RequiredTics <= _totalTics)
            {
                int freeTics = Options.RequiredTics - _totalTics;
                _totalTics = Options.RequiredTics;
                IsComplited = true;
                return tics - freeTics;
            }
            
            return tics;
        }
    }
}
