namespace ABB.Web.Modules.Account.Models
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            UserProfile = new ChangeProfileModel();
            UserPassword = new ChangeCurrentPasswordModel();
        }

        public ChangeProfileModel UserProfile { get; set; }
        public ChangeCurrentPasswordModel UserPassword { get; set; }
    }
}