using Microsoft.EntityFrameworkCore;

namespace HMS_WebAPI.DbAccess
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        public int UserId { get; set; }
        public SysUser User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
