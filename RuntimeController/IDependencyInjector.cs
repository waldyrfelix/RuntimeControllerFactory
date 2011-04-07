using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Fusonic.Web.Mvc.RuntimeController
{
    public interface IDependencyInjector
    {
        IController CreateControllerInstance(Type controllerType);
    }
}
