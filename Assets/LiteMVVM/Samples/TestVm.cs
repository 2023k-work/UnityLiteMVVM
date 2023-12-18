using System.Collections.Generic;
using iVLBB.LiteMVVM._3.ViewList;
using LiteMVVM;
using UnityEngine;

namespace iVLBB.LiteMVVM
{
    public class TestVm : VmBase<TestVmData>
    {
        private List<TestVmItem> _subVms = new List<TestVmItem>();
        private readonly TestModel _m;

        public TestVm()
        {
            _m = GameObject.FindObjectOfType<TestModel>();
            _m.ModelEvent += OnGetModelData;
            UpdateVmData(_m.transform.eulerAngles,_m.GetItems(),_m.GetFloat());
        }


        private void OnGetModelData(Vector3 v, TestModel.TestModelItem[] models,float f)
        {
            UpdateVmData(v, models, f);
        }

        private void UpdateVmData(Vector3 v, TestModel.TestModelItem[] models, float f)
        {
            ModifyData(out Data.FloatData,f);
            ModifyData(out Data.Euler,v);

            var countChanged = UpdateListCount(_subVms, models.Length);

            if (countChanged)
            {
                ModifyData(out Data.SubVms,_subVms.ToArray());
                
                for (int i = 0; i < models.Length; i++)
                {
                    _subVms[i].SetModel(models[i]);
                }
            }
            
        }

        public void SetModelFloat(float f)
        {
            _m.SetFloat(f);
        }
    }

    public struct TestVmData
    {
        public Vector3 Euler;
        public TestVmItem[] SubVms;
        public float FloatData;
    }

}