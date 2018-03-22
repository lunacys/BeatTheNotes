using System;
using System.Collections.Generic;
using System.Text;

namespace Jackhammer.Screens
{
    public interface IScreenManager
    {
        T FindScreen<T>() where T : Screen;
    }
}
