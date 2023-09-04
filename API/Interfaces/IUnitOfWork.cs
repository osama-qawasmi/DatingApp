
namespace API.Interfaces
{
    public interface IUnitOfWork
    {
         public IUserRepository UserRepository { get; }
         public IMessageRepository MessageRepository { get; }
         public ILikeRepository LikeRepository { get; }
         Task<bool> Complete();
         bool HasChanges();
    }
}