using Newtonsoft.Json;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;
using uSync.Migrations.Migrators.Core;
using UmbConstants = Umbraco.Cms.Core.Constants;

namespace uSyncMigrationSite.Extensions.Migrators
{

    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.MultiUrlPicker)]
    [SyncMigratorVersion(8)]
    public class SMMultiUrlPickerMigrator : MultiUrlPickerMigrator
    {
        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {
            if (string.IsNullOrWhiteSpace(contentProperty.Value))
            {
                return contentProperty.Value;
            }

            try
            {
                var config = JsonConvert.DeserializeObject<List<MultiUrlPickerValueEditor.LinkDto>>(contentProperty.Value);
                return JsonConvert.SerializeObject(config);
            }
            catch
            {
                var x = base.GetContentValue(contentProperty, context);
                return x;
            }
        }
    }
}
