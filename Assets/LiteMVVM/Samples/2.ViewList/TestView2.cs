using LiteMVVM;
using UnityEngine;

namespace iVLBB.LiteMVVM._3.ViewList
{
    /// <summary>
    /// 用於更新List
    /// </summary>
    public class TestView2 : ViewBase<TestVm, TestVmData>
    {
        [SerializeField] private TestViewList _viewList;
        
        private void Start()
        {
            ClearBinding();
            
            Bind(data => data.SubVms, v =>_viewList.ViewModels =v); //資料綁定UI
            Vm1 = new TestVm(); //直接指定VM就完成註冊
        }
    }
}
