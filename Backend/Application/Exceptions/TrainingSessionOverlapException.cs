using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    [DebuggerStepThrough]
    public class TrainingSessionOverlapException : Exception
    {

        public TrainingSessionOverlapException() :
            base("Overlapping training session log exists for the given date and duration.")
        {
        }
    }
}
