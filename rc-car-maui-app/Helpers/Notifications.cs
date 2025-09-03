using rc_car_maui_app.Controls;

namespace rc_car_maui_app.Helpers;

public static class Notifications
{
    private static readonly Dictionary<string, Notification> notifications = new Dictionary<string, Notification>()
    {
        { "stopp", new Notification("street_stop_sign", "Caution: Stop Sign ahead!") },
        { "achtung", new Notification("street_attention_sign", "Caution: Caution Sign ahead!") },
        { "sackgasse", new Notification("street_dead_end_sign", "Caution: Dead End ahead!") },
        { "unbegrenzt", new Notification("street_unlimited_sign", "Caution: Unlimited ahead!") },
        { "kreuzung", new Notification("street_straight_or_turn_right_sign", "Caution: Straight Or Turn Right ahead!") },
        { "abbiegen", new Notification("street_turn_right_sign", "Caution: Turn Right Ahead!") },
        { "durchfahrt_verboten", new Notification("street_no_entry_sign", "Caution: No Entry Ahead!") },
        { "dreissig", new Notification("street_30_sign", "Caution: Speed limit ahead!") },
        { "fuenfzig", new Notification("street_50_sign", "Caution: Speed limit ahead!") },
    };
    
    public static Notification? Of(string key)
    {
        return notifications.TryGetValue(key, out var notification) 
            ? notification 
            : null;
    }

}