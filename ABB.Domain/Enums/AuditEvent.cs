namespace ABB.Domain.Enums
{
    public enum AuditEvent
    {
        LoginAttempt,
        LockOut,
        UserCreation,
        UserModification,
        RoleCreation,
        RoleModification,
        PasswordReset
    }
}