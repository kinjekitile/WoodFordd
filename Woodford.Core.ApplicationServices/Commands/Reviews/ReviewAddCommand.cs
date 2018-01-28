using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReviewAddCommand : ICommand {
        public ReviewModel Review { get; set; }
    }

    public class ReviewAddCommandHandler : ICommandHandler<ReviewAddCommand> {
        private IReviewRepository _reviewRepo;
        public ReviewAddCommandHandler(IReviewRepository reviewRepo) {
            _reviewRepo = reviewRepo;
        }

        public void Handle(ReviewAddCommand command) {
            command.Review = _reviewRepo.Create(command.Review);
        }
    }
}
