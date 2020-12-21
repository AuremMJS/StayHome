using Assets.SimpleAndroidNotifications;
using System;
using UnityEngine;

public class GameNotificationManager : MonoBehaviour
{
    public static GameNotificationManager Instance;
    // Use this for initialization
    void Start()
    {
        NotificationManager.CancelAll();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame()
    {
        SendFullTicketsNotification();
        SendDailyNotification();
        SendWeeklyNotification();
        SendMonthlyNotification();
    }

    public void CancelAllNotification()
    {
        NotificationManager.CancelAll();
    }

    private void SendFullTicketsNotification()
    {
        int tickets = PlayerPrefs.GetInt("Tickets");
        if (tickets < 5)
        {
            var time = ((5 - tickets) * 20 * 60);
            if (PlayerPrefs.HasKey("TicketDecrementedTime"))
            {
                var TimeDecremented = Convert.ToDateTime(PlayerPrefs.GetString("TicketDecrementedTime"));
                var CurrentTime = DateTime.Now;
                var TimeDifference = CurrentTime - TimeDecremented;
                time = ((5 - tickets) * 20 * 60) - TimeDifference.Seconds;
            }
            var notificationParams = new NotificationParams
            {
                Id = UnityEngine.Random.Range(0, int.MaxValue),
                Delay = TimeSpan.FromSeconds(time),
                Title = "Full Tickets",
                Message = "You have full tickets now",
                Ticker = "Ticker",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Heart,
                SmallIconColor = new Color(0, 0.5f, 0),
                LargeIcon = "app_icon"
            };

            NotificationManager.SendCustom(notificationParams);
        }
    }

    private void SendDailyNotification()
    {
        var dailyNotificationParams = new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(24 * 60 * 60),
            Title = "Color Snipers U",
            Message = "Keep quit and just close the windows!",
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        NotificationManager.SendCustom(dailyNotificationParams);
    }

    private void SendWeeklyNotification()
    {
        var weeklyNotificationParams = new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(7 * 24 * 60 * 60),
            Title = "Color Snipers U",
            Message = "Its been a week since you battled against the Color Snipers! Tap to send colors back to soldiers!",
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        NotificationManager.SendCustom(weeklyNotificationParams);
    }
    private void SendMonthlyNotification()
    {
        var monthlyNotificationParams = new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(30 * 24 * 60 * 60),
            Title = "Color Snipers U",
            Message = "Come back and show the power of closing Windows!",
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Heart,
            SmallIconColor = new Color(0, 0.5f, 0),
            LargeIcon = "app_icon"
        };

        NotificationManager.SendCustom(monthlyNotificationParams);
    }
}
