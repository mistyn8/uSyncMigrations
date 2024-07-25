using Umbraco.Cms.Core.Composing;
using Umbraco.Extensions;

namespace uSync.Migrations.Migrators.BlockGrid.SettingsMigrators;

public interface IGridSettingsViewMigrator : IDiscoverable
{
    string ViewKey { get; }
    string NewDataTypeAlias { get; }
    public object ConvertContentString(string value);
}

public class GridSettingsViewMigratorCollectionBuilder
    : LazyCollectionBuilderBase<GridSettingsViewMigratorCollectionBuilder, GridSettingsViewMigratorCollection, IGridSettingsViewMigrator>
{
    protected override GridSettingsViewMigratorCollectionBuilder This => this;
}

public class GridSettingsViewMigratorCollection
    : BuilderCollectionBase<IGridSettingsViewMigrator>
{
    public GridSettingsViewMigratorCollection(Func<IEnumerable<IGridSettingsViewMigrator>> items) : base(items)
    { }

    public IGridSettingsViewMigrator? GetMigrator(string? key, string? viewKey)
    {
        if (viewKey == null) return null;
        
        //get by key and view
        if (this.FirstOrDefault(x => x.ViewKey.InvariantEquals($"{viewKey}-{key}")) is IGridSettingsViewMigrator vmg)
        {
            return vmg;
        }

        return this.FirstOrDefault(x => x.ViewKey.InvariantEquals(viewKey));
    }
}