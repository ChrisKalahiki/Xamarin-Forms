
using Xamarin.Forms;

namespace InstagramApp
{
	public partial class UserProfilePage : ContentPage
	{
		private UserService _service = new UserService();

		public UserProfilePage(int userId)
		{
			BindingContext = _service.GetUser(userId);

			InitializeComponent();
		}
	}
}
