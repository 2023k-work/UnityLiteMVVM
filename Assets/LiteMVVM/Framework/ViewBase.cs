using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiteMVVM
{
    public abstract class ViewBase : MonoBehaviour
    {
    }
    
    public abstract class ViewBase<TVm1> : ViewBase 
        where TVm1 : class, IVm
    {
        public virtual TVm1 Vm1 { get; set; }
    }
    
    public abstract class ViewBase<TVm1,TVm1Data> : ViewBase<TVm1> 
        where TVm1 : VmBase<TVm1Data> 
        where TVm1Data : struct
    {
        public override TVm1 Vm1
        {
            get => _vm1;
            set
            {
                if (_vm1!=null)
                {
                    _vm1.OnVmChanged -= OnGetData;
                }
                
                _vm1 = value;

                if (_vm1!=null)
                {
                    _vm1.OnVmChanged += OnGetData;
                    OnGetData(_vm1.GetData());
                }
            }
        }


        private TVm1 _vm1;
        private TVm1Data _tempData;
        private readonly List<Action<TVm1Data>> _bindList = new List<Action<TVm1Data>>();

        private void OnGetData(TVm1Data data1)
        {
            _tempData = data1;
            for (int i = 0; i < _bindList.Count; i++)
            {
                _bindList[i].Invoke(data1);
            }
        }

        protected void Bind<T1DataField>(Func<TVm1Data,T1DataField> getData,Action<T1DataField> setValue)
        {
            var vp = new ViewProperty<T1DataField>(setValue);
            _bindList.Add(updateData =>vp.Set(getData.Invoke(_tempData)));

            if (Vm1!=null)
            {
                _bindList[_bindList.Count-1].Invoke(_tempData);
            }
        }

        protected void ClearBinding()
        {
            _bindList.Clear();
        }

        private void OnDestroy()
        {
            Vm1 = null;
            ClearBinding();
        }
    }

}
