using API.DTOs;
using API.Enitities;
using API.Helpers;


namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<IEnumerable<AppUser>> GetUsersAsync();
         Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUsernameAsync(string username);
         Task<MemberDto> GetMemberAsync(string username);
         Task<string> GetUserGender(string username);
    }
}