using uSync.Migrations.Core;
using uSync.Migrations.Core.Composing;
using uSync.Migrations.Core.Configuration.Models;
using uSync.Migrations.Core.Extensions;
using uSync.Migrations.Migrators.BlockGrid;
using uSync.Migrations.Migrators.Optional;
using UmbConstants = Umbraco.Cms.Core.Constants;

namespace Project.Extensions;

public class SMBlockMigrationProfile : ISyncMigrationPlan
{
    private readonly SyncMigrationHandlerCollection _migrationHandlers;

    public SMBlockMigrationProfile(SyncMigrationHandlerCollection migrationHandlers)
    {
        _migrationHandlers = migrationHandlers;
    }

    public int Order => 201;

    public string Name => "SM Convert to BlockLists and BlockGrid";

    public string Icon => "icon-brick color-green";

    public string Description => "SM Convert Nested content and Grid to BlockList and BlockGrid  (Experimental!)";

    //public MigrationOptions Options
    //{
    //    get
    //    {
    //        var handlers = _migrationHandlers.SelectGroup(8, string.Empty)
    //            //.Where(x=>x.Name == "DataType").ToList()
    //            ;

    //        return new MigrationOptions
    //        {
    //            Group = "Convert",
    //            Source = "uSync/v9-source",
    //            Target = $"{uSyncMigrations.MigrationFolder}/My-Custom-Migration",
    //            Handlers = handlers,
    //            SourceVersion = 8,
    //            PreferredMigrators = new Dictionary<string, string>
    //                                {
    //                                    { UmbConstants.PropertyEditors.Aliases.NestedContent, nameof(NestedToBlockListMigrator) },
    //                                    { UmbConstants.PropertyEditors.Aliases.Grid, nameof(GridToBlockGridMigrator) },
    //                                    //{UmbConstants.PropertyEditors.Aliases.MediaPicker, nameof(Migrator.TsdMediaPickerMigrator) },
    //                                    //{"Umbraco.MediaPicker2", nameof(Migrator.TsdMediaPickerMigrator)},
    //                                    //{UmbConstants.PropertyEditors.Aliases.MultipleMediaPicker, nameof(Migrator.TsdMediaPickerMigrator)},
    //                                }
    //        };
    //    }
    //}

    public MigrationOptions Options
    {
        get
        {
            //var handlers = _migrationHandlers.SelectGroup(8, string.Empty);
            var handlers = _migrationHandlers
                        .Handlers
                        // .Select(x => x.ToHandlerOption(x.Group == uSync.BackOffice.uSyncConstants.Groups.Content))
                        .Select(x => x.ToHandlerOption(true))
                        .ToList();

            return new()
            {
                Group = "Convert",
                Source = "uSync/v9-source",
                SourceVersion = 8,
                // write out to the same folder each time.
                Target = $"{uSyncMigrations.MigrationFolder}/My-Custom-Migration",

                // load all the handlers just enable the content ones.
                Handlers = handlers,

                // for this migrator we want to use our special grid migrator.
                PreferredMigrators = new Dictionary<string, string>()
        {
             { UmbConstants.PropertyEditors.Aliases.Grid, nameof(GridToBlockGridMigrator) },
            { UmbConstants.PropertyEditors.Aliases.NestedContent, nameof(NestedToBlockListMigrator) }
        },

                ReplacementAliases = new Dictionary<string, string>()
                {
                    
                }

                // eveything beneath is optional... 

                //PropertyMigrators = new Dictionary<string, string>()
                //{
                //    // use the NestedToBlockListMigrator For myProperty in the 'MyContentType' contentType
                //    { "myContentType_myProperty", nameof(NestedToBlockListMigrator) }, 

                //    // Convert all properties called myGridProperty to blocklist 
                //    { "myGridProperty", nameof(GridToBlockListMigrator) }
                //},


                // add a list of things we don't want to import 
                //BlockedItems = new Dictionary<string, List<string>>
                //{
                //    { nameof(DataType),
                //        new List<string> {
                //            "Custom.LegacyType", "My.BoxGrid.Things"
                //        }
                //    }
                //},

                //// add a list of properties we are ignoring on all content
                //IgnoredProperties = new List<string>
                //{
                //    "SeoMetaDescription", "SeoToastPopup", "Keywords"
                //},

                //// add things we only want to ignore on certain types

                //IgnoredPropertiesByContentType = new Dictionary<string, List<string>>
                //{
                //    { "HomePage", new List<string>
                //        {
                //            "SiteName", "GoogleAnalyticsCode"
                //        }
                //    }
                //},

                //// change the tabs around a bit if needed/
                //ChangeTabs = new List<TabOptions>
                //{
                //    {
                //        //Rename the Meta Data tab to SEO with the alias of seo
                //        new TabOptions{
                //            OriginalName = "Meta Data",
                //            NewName = "SEO",
                //            Alias = "seo" }
                //    },
                //    {
                //        //Move the contents of the tab Carousel into the Content tab.  If content doesn't exist it will
                //        //be created with the alias "Content"
                //        new TabOptions
                //        {
                //            OriginalName = "Carousel",
                //            NewName = "Content",
                //            Alias = string.Empty
                //        }
                //    },
                //    {
                //        //No new name or alias means delete the tab, so delete the "Cookie Law" tab in this example
                //        new TabOptions
                //        {
                //            OriginalName = "Cookie Law",
                //            NewName = string.Empty,
                //            Alias = string.Empty
                //        }
                //    }
                //}
            };
        }
    }
}

public class SMBlockMigrationPlan : ISyncMigrationPlan
{
    private readonly SyncMigrationHandlerCollection _migrationHandlers;

    public SMBlockMigrationPlan(SyncMigrationHandlerCollection migrationHandlers)
    {
        _migrationHandlers = migrationHandlers;
    }

    public int Order => 202;

    public string Name => "Convert Nested Content to BlockLists and Grid to BlockGrid and Legacy Media to mediaPicker3";

    public string Icon => "icon-brick color-green";

    public string Description => "Convert Nested content and Grid to BlockList and BlockGrid and Legacy Media to mediaPicker3";

    public MigrationOptions Options => new()
    {
        Group = "Convert",
        Source = "uSync/v9",
        Target = $"{uSyncMigrations.MigrationFolder}/{DateTime.Now:yyyyMMdd_HHmmss}",
        Handlers = _migrationHandlers.SelectGroup(8, string.Empty),
        SourceVersion = 8,
        PreferredMigrators = new Dictionary<string, string>
        {
            { UmbConstants.PropertyEditors.Aliases.NestedContent, nameof(NestedToBlockListMigrator) },
            { UmbConstants.PropertyEditors.Aliases.Grid, nameof(GridToBlockGridMigrator) },
            { UmbConstants.PropertyEditors.Aliases.MediaPicker, nameof(Lovell.Web.Extensions.Migrations.Migrators.SMMediaPickerMigrator) },
            { "Umbraco.MediaPicker2", nameof(Lovell.Web.Extensions.Migrations.Migrators.SMMediaPickerMigrator) },
            { UmbConstants.PropertyEditors.Aliases.MultipleMediaPicker, nameof(Lovell.Web.Extensions.Migrations.Migrators.SMMediaPickerMigrator)},
            //{ UmbConstants.PropertyEditors.Aliases.RadioButtonList, nameof(Lovell.Web.Extensions.Migrations.Migrators.SMRadioButtonListMigrator) }
        }
    };
}