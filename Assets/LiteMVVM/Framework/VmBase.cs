using System;
using System.Collections.Generic;
using UnityEngine;

namespace LiteMVVM
{
    /// <summary>
    /// todo 負責工作
    /// </summary>
    /// <typeparam name="T">T為想要提供給外部的資料</typeparam>
    public abstract class VmBase<T> : IVm where T : struct
    {
        public event Action<T> OnVmChanged;

        protected T Data;

        private int _invokeFrame;

        public T GetData()
        {
            return Data;
        }

        /// <summary>
        /// 更改呈現資料
        /// </summary>
        /// <param name="dataField"></param>
        /// <param name="newValue"></param>
        /// <typeparam name="TData"></typeparam>
        protected void ModifyData<TData>(out TData dataField, TData newValue)
        {
            dataField = newValue;
            InvokeDataChange();
        }
        
        /// <summary>
        /// 觸發View改變
        /// </summary>
        private void InvokeDataChange()
        {
            if (_invokeFrame != Time.frameCount)
            {
                _invokeFrame = Time.frameCount;
                VmDispatcher.Instance.DoOnLateUpdate(() =>
                {
                    OnVmChanged?.Invoke(Data);
                });
            }
        }
        
        protected bool UpdateListCount<T>(List<T> vmList, int count) where T : new()
        {
            var diff = count - vmList.Count;
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    vmList.Add(new T());
                }
            }
            else if (diff < 0)
            {
                for (int i = 0; i < -diff; i++)
                {
                    vmList.RemoveAt(vmList.Count - 1);
                }
            }

            return diff != 0;
        }
    }
}
