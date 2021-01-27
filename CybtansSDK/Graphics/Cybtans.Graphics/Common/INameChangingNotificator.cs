using System;

namespace Cybtans.Graphics.Common
{
    public interface INameChangingNotificator
    {
        event Action<object, string> NameChanged;
    }
}