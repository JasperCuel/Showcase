using Cue.Core;
using UnityEngine;

namespace Project
{
    public class TestSingleton : MonoSingleton<TestSingleton>
    {
        public string Test()
        {
            return "Test";
        }
    }
}