using System.IO;
using System.Threading.Tasks;

namespace Common.Web.Core.TestWebApp
{
    public class ExceptionalTask : IScheduledTask
    {
        public bool IsShuttingDown { get; set; }

        public Task RunAsync()
        {
            throw new FileNotFoundException("Couldn't find the xyz.abc file.");
        }
    }
}