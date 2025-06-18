using DotnetAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    public UserEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList();
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"
           SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            FROM TutorialAppSchema.Users 
            WHERE UserId = " + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
            UPDATE TutorialAppSchema.Users  
                SET [FirstName] = '" + user.FirstName +
                "', [LastName] = '" + user.LastName +
                "', [Email] = '" + user.Email +
                "', [Gender] = '" + user.Gender +
                "', [Active] = '" + user.Active +
                "' WHERE UserId = " + user.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDTO user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
                ) VALUES (" +
                    "'" + user.FirstName +
                    "','" + user.LastName +
                    "','" + user.Email +
                    "','" + user.Gender +
                    "','" + user.Active +
                "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Add User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.Users
                WHERE UserId =" + userId.ToString();
               
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Delete User");
    }
    
}

