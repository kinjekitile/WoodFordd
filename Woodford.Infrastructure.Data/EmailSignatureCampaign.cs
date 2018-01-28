//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Woodford.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailSignatureCampaign
    {
        public int Id { get; set; }
        public string CampaignName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int MainContentNarrowFileUploadId { get; set; }
        public int MainContentFileUploadId { get; set; }
        public int FooterContentNarrowFileUploadId { get; set; }
        public int FooterContentFileUploadId { get; set; }
        public int SidePanelContentFileUploadId { get; set; }
        public int UnderlineContentFileUploadId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsLive { get; set; }
        public bool IsDefault { get; set; }
    
        public virtual FileUpload FileUploadFooter { get; set; }
        public virtual FileUpload FileUploadFooterNarrow { get; set; }
        public virtual FileUpload FileUploadMainContent { get; set; }
        public virtual FileUpload FileUploadMainContentNarrow { get; set; }
        public virtual FileUpload FileUploadSidePanel { get; set; }
        public virtual FileUpload FileUploadUnderline { get; set; }
    }
}
