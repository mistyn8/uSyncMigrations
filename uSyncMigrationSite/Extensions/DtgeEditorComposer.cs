using Our.Umbraco.DocTypeGridEditor;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Manifest;

namespace Lovell.Web.Extensions.Migrations
{
    public class DtgeEditorComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Remove<DocTypeGridEditorManifestFilter>();
            builder.ManifestFilters().Append<DtgeEditorManifestFilter>();
        }
    }

    public class DtgeEditorManifestFilter : IManifestFilter
    {
        public void Filter(List<PackageManifest> manifests)
        {
            manifests.Add(new PackageManifest()
            {
                AllowPackageTelemetry = true,
                PackageName = "Doc Type Grid Editor",
                Scripts = new[]
                {
                    "/App_Plugins/DocTypeGridEditor/Js/doctypegrideditor.resources.js",
                    "/App_Plugins/DocTypeGridEditor/Js/doctypegrideditor.services.js",
                    "/App_Plugins/DocTypeGridEditor/Js/doctypegrideditor.controllers.js",
                    "/App_Plugins/DocTypeGridEditor/Js/doctypegrideditor.directives.js"
                },
                Stylesheets = new[]
                {
                    "/App_Plugins/DocTypeGridEditor/Css/doctypegrideditor.css"
                }

            });
        }
    }
}
