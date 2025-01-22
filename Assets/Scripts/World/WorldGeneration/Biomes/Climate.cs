
namespace World.WorldGeneration.Biomes
{
    public static class Climate
    {
        public const int ParameterSpace = 4;

        public static TargetPoint Target(float temperature, float humidity, float continentalness, float erosion)
        {
            return new TargetPoint(temperature, humidity, continentalness, erosion);
        }

        public static ParamPoint Parameters(float temperature, float humidity, float continentalness, float erosion)
        {
            return new ParamPoint(
                Param(temperature),
                Param(humidity),
                Param(continentalness),
                Param(erosion)
            );
        }

        public static Param Param(float value, float max = default)
        {
            return new Param(value, max == default ? value : max);
        }
    }
}