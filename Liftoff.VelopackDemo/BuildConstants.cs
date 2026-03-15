namespace Liftoff.VelopackDemo;

internal static partial class BuildConstants
{
    internal const string FAKE_BUILD_CONSTANT = "FAKE_TEST_VALUE";

    internal static string GetValue(string propertyName)
    {
        var property = typeof(BuildConstants).GetField(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (property == null)
        {
            return null;
        }

        return property.GetValue(null) as string;
    }
}
