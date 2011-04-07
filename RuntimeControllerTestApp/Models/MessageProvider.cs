using System;
using RuntimeControllerTestApp.Controllers;

namespace RuntimeControllerTestApp.Models
{
    public class MessageProvider : IMessageProvider
    {
        public string GetMessage(string name)
        {
            return String.Format("Hello {0}, I'm the MessageProvider implementation!", name);
        }
    }
}