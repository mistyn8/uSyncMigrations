using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Infrastructure.PublishedCache.DataSource;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;
using uSync.Migrations.Migrators.Core;
using UmbConstants = Umbraco.Cms.Core.Constants;

namespace Lovell.Web.Extensions.Migrations.Migrators
{
    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.MediaPicker)]
    [SyncMigrator("Umbraco.MediaPicker2")]
    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.MultipleMediaPicker)]
    [SyncMigratorVersion(8)]
    public class SMMediaPickerMigrator : MediaPickerMigrator
    {
        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {
            if (contentProperty.ContentTypeAlias == "gridDownloadTourPDF")
            {
                return contentProperty.Value;
            }
            var x = base.GetContentValue(contentProperty, context);
            return x;
        }        
        
    }
}
