using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;
using uSync.Migrations.Migrators.Core;
using static Umbraco.Cms.Core.PropertyEditors.ValueListConfiguration;
using cst = Umbraco.Cms.Core.Constants.PropertyEditors.Aliases;


namespace uSyncMigrationSite.Extensions.Migrators
{

    [SyncMigrator(cst.MultipleTextstring, typeof(MultipleTextStringConfiguration))]
    [SyncMigratorVersion(8)]
    public class SMMultipleTextStringMigrator : MultipleTextStringMigrator
    {
        private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
        private static readonly string NewLine = "\n";

        public SMMultipleTextStringMigrator(IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        {
            _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
        }

        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {
            if (!contentProperty.Value.IsNullOrWhiteSpace())
            {
                try
                {
                    ValueListItem[]? valueListItems = _configurationEditorJsonSerializer.Deserialize<ValueListItem[]>(contentProperty.Value);

                    // The legacy property editor saved this data as new line delimited! strange but we have to maintain this
                    if (valueListItems != null && valueListItems.Length > 0)
                    {
                        var flattened = valueListItems.Where(item => item.Value is not null).Select(item => item.Value).ToArray();

                        return string.Join(NewLine, flattened);
                    }
                    //return _configurationEditorJsonSerializer.Serialize(flattened);
                }
                catch
                {
                    // empty
                }
            }

            return base.GetContentValue(contentProperty, context);
        }

    }

}