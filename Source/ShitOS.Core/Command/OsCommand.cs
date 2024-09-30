using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShitOS.Core
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
