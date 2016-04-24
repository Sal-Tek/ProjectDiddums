namespace Diddums.Data
{
    internal static class Converter
    {
        /// <summary>
        /// Converts the temperature value into the specified system equivilent.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="system">The desired output system.</param>
        /// <returns>Float in the desired system.</returns>
        public static float ConvertTemperature(float value, UnitSystem system)
        {
            return system == UnitSystem.Metric ? ((value - 32) * (5 / 9)) : ((value / (5 / 9)) + 32);
        }

        /// <summary>
        /// Converts the velocity value into the specified system equivilent.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="system">The desired output system.</param>
        /// <returns>Float in the desired system.</returns>
        public static float ConvertVelocity(float value, UnitSystem system)
        {
            return system == UnitSystem.Metric ? (value * 0.44704f) : (value * 2.23694f);
        }
    }
}