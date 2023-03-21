using CsGOStateEmitter.Entities;

namespace CsGOStateEmitter
{
    public class StateManagement
    {
        public Result LastMatch { get; set; }

        public string Rollback { get; set; }
        public StateManagement()
        {
            LastMatch = null;
        }

        
    }
}
