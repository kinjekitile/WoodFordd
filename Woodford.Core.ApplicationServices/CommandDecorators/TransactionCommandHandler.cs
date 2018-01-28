using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.CommandDecorators
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> decorated;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            this.decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions)) {
                
                this.decorated.Handle(command);

                scope.Complete();
            }
        }
    }
}