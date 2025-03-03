using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HMS_Phase1;
using HMS_WebAPI.DbAccess;
using HMS_WebAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HMS_WebAPI.Managers
{
    public class AccountManager
    {
        private readonly HMSContext dbcontext;
        private readonly string _secretKey;

        public AccountManager(HMSContext context, IConfiguration configuration)
        {
            dbcontext = context;
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        public void Register(UserDTO user)
        {
            var newUser = new SysUser()
            {
                UserName = user.UserName,
                Password = user.Password
            };

            newUser.UserRoles = user.RoleIds.Select(roleId => new UserRole()
            { RoleId = roleId, User = newUser }).ToList();

            dbcontext.Users.Add(newUser);
            dbcontext.SaveChanges();
        }

        public string? Authenticate(LoginDTO loginUser)
        {
            var systemUser = dbcontext.Users
                .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)   
                .Where(user => user.UserName == loginUser.UserName && user.Password == loginUser.Password)
                .FirstOrDefault();

            if (systemUser == null)
                return null;

            List<Claim> myClaims = systemUser.UserRoles
                .Select(x => new Claim(ClaimTypes.Role, x.Role.Name)).ToList();

            myClaims.Add(new Claim(ClaimTypes.NameIdentifier, systemUser.Id.ToString()));


            SecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            
            SigningCredentials mySigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken mySecurityToken = new JwtSecurityToken
                (
                    expires: DateTime.Now.AddHours(1),
                    claims : myClaims,
                    signingCredentials : mySigningCredentials
                    
                );

            return new JwtSecurityTokenHandler().WriteToken(mySecurityToken);
        }
    }
}
