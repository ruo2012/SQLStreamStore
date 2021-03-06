namespace SqlStreamStore
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using SqlStreamStore.Imports.Ensure.That;
    using SqlStreamStore.Internal.HoneyBearHalClient;
    using SqlStreamStore.Internal.HoneyBearHalClient.Models;
    using SqlStreamStore.Streams;

    partial class HttpClientSqlStreamStore
    {
        public async Task DeleteStream(
            StreamId streamId,
            int expectedVersion = ExpectedVersion.Any,
            CancellationToken cancellationToken = default)
        {
            Ensure.That(expectedVersion, nameof(expectedVersion)).IsGte(ExpectedVersion.Any);

            GuardAgainstDisposed();

            var client = CreateClient(new Resource
            {
                Links =
                {
                    new Link
                    {
                        Href = LinkFormatter.Stream(streamId),
                        Rel = Constants.Relations.DeleteStream
                    }
                }
            });

            client = await client.Delete(
                Constants.Relations.DeleteStream,
                null,
                null,
                new Dictionary<string, string[]>
                {
                    [Constants.Headers.ExpectedVersion] = new[] { $"{expectedVersion}" }
                },
                cancellationToken);

            ThrowOnError(client);
        }

        public async Task DeleteMessage(
            StreamId streamId,
            Guid messageId,
            CancellationToken cancellationToken = default)
        {
            GuardAgainstDisposed();

            var client = CreateClient(new Resource
            {
                Links =
                {
                    new Link
                    {
                        Href = LinkFormatter.StreamMessageByMessageId(streamId, messageId),
                        Rel = Constants.Relations.DeleteMessage
                    }
                }
            });

            client = await client.Delete(
                Constants.Relations.DeleteMessage,
                null,
                null,
                cancellationToken: cancellationToken);

            ThrowOnError(client);
        }
    }
}