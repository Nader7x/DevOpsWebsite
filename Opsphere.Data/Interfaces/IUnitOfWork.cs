namespace Opsphere.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICardRepository CardRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IUserRepository UserRepository { get; }
    IProjectDeveloperRepository ProjectDeveloperRepository { get; }
    IAttachmentRepository AttachmentRepository { get; }
    ICardCommentRepository CardCommentRepository { get; }
    INotificationRepository NotificationRepository { get; }
    
    IReplyRepository ReplyRepository { get; }
    Task<int> CompleteAsync();
}