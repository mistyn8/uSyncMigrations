﻿using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

using uSync.Migrations.Core.Legacy.Grid;
using uSync.Migrations.Migrators.BlockGrid.BlockMigrators;
using uSync.Migrations.Migrators.BlockGrid.Models;

using GridConfiguration = Umbraco.Cms.Core.PropertyEditors.GridConfiguration;

namespace uSync.Migrations.Migrators.BlockGrid.Extensions;
internal static class GridConfigurationExtensions
{
    public static int? GetGridColumns(this GridConfiguration gridConfiguration)
    {
        if (gridConfiguration.Items?.TryGetValue("columns", out var columns) == true)
        {
            return columns.Value<int>();
        }

        return 12;
    }

    public static JToken? GetItemBlock(this GridConfiguration gridConfiguration, string name)
    {
        if (gridConfiguration.Items?.TryGetValue(name, out var block) == true)
        {
            return block;
        }

        return null;
    }

    /// <summary>
    ///  returns all the allowed content type aliases for a given grid editor config block
    /// </summary>
    public static IEnumerable<string> GetAllowedContentTypeAliasesForBlock(this ILegacyGridEditorConfig editorConfig, SyncMigrationContext context, SyncBlockMigratorCollection blockMigrators)
    {
        // mainly a doctypegrid thing, but also for generic text, rtes

        var blockMigrator = blockMigrators.GetMigrator(editorConfig);
        if (blockMigrator == null) return Enumerable.Empty<string>();
        return blockMigrator.GetAllowedContentTypes(editorConfig, context);
    }

    /// <summary>
    ///  Converts a GriddEditorConfig into a BlockGridBlock 
    /// </summary>
    public static IEnumerable<BlockGridConfiguration.BlockGridBlockConfiguration> ConvertToBlockGridBlocks(this ILegacyGridEditorConfig editorConfig, SyncMigrationContext context, SyncBlockMigratorCollection blockMigrators, Guid groupKey)
    {
        foreach (var allowedAlias in editorConfig.GetAllowedContentTypeAliasesForBlock(context, blockMigrators))
        {
            var keys = new List<Guid>();
            if (Regex.IsMatch(allowedAlias, "\\W"))
            {
                var matchingAliases = context.ContentTypes.GetAllAliases().Where(x => Regex.IsMatch(x, allowedAlias)).ToList();
                keys.AddRange(matchingAliases.Select(context.ContentTypes.GetKeyByAlias));
            }
            else
            {
                keys.Add(context.ContentTypes.GetKeyByAlias(allowedAlias));
            }

            foreach (var elementKey in keys)
            {
                var label = editorConfig.GetBlockname();
                if (keys.Count > 0)
                {
                    label = $"{label} ({context.ContentTypes.GetAliasByKey(elementKey)})";
                }
                yield return new BlockGridConfiguration.BlockGridBlockConfiguration
                {
                    Label = label,
                    ContentElementTypeKey = elementKey,
                    GroupKey = groupKey != Guid.Empty ? groupKey.ToString() : null,
                    BackgroundColor = Grid.GridBlocks.Background,
                    IconColor = Grid.GridBlocks.Icon,
                    AllowAtRoot = false,
                };
            }
        }
    }

    /// <summary>
    ///  return the name name for a block based on the editor config. 
    /// </summary>
    /// <param name="editorConfig"></param>
    /// <returns></returns>
    public static string GetBlockname(this ILegacyGridEditorConfig? editorConfig)
    {
        var customLabels = new Dictionary<string, string> { {"rte", "RTE :: {{ (rte | ncRichText).length > 0  ? (rte | ncRichText | umbWordLimit:5 ) : \"no text content\"}}..."},
{"media", "IMAGE :: {{(media[0].mediaKey | mediaItemResolver).name}}"},
{"embed", "EMBED :: {{embed[0].url ? embed[0].url : \"no video\"}}"},
{"plaintext", "PLAIN INFO TEXT :: {{plaintext ? (plaintext | umbWordLimit:5) : \"no text\"}}..."},
{"umbraco_form_picker", "FORM :: {{formGuid? (formGuid | formItemResolver).name : \"no form\"}}"},
{"docType_gridQuote", "QUOTE :: {{summary ? summary.split(\" \").splice(0, 5).join(\" \")  : \"no summary\"}}... : {{author ? author : \"\"}}"} };

        if (customLabels.TryGetValue(editorConfig?.Alias ?? string.Empty, out var customTemplateValue))
        {
            if (!customTemplateValue.IsNullOrWhiteSpace())
            {
                return customTemplateValue;
            }
        }

        if (editorConfig?.Config.TryGetValue("nameTemplate", out var nameTemplateValue) == true)
        {
            return nameTemplateValue as string ?? editorConfig?.Name ?? string.Empty;
        }

        // 
        return editorConfig?.Name ?? string.Empty;
    }
}
