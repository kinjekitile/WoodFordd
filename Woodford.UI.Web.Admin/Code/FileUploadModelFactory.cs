using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Code {
    public static class FileUploadModelFactory {

        public static FileUploadModel CreateInstance(VehicleManufacturerViewModel model) {

            FileUploadModel res = null;

            if (model.ManufacturerImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.ManufacturerImage);

                res = new FileUploadModel {
                    Title = Utilities.GenerateSlug(model.Manufacturer.Title),
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.ManufacturerImage.FileName).ToLower(),
                    FileContext = "VehicleManufacturer",
                    IsStandalone = false
                };
            }
            return res;
        }

        public static FileUploadModel CreateInstance(VehicleGroupViewModel model) {

            FileUploadModel res = null;

            if (model.VehicleGroupImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.VehicleGroupImage);

                res = new FileUploadModel {
                    Title = Utilities.GenerateSlug(model.VehicleGroup.Title),
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.VehicleGroupImage.FileName).ToLower(),
                    FileContext = "VehicleGroups",
                    IsStandalone = false
                };
            }
            return res;
        }

        public static FileUploadModel CreateInstance(HerospaceItemViewModel model) {

            FileUploadModel res = null;
            if (model.HerospaceImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.HerospaceImage);

                res = new FileUploadModel {
                    Title = model.Item.Title,
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.HerospaceImage.FileName).ToLower(),
                    FileContext = "Herospace",
                    IsStandalone = false
                };
            }
            return res;
        }

        public static FileUploadModel CreateInstance(BranchViewModel model) {

            FileUploadModel res = null;

            if (model.BranchImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.BranchImage);

                res = new FileUploadModel {
                    Title = Utilities.GenerateSlug(model.Branch.Title),
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.BranchImage.FileName).ToLower(),
                    FileContext = "Branches",
                    IsStandalone = false
                };
            }
            return res;
        }

        public static FileUploadModel CreateInstance(NewsViewModel model) {

            FileUploadModel res = null;

            if (model.NewsImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.NewsImage);

                res = new FileUploadModel {
                    Title = Utilities.GenerateSlug(model.News.Headline),
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.NewsImage.FileName).ToLower(),
                    FileContext = "News",
                    IsStandalone = false
                };
            }
            return res;
        }


        public static FileUploadModel CreateInstance(HttpPostedFileBase model, string filename, string context) {

            FileUploadModel res = null;

           
                if (model != null) {
                //byte[] fileData = null;
                //using (var binaryReader = new BinaryReader(model.InputStream)) {
                //    fileData = binaryReader.ReadBytes(model.ContentLength);
                //}
                byte[] fileContent2 = Utilities.ConvertPostedFileContentsToByteArray(model);
                    
                    res = new FileUploadModel {
                        Title = Utilities.GenerateSlug(filename),
                        FileContents = fileContent2,
                        FileExtension = Path.GetExtension(model.FileName).ToLower(),
                        FileContext = context,
                        IsStandalone = false
                    };
                }
          

            return res;
        }

        public static FileUploadModel CreateInstance(VehicleViewModel model, bool isInteriorImage) {

            FileUploadModel res = null;

            if (isInteriorImage) {
                if (model.VehicleImage2 != null) {
                    byte[] fileContent2 = Utilities.ConvertPostedFileContentsToByteArray(model.VehicleImage2);
                    res = new FileUploadModel {
                        Title = Utilities.GenerateSlug(model.Vehicle.Title) + "-interior",
                        FileContents = fileContent2,
                        FileExtension = Path.GetExtension(model.VehicleImage2.FileName).ToLower(),
                        FileContext = "Vehicles",
                        IsStandalone = false
                    };
                }
            } else {
                if (model.VehicleImage != null) {
                    byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.VehicleImage);
                    res = new FileUploadModel {
                        Title = Utilities.GenerateSlug(model.Vehicle.Title),
                        FileContents = fileContent,
                        FileExtension = Path.GetExtension(model.VehicleImage.FileName).ToLower(),
                        FileContext = "Vehicles",
                        IsStandalone = false
                    };

                }
                
            }

            return res;
        }

        public static FileUploadModel CreateInstance(FileUploadViewModel model) {

            FileUploadModel res = null;

            if (model.Upload != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.Upload);

                res = new FileUploadModel {
                    Title = model.File.Title,
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.Upload.FileName).ToLower(),
                    FileContext = "",
                    IsStandalone = true
                };
            } else {
                res = new FileUploadModel {
                    Title = model.File.Title,
                    FileContext = "",
                    IsStandalone = true
                };
            }
            return res;
        }

        public static FileUploadModel CreateInstance(CampaignViewModel model) {

            FileUploadModel res = null;

            if (model.CampaignImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.CampaignImage);

                res = new FileUploadModel {
                    Title = Utilities.GenerateSlug(model.Campaign.Title),
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.CampaignImage.FileName).ToLower(),
                    FileContext = "Campaigns",
                    IsStandalone = false
                };
            }
            return res;
        }

        public static FileUploadModel CreateInstanceSearchResult(CampaignViewModel model) {

            FileUploadModel res = null;

            if (model.CampaignSearchResultIconImage != null) {
                byte[] fileContent = Utilities.ConvertPostedFileContentsToByteArray(model.CampaignSearchResultIconImage);

                res = new FileUploadModel {
                    Title = Utilities.GenerateSlug(model.Campaign.Title),
                    FileContents = fileContent,
                    FileExtension = Path.GetExtension(model.CampaignSearchResultIconImage.FileName).ToLower(),
                    FileContext = "Campaigns Search Result Icon",
                    IsStandalone = false
                };
            }
            return res;
        }

    }
}
