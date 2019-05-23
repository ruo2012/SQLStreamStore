namespace SqlStreamStore
{
    using System.Threading.Tasks;
    using Shouldly;
    using SqlStreamStore.Streams;
    using Xunit;
    using static Streams.Deleted;

    partial class AcceptanceTests
    {
        [Fact]
        public async Task When_deletion_tracking_is_disabled_deleted_message_should_not_be_tracked()
        {
            _fixture.DisableDeletionTracking = true;

            var messages = CreateNewStreamMessages(1);

            await _fixture.Store.AppendToStream("stream", ExpectedVersion.NoStream, messages);

            await _fixture.Store.DeleteMessage("stream", messages[0].MessageId);

            var page = await store.ReadStreamBackwards(DeletedStreamId, StreamVersion.End, 1);

            page.Messages.Length.ShouldBe(0);
        }

        [Fact]
        public async Task When_deletion_tracking_is_disabled_deleted_stream_should_not_be_tracked()
        {
            _fixture.DisableDeletionTracking = true;
            
            var messages = CreateNewStreamMessages(1);

            await fixture.Store.AppendToStream("stream", ExpectedVersion.NoStream, messages);

            await fixture.Store.DeleteStream("stream");

            var page = await store.ReadStreamBackwards(DeletedStreamId, StreamVersion.End, 1);

            page.Messages.Length.ShouldBe(0);
        }
    }
}