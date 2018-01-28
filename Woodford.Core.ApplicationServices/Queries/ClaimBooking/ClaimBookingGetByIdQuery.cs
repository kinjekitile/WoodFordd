using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class ClaimBookingGetByIdQuery : IQuery<BookingClaimModel> {
        public int Id { get; set; }
    }

    public class ClaimBookingGetByIdQueryHandler : IQueryHandler<ClaimBookingGetByIdQuery, BookingClaimModel> {
        private readonly IClaimBookingRepository _claimBookingRepo;
        private readonly IBranchRepository _branchRepo;
        private readonly IUserService _userService;
        public ClaimBookingGetByIdQueryHandler(IClaimBookingRepository claimBookingRepo, IBranchRepository branchRepo, IUserService userService) {
            _claimBookingRepo = claimBookingRepo;
            _branchRepo = branchRepo;
            _userService = userService;
        }
        public BookingClaimModel Process(ClaimBookingGetByIdQuery query) {
            var claim = _claimBookingRepo.GetById(query.Id);
            claim.PickupBranch = _branchRepo.GetById(claim.BookingPickupBranchId);
            claim.DropoffBranch = _branchRepo.GetById(claim.BookingDropofBranchId);
            return claim;
        }

    }
}
