using API.DTOs;
using API.Enitities;
using API.Helpers;


namespace API.Interfaces
{
    public interface ILikeRepository
    {
         Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
         Task<AppUser> GetUserWithLikes(int userId);
         Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams);
    }
}