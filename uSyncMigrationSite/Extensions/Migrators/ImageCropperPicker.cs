using Newtonsoft.Json;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;

namespace uSync.Migrations.Migrators;

[SyncMigrator("Our.Umbraco.ImageCropPicker")]
[SyncMigratorVersion(8)]
public class ImageCropperPickerMigrator : SyncPropertyMigratorBase, ISyncPropertyMigrator
{
    public override string GetEditorAlias(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        => "Our.Umbraco.ImageCropPicker";

    
}