using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using UnityEngine.TestTools;

namespace Padoru.Core.Tests
{
    public class CommandQueueTests
    {
        [Test]
        public void QueueCommand_WhenCommandAlreadyQueued_ShouldThrow()
        {
            var commandQueue = new CommandQueue();
            var command = new TestCommand(OpResult.Succeeded);

            commandQueue.QueueCommand(command);

            Assert.Throws<Exception>(() => commandQueue.QueueCommand(command));
        }

        [Test]
        public void QueueCommand_WhenCommandNull_ShouldThrow()
        {
            var commandQueue = new CommandQueue();

            Assert.Throws<Exception>(() => commandQueue.QueueCommand(null));
        }

        [Test]
        public void ExecuteQueue_WhenSuccessfulCommandQueued_ShouldFinish()
        {
            var commandQueue = new CommandQueue();
            var command = new TestCommand(OpResult.Succeeded);
            var commandFinished = false;
            command.OnFinish += (result) =>
            {
                commandFinished = true;
            };

            commandQueue.QueueCommand(command);
            commandQueue.Execute();

            Assert.IsTrue(commandFinished);
        }

        [Test]
        public void ExecuteQueue_WhenFailedCommandQueued_ShouldFinish()
        {
            var commandQueue = new CommandQueue();
            var command = new TestCommand(OpResult.Failed);
            var commandFinished = false;
            command.OnFinish += (result) =>
            {
                commandFinished = true;
            };

            commandQueue.QueueCommand(command);
            commandQueue.Execute();

            Assert.IsTrue(commandFinished);
        }

        [Test]
        public void ExecuteQueue_WhenSucessfulCommandsQueued_ShouldFinish()
        {
            var commandQueue = new CommandQueue();
            var command = new TestCommand(OpResult.Succeeded);
            var command2 = new TestCommand(OpResult.Succeeded);
            var finishedCommands = 0;

            command.OnFinish += (result) =>
            {
                finishedCommands++;
            };
            command2.OnFinish += (result) =>
            {
                finishedCommands++;
            };

            commandQueue.QueueCommand(command);
            commandQueue.QueueCommand(command2);
            commandQueue.Execute();

            Assert.IsTrue(finishedCommands == 2);
        }

        [Test]
        public void ExecuteQueue_WhenFailedCommandsQueued_ShouldFinish()
        {
            var commandQueue = new CommandQueue();
            var command = new TestCommand(OpResult.Failed);
            var command2 = new TestCommand(OpResult.Failed);
            var finishedCommands = 0;

            command.OnFinish += (result) =>
            {
                finishedCommands++;
            };
            command2.OnFinish += (result) =>
            {
                finishedCommands++;
            };

            commandQueue.QueueCommand(command);
            commandQueue.QueueCommand(command2);
            commandQueue.Execute();

            Assert.IsTrue(finishedCommands == 2);
        }

        [Test]
        public void ExecuteQueue_WhenCommandThrows_ShouldLogAndFinish()
        {
            var exceptionMessage = "TestMessage";

            var commandQueue = new CommandQueue();
            var command = new ThrowCommand(exceptionMessage);
            var command2 = new TestCommand(OpResult.Succeeded);
            var finishedCommands = 0;

            command.OnFinish += (result) =>
            {
                finishedCommands++;
            };
            command2.OnFinish += (result) =>
            {
                finishedCommands++;
            };

            commandQueue.QueueCommand(command);
            commandQueue.QueueCommand(command2);

            LogAssert.Expect(UnityEngine.LogType.Error, new Regex(exceptionMessage));

            commandQueue.Execute();

            Assert.IsTrue(finishedCommands == 1);
        }
    }
}