using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LiteMVVM
{
    public class VmDispatcher : MonoBehaviour
    {
        public static VmDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    var o = new GameObject();
                    o.name = typeof(VmDispatcher).ToString();
                    DontDestroyOnLoad(o);
                    _instance = o.AddComponent<VmDispatcher>();
                }

                return _instance;
            }
        }

        private static VmDispatcher _instance;
        private Queue<Action> _vmJobs = new Queue<Action>();
        [SerializeField] private long _totalTick;
        [SerializeField] private List<long> _largeTickList = new List<long>();
        [SerializeField] private int _largeTickDetect = 1000;

        public void DoOnLateUpdate(Action action)
        {
            if (action != null)
            {
                _vmJobs.Enqueue(action);
            }
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            var watch = Stopwatch.StartNew();
#endif
            while (_vmJobs.Count > 0)
            {
                var job = _vmJobs.Dequeue();
                try
                {
                    job?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"DoVmJob err : {e}");
                }
            }

#if UNITY_EDITOR
            watch.Stop();
            _totalTick = watch.ElapsedTicks;
            if (_totalTick > _largeTickDetect)
            {
                _largeTickList.Add(_totalTick);
            }
#endif
        }

#if UNITY_EDITOR
        
        private void OnGUI()
        {
            GUI.skin.label.fontSize = 32;
            GUI.skin.label.normal.textColor = Color.red;
            GUI.Label(new Rect(0,0,400,100),$"Total tick {_totalTick} t");
        }
#endif
    }
}