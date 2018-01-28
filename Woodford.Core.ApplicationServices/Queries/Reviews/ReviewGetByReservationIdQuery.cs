using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries.Reviews {
    public class ReviewGetByReservationIdQuery : IQuery<ReviewModel> {
        public int ReservationId { get; set; }
    }

    public class ReviewGetByReservationIdQueryHandler : IQueryHandler<ReviewGetByReservationIdQuery, ReviewModel> {
        private readonly IReviewRepository _reviewRepo;
        public ReviewGetByReservationIdQueryHandler(IReviewRepository reviewRepo) {
            _reviewRepo = reviewRepo;
        }
        public ReviewModel Process(ReviewGetByReservationIdQuery query) {

            var review = _reviewRepo.Get(new ReviewFilterModel { ReservationId = query.ReservationId }, null).SingleOrDefault();

            if (review == null) {
                throw new Exception("Review could not be found");
            }

           
            return review;
        }
    }
}
