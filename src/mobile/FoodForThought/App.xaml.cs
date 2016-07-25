using FoodForThought.Abstractions;
using FoodForThought.Services;
using Xamarin.Forms;

namespace FoodForThought
{
	public partial class App : Application
	{
		public static ICloudService CloudService { get; set; }

		public App()
		{
			InitializeComponent();

			// Initialize the Cloud Service
			CloudService = new AzureCloudService();

			MainPage = new FoodForThoughtPage();
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

