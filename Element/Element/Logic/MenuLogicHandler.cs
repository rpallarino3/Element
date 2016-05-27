using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Element.ResourceManagement;
using Element.Input;
using Element.Common.Enumerations;
using Element.Common.Menus;

namespace Element.Logic
{
    public class MenuLogicHandler
    {
        private ResourceManager _resourceManager;
        private InputHandler _inputHandler;

        private StartAndExitMenuLogicHandler _startAndExitMenuLogicHandler;

        public MenuLogicHandler(ResourceManager resourceManager, InputHandler inputHandler)
        {
            _resourceManager = resourceManager;
            _inputHandler = inputHandler;

            _startAndExitMenuLogicHandler = new StartAndExitMenuLogicHandler(_resourceManager, _inputHandler);
        }
    }
}
