using rc_car_maui_app.Websocket;

namespace rc_car_maui_app.Services;

public class GyroscopeService
{
    public void StartGyroscope()
    {
        OrientationSensor.ReadingChanged += (s, e) =>
        {
            var q = e.Reading.Orientation; // quaternion
            // Convert to roll in degrees
            var sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            var cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            var roll = Math.Atan2(sinr_cosp, cosr_cosp) * 180.0 / Math.PI;

            // Apply calibration
            const double baselineRoll = 0.0;
            var zeroedRoll = roll - baselineRoll;

            // Dead zone
            if (Math.Abs(zeroedRoll) < 5)
                zeroedRoll = 0;

            double steering = Math.Clamp((zeroedRoll / 30.0 + 1) * 50.0, 0.0, 100.0);

            WebsocketClient.SetControlData("steering", steering);
        };
        OrientationSensor.Start(SensorSpeed.Game);
    }

    public void StopGyroscope()
    {
        OrientationSensor.Stop();
    }

    private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
    {
        Console.WriteLine($"Gyroscope reading: {e.Reading}");
    }
}