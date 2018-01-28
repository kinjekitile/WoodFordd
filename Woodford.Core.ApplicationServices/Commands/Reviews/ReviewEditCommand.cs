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
    public class ReviewEditCommand : ICommand {
        public ReviewModel Review { get; set; }
    }

    public class ReviewEditCommandHandler : ICommandHandler<ReviewEditCommand> {
        private IReviewRepository _reviewRepo;

        public ReviewEditCommandHandler(IReviewRepository reviewRepo) {
            _reviewRepo = reviewRepo;
        }

        public void Handle(ReviewEditCommand command) {
            command.Review = _reviewRepo.Update(command.Review);
        }
    }
}
