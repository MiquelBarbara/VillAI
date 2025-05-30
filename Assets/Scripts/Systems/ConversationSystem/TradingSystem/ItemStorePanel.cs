using System;

public class ItemStorePanel : ItemPanel
{
    public override void OnClick(int id)
    {
        SellItem();

        Show();
    }

    private void SellItem()
    {
        throw new NotImplementedException();
    }
}