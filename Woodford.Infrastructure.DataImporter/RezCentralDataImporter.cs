using Excel.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;
using Woodford.Infrastructure.DataImporter.Models;

namespace Woodford.Infrastructure.DataImporter {
    public class RezCentralDataImporter : IDataImportService {
        private readonly ISettingService _settings;
        private readonly IBranchService _branchService;
        private readonly IBookingHistoryService _bookingHistoryService;
        private readonly IVehicleGroupService _vehicleGroupService;
        private readonly IUserService _userService;
        private readonly IAuthenticate _authenticate;
        private readonly INotify _notify;
        private readonly ILoyaltyService _loyaltyService;
        private readonly IReservationService _reservationService;


        public RezCentralDataImporter(ISettingService settings, IBranchService branchService, IBookingHistoryService bookingHistoryService, IVehicleGroupService vehicleGroupService, IUserService userService, IAuthenticate authenticate, INotify notify, ILoyaltyService loyaltyService, IReservationService reservationService) {
            _settings = settings;
            _branchService = branchService;
            _bookingHistoryService = bookingHistoryService;
            _vehicleGroupService = vehicleGroupService;
            _userService = userService;
            _authenticate = authenticate;
            _notify = notify;
            _loyaltyService = loyaltyService;
            _reservationService = reservationService;
        }

        public DataImportResultModel ImportAdvanceUsers(string fileName) {
            LoyaltyTierLevel level = LoyaltyTierLevel.Green;

            if (fileName.Contains("silver")) {
                level = LoyaltyTierLevel.Silver;
            }

            if (fileName.Contains("gold")) {
                level = LoyaltyTierLevel.Gold;
            }

            var users = _userService.Get(new UserFilterModel { }, null).Items;

            DataImportResultModel result = new DataImportResultModel();

            string fileFolder = _settings.GetValue<string>(Setting.DataExportLocalPath);
            List<LoyaltyImportModel> records = new List<LoyaltyImportModel>();

            string filePath = Path.Combine(fileFolder, fileName);
            using (ExcelDataReaderHelper excelHelper = new ExcelDataReaderHelper(filePath)) {
                excelHelper.SuppressExcelDataReaderHelperException = true;
                records = excelHelper.GetRange<LoyaltyImportModel>(0, 0, 1, numberOfColumns: 15).ToList();
            }



            foreach (var item in records) {

                var user = users.SingleOrDefault(x => x.Email.ToLower() == item.Email.ToLower());



                
                if (user != null) {
                    _userService.UpdateLoyaltyTier(user.Id, 2);
                }

            }

            return result;
        }

        public DataImportResultModel ImportExternalBookings(string fileName) {
            string filePath = _settings.GetValue<string>(Setting.DataExportLocalPath);
            var branches = _branchService.Get(new BranchFilterModel { }, null).Items;
            var vehicleGroups = _vehicleGroupService.Get(new VehicleGroupFilterModel { }, null).Items;
            var users = _userService.GetAll(null).Items;
            var loyaltyTiers = _loyaltyService.GetAll().ToList();

            List<RezCentralBookingModel> records = new List<RezCentralBookingModel>();

            using (ExcelDataReaderHelper excelHelper = new ExcelDataReaderHelper(filePath + fileName)) {
                excelHelper.SuppressExcelDataReaderHelperException = true;
                excelHelper.InvalidIdentifierCharacterReplacement = "";
                records = excelHelper.GetRange<RezCentralBookingModel>(0, 0, 1, numberOfColumns: 23).ToList();
            }

            List<BookingHistoryModel> bookingHistory = new List<BookingHistoryModel>();



            foreach (var record in records) {
                BookingHistoryModel b = new BookingHistoryModel();

                b.ExternalId = record.RANumber;
                b.AlternateId = record.AlternateID;

                if (!string.IsNullOrEmpty(record.EMail)) {

                    
                    UserModel user = mapUser(record, users);
                    if (user != null) {
                        b.UserId = user.Id;
                        if (user.IsLoyaltyMember) {
                            var tier = loyaltyTiers.Single(x => x.TierLevel == (LoyaltyTierLevel)user.LoyaltyTierId);

                            b.LoyaltyPointsEarned = decimal.Round(Convert.ToDecimal(record.TotalTM) * tier.PointsEarnedPerRandSpent, 2);
                            b.TotalForLoyaltyAward = Convert.ToDecimal(record.TotalTM);


                            b.Email = record.EMail;

                            b.PickupBranchId = mapBranch(record.PickupLocation, branches);
                            b.DropoffBranchId = mapBranch(record.ReturnLocation, branches);
                            
                            b.PickupDate = DateTime.FromOADate(Convert.ToDouble(record.PickupDate));
                            b.DropOffDate = DateTime.FromOADate(Convert.ToDouble(record.DropOffDate));


                            b.RentalDays = Convert.ToInt32(record.RentalDays);
                            b.FreeKms = Convert.ToInt32(record.FreeKms);
                            b.KmsRate = Convert.ToDecimal(record.KilometersRate);
                            b.TotalForLoyaltyAward = Convert.ToDecimal(record.TotalTM);

                            b.TotalAmount = Convert.ToDecimal(record.TotalBillTotalRevenue);


                            bookingHistory.Add(b);

                            
                        }
                    }
                }


            }

            DateTime start = bookingHistory.OrderBy(x => x.PickupDate).Select(x => x.PickupDate).First();
            DateTime end = bookingHistory.OrderBy(x => x.PickupDate).Select(x => x.PickupDate).Last();

            var existingHistoryIds = _bookingHistoryService.Get(new BookingHistoryFilterModel { StartDate = start, EndDate = end }, null).Items.Select(x => x.ExternalId).ToList();

            //We use the collection below to see if a reservation took advantage of loyalty points, if so the reservation does not earn new points.
            var existingReservations = _reservationService.Get(new ReservationFilterModel { DateFilterType = ReservationDateFilterTypes.PickupDate, DateSearchStart = start, DateSearchEnd = end, ReservationState = ReservationState.Completed, IsCompletedInvoice = true }, null).Items.ToList();

            List<BookingHistoryModel> toAdd = new List<BookingHistoryModel>();
            foreach (var item in bookingHistory) {
                bool canEarnPoints = true;

                //Assume that booking can earn points and must send email
                item.SendLoyaltyPointsEarnedEmail = true;

                if (!existingHistoryIds.Contains(item.ExternalId)) {
                    if (item.ExternalId.StartsWith("ORD")) {
                        //Website booking, check if it used loyalty points
                        int reservationId = Convert.ToInt32(item.ExternalId.Replace("ORD-", ""));
                        var reservation = existingReservations.SingleOrDefault(x => x.Id == reservationId);
                        
                        if (reservation != null) {
                            if (reservation.LoyaltyPointsSpent.HasValue) {
                                if (reservation.LoyaltyPointsSpent.Value > 0) {
                                    canEarnPoints = false;
                                }
                            }
                        }

                        if (!canEarnPoints) {
                            //Points were spent on the booking, therefore no points can be earned.
                            item.LoyaltyPointsEarned = 0m;
                            item.SendLoyaltyPointsEarnedEmail = false;
                        } else {
                            item.SendLoyaltyPointsEarnedEmail = true;
                        }

                    }
                    
                    toAdd.Add(item);

                }
            }

            _bookingHistoryService.CreateBatch(toAdd);
            

            return new DataImportResultModel { };
        }

        private int mapVehicleGroup(string groupName, List<VehicleGroupModel> groups) {
            var group = groups.SingleOrDefault(x => x.Title.ToUpper() == groupName.ToUpper());
            if (group == null) {
                return 0;
            } else {
                return group.Id;
            }
        }


        private UserModel mapUser(RezCentralBookingModel model, List<UserModel> users) {

            if (string.IsNullOrEmpty(model.AlternateID) && string.IsNullOrEmpty(model.EMail)) {
                return null;
            }
            UserModel user;

            if (!string.IsNullOrEmpty(model.EMail)) {
                user = users.FirstOrDefault(x => x.Email.ToUpper() == model.EMail.ToUpper());
                if (user != null) {
                    return user;
                }
            }

            if (!string.IsNullOrEmpty(model.AlternateID)) {
                user = users.FirstOrDefault(x => x.IdNumber.ToUpper() == model.AlternateID.ToUpper());
                if (user != null) {
                    return user;
                }
            }



            return null;


        }

        private int? mapUser(string email, List<UserModel> users) {
            var user = users.SingleOrDefault(x => x.Email.ToUpper() == email.ToUpper());
            if (user == null) {
                return null;
            } else {
                return user.Id;
            }
        }
        private int mapBranch(string branch, List<BranchModel> branches) {


            switch (branch.Trim().ToUpper()) {
                case "TAMBO":
                    return branches.Where(x => x.Title.Contains("Tambo")).First().Id;
                case "KSIA":
                    return branches.Where(x => x.Title.Contains("King")).First().Id;
                case "PE":
                    return branches.Where(x => x.Title.Contains("Port")).First().Id;
                case "ORD":
                    return branches.Where(x => x.Title.Contains("Downtown")).First().Id;
                case "CAPE":
                    return branches.Where(x => x.Title.Contains("Cape")).First().Id;
                default:
                    return 0;

            }
        }

        public static string GeneratePassword(int length) {

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--) {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }

}


//if (user == null) {
//    string autoPassword = GeneratePassword(6);
//    UserModel newUser = new UserModel();
//    newUser.FirstName = item.FirstName;
//    newUser.LastName = item.LastName;
//    newUser.Email = item.Email;

//    newUser.IsLoyaltyMember = true;
//    newUser.MobileNumber = item.CellPhone;
//    newUser.LoyaltyTierId = (int)level;
//    newUser.DateCreated = DateTime.Today;
//    newUser.IdNumber = "";

//    if (!string.IsNullOrEmpty(item.FRP)) {
//        newUser.ExistingLoyaltyNumber = item.FRP;
//        newUser.HasExistingLoyaltyNumber = true;
//    }
//    _authenticate.CreateUser(newUser, autoPassword);
//    newUser = _userService.GetByUsername(newUser.Email);

//    newUser.LoyaltyPeriodStart = DateTime.Today;
//    newUser.LoyaltyPeriodEnd = newUser.LoyaltyPeriodStart.Value.AddMonths(3);

//    _userService.UpdateLoyaltyPeriod(newUser.Id, newUser.LoyaltyPeriodStart.Value, newUser.LoyaltyPeriodEnd.Value);




//    UserRegistrationNotifcationModel emailModel = new UserRegistrationNotifcationModel();

//    emailModel.User = newUser;
//    emailModel.AutoGeneratedPassword = autoPassword;
//    emailModel.IsAdminGenerated = true;

//    _notify.SendNotifyAccountCreated(emailModel, Setting.Public_Website_Domain);
//    _notify.SendNotifyLoyaltyAccountCreatedByAdmin(emailModel, Setting.Public_Website_Domain);

//} else {
//    //Exists, check if loyalty memember
//    if (!user.IsLoyaltyMember) {

//        _userService.EnrollLoyalty(user.Id);

//        if (!string.IsNullOrEmpty(item.FRP)) {
//            user.ExistingLoyaltyNumber = item.FRP;
//            user.HasExistingLoyaltyNumber = true;
//        }

//        _userService.UpdateUser(user);

//        _userService.UpdateLoyaltyTier(user.Id, (int)level);

//        user = _userService.GetById(user.Id);

//        UserRegistrationNotifcationModel emailModel = new UserRegistrationNotifcationModel();
//        emailModel.User = user;

//        emailModel.HasAutoGeneratedPassword = false;
//        emailModel.IsAdminGenerated = false;

//        _notify.SendNotifyLoyaltyAccountCreatedByAdmin(emailModel, Setting.Public_Website_Domain);
//    }
//}