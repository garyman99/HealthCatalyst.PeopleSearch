using System;

namespace PeopleSearch.Seeder.Seeders.Random
{
    public class RandomSeederOptions
    {
        /// <summary>
        /// Two seconds
        /// </summary>
        public const int DefaultMinDelayMilliseconds = 2000;

        /// <summary>
        /// 10 seconds
        /// </summary>
        public const int DefaultMaxDelayMilliseconds = 10000;

        /// <summary>
        /// 100 seeds
        /// </summary>
        public const int DefaultLogEverySeeds = 100;

        private readonly TimeSpan? _delay;
        private int? _logEverySeeds;

        /// <summary>
        /// Options for controlling the random seeding of data.
        /// </summary>
        /// <param name="initialSeedAmount">The number of items to seed before introducing a delay between seeds.</param>
        /// <param name="maxSeedAmount">The maximum number of seeds to generate. Period.</param>
        /// <param name="delay">
        ///     After the initial seed amount has been reached, this is the amount of time between subsequent 
        ///     seeds are generated.  If not specified, the delay will be random between <see cref="DefaultMinDelayMilliseconds" />
        ///     and <see cref="DefaultMaxDelayMilliseconds" /> for each call.
        /// </param>
        /// <param name="logEverySeeds">
        ///     The number of seeds to log generate between logging progress. Defaults to  <see cref="DefaultLogEverySeeds" />
        /// </param>
        public RandomSeederOptions(int initialSeedAmount, int maxSeedAmount, TimeSpan? delay = null, int? logEverySeeds = null)
        {
            if (initialSeedAmount > maxSeedAmount)
            {
                throw new ArgumentException($"The initial seed amount {initialSeedAmount} specified is " +
                                            $"greater than the max seed amount {maxSeedAmount}.");
            }
            InitialSeedAmount = initialSeedAmount;
            MaxSeedAmount = maxSeedAmount;
            _delay = delay;
            _logEverySeeds = logEverySeeds;
        }

        public int InitialSeedAmount { get; private set; }
      
        public int MaxSeedAmount { get; private set; }

        public TimeSpan Delay
        {
            get
            {
                if (_delay.HasValue)
                {
                    return _delay.Value;
                }

                var randomDelay = new System.Random().Next(DefaultMinDelayMilliseconds, DefaultMaxDelayMilliseconds);
                return new TimeSpan(0, 0, 0, 0, randomDelay);
            }
        }

        public int LogEverySeeds
        {
            get { return _logEverySeeds ?? DefaultLogEverySeeds; }
        }
    }
}