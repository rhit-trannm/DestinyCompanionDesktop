using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFD2
{
    public class Manager
    {
        APIManager APIManager;
        SQLManager SQLManager;

        public Manager()
        {
            this.APIManager = new APIManager();
            this.SQLManager = new SQLManager();

        }
        public APIManager getAPIManager()
        {
            return this.APIManager;
        }
        public SQLManager getSQLManager()
        {
            return this.SQLManager;
        }

    }
}
