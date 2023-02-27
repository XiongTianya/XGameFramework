using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XGameFramework.Event;

public class TestGameEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(TestGameEventArgs).GetHashCode();

    public TestGameEventArgs()
    {
    }

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    public override void Clear()
    {

    }
}