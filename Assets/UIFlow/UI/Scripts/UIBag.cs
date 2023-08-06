using UIFlow.UI;
using UnityEngine;
using UnityEngine.UI;

public partial class UIBag : UIBase
{
    public override UIType Layer => UIType.Normal;
    public override string PrefabName => "UIBag";

    protected override void OnShow()
    {
        Close.onClick.AddListener((() =>
        {
            UIManager.Instance.Close<UIBag>();
        }));

        for (int i = 0; i < 10; i++)
        {
            var item = Object.Instantiate(Item, Bg.transform);
            item.gameObject.SetActive(true);
            var btn = item.GetComponent<Button>();
            var index = i;
            var text = item.GetComponentInChildren<Text>();
            text.text = index.ToString();
            btn.onClick.AddListener((() =>
            {
                Debug.Log($"Use:{index}");
            }));
        }
    }

    protected override void OnClose()
    {
        Close.onClick.RemoveAllListeners();
        for (int i = 9; i >= 0; i--)
        {
            Object.DestroyImmediate(Bg.transform.GetChild(i).gameObject);
        }
    }
}
