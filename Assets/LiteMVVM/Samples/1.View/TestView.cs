using System.Diagnostics;
using LiteMVVM;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace iVLBB.LiteMVVM
{
    public class TestView : ViewBase<TestVm, TestVmData>
    {
        [SerializeField] public Text _text;
        [SerializeField] public Slider _slider;

        private static TestVm _vm;

        private void Start()
        {
            if (_vm==null)
            {
                _vm =new TestVm();
            }
            
            var watch = Stopwatch.StartNew(); //-----SpeedTest
            
            Bind(data => data.FloatData, v => _slider.value = v); //資料綁定UI
            Bind(data => data.Euler, v => _text.text = $"Euler : {v.ToString()}"); //資料綁定UI

            _slider.onValueChanged.AddListener(f => Vm1.SetModelFloat(f)); //UI綁定Method
            Vm1 = _vm; //直接指定VM就完成註冊
            
            watch.Stop(); //-----SpeedTest
            Debug.LogError($"Stopwatch : {watch.ElapsedMilliseconds}ms {watch.ElapsedTicks}t");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))//-----測速
            {
                ClearBinding();
                _slider.onValueChanged.RemoveAllListeners();
                Start();
            }
        }
    }
}