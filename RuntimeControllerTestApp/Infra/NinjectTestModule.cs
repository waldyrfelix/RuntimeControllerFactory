using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using RuntimeControllerTestApp.Controllers;
using RuntimeControllerTestApp.Models;

namespace RuntimeControllerTestApp.Infra
{
    public class NinjectTestModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageProvider>().To<MessageProvider>();
        }
    }
}