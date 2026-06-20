using System;

namespace HRSYSTEM
{
    public class UserSession
    {
        public int UserId { get; init; }
        public int? EmployeeId { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public DateTime? LastLogin { get; init; }

        public string DisplayName
        {
            get
            {
                string fullName = $"{FirstName} {LastName}".Trim();
                return string.IsNullOrWhiteSpace(fullName) ? Username : fullName;
            }
        }
    }
}
