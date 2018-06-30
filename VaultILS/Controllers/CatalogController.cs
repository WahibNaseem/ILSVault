using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VaultILS.Models.Catalog;

namespace VaultILS.Controllers
{
    public class CatalogController:Controller
    {
        private ILibraryAsset _assets;
        public CatalogController(ILibraryAsset assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id),
                    Title = result.Title,
                    Type = _assets.GetType(id:result.Id)
                });

            var model = new AssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetById(id);

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                Type = _assets.GetType(asset.Id),
                ISBN = _assets.GetIsbn(asset.Id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                AuthorOrDirector = _assets.GetAuthorOrDirector(asset.Id),               
                DeweyCallNumber = _assets.GetDeweyIndex(asset.Id),
            };

            return View(model);
        }
    }
}
