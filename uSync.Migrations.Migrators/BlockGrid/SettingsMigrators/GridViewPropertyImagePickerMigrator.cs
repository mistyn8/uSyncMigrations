namespace uSync.Migrations.Migrators.BlockGrid.SettingsMigrators;

public class GridViewPropertyImagePickerMigrator : IGridSettingsViewMigrator
{
    public string ViewKey => "imagepicker";

    public string NewDataTypeAlias => "Image Media Picker";

    public object ConvertContentString(string value)
    {
        return value;
    }
}