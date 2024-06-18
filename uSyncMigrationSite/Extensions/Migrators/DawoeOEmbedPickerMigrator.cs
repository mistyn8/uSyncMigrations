using Newtonsoft.Json;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;

namespace uSync.Migrations.Migrators;

[SyncMigrator("Dawoe.OEmbedPickerPropertyEditor")]
[SyncMigratorVersion(8)]
public class DawoeOEmbedPickerMigrator : SyncPropertyMigratorBase, ISyncPropertyMigrator
{
    public override string GetEditorAlias(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        => "Dawoe.OEmbedPickerPropertyEditor";

    public override object GetConfigValues(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
    {
        var config = new Dawoe.OEmbedPickerPropertyEditor.Core.Configuration.OEmbedPickerConfiguration();
        if (dataTypeProperty.ConfigAsString is string configString)
        {
            config = JsonConvert.DeserializeObject<Dawoe.OEmbedPickerPropertyEditor.Core.Configuration.OEmbedPickerConfiguration>(configString);
        }
        return config!;
    }
}