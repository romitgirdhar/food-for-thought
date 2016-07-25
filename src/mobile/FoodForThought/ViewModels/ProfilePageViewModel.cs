using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using Xamarin.Forms;


namespace FoodForThought.ViewModels
{
	public class ProfilePageViewModel : BaseViewModel
	{
		public ProfilePageViewModel()
		{
			 Title = "Profile Page";
			//Title = App.DeviceId.ToString();
		}
	}
}

