using RC2K.Resources.DAOs;

namespace RC2K.Resources
{
    public static class Cars
    {
        private readonly static List<CarDao> _carsData = [];

        static Cars()
        {
            string cars = Properties.Resources.Cars;
            using StringReader reader = new(cars);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split(',');
                _carsData.Add(new(split[0], split[1]));
            }
        }

        /// <summary>
        /// Returns list of all cars with given class.
        /// </summary>
        public static IReadOnlyList<CarDao> GetAll() => _carsData;
    }
}
