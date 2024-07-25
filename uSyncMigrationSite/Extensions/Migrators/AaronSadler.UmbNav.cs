using Newtonsoft.Json;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;

namespace uSync.Migrations.Migrators;

[SyncMigrator("AaronSadler.UmbNav")]
[SyncMigratorVersion(8)]
public class AaronSadlerUmbNavMigrator : SyncPropertyMigratorBase, ISyncPropertyMigrator
{
    public override string GetEditorAlias(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        => "AaronSadler.UmbNav";

    //public override object GetConfigValues(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
    //{
    //    var config = new Bergmania.OpenStreetMap.Core.OpenStreetMapConfiguration();
    //    if (dataTypeProperty.ConfigAsString is string configString)
    //    {
    //        config = JsonConvert.DeserializeObject<Bergmania.OpenStreetMap.Core.OpenStreetMapConfiguration>(configString);
    //    }
    //    return config!;
    //}
}