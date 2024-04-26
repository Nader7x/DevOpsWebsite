namespace Opsphere.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICardRepository CardRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IUserRepository UserRepository { get; }
    IProjectDeveloperRepository ProjectDeveloperRepository { get; }
    IAttachmentRepository AttachmentRepository { get; }
    ICardCommentRepository CardCommentRepository { get; }
    Task<int> CompleteAsync();
}