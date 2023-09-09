using System.Security;

namespace Chat.Domain.ViewModels
{
    public class NewUserVm
    {
        //public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }

    public class SignUpUserPhaseOneVm
    {
        //public int RoleId { get; set; }
        
	    public string PhoneNumber { get; set; }
	    public string UserEmail { get; set; }
	    //public string Password { get; set; }
	    //public string UserName { get; set; }
    }

    public class SignUpUserPhaseTwoVm
    {
	    //public int RoleId { get; set; }
	    //public string PhoneNumber { get; set; }
	    public string UserEmail { get; set; }
	    public string? UserName { get; set; }
		public string VerificationCode { get; set; }
	    public string Password { get; set; }

    }


}
