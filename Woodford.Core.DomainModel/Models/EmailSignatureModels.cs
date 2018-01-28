using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {

    public static class SignatureLayout {
        public const int BaseImageWidth = 1375;
        public const int BaseImageHeight = 500;

        public const int MainWidth = BaseImageWidth;
        public const int MainHeight = 346;
        public const int MainStartX = 0;
        public const int MainStartY = 0;

        public const int MainNarrowWidth = 967;
        public const int MainNarrowHeight = MainHeight;
        public const int MainNarrowStartX = MainStartX;
        public const int MainNarrowStartY = MainStartY;


        public const int SideWidth = 408;
        public const int SideHeight = 492;
        public const int SideStartX = MainNarrowWidth;
        public const int SideStartY = 0;


        public const int FooterWidth = MainWidth;
        public const int FooterHeight = 146;
        public const int FooterStartX = 0;
        public const int FooterStartY = MainHeight;

        public const int FooterNarrowWidth = MainNarrowWidth;
        public const int FooterNarrowHeight = FooterHeight;
        public const int FooterNarrowStartX = FooterStartX;
        public const int FooterNarrowStartY = FooterStartY;

        public const int UnderlineWidth = BaseImageWidth;
        public const int UnderlineHeight = 8;
        public const int UnderlineStartX = 0;
        public const int UnderlineStartY = MainHeight + FooterHeight;


    }

    public class EmailSignatureCampaignModel {




        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CampaignName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsLive { get; set; }
        public int MainContentFileUploadId { get; set; }
        public FileUploadModel MainContentFile { get; set; }
        public int MainContentNarrowFileUploadId { get; set; }
        public FileUploadModel MainContentNarrowFile { get; set; }
        public int FooterContentFileUploadId { get; set; }
        public FileUploadModel FooterContentFile { get; set; }

        public int FooterContentNarrowFileUploadId { get; set; }
        public FileUploadModel FooterContentNarrowFile { get; set; }


        public int SidePanelContentFileUploadId { get; set; }
        public FileUploadModel SidePanelContentFile { get; set; }

        public int UnderlineContentFileUploadId { get; set; }
        public FileUploadModel UnderlineContentFile { get; set; }


    }

    public class EmailSignatureCampaignFilterModel {
        public int? Id { get; set; }

        public bool? IsDefault { get; set; }

        public bool? IsLive { get; set; }
    }

    public class EmailSignatureFilterModel {
        public int? Id { get; set; }
    }

    public class EmailSignatureModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string CellContact { get; set; }
        public string FixedContact { get; set; }
        public string Email { get; set; }

        //Used to be database field, changed to read only property that
        //uses existence of senior and director info to determine whether to show the side panel
        public bool ShowSidePanel {
            get {
                bool show = ShowSeniorDetails || ShowDirectorDetails;
                return show;
            }
        }

        public bool ShowSeniorDetails {
            get {
                bool show = !string.IsNullOrEmpty(SeniorName) && (!string.IsNullOrEmpty(SeniorFixedContact) || !string.IsNullOrEmpty(SeniorCellContact));
                return show;
            }
        }

        public bool ShowDirectorDetails {
            get {
                bool show = !string.IsNullOrEmpty(DirectorName) && !string.IsNullOrEmpty(DirectorEmail);
                return show;
            }
        }

        public string SeniorName { get; set; }
        public string SeniorFixedContact { get; set; }
        public string SeniorCellContact { get; set; }

        public string SeniorEmail { get; set; }

        public string DirectorName { get; set; }
        public string DirectorEmail { get; set; }
    }
}
