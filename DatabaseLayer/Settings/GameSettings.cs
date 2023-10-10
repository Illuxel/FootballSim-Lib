using System;

namespace DatabaseLayer.Settings
{
    public static class GameSettings
    {
        private static string _baseGamePath;

        /// <summary>
        /// Base folder path of the game
        /// </summary>
        public static string BaseGamePath
        {
            get
            {
                if (string.IsNullOrEmpty(_baseGamePath))
                {
                    throw new Exception("Game files path was not specified by Unity");
                }

                return _baseGamePath;
            }
            set
            { 
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Game files path was set to empty");
                }

                _baseGamePath = value;
            }
        }
    }
}
