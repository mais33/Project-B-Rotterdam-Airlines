public class ActionMenu
{
    public string Label;
    public Action actionMenu;

    public ActionMenu(string label, Action actionmenu)
    {
        Label = label;
        actionMenu = actionmenu;
    }
}