namespace Padoru.Core
{
    public class Constants
    {
        public const string SETTINGS_OBJECT_NAME = "PadoruSettings";
        
        public const string PROTOCOL_REGEX_PATTERN = @"^(?:jar:)?[a-zA-Z]+:\/\/";
        
        public const string DEBUG_CHANNEL_INIT = "Initialization";
        public const string DEBUG_CHANNEL_FILES = "FilesManagement";
        public const string DEBUG_CHANNEL_FSM = "Fsm";
        public const string DEBUG_CHANNEL_ACTION_ROUTER = "ActionRouter";
        public const string DEBUG_CHANNEL_COMMANDS = "Commands";
        public const string DEBUG_CHANNEL_UI = "Ui";
        public const string DEBUG_CHANNEL_TIME = "Time";
        public const string DEBUG_CHANNEL_CAMERA = "Camera";
        public const string DEBUG_CHANNEL_TEXTURE = "Texture";
    }
}