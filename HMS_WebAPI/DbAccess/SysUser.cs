namespace HMS_WebAPI.DbAccess
{
    public class SysUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

       // public SysUser(string name, string password) 
       // {
       //     UserName = name;
       //     Password = password;
       // }
    }
}
