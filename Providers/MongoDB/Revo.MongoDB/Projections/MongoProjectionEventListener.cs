using Revo.Core.Commands;
using Revo.Core.Events;
using Revo.Core.Transactions;
using Revo.Domain.Events;
using Revo.Infrastructure.Events.Async;
using Revo.Infrastructure.Projections;
using System.Collections.Generic;

namespace Revo.MongoDB.Projections
{
    public class MongoProjectionEventListener : ProjectionEventListener
    {
        public MongoProjectionEventListener(IMongoProjectionSubSystem projectionSubSystem,
            IUnitOfWorkFactory unitOfWorkFactory,
            CommandContextStack commandContextStack,
            MongoProjectionEventSequencer eventSequencer) :
            base(projectionSubSystem,
                unitOfWorkFactory,
                commandContextStack)
        {
            EventSequencer = eventSequencer;
        }

        public override IAsyncEventSequencer EventSequencer { get; }

        public class MongoProjectionEventSequencer : AsyncEventSequencer<DomainAggregateEvent>
        {
            public readonly string QueueNamePrefix = "MongoProjectionEventSequencer:";

            protected override IEnumerable<EventSequencing> GetEventSequencing(IEventMessage<DomainAggregateEvent> message)
            {
                yield return new EventSequencing
                {
                    SequenceName = QueueNamePrefix + message.Event.AggregateId.ToString(),
                    EventSequenceNumber = message.Metadata.GetStreamSequenceNumber()
                };
            }

            protected override bool ShouldAttemptSynchronousDispatch(IEventMessage<DomainAggregateEvent> message)
            {
                return true;
            }
        }
    }
}
