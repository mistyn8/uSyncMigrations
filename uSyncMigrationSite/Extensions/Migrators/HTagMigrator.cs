using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Legacy.Grid;
using uSync.Migrations.Core.Models;
using uSync.Migrations.Migrators.BlockGrid.BlockMigrators;


namespace uSync.Migrations.Migrators;

public class GridHtagBlockMigrator : GridBlockMigratorSimpleBase, ISyncBlockMigrator
{
    public GridHtagBlockMigrator(IShortStringHelper shortStringHelper) : base(shortStringHelper)
    {
    }

    public string[] Aliases => new[] { "htmlHeading" };

    public override string GetEditorAlias(ILegacyGridEditorConfig editor) => "Headline";
}



public class GridEmbedBlockMigrator : GridBlockMigratorSimpleBase, ISyncBlockMigrator
{
    public GridEmbedBlockMigrator(IShortStringHelper shortStringHelper) : base(shortStringHelper)
    {
    }

    public string[] Aliases => new[] { "embed" };

    public override string GetEditorAlias(ILegacyGridEditorConfig editor) => "Video - OEmbed Picker";

    public override Dictionary<string, object> GetPropertyValues(GridValue.GridControl control, SyncMigrationContext context)
    {
        //todo: support - allow picking of multiple items though could you embed multiple items from the legacy grid?
        var propertyValues = new Dictionary<string, object>();

        var list = new List<JToken?>() { control.Value };
        propertyValues.Add("embed", JsonConvert.SerializeObject(list));

        return propertyValues;
    }
}


public class GridUmbracoFormPickerBlockMigrator : GridBlockMigratorSimpleBase, ISyncBlockMigrator
{
    private readonly IContentTypeService _contentTypeService;
    public GridUmbracoFormPickerBlockMigrator(IShortStringHelper shortStringHelper, IContentTypeService contentTypeService) : base(shortStringHelper)
    {
        _contentTypeService = contentTypeService;
    }

    public string[] Aliases => new[] { "umbraco_form_picker" };

    public override string GetEditorAlias(ILegacyGridEditorConfig editor) => "Form Picker";

    public override Dictionary<string, object> GetPropertyValues(GridValue.GridControl control, SyncMigrationContext context)
    {
        var properties = new Dictionary<string, object>();
        if (control.Value == null) return properties;

        var macroObject = JsonConvert.DeserializeObject<MacroObject>(control.Value.ToString());

        if (macroObject == null) return properties;

        if (macroObject.MacroParams == null
            || macroObject.MacroParams.RawPropertyValues == null
            || !macroObject.MacroParams.RawPropertyValues.Any()) return properties;


        var contentType = _contentTypeService.GetAllElementTypes().Where(x => x.Alias.ToLower() == macroObject.MacroEditorAlias.ToLower()).FirstOrDefault();

        foreach (var item in macroObject.MacroParams.RawPropertyValues)
        {
            if (item.Value != null)
            {
                var x = item.Key.ToSafeAlias(_shortStringHelper, true);
                properties.Add(x, item.Value);
            }
        }

        return properties;
    }

    new public IEnumerable<NewContentTypeInfo> AdditionalContentTypes(ILegacyGridEditorConfig editor)
    {
        var alias = this.GetContentTypeAlias(editor);

        return new NewContentTypeInfo(
            alias.ToGuid(),
            alias,
            editor.Name ?? editor.Alias!,
            $"{editor.Icon ?? "icon-book"} color-purple",
            "BlockGrid/Elements")
        {
            Description = $"Converted from Grid {editor.Name} element",
            IsElement = true,
            Properties = new List<NewContentTypeProperty>
            {
                new NewContentTypeProperty(
                    alias: "formGuid",
                    name: "Choose a form",
                    dataTypeAlias: "FormGuid"),
                new NewContentTypeProperty(
                    alias: "formTheme",
                    name: "Theme",
                    dataTypeAlias: "formTheme"),
                 new NewContentTypeProperty(
                    alias: "excludeScripts",
                    name: "Exclude Scripts",
                    dataTypeAlias: "excludeScripts")
            }
        }.AsEnumerableOfOne();
    }    
}

public class GridSeparatorBlockMigrator : GridBlockMigratorSimpleBase, ISyncBlockMigrator
{
    public GridSeparatorBlockMigrator(IShortStringHelper shortStringHelper) : base(shortStringHelper)
    {
    }

    public string[] Aliases => new[] { "separator" };

    public override string GetEditorAlias(ILegacyGridEditorConfig editor) => "Label";


    new public IEnumerable<NewContentTypeInfo> AdditionalContentTypes(ILegacyGridEditorConfig editor)
    {
        var alias = this.GetContentTypeAlias(editor);

        return new NewContentTypeInfo(
            alias.ToGuid(),
            alias,
            editor.Name ?? editor.Alias!,
            $"{editor.Icon ?? "icon-book"} color-purple",
            "BlockGrid/Elements")
        {
            Description = $"Converted from Grid {editor.Name} element",
            IsElement = true,
            Properties = new List<NewContentTypeProperty>
            {
                new NewContentTypeProperty(
                    alias: editor.Alias!,
                    name: "No Settings Available",                    
                    dataTypeAlias: this.GetEditorAlias(editor))                    
            }
        }.AsEnumerableOfOne();
    }
}