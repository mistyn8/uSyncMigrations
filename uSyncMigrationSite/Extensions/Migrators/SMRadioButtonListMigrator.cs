using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;
using uSync.Migrations.Migrators.Core;
using UmbConstants = Umbraco.Cms.Core.Constants;

namespace Lovell.Web.Extensions.Migrations.Migrators
{
    [SyncMigrator(UmbConstants.PropertyEditors.Aliases.RadioButtonList)]
    [SyncMigratorVersion(8)]
    public class SMRadioButtonListMigrator : RadioButtonListMigrator
    {

        //public override object? GetConfigValues(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        //{
        //    if (dataTypeProperty.DataTypeAlias == "Visible Slides")
        //    {
        //        context.Migrators.AddCustomValues(
        //        $"dataType_{dataTypeProperty.DataTypeAlias}_items", new Dictionary<string, object> { { "20", "3" }, { "2118", "1 with overlay" }, { "21", "1" } });
        //    }
        //    return base.GetConfigValues(dataTypeProperty, context);
        //}

        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {

            if (contentProperty.PropertyAlias == "visibleSlides")
            {
                var items = new Dictionary<string, object> { { "20", "3" }, { "2118", "1 with Overlay" }, { "21", "1" } };

                if (items.TryGetValue(contentProperty.Value, out var value) == true && value is string str)
                {
                    return str;
                }
            }

            return base.GetContentValue(contentProperty, context);

        }
    }
}


