using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using Xamarin.Forms;


namespace FoodForThought.ViewModels
{
	public class ProfilePageViewModel : BaseViewModel
	{
		private UserProfile _item;

		public UserProfile Item
		{
			get { return _item; }
			set { SetProperty(ref _item, value, "Item"); }
		}

		public ProfilePageViewModel()
		{
			Title = "Profile Page";
			//Load profile
			Item = App.user;
		}
	}
}

