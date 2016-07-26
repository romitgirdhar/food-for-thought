using System;
using FoodForThought.Abstractions;
using FoodForThought.Models;
using FoodForThought.Services;
using Xamarin.Forms;


namespace FoodForThought
{
	public partial class App : Application
	{
		public static ICloudService CloudService { get; set; }
		public static Guid DeviceId { get; private set; }

		//Testing constants
		public static bool kADD_TESTING_UPC = true;
		public static string k_TESTING_UPC = "708163109362";
		public static string kGCM_PROJECT_NUMBER = "476736012638";

		public static UserProfile user;

		public App()
		{
			InitializeComponent();

			LoadDeviceId();

			// Initialize the Cloud Service
			CloudService = new AzureCloudService();

			//MainPage = new FoodForThoughtPage();
			MainPage = new NavigationPage(new Pages.EntryPage());
		}

		private void LoadDeviceId()
		{
			if (Application.Current.Properties.ContainsKey("DeviceId"))
			{
				DeviceId = Guid.Parse(Application.Current.Properties["DeviceId"].ToString());
			}
			else {
				DeviceId = Guid.NewGuid();
				Application.Current.Properties["DeviceId"] = DeviceId.ToString();
				Application.Current.SavePropertiesAsync();
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

