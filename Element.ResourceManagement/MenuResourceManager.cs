using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Element.ResourceManagement
{
    public class MenuResourceManager
    {
        private IServiceProvider _serviceProvider;
        private string _rootDirectory;

        public MenuResourceManager(IServiceProvider serviceProvider, string rootDirectory)
        {
            _serviceProvider = serviceProvider;
            _rootDirectory = rootDirectory;
        }
    }
}
