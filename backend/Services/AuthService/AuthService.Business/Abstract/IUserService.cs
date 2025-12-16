using AuthService.Entities.Concrete;
using AuthService.Entities.DTOs;
using Core.Utilities.Result;

namespace AuthService.Business.Abstract
{
    public interface IUserService
    {
        IDataResult<User> GetById(int id);
        IDataResult<User> GetByEmail(string email);
        IDataResult<UserDto> GetUserDto(int id);
        IDataResult<List<UserDto>> GetAll();
        IResult Update(User user);
        IResult Delete(int id);
        IDataResult<List<OperationClaim>> GetClaims(User user);
        IResult AddRole(int userId, string roleName);
        IResult RemoveRole(int userId, string roleName);
    }
}

