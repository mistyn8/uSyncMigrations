using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Infrastructure.PublishedCache.DataSource;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;
using uSync.Migrations.Migrators.Core;
using UmbConstants = Umbraco.Cms.Core.Constants;

namespace uSyncMigrationSite.Extensions.Migrators
{
    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.MediaPicker)]
    [SyncMigrator("Umbraco.MediaPicker2")]
    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.MultipleMediaPicker)]
    [SyncMigratorVersion(8)]
    public class SMMediaPickerMigrator : MediaPickerMigrator
    {
        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {
            if (string.IsNullOrWhiteSpace(contentProperty.Value))
            {
                return contentProperty.Value;
            }
            //TODO: check it's not mediaPicker3Already... (seems to loop though twice for grid, and wipes out the ogImage)

            try
            {
                var mp3 = JsonConvert.DeserializeObject<IEnumerable<MediaWithCropsDto>>(contentProperty.Value);
                return contentProperty.Value;
            }
            catch
            {

                //if (contentProperty.Value.Contains("mediaKey"))
                //{
                //    return contentProperty.Value;
                //}

                //otherwsie try and convert
                var x = base.GetContentValue(contentProperty, context);
                return x;
            }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        private sealed class MediaWithCropsDto
        {
            public Guid Key { get; set; }

            public Guid MediaKey { get; set; }

            public IEnumerable<ImageCropperValue.ImageCropperCrop>? Crops { get; set; }

            public ImageCropperValue.ImageCropperFocalPoint? FocalPoint { get; set; }
        }

    }
}
