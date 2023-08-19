using System;

namespace Cue.Networking
{
    public abstract class IUnityWebRequest
    {
        public string requestKey;
        public Action<string> successHandler;
        public Action<string> errorHandler;

        public IUnityWebRequest(string requestKey, Action<string> successHandler, Action<string> errorHandler)
        {
            this.requestKey = requestKey;
            this.successHandler = successHandler;
            this.errorHandler = errorHandler;
        }
    }
}