using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LiteMVVM
{
    public abstract class ViewListBase<TView,TViewModel> : MonoBehaviour
        where TView : ViewBase<TViewModel>
        where TViewModel : class,IVm
    {
        [SerializeField] protected Transform _attach;
        [SerializeField] protected TView _prefab;
        [SerializeField] private int _preloadPrefab = 0;

        private Transform _itemPoolParent;
        private readonly List<TView> _itemList = new List<TView>();
        private readonly Queue<TView> _itemPool = new Queue<TView>();

        public TViewModel[] ViewModels
        {
            get => _viewModels;
            set
            {
                if (_viewModels != value)
                {
                    _viewModels = value;
                    UpdateItemVm();
                }
            }
        }

        private TViewModel[] _viewModels;

        private void Awake()
        {
            PreLoad();
        }

        private void Start()
        {
            _prefab?.gameObject.SetActive(false);
        }

        private void PreLoad()
        {
            CheckPool();
            var preloadCount = _preloadPrefab -_itemList.Count - _itemPool.Count;
            for (int i = 0; i < preloadCount; i++)
            {
                var clone =Instantiate(_prefab, _itemPoolParent);
                clone.transform.localPosition = Vector3.zero;
                clone.transform.localScale = Vector3.one;
                clone.gameObject.SetActive(true);
                _itemPool.Enqueue(clone);
            }
        }

        private void UpdateItemVm()
        {
            if (_prefab == null||_attach == null)
            {
                Debug.LogError($"{gameObject} _prefab or _attach is null");
                return;
            }

            CheckPool();

            int vmCount = 0;
            if (_viewModels != null)
            {
                vmCount = _viewModels.Length;
            }

            var listCount = _itemList.Count;
            if (listCount < vmCount)
            {
                for (int i = 0; i < vmCount-listCount; i++)
                {
                    TView clone = null;
                    if (_itemPool.Count>0)
                    {
                        clone = _itemPool.Dequeue();
                        clone.transform.SetParent(_attach);
                    }
                    else
                    {
                        clone =Instantiate(_prefab, _attach);
                        clone.transform.localPosition = Vector3.zero;
                        clone.transform.localScale = Vector3.one;
                        clone.gameObject.SetActive(true);
                    }
                    clone.transform.SetAsLastSibling();
                    _itemList.Add(clone);
                }
            }
            else if (listCount > vmCount)
            {
                for (int i = 0; i < listCount-vmCount; i++)
                {
                    var toRemove = _itemList[_itemList.Count - 1];
                    _itemList.Remove(toRemove);
                    toRemove.transform.SetParent(_itemPoolParent);
                    toRemove.Vm1 = null;
                    _itemPool.Enqueue(toRemove);
                }
            }

            for (int i = 0; i < _itemList.Count; i++)
            {
                _itemList[i].Vm1 = _viewModels[i];
            }
            
        }

        private void CheckPool()
        {
            if (_itemPoolParent==null)
            {
                _itemPoolParent = new GameObject().transform;
                _itemPoolParent.gameObject.name = "ItemPoolParent";
                _itemPoolParent.SetParent(_attach);
                _itemPoolParent.gameObject.SetActive(false);
            }
        }
    }
}