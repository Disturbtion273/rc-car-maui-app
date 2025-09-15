using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Services;

public class GyroscopeService
{
    public void StartGyroscope()
    {
        if (!OrientationSensor.IsSupported) return;
        OrientationSensor.ReadingChanged += (s, e) =>
        {
            var q = e.Reading.Orientation;
            // Convert to roll in degrees
            var sinrCosp = 2 * (q.W * q.X + q.Y * q.Z);
            var cosrCosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            var roll = Math.Atan2(sinrCosp, cosrCosp) * 180.0 / Math.PI;

            // Dead zone
            if (Math.Abs(roll) < 5)
                roll = 0;

            double steering = Math.Clamp((roll / 30.0 + 1) * 50.0, 0.0, 100.0);

            WebsocketClient.SetControlData("steering", (int)steering);
        };
        OrientationSensor.Start(SensorSpeed.Game);
    }

    public void StopGyroscope()
    {
        if (!OrientationSensor.IsSupported) return;
        OrientationSensor.Stop();
    }
}