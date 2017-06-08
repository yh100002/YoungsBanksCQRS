using System;
using System.Threading.Tasks;
using CQRSlite.Bus;
using CQRSlite.Tests.Substitutes;
using Xunit;
using CQRSlite.Commands;
using CQRSlite.Domain.Exception;

namespace CQRSlite.Tests.Bus
{
    public class When_sending_command
    {
        private InProcessBus _bus;

        public When_sending_command()
        {
            _bus = new InProcessBus();
        }

        [Fact]
        public async Task Should_run_handler()
        {
            var handler = new TestAggregateDoSomethingHandler();
            _bus.RegisterHandler<TestAggregateDoSomething>(handler.Handle);
            await _bus.Send(new TestAggregateDoSomething());

            Assert.Equal(1,handler.TimesRun);
        }

        [Fact]
        public async Task Should_throw_if_more_handlers()
        {
            var x = new TestAggregateDoSomethingHandler();
            _bus.RegisterHandler<TestAggregateDoSomething>(x.Handle);
            _bus.RegisterHandler<TestAggregateDoSomething>(x.Handle);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _bus.Send(new TestAggregateDoSomething()));
        }

        [Fact]
        public async Task Should_throw_if_no_handlers()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _bus.Send(new TestAggregateDoSomething()));
        }

        [Fact]
        public async Task Should_handle_dynamically_generated_commands()
        {
            var handler = new TestAggregateDoSomethingHandler();
            var command = (ICommand)Activator.CreateInstance(typeof(TestAggregateDoSomething));

            _bus.RegisterHandler<TestAggregateDoSomething>(handler.Handle);
            await _bus.Send(command);

            Assert.Equal(1, handler.TimesRun);
        }

        [Fact]
        public async Task Should_throw_if_handler_throws()
        {
            var handler = new TestAggregateDoSomethingHandler();
            _bus.RegisterHandler<TestAggregateDoSomething>(handler.Handle);
            await Assert.ThrowsAsync<ConcurrencyException>(
                async () => await _bus.Send(new TestAggregateDoSomething {ExpectedVersion = 30}));
        }

        [Fact]
        public async Task Should_wait_for_send_to_finish()
        {
            var handler = new TestAggregateDoSomethingHandler();
            _bus.RegisterHandler<TestAggregateDoSomething>(handler.Handle);
            await _bus.Send(new TestAggregateDoSomething { LongRunning = true });
            Assert.Equal(1, handler.TimesRun);
        }
    }
}