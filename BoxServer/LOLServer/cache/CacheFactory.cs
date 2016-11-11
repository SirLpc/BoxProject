using LOLServer.cache.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LOLServer.cache
{
    class CacheFactory
    {
        public readonly static IAccountCache accountCache;

        static CacheFactory() {
            accountCache = new AccountCache();
        }
    }
}
