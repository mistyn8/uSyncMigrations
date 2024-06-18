using Our.Umbraco.GMaps.Models;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using uSync.Migrations.Core.Context;
using uSync.Migrations.Core.Migrators;
using uSync.Migrations.Core.Migrators.Models;

namespace Lovell.Web.Extensions.Migrations.Migrators
{
    [SyncMigrator("Our.Umbraco.GMaps")]
    [SyncMigratorVersion(8)]
    public class OurUmbracoGMapsMigrator : SyncPropertyMigratorBase, ISyncPropertyMigrator
    {
        public override string GetEditorAlias(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        => "Our.Umbraco.GMaps";

        public override string? GetContentValue(SyncMigrationContentProperty contentProperty, SyncMigrationContext context)
        {
            var model = new Map();
            if (contentProperty.Value is string configString)
            {
                // reused from gmaps propertyValueConvertor https://github.com/ArnoldV/Our.Umbraco.GMaps/tree/develop/Our.Umbraco.GMaps.Core/PropertyValueConverter
                // whilst we can use the convertor whilst we are migrating might as well bring up to parity...

                // Handle pre v2.0.0 data (Removes the prefix 'google.maps.maptypeid.')
                string jsonString = configString.Replace("google.maps.maptypeid.", string.Empty, StringComparison.InvariantCultureIgnoreCase);

                bool legacyData = jsonString.Contains("latlng", StringComparison.CurrentCultureIgnoreCase);
                if (legacyData)
                {
                    var interm = Newtonsoft.Json.JsonConvert.DeserializeObject<LegacyMap>(jsonString);
                    model = new Map
                    {
                        Address = interm.Address,
                        MapConfig = interm.MapConfig
                    };

                    // Map the LatLng property.
                    model.Address.Coordinates = Location.Parse(interm.Address.LatLng);

                    // Map the center if we have one otherwise it's the address.
                    model.MapConfig.CenterCoordinates = interm.MapConfig.MapCenter.IsNullOrWhiteSpace() ? model.Address.Coordinates : Location.Parse(interm.MapConfig.MapCenter);

                    if (model.MapConfig.Zoom == 0)
                    {
                        model.MapConfig.Zoom = string.IsNullOrEmpty(interm.MapConfig.Zoom) ? 17 : Convert.ToInt32(interm.MapConfig.Zoom);
                    }

                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
            }

            return base.GetContentValue(contentProperty, context);
        }

        public override object? GetConfigValues(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        {
            if (dataTypeProperty.ConfigAsString is string configString)
            {
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Our.Umbraco.GMaps.Core.Models.Configuration.Config>(configString);

                return config;
            }
            return base.GetConfigValues(dataTypeProperty, context);
        }
        //public override object GetConfigValues(SyncMigrationDataTypeProperty dataTypeProperty, SyncMigrationContext context)
        //{
        //    var model = new Map();
        //    if (dataTypeProperty.ConfigAsString is string configString)
        //    {
        //        // reused from gmaps propertyValueConvertor https://github.com/ArnoldV/Our.Umbraco.GMaps/tree/develop/Our.Umbraco.GMaps.Core/PropertyValueConverter
        //        // whilst we can use the convertor whilst we are migrating might as well bring up to parity...

        //        // Handle pre v2.0.0 data (Removes the prefix 'google.maps.maptypeid.')
        //        var jsonString = configString.Replace("google.maps.maptypeid.", string.Empty, StringComparison.InvariantCultureIgnoreCase);

        //        bool legacyData = jsonString.Contains("latlng", StringComparison.CurrentCultureIgnoreCase);
        //        if (legacyData)
        //        {
        //            var intermediate = JsonSerializer.Deserialize<LegacyMap>(jsonString);
        //            model = new Map
        //            {
        //                Address = intermediate.Address,
        //                MapConfig = intermediate.MapConfig
        //            };

        //            // Map the LatLng property.
        //            model.Address.Coordinates = Location.Parse(intermediate.Address.LatLng);
        //            model.MapConfig.CenterCoordinates = Location.Parse(intermediate.MapConfig.MapCenter);
        //            if (model.MapConfig.Zoom == 0)
        //            {
        //                model.MapConfig.Zoom = string.IsNullOrEmpty(intermediate.MapConfig.Zoom) ? 17 : Convert.ToInt32(intermediate.MapConfig.Zoom);
        //            }
        //        }
        //    }

        //    return model!;
        //}
    }

    internal class LegacyMap
    {
        [DataMember(Name = "address")]
        [Newtonsoft.Json.JsonProperty("address")]
        [JsonPropertyName("address")]
        internal LegacyAddress Address { get; set; } = new LegacyAddress();

        [DataMember(Name = "mapconfig")]
        [Newtonsoft.Json.JsonProperty("mapconfig")]
        [JsonPropertyName("mapconfig")]
        internal LegacyMapConfig MapConfig { get; set; } = new LegacyMapConfig();

    }

    internal class LegacyAddress : Address
    {
        [DataMember(Name = "latlng")]
        [Newtonsoft.Json.JsonProperty("latlng")]
        [JsonPropertyName("latlng")]
        public string LatLng { get; set; } = string.Empty;
    }

    internal class LegacyMapConfig : MapConfig
    {
        [DataMember(Name = "mapcenter")]
        [Newtonsoft.Json.JsonProperty("mapcenter")]
        [JsonPropertyName("mapcenter")]
        public string MapCenter { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public new string Zoom { get; set; }

        [DataMember(Name = "zoom")]
        [Newtonsoft.Json.JsonProperty("zoom")]
        [JsonPropertyName("zoom")]
        public object _value
        {
            get
            {
                if (int.TryParse(Zoom, out var intValue)) return intValue;
                return this.Zoom;
            }
            set { this.Zoom = value?.ToString() ?? string.Empty; }
        }
    }
}


