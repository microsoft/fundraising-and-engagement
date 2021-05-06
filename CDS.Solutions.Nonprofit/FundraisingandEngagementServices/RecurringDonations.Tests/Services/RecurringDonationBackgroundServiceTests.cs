using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Data.Tests;
using FluentAssertions;
using FundraisingandEngagement.Data;
using FundraisingandEngagement.Data.DataSynchronization;
using FundraisingandEngagement.DataverseSynchronization;
using FundraisingandEngagement.RecurringDonations.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Moq;
using NUnit.Framework;
using PaymentProcessors;

namespace RecurringDonations.Tests.Services
{
    public class RecurringDonationBackgroundServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SynchronizesChangesBeforeCalculation()
        {
            var paymentRepoMock = new Mock<IPaymentRepoService>();
            paymentRepoMock.Setup(it => it.GetFailedPaymentsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IReadOnlyList<SinglePaymentVariables>>(new List<SinglePaymentVariables>()));
            paymentRepoMock.Setup(it => it.GetScheduledPaymentsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IReadOnlyList<SinglePaymentVariables>>(new List<SinglePaymentVariables>()));

            var dbContextMock = new Mock<PaymentContext>();
            dbContextMock.Setup(it => it.Model).Returns(PaymentContextSpy.GetModel());

            var dataverseSynchronizerMock = new Mock<IDataverseSynchronizer>();
            var service = new RecurringDonationBackgroundService(
                paymentRepoMock.Object,
                Mock.Of<IPaymentProcessorGateway>(),
                Mock.Of<IHostApplicationLifetime>(),
                LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<RecurringDonationBackgroundService>(),
                new DbEntitySynchronizer(dataverseSynchronizerMock.Object, dbContextMock.Object)
            );

            var result = service.StartAsync(new CancellationToken());
            result.Wait();
            result.Status.Should().Be(TaskStatus.RanToCompletion);
            dataverseSynchronizerMock.Verify(it => it.Synchronize(It.IsNotNull<IEnumerable<string>>()));
        }
    }
}