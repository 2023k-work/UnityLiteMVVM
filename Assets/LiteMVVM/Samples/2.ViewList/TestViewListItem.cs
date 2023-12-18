using LiteMVVM;
using UnityEngine;
using UnityEngine.UI;

namespace iVLBB.LiteMVVM._3.ViewList
{
    public class TestViewListItem : ViewBase<TestVmItem,TestVmItemData>
    {
        [SerializeField] private Text _text1;
        [SerializeField] private Text _text2;
        [SerializeField] private Text _text3;

        private void Start()
        {
            Bind(data=>data.Name,value=>_text1.text = value);
            Bind(data=>data.V1,value=>_text2.text = value.ToString());
            Bind(data=>data.V2,value=>_text3.text =  value.ToString());
        }
    }
}