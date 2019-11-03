using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GameSignals
{
    public static event Action<bool> OnLevelPaused;
    public static void PauseLevel (bool shouldPause) { OnLevelPaused?.Invoke (shouldPause); }
}
