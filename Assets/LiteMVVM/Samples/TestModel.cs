using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iVLBB.LiteMVVM
{
    public class TestModel : MonoBehaviour
    {
        public event Action<Vector3,TestModelItem[],float> ModelEvent;

        public List<TestModelItem> Items = new List<TestModelItem>();

        [SerializeField] private float _testFloat;
        [SerializeField] private bool _runUpdate;

        private void Start()
        {
            Application.targetFrameRate = 60;
            StartCoroutine(ChangeItems());
        }

        void Update()
        {
            if (!_runUpdate)
            {
                return;
            }
            transform.Rotate( new Vector3(1, 1, 1));
            
            var e = transform.eulerAngles;
            ModelEvent?.Invoke(e,Items.ToArray(),_testFloat);
        }

        public void SetFloat(float f)
        {
            _testFloat = f;
            var pos = transform.position;
            pos.x = _testFloat;
            transform.position = pos;
        }
        
        public TestModelItem[] GetItems()
        {
            return Items.ToArray();
        }
        
        public float GetFloat()
        {
            return _testFloat;
        }

        private IEnumerator ChangeItems()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                if (Items.Count>2)
                {
                    var pick = Items[0];
                    Items.RemoveAt(0);
                    Items.Add(pick);
                }
                
            }
        }
        
        [Serializable]
        public class TestModelItem
        {
            public string Name;
            public int V1;
            public float V2;
        }

    }
}
