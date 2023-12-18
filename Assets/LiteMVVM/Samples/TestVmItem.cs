using LiteMVVM;
using UnityEngine;

namespace iVLBB.LiteMVVM._3.ViewList
{
    public class TestVmItem : VmBase<TestVmItemData>
    {
        private TestModel.TestModelItem _model;

        public void SetModel(TestModel.TestModelItem model)
        {
            ModifyData(out Data.Name , model.Name);
            ModifyData(out Data.V1 , model.V1);
            ModifyData(out Data.V2 , model.V2);
        }
        
    }

    public struct TestVmItemData
    {
        public string Name;
        public int V1;
        public float V2;
    }
}