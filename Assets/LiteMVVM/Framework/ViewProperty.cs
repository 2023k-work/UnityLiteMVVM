using System;

namespace LiteMVVM
{
    /// <summary>
    /// 用於檢測數值有無變動
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewProperty<T>
    {
        private T _viewData;
        private readonly Action<T> _onValueChanged;
        private bool _firstTime = true;

        public ViewProperty(Action<T> onValueChanged)
        {
            _onValueChanged = onValueChanged;
        }

        public void Set(T newValue)
        {
            if (newValue==null)
            {
                return;
            }

            if (_firstTime)
            {
                _firstTime = false;
                _viewData = newValue;
                _onValueChanged?.Invoke(newValue);
                return;
            }
            if (_viewData==null)
            {
                _viewData = newValue;
                _onValueChanged?.Invoke(newValue);
            }
            if (!_viewData.Equals(newValue))
            {
                _viewData = newValue;
                _onValueChanged?.Invoke(newValue);
            }
        }

        public T Get()
        {
            return _viewData;
        }

    }
}