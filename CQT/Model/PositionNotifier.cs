using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model
{
    interface PositionNotifier
    {
        void watch(PositionWatcher watcher);
        void unWatch(PositionWatcher watcher);
    }
}
