namespace OnlineCoursesPlatform.SERVER.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
