using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands.Users {
    public class UserModifyReservationDatesCommand : ICommand {
        public int ReservationId { get; set; }
        public DateTime PickupDate { get; set; }

        public DateTime DropOffDate { get; set; }
        public int DropOffTime { get; set; }
        public int PickupTime { get; set; }

        public int PickupLocationId { get; set; }

        public int DropOffLocationId { get; set; }
        public bool Success { get; set; }

        public List<SearchResultItemRateModel> AlternateRates { get; set; }

        public List<SearchResultItemModel> AlternateResults { get; set; }

        public int VehicleId { get; set; }

        public bool IsDateUpdate { get; set; }

        public bool IsVehicleUpdate { get; set; }

        public bool IsLocationUpdate { get; set; }
        public UserModifyReservationDatesCommand() {
            AlternateRates = new List<SearchResultItemRateModel>();
            AlternateResults = new List<SearchResultItemModel>();
        }


    }

    public class UserModifyReservationDatesCommandHandler : ICommandHandler<UserModifyReservationDatesCommand> {
        private IReservationService _reservationService;
        private IUserService _userService;
        private ISearchService _searchService;


        public UserModifyReservationDatesCommandHandler(IReservationService reservationService, IUserService userService, ISearchService searchService) {
            _reservationService = reservationService;
            _userService = userService;
            _searchService = searchService;
        }

        public void Handle(UserModifyReservationDatesCommand command) {
            var reservation = _reservationService.GetById(command.ReservationId);
            
            //Removed owner check, instead we let users use id and pin combo to modify, this allows guests to modify as well
            //var currentUser = _userService.GetCurrentUser();
            //if (currentUser.Id != reservation.UserId) {
            //    throw new Exception("User does not own reservation: " + reservation.Id);
            //}

            //Perform search with new criteria and ensure that rate id is still valid, check modifiers

            SearchCriteriaModel criteria = new SearchCriteriaModel();
            criteria = applyReservationDefaults(criteria, reservation);

            if (command.IsDateUpdate) {
                criteria.PickupDate = command.PickupDate;
                criteria.DropOffDate = command.DropOffDate;
                criteria.DropOffTime = command.DropOffTime;
                criteria.PickupTime = command.PickupTime;
            }
            if (command.IsVehicleUpdate) {
                
            }
            
            var results = _searchService.Search(criteria);

            bool rateStillValid = false;


            foreach (var result in results.Items) {
                if (result.Vehicle.Id == reservation.VehicleId) {
                    foreach (var rate in result.Rates) {
                        if (rate.RateId == reservation.RateId) {
                            rateStillValid = true;
                            break;
                        }
                    }
                    break;
                }
                
            }

            if (rateStillValid) {
                if (command.IsDateUpdate) {
                    reservation.PickupDate = command.PickupDate;
                    reservation.DropOffDate = command.DropOffDate;
                }
                if(command.IsVehicleUpdate) {
                    reservation.VehicleId = command.VehicleId;
                }
                reservation.HasBeenModified = true;
                reservation.ModifiedDate = DateTime.Now;
                _reservationService.Update(reservation);
                command.Success = true;
            } else {
                if (command.IsDateUpdate) {
                    command.AlternateResults.AddRange(results.Items);
                    foreach (var result in results.Items) {
                        //Vehicle has not changed - display only rates that match the vehicle id
                        if (result.Vehicle.Id == reservation.VehicleId) {
                            command.AlternateRates.AddRange(result.Rates);
                            break;
                        }
                       
                    }
                }
            }

            
        }

        private SearchCriteriaModel applyReservationDefaults(SearchCriteriaModel criteria, ReservationModel reservation) {
            criteria.UserId = reservation.UserId;
            criteria.PickUpLocationId = reservation.PickupBranchId;
            criteria.DropOffLocationId = reservation.DropOffBranchId;
            return criteria;
        }
    }
}
