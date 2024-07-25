using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Infrastructure.PublishedCache.DataSource;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;
using uSync.Migrations.Migrators.Core;
using UmbConstants = Umbraco.Cms.Core.Constants;

namespace uSyncMigrationSite.Extensions.Migrators
{
    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.MediaPicker3)]
    [SyncMigratorVersion(8)]
    public class SMMediaPicker3Migrator : MediaPickerMigrator
    {
        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {
            //if (contentProperty.ContentTypeAlias == "gridDownloadTourPDF")
            //{
                return contentProperty.Value;
            //}
            //var x = base.GetContentValue(contentProperty, context);
            //return x;
        }

    }
}
