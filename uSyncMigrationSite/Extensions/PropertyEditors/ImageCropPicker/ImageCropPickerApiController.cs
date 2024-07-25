using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Extensions;

namespace Our.Umbraco.ImageCropPicker.Web.Controllers
{
    [PluginController("ImageCropPicker")]
    public class ImageCropPickerApiController : UmbracoAuthorizedJsonController
    {
        private readonly IDataTypeService _dataTypeService;

        public ImageCropPickerApiController(IDataTypeService dataTypeService)
            => _dataTypeService = dataTypeService;

        public IEnumerable<IDataType> GetDataTypes()
            => _dataTypeService.GetAll()
                .Where(dataType => dataType.EditorAlias.InvariantEquals(Constants.PropertyEditors.Aliases.ImageCropper))
                .ToArray();

        [HttpGet]
        public IEnumerable<ImageCropperConfiguration.Crop> GetImageCropsDataForDataType(int id)
        {
            var dataType = _dataTypeService.GetDataType(id);

            return dataType?.Configuration == null
                ? Enumerable.Empty<ImageCropperConfiguration.Crop>()
                : (dataType.Configuration as ImageCropperConfiguration).Crops
                    ?? Enumerable.Empty<ImageCropperConfiguration.Crop>();
        }
    }
}