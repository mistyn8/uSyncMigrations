using Umbraco.Cms.Web.Common.Security;

namespace uSync.Migrations.Migrators.BlockGrid.SettingsMigrators;

public class GridViewPropertyBackgroundColorPickerMigrator : IGridSettingsViewMigrator
{
    public string ViewKey => "colorpicker-background-color";

    public string NewDataTypeAlias => "Background Colour Picker";

    public object ConvertContentString(string value)
    {
        return value;
    }
    
}

public class GridViewPropertyAccentColorPickerMigrator : IGridSettingsViewMigrator
{
    public string ViewKey => "colorpicker-accent-color";

    public string NewDataTypeAlias => "Accent Colour";

    public object ConvertContentString(string value)
    {
        return value;
    }

}