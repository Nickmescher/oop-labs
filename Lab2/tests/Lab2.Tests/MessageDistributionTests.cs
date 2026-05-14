using Lab2;
using Lab2.Archivers;
using Lab2.Recipients;
using NSubstitute;
using Xunit;

namespace Lab2.Tests;

public class MessageDistributionTests
{
    // Test: user receives message -> stored as unread
    [Fact]
    public void UserReceivesMessage_StoredAsUnread()
    {
        // Arrange
        var user = new User("Alice");
        var message = new Message("Test", "Body", ImportanceLevel.Low);

        // Act
        user.Receive(message);

        // Assert
        Assert.Single(user.Messages);
        Assert.Equal(UserMessageStatus.Unread, user.Messages[0].Status);
    }

    // Test: mark unread message as read -> status changes to read
    [Fact]
    public void MarkUnreadMessage_StatusChangesToRead()
    {
        // Arrange
        var user = new User("Bob");
        var message = new Message("Hello", "World", ImportanceLevel.Medium);
        user.Receive(message);

        // Act
        MarkReadResult result = user.Messages[0].TryMarkAsRead();

        // Assert
        Assert.IsType<MarkReadSuccess>(result);
        Assert.Equal(UserMessageStatus.Read, user.Messages[0].Status);
    }

    // Test: mark already-read message -> returns failure
    [Fact]
    public void MarkAlreadyReadMessage_ReturnsFailure()
    {
        // Arrange
        var user = new User("Carol");
        var message = new Message("Notice", "Content", ImportanceLevel.High);
        user.Receive(message);
        user.Messages[0].TryMarkAsRead();

        // Act
        MarkReadResult result = user.Messages[0].TryMarkAsRead();

        // Assert
        var failure = Assert.IsType<MarkReadFailure>(result);
        Assert.False(string.IsNullOrEmpty(failure.Reason));
    }

    // Test: filtering recipient blocks message below minimum importance (uses mock)
    [Fact]
    public void FilteringRecipient_BlocksMessageBelowMinimumImportance()
    {
        // Arrange
        var mockInner = Substitute.For<IRecipient>();
        var filtering = new FilteringRecipient(mockInner, ImportanceLevel.High);
        var lowMessage = new Message("Low", "Body", ImportanceLevel.Low);

        // Act
        filtering.Send(lowMessage);

        // Assert
        mockInner.Received(0).Send(Arg.Any<Message>());
    }

    // Test: logging recipient calls logger when message arrives (uses mock)
    [Fact]
    public void LoggingRecipient_CallsLoggerOnMessageReceived()
    {
        // Arrange
        var mockLogger = Substitute.For<ILogger>();
        var mockInner = Substitute.For<IRecipient>();
        var logging = new LoggingRecipient(mockInner, mockLogger);
        var message = new Message("Important", "Details", ImportanceLevel.High);

        // Act
        logging.Send(message);

        // Assert
        mockLogger.Received(1).Log(Arg.Is<string>(s => s.Contains("Important")));
        mockInner.Received(1).Send(message);
    }

    // Test: FormattingArchiver calls formatter's WriteTitle and WriteBody (uses mock)
    [Fact]
    public void FormattingArchiver_CallsFormatterMethods()
    {
        // Arrange
        var mockFormatter = Substitute.For<IMessageFormatter>();
        var archiver = new FormattingArchiver(mockFormatter);
        var message = new Message("Archive Title", "Archive Body", ImportanceLevel.Medium);

        // Act
        archiver.Archive(message);

        // Assert
        mockFormatter.Received(1).WriteTitle("Archive Title");
        mockFormatter.Received(1).WriteBody("Archive Body");
    }

    // Test: two UserRecipients for same user, one with importance filter -> user receives message once
    [Fact]
    public void TwoUserRecipients_OneWithFilter_LowMessage_UserReceivesOnce()
    {
        // Arrange
        var user = new User("Dave");
        var directRecipient = new UserRecipient(user);
        var filteredRecipient = new FilteringRecipient(new UserRecipient(user), ImportanceLevel.High);
        var group = new GroupRecipient(new List<IRecipient> { directRecipient, filteredRecipient });
        var lowMessage = new Message("Low Importance", "Body", ImportanceLevel.Low);

        // Act
        group.Send(lowMessage);

        // Assert
        Assert.Single(user.Messages);
    }
}
