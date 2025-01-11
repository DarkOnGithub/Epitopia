using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Core
{
    public class BetterLogger
    {
        private readonly string _name;

        public BetterLogger(Type obj)
        {
            _name = obj.Name;
        }

        private void Log([CanBeNull] object content, Action<object> logFunction)
        {
            var time = DateTime.Now;
            var text = content == null ? string.Empty : content.ToString();
            logFunction($"[{time.ToString("yyyy-MM-dd HH:mm:ss")}] / [{_name}] : {text}");
        }

        public void LogInfo([CanBeNull] object content)
        {
            Log(content, Debug.Log);
        }

        public void LogWarning([CanBeNull] object content)
        {
            Log(content, Debug.LogWarning);
        }

        public void LogError([CanBeNull] object content)
        {
            Log(content, Debug.LogError);
        }
    }
}