using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devon4Net.Infrastructure.Common.Options.Cache
{
    public class CacheOptions
    {
        public string ConectionString { get; set; }
        public long ? CacheSize { get; set; }
        public bool EnableCache { get; set; }
    }
}
