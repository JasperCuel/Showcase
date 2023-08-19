To test:
1. Place the scriptable object DebugLoggerConfig inside your resources folder (Assets/Resources/)
2. Open the provided showcase scene SC_DebugLogger
3. Start the game and press spacebar, change the values of the Test GameObject in the inspector for different results

To add a new value:
1. Open the DebugType Enum and add your new value
2. Open the DebugLoggerConfig and add another item to the Debug Types list
3. Configure the new item and call the method DebugLogger.Log(string, DebugType) in your MonoBehaviour script