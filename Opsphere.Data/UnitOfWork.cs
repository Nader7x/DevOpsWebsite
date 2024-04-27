using Opsphere.Data.Repositories;
using Opsphere.Interfaces;

namespace Opsphere.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public ICardRepository CardRepository { get; private set; }
    public IProjectRepository ProjectRepository { get; private set; }
    public IUserRepository UserRepository { get; private set; }
    public IProjectDeveloperRepository ProjectDeveloperRepository { get; private set; }
    public IAttachmentRepository AttachmentRepository { get; private set; }
    public ICardCommentRepository CardCommentRepository { get; private set; }
    public INotificationRepository NotificationRepository { get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        CardRepository = new CardRepository(_dbContext);
        ProjectRepository = new ProjectRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
        ProjectDeveloperRepository = new ProjectDeveloperRepository(_dbContext);
        AttachmentRepository = new AttachmentRepository(_dbContext);
        CardCommentRepository = new CardCommentRepository(_dbContext);
        NotificationRepository = new NotificationRepository(_dbContext);
    }
    public async void Dispose()
    {
        await _dbContext.DisposeAsync();
    }
    public async Task<int> CompleteAsync()
    {
       return await _dbContext.SaveChangesAsync();
    }
}