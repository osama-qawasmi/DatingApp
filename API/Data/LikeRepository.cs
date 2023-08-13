using API.DTOs;
using API.Enitities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace API.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        public LikeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likeParams.Predicate == "liked")
            {
                likes = likes.Where(l => l.SourceUserId == likeParams.UserId);
                users = likes.Select(l => l.TargetUser);
            }
            else if(likeParams.Predicate == "likedBy")
            {
                likes = likes.Where(l => l.TargetUserId == likeParams.UserId);
                users = likes.Select(l => l.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto 
            {
                Id = user.Id,
                UserName = user.UserName,
                KnownAs = user.UserName,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                MainPhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likeParams.PageNumber, likeParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(x => x.Id == userId);
        }

    }
}