using System;

namespace WorkerServer
{
    public partial class Commands
    {
        public void UCOEF(object client, string[] arg)
        {
            Database.UpdateCoefficients(Convert.ToInt64(arg[0]), arg[1]);
            Database.UpdateLevel(Convert.ToInt64(arg[0]), "1");
        }
    }
}