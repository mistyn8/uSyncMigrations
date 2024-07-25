using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace www.Extensions.PropertyEditors
{
    // we are just reusing the core multipleStringValueConvertor
    public class TsdMultipleTextStringValueConvertor : MultipleTextStringValueConverter
    {
        public override bool IsConverter(IPublishedPropertyType propertyType)
            => "TsdMultipleTextstring".Equals(propertyType.EditorAlias);
    }
}
