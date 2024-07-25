namespace uSync.Migrations.Migrators.BlockGrid.SettingsMigrators;

public class GridViewPropertyTextstringMigrator : IGridSettingsViewMigrator
{
    public string ViewKey => "Textstring";

    public string NewDataTypeAlias => "Textstring";

    public object ConvertContentString(string value)
    {
        return value;
    }
}