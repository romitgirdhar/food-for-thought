using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;

namespace FoodForThought.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
			ZXing.Net.Mobile.Forms.iOS.Platform.Init();
			global::Xamarin.Forms.Forms.Init();

			// Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			LoadApplication(new App());

			// Register for push notifications.
			var settings = UIUserNotificationSettings.GetSettingsForTypes(
				UIUserNotificationType.Alert
				| UIUserNotificationType.Badge
				| UIUserNotificationType.Sound,
				new NSSet());

			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();

			return base.FinishedLaunching(app, options);
		}

		public override void RegisteredForRemoteNotifications(UIApplication application,NSData deviceToken)
		{
			const string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

			JObject templates = new JObject();
			templates["genericMessage"] = new JObject
				{
				  {"body", templateBodyAPNS}
				};

			// Register for push with your mobile app
			var push = FoodForThought.App.CloudService.GetClient().GetPush();
			push.RegisterAsync(deviceToken, templates);

			// Define two new tags as a JSON array.
			var body = new JArray();
			body.Add("broadcast");
			body.Add(FoodForThought.App.user.UserId);


			FoodForThought.App.CloudService.UpdateTags(body);
		}

		public override void DidReceiveRemoteNotification(UIApplication application,NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

			string alert = string.Empty;
			if (aps.ContainsKey(new NSString("alert")))
				alert = (aps[new NSString("alert")] as NSString).ToString();

			//show alert
			if (!string.IsNullOrEmpty(alert))
			{
				UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
				avAlert.Show();
			}
		}
	}
}

