using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class FileUploadModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] FileContents { get; set; }
        public string FileExtension { get; set; }
        public DateTime DateUploaded { get; set; }
        public string FileContext { get; set; }
        public string MimeType { get; set; }
        public bool IsStandalone { get; set; }
    }

    //public class ListOfFileUploadsModel {
    //    public List<FileUploadModel> Files { get; set; }
    //    public ListPaginationModel Pagination { get; set; }
    //}

    public class FileUploadFilterModel {
        public string FileContext { get; set; }      
        public bool? IsStandalone { get; set; }  
    }
}
