using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuntimeControllerTestApp.Models
{
    public interface IMessageProvider
    {
        string GetMessage(string name);
    }
}
